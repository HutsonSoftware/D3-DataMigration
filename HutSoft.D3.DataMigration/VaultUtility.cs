using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HutSoft.D3.DataMigration
{
    internal class VaultUtility
    {
        private Settings _settings;
        private WebServiceManager _webServiceManager;

        internal VaultUtility(Settings settings)
        {
            _settings = settings;
        }

        internal Settings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        internal WebServiceManager WebServiceManager
        {
            get { return _webServiceManager; }
        }
        
        internal void LoginToVault(string vaultServer, string vaultInstance, string vaultUserName, string vaultPassword)
        {
            try
            {
                _webServiceManager = new WebServiceManager(new UserPasswordCredentials(vaultServer, vaultInstance, vaultUserName, vaultPassword, false));
                _webServiceManager.ReSignIn = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void LoginToVault()
        {
            try
            {
                LoginToVault(_settings.VaultServer, _settings.VaultInstance, _settings.VaultUserName, _settings.VaultPassword);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal bool FlightCheck()
        {
            bool passesLifeCycleDefName, passesWipStateName, passesWipStateID, passesReleasedStateName, passesReleasedStateID;
            passesLifeCycleDefName = passesWipStateName = passesWipStateID = passesReleasedStateName = passesReleasedStateID = false;
            try
            {
                SettingsValuesExistInVault(
                    _settings.LifeCycleDefName, out passesLifeCycleDefName, 
                    _settings.WipStateName, out passesWipStateName, 
                    _settings.WipStateID, out passesWipStateID, 
                    _settings.ReleasedStateName, out passesReleasedStateName, 
                    _settings.ReleasedStateID, out passesReleasedStateID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return (passesLifeCycleDefName && passesWipStateName && passesWipStateID && passesReleasedStateName && passesReleasedStateID);
        }

        internal void SettingsValuesExistInVault(
            string lifeCycleDefName, out bool LifeCycleDefName_found,
            string wipStateName, out bool WipStateName_found,
            long wipStateID, out bool WipStateID_found,
            string releasedStateName, out bool ReleasedStateName_found,
            long releasedStateID, out bool ReleasedStateID_found)
        {
            LifeCycleDefName_found = WipStateName_found = WipStateID_found = ReleasedStateName_found = ReleasedStateID_found = false;
            try
            {
                LfCycDef[] lfCycDefs = _webServiceManager.DocumentServiceExtensions.GetAllLifeCycleDefinitions();
                LfCycDef lfCycDef = (from row in lfCycDefs where row.DispName == lifeCycleDefName select row).FirstOrDefault();
                if (lfCycDef != null)
                {
                    LifeCycleDefName_found = true;
                    LfCycState[] lfCycStates = lfCycDef.StateArray;
                    LfCycState lfCycState_WipStateName = (from row in lfCycStates where row.DispName == wipStateName select row).FirstOrDefault();
                    if (lfCycState_WipStateName != null)
                    {
                        WipStateName_found = true;
                        LfCycState lfCycState_WipStateID = (from row in lfCycStates where row.DispName == wipStateName && row.Id == wipStateID select row).FirstOrDefault();
                        if (lfCycState_WipStateID != null)
                        {
                            WipStateID_found = true;
                        }
                    }
                    LfCycState lfCycState_ReleasedStateName = (from row in lfCycStates where row.DispName == releasedStateName select row).FirstOrDefault();
                    if (lfCycState_ReleasedStateName != null)
                    {
                        ReleasedStateName_found = true;
                        LfCycState lfCycState_ReleasedStateID = (from row in lfCycStates where row.DispName == releasedStateName && row.Id == releasedStateID select row).FirstOrDefault();
                        if (lfCycState_ReleasedStateID != null)
                        {
                            ReleasedStateID_found = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal List<Autodesk.Connectivity.WebServices.File> GetVaultFilesByFileName(string fileName)
        {
            List<Autodesk.Connectivity.WebServices.File> foundFiles = new List<Autodesk.Connectivity.WebServices.File>();
            PropDef[] filePropDefs = _webServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            PropDef fileNamePropDef = filePropDefs.Single(n => n.SysName == "FileName");
            SrchCond fileNameCond = new SrchCond()
            {
                PropDefId = fileNamePropDef.Id,
                PropTyp = PropertySearchType.SingleProperty,
                SrchOper = 3,
                SrchRule = SearchRuleType.Must,
                SrchTxt = fileName
            };
            string bookmark = string.Empty;
            SrchStatus status = null;
            while (status == null || foundFiles.Count < status.TotalHits)
            {
                Autodesk.Connectivity.WebServices.File[] results =
                    _webServiceManager.DocumentService.FindFilesBySearchConditions(
                        new SrchCond[] { fileNameCond }, null, null, false, true, ref bookmark, out status);
                if (results != null)
                    foundFiles.AddRange(results);
                else
                    break;
            }
            return foundFiles;
        }

        internal void UpdateVaultFileToWipState(long masterId)
        {
            try
            {
                _webServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(
                    new long[] { masterId }, new long[] { _settings.WipStateID }, "Data Migration");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void CheckOutFile(long masterId)
        {
            Autodesk.Connectivity.WebServices.File latestFileVersion =
                _webServiceManager.DocumentService.FindLatestFilesByMasterIds(new long[] { masterId }).First();
            ByteArray ticket = null;
            _webServiceManager.DocumentService.CheckoutFile(
                latestFileVersion.Id,
                CheckoutFileOptions.Master,
                null,
                null,
                "Data Migration - Updating File Revision",
                out ticket);
        }

        internal long GetVaultFolderId(string vaultFolder)
        {
            long folderId = -1;
            try
            {
                Folder folder = _webServiceManager.DocumentService.GetFolderByPath(vaultFolder);
                if (folder == null)
                {
                    Folder currentFolder = _webServiceManager.DocumentService.GetFolderRoot();
                    string currentPath = "";
                    string[] splitVaultFolders = vaultFolder.Split('/');
                    foreach (var splitVaultFolder in splitVaultFolders)
                    {
                        currentPath = currentFolder.FullName + "/" + splitVaultFolder;
                        Folder nextFolder = _webServiceManager.DocumentService.GetFolderByPath(currentPath);
                        if (nextFolder == null)
                        {
                            nextFolder = _webServiceManager.DocumentService.AddFolder(splitVaultFolder, currentFolder.Id, false);
                        }
                        currentFolder = nextFolder;
                    }
                }
                folderId = folder.Id;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return folderId;
        }

        internal bool CheckinUploadedFile(string localFilePath, long masterID)
        {
            bool success = false;
            Autodesk.Connectivity.WebServices.File file;
            try
            {
                DateTime fileLastModDate = new DateTime(); //TODO: pull this from DateTime from the database
                file = _webServiceManager.DocumentService.CheckinUploadedFile(
                    masterID,
                    "Data Migration - Added new file revision",
                    false,
                    fileLastModDate,
                    null,
                    null,
                    false,
                    new FileInfo(localFilePath).Name,
                    FileClassification.DesignDocument,
                    false,
                    GetFileBytes(localFilePath));
                if (file != null)
                    success = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return success;
        }

        internal bool AddUploadedFile(string localFilePath, long masterId, long vaultFolderId)
        {
            bool success = false;
            Autodesk.Connectivity.WebServices.File file;
            try
            {
                DateTime fileLastModDate = new DateTime(); //TODO: pull this from DateTime from the database
                file = _webServiceManager.DocumentService.AddUploadedFile(
                    vaultFolderId,
                    new FileInfo(localFilePath).Name,
                    "Data Migration - Initial File CheckIn",
                    fileLastModDate,
                    null,
                    null,
                    FileClassification.DesignDocument,
                    false,
                    GetFileBytes(localFilePath));
                if (file != null)
                    success = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return success;
        }

        private ByteArray GetFileBytes(string filePath)
        {
            ByteArray fileBytes = null;
            try {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                byte[] buffer = null;
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
                fileBytes = new ByteArray() { Bytes = buffer };
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return fileBytes;
        }

        internal void UpdateVaultFileProperties(Dictionary<string, string> fileProps, long masterID, out string missingVaultProperties)
        {
            try
            {
                missingVaultProperties = string.Empty;
                int missingVaultPropertyCounter = 0;
                PropDef[] filePropDefs = _webServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
                List<PropInstParamArray> updateProps = new List<PropInstParamArray>();
                foreach (string filePropKey in fileProps.Keys)
                {
                    PropDef nPropDef = filePropDefs.FirstOrDefault(n => n.DispName == filePropKey);
                    if (nPropDef != null)
                    {
                        PropInstParam propInstParam = new PropInstParam();
                        propInstParam.PropDefId = nPropDef.Id;
                        propInstParam.Val = fileProps[filePropKey];

                        PropInstParamArray propInstParamArr = new PropInstParamArray();
                        propInstParamArr.Items = new PropInstParam[] { propInstParam };

                        updateProps.Add(propInstParamArr);
                    }
                    else
                    {
                        //Keep going but make note of the missing Vault Property on the Database record
                        missingVaultProperties = missingVaultPropertyCounter == 0 ? filePropKey : string.Format("{0}, {1}", missingVaultProperties, filePropKey);
                        missingVaultPropertyCounter++;
                    }
                }
                _webServiceManager.DocumentService.UpdateFileProperties(new long[] { masterID }, updateProps.ToArray());
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void SetFileSecurityReadGroups(string[] readACLGroups, long masterID, out string missingACLGroup)
        {
            missingACLGroup = string.Empty;
            try
            {
                AccessPermis readAccessPermission = new AccessPermis();
                readAccessPermission.Id = 1;
                readAccessPermission.Val = true;
                foreach (var aclGroup in readACLGroups)
                {
                    Group group = _webServiceManager.AdminService.GetGroupByName(aclGroup);
                    if (group != null)
                    {
                        ACE ace = new ACE();
                        ace.UserGrpId = group.Id;
                        ace.PermisArray = new AccessPermis[] { readAccessPermission };
                        ACE[] aces = new ACE[1];
                        aces[0] = ace;
                        ACL myACL = _webServiceManager.SecurityService.AddSystemACL(aces);
                        _webServiceManager.SecurityService.SetSystemACLs(new long[] { masterID }, myACL.Id);
                    }
                    else
                    {
                        missingACLGroup = aclGroup;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void SetFileSecurityReadWriteGroups(string[] writeACLGroups, long masterID, out string missingACLGroup)
        {
            missingACLGroup = string.Empty;
            try
            {
                AccessPermis readAccessPermission = new AccessPermis();
                readAccessPermission.Id = 1;
                readAccessPermission.Val = true;
                AccessPermis writeAccessPermission = new AccessPermis();
                writeAccessPermission.Id = 2;
                writeAccessPermission.Val = true;
                foreach (var aclGroup in writeACLGroups)
                {
                    Group group = _webServiceManager.AdminService.GetGroupByName(aclGroup);
                    if (group != null)
                    {
                        ACE ace = new ACE();
                        ace.UserGrpId = group.Id;
                        ace.PermisArray = new AccessPermis[] { readAccessPermission, writeAccessPermission };
                        ACE[] aces = new ACE[1];
                        ACL myAcl = _webServiceManager.SecurityService.AddSystemACL(aces);
                        _webServiceManager.SecurityService.SetSystemACLs(new long[] { masterID }, myAcl.Id);
                    }
                    else
                    {
                        missingACLGroup = aclGroup;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal void UpdateLifeCycleStateInfo(long masterID, out long revMasterId, out long revFileId)
        {
            try
            {
                _webServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(
                    new long[] { masterID }, new long[] { _settings.ReleasedStateID }, "Data Migration - Released new File Revision");
                Autodesk.Connectivity.WebServices.File file = 
                    _webServiceManager.DocumentService.FindLatestFilesByMasterIds(
                        new long[] { masterID }).First();
                revMasterId = file.MasterId;
                revFileId = file.Id;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}

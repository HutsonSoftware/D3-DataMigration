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
        private bool _isLoggedIn = false;
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

        internal bool IsLoggedIn { get { return _isLoggedIn; } }

        internal WebServiceManager WebServiceManager { get { return _webServiceManager; } }

        internal WebServiceManager LoginToVault(string vaultServer, string vaultInstance, string vaultUserName, string vaultPassword)
        {
            WebServiceManager webServiceManager;
            try
            {
                webServiceManager = new WebServiceManager(new UserPasswordCredentials(vaultServer, vaultInstance, vaultUserName, vaultPassword, false));
                if (webServiceManager != null)
                    webServiceManager.ReSignIn = true;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return webServiceManager;
        }

        internal void LoginToVault()
        {
            //Step0
            try
            {
                if (_webServiceManager != null)
                {
                    _webServiceManager = LoginToVault(_settings.VaultServer, _settings.VaultInstance, _settings.VaultUserName, _settings.VaultPassword);
                    _isLoggedIn = true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        internal Autodesk.Connectivity.WebServices.File GetExistingFile(string localFilePath)
        {
            //Step1
            //If the file being migrated has a previous version,
            //we need to look up that file so we can change its state to WIP, then check it out
            //If not previous version, skip to Step4
            //if we have captured the File MasterID from a previous version we can use that to find
            //the latest version when checking in a newer version
            FileInfo fi = new FileInfo(localFilePath);
            if (!fi.Exists)
            {
                throw (new Exception("File does not exist: " + localFilePath));
            }

            //Search for file by name
            PropDef[] filePropDefs = _webServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
            PropDef fileNamePropDef = filePropDefs.Single(n => n.SysName == "FileName");

            SrchCond fileNameCond = new SrchCond()
            {
                PropDefId = fileNamePropDef.Id,
                PropTyp = PropertySearchType.SingleProperty,
                SrchOper = 3,
                SrchRule = SearchRuleType.Must,
                SrchTxt = fi.Name
            };

            string bookmark = string.Empty;
            SrchStatus status = null;
            List<Autodesk.Connectivity.WebServices.File> foundFiles = new List<Autodesk.Connectivity.WebServices.File>();

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

            Autodesk.Connectivity.WebServices.File existingFile = null;

            //Duplicate Files found in Vault
            //This shouldn't happen but we can check just in case
            if (foundFiles.Count > 1)
            {
                //TODO: Record the duplicate file error in the database
                string fileIDs = "";
                int i = 0;
                foreach (var ff in foundFiles)
                {
                    fileIDs = i == 0 ? ff.Id.ToString() : string.Format("{0}, {1}", fileIDs, ff.Id);
                    i++;
                }
            }
            //We found just one file with the same FileName so we'll check our new file in over this one
            else
            {
                //Step 1A: Set previous File Version to WIP and Check Out
                //Now that we have the latest version of the file we can change its state and check it out

                //Set the MasterID so we can reference it later
                existingFile = foundFiles.First();

                //First we need to get a list of the Lifecycle States on this file
                //We are using the same lifecycle definition for all files so this should be fairly straight forward
                LfCycDef[] lcDefs = _webServiceManager.DocumentServiceExtensions.GetAllLifeCycleDefinitions();
                LfCycDef stdLcDef = (from l in lcDefs
                                     where l.DispName == _settings.LifeCycleDefName
                                     select l).FirstOrDefault();
                if (stdLcDef == null)
                {
                    //Lifecycle Definitions was not found, so handle this error
                }

                //Step 1B: Set the state of the file to WIP so we can Check In a new Revision

                //Check the state of the existing file

                //Find the LfCycState that matches the To State Name and get it's ID
                IdPair[] wipFileLCStates = _webServiceManager.DocumentServiceExtensions.GetLifeCycleStateIdsByFileMasterIds(new long[] { existingFile.MasterId });
                foreach (var state in wipFileLCStates)
                {
                    LfCycState lfState = _webServiceManager.LifeCycleService.GetLifeCycleStatesByIds(new long[] { state.ValId }).First();
                    if (lfState.DispName == _settings.WipStateName)
                    {
                        _settings.WipStateID = lfState.Id;
                        break;
                    }
                }

                //Update the Files Lifecycle State
                _webServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(new long[] { existingFile.MasterId }, new long[] { _settings.WipStateID }, "Data Migration");

                //Get the latest version now that we have changed the state, as the state change may have created a new version
                Autodesk.Connectivity.WebServices.File latestFileVersion =
                    _webServiceManager.DocumentService.FindLatestFilesByMasterIds(new long[] { existingFile.MasterId }).First();

                ByteArray ticket = null;
                //Check out the file, but don't worry about downloading it, we're going to just upload a new version on top of this version
                _webServiceManager.DocumentService.CheckoutFile(
                    latestFileVersion.Id,
                    CheckoutFileOptions.Master,
                    null,
                    null,
                    "Data Migration - Updating File Revision",
                    out ticket);
            }
            return existingFile;
        }

        internal void VerifyVaultFolderExists(string vaultFolder)
        {
            //Step2
            //Find the Folder we are checking into, if it doesn't exist then we'll create it
            Folder vFolder = _webServiceManager.DocumentService.GetFolderByPath(vaultFolder);
            if (vFolder == null)
            {
                Folder currentFolder = _webServiceManager.DocumentService.GetFolderRoot();
                string currentPath = "";
                string[] folders = vaultFolder.Split('/');

                //Recursively build the Vault Folders
                foreach (var folder in folders)
                {
                    currentPath = currentFolder.FullName + "/" + folder;
                    Folder nextFolder = _webServiceManager.DocumentService.GetFolderByPath(currentPath);
                    //If the folder doesn't exist then create it
                    if (nextFolder == null)
                    {
                        nextFolder = _webServiceManager.DocumentService.AddFolder(folder, currentFolder.Id, false);
                    }

                    currentFolder = nextFolder;
                }

                vFolder = currentFolder;
            }
        }

        internal void CheckInFile(string localFilePath, Autodesk.Connectivity.WebServices.File existingFile, long fldrID)
        {
            //Step3
            //Upload or checkin the file
            DateTime fileLastModDate = new DateTime(); //TODO: pull this from DateTime from the database
            FileStream fs = new FileStream(localFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            byte[] buffer = null;
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            ByteArray fileBytes = new ByteArray() { Bytes = buffer };

            Autodesk.Connectivity.WebServices.File newFileRev = null;
            FileInfo fi = new FileInfo(localFilePath);
            if (existingFile != null)
            {
                //Step 3A: Check In the File
                newFileRev = _webServiceManager.DocumentService.CheckinUploadedFile(
                    existingFile.MasterId,
                    "Data Migration - Added new file revision",
                    false,
                    fileLastModDate,
                    null,
                    null,
                    false,
                    fi.Name,
                    FileClassification.DesignDocument,
                    false,
                    fileBytes);
            }
            else
            {
                //Step 3B: Upload In the File
                newFileRev = _webServiceManager.DocumentService.AddUploadedFile(
                    fldrID,
                    fi.Name,
                    "Data Migration - Initial File Checkin",
                    fileLastModDate,
                    null,
                    null,
                    FileClassification.DesignDocument,
                    false,
                    fileBytes);
            }

            if (newFileRev == null)
            {
                //TODO: Checkin Failed so handle the error
            }
        }

        internal void UpdateVaultFileProperties(Dictionary<string, string> fileProps, long masterID)
        {
            //Step4
            //Update the Vault File Properties
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
                    //TODO: Property Not Found
                    //Keep going but make note of the missing Vault Property on the Database record
                }
            }
            _webServiceManager.DocumentService.UpdateFileProperties(new long[] { masterID }, updateProps.ToArray());
        }

        internal void SetFileSecurityGroups(string[] writeACLGroups, string[] readACLGroups, long masterID)
        {
            //Step5
            //Set the file security Groups (Access Control List)
            AccessPermis readAccessPermission = new AccessPermis();
            readAccessPermission.Id = 1;
            readAccessPermission.Val = true;

            AccessPermis writeAccessPermission = new AccessPermis();
            writeAccessPermission.Id = 2;
            writeAccessPermission.Val = true;

            //AccessPermis deleteAccessPermission = new AccessPermis();
            //deleteAccessPermission.Id = 3;
            //deleteAccessPermission.Val = true;

            //Set the Read/Write ACL Groups
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
                    //TODO: Security Group not found so log the error in the database
                }
            }

            //Set the Read Only ACL Groups
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
                    //TODO: Security Group not found so log the error in the database
                }
            }
        }

        internal Autodesk.Connectivity.WebServices.File UpdateLifeCycleStateInfo(long masterID)
        {
            //Step6
            //Set the state of the file to Released
            //Find the LfCycState that matches the To State Name and get its ID
            IdPair[] releasedRildLCStates = _webServiceManager.DocumentServiceExtensions.GetLifeCycleStateIdsByFileMasterIds(new long[] { masterID });
            foreach (var state in releasedRildLCStates)
            {
                LfCycState lfState = _webServiceManager.LifeCycleService.GetLifeCycleStatesByIds(new long[] { state.ValId }).First();
                if (lfState.DispName == _settings.ReleasedStateName)
                {
                    _settings.ReleasedStateID = lfState.Id;
                    break;
                }
            }

            //Update the Files Lifecycl State
            _webServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(
                new long[] { masterID }, new long[] { _settings.ReleasedStateID }, "Data Migration - Released new File Revision");

            //Get the latest version now that we have changed the state, as the state change may have created a new version
            return _webServiceManager.DocumentService.FindLatestFilesByMasterIds(new long[] { masterID }).First();
        }
    }
}

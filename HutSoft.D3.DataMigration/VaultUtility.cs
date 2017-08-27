using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using System;
using System.Collections.Generic;
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

        internal void LoginToVault()
        {
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

        internal void VerifyVaultFolderExists(string vaultFolder)
        {
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

        internal void UpdateVaultFileProperties(Dictionary<string, string> fileProps, PropDef[] filePropDefs, long masterID)
        {
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

        internal File UpdateLifeCycleStateInfo(long masterID)
        {
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

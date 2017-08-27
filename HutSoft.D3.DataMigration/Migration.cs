using Autodesk.Connectivity.WebServices;
using Autodesk.Connectivity.WebServicesTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HutSoft.D3.DataMigration
{
    public partial class Migration : Form
    {
        BindingSource _bsAgileTop100Sample = new BindingSource();
        BindingSource _bsAgileStatuses = new BindingSource();
        MyBackgroundWorker[] _backgroundWorkers = null;
        Settings _settings;
        LogUtility _logUtility;
        AgileUtility _agileUtility;
        VaultUtility _vaultUtility;
        int _numThreads = 1;
        List<String> _distinctFileIds;
        Timer timer = new Timer();
        
        public Migration()
        {
            InitializeComponent();
            _settings = new Settings();
            _logUtility = new LogUtility();
            _agileUtility = new AgileUtility(_settings);
            _vaultUtility = new VaultUtility(_settings);

            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = 1000; //1 sec
            timer.Enabled = true;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Point pnt = LogText.AutoScrollOffset;
            LogText.AppendText(_logUtility.TailFollow());
            LogText.AutoScrollOffset = pnt;
        }

        private void Migration_Load(object sender, EventArgs e)
        {
            SelectDatabaseType(DatabaseType.SQLite);
            dgvAgileTop100Sample.DataSource = _bsAgileTop100Sample;
            dgvAgileStatuses.DataSource = _bsAgileStatuses;
            LogFile.Text = string.Format("Log File: {0}", _logUtility.LogPath);
        }

        private void SelectDatabaseType(DatabaseType databaseType)
        {
            rbnSQLite.Checked = (databaseType == DatabaseType.SQLite);
            rbnOracle.Checked = (databaseType == DatabaseType.Oracle);
            _agileUtility.DatabaseType = databaseType;
        }

        private void btnRefreshAgile_Click(object sender, EventArgs e)
        {
            RefreshAgile.Text = "Loading...";
            RefreshAgile.Enabled = false;
            RefreshAgileGrids();
            RefreshAgile.Text = "Refresh";
            RefreshAgile.Enabled = true;
        }

        private void RefreshAgileGrids()
        {
            BindAgileStatusesGrid(_agileUtility.GetStatuses());
            BindAgileTop100SampleGrid(_agileUtility.GetTop100Sample());
        }

        private void BindAgileTop100SampleGrid(DataTable dt)
        {
            _bsAgileTop100Sample.DataSource = dt;
            dgvAgileTop100Sample.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void BindAgileStatusesGrid(DataTable dt)
        {
            _bsAgileStatuses.DataSource = dt;
            dgvAgileStatuses.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void btnMigrate_Click(object sender, EventArgs e)
        {
            //TODO: visual cue of button pushed while migrating.  Tricky to know when threads are done though
            GetThreadCount();
            InitializeBackgroundWorkers();
            SetupTableLayoutPanel();
            PopulateDistinctFileIds();
            AssignFileIdsToBackgroundWorkers();
            RunBackgroundWorkers();
        }

        private void GetThreadCount()
        {
            _numThreads = (int)nudThreads.Value;
        }

        private void InitializeBackgroundWorkers()
        {
            _backgroundWorkers = new MyBackgroundWorker[_numThreads];
            for (int i = 0; i < _numThreads; i++)
            {
                _backgroundWorkers[i] = new MyBackgroundWorker();
                _backgroundWorkers[i].DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                _backgroundWorkers[i].ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                _backgroundWorkers[i].RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                _backgroundWorkers[i].WorkerReportsProgress = true;
                _backgroundWorkers[i].WorkerSupportsCancellation = true;
                _backgroundWorkers[i].Guid = Guid.NewGuid();
                _backgroundWorkers[i].ThreadId = i + 1;
                _backgroundWorkers[i].LogUtility = _logUtility;
                _backgroundWorkers[i].Log("Started");
            }
        }

        private void SetupTableLayoutPanel()
        {
            tableLayoutPanel.SuspendLayout();
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowCount = _backgroundWorkers.Length;

            for (int i = 0; i < _backgroundWorkers.Length; i++)
            {
                string guid = _backgroundWorkers[i].Guid.ToString();

                System.Windows.Forms.Label labelThread = new System.Windows.Forms.Label()
                {
                    Text = string.Format("Thread: {0}", (i + 1).ToString()),
                    Name = "lblThread" + guid,
                    Dock = DockStyle.Top
                };
                tableLayoutPanel.Controls.Add(labelThread, 0, i);

                ProgressBar progressBar = new ProgressBar()
                {
                    Minimum = 0,
                    Maximum = 100,
                    Name = "pgb" + guid,
                    Dock = DockStyle.Top
                };
                tableLayoutPanel.Controls.Add(progressBar, 1, i);

                System.Windows.Forms.Label labelProcessed = new System.Windows.Forms.Label()
                {
                    Text = ProcessedLabel(0, 0, 0),
                    Name = "lblProcessed" + guid,
                    Dock = DockStyle.Top
                };
                tableLayoutPanel.Controls.Add(labelProcessed, 2, i);
            }
            tableLayoutPanel.ResumeLayout();
        }

        private void PopulateDistinctFileIds()
        {
            DataTable dataTable = _agileUtility.GetDistinctFileIds();
            _distinctFileIds = new List<String>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                _distinctFileIds.Add(dataTable.Rows[i]["FILE_ID"].ToString());
            }
        }

        private void AssignFileIdsToBackgroundWorkers()
        {
            int numFileIdsPerThread = _distinctFileIds.Count / _numThreads;
            int numRemainderFileIds = _distinctFileIds.Count % _numThreads;
            int threadFileIdIndex = 0;
            int numRecsForThisThread = 0;
            for (int i = 0; i < _backgroundWorkers.Length; i++)
            {
                numRecsForThisThread = numFileIdsPerThread;
                if (numRemainderFileIds > 0)
                {
                    numRecsForThisThread++;
                    numRemainderFileIds--;
                }
                _backgroundWorkers[i].FileIds = _distinctFileIds.GetRange(threadFileIdIndex, numRecsForThisThread);
                threadFileIdIndex = threadFileIdIndex + numRecsForThisThread;
            }
        }

        private void RunBackgroundWorkers()
        {
            for (int i = 0; i < _backgroundWorkers.Length; i++)
            {
                _backgroundWorkers[i].RunWorkerAsync();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CancelBackgroundWorkers();
        }

        private void CancelBackgroundWorkers()
        {
            for (int i = 0; i < _backgroundWorkers.Length; i++)
            {
                _backgroundWorkers[i].CancelAsync();
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MyBackgroundWorker bw = sender as MyBackgroundWorker;
            int numCurrentFileId = 0;

            for (int i = 0; i < bw.FileIds.Count; i++)
            {
                numCurrentFileId++;
                DataTable dt = _agileUtility.GetFileByFileId(bw.FileIds[i]);
                foreach (DataRow dr in dt.Rows)
                {
                    MigrateFile(dr);
                    if (bw.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                bw.ReportProgress(numCurrentFileId);
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MyBackgroundWorker bw = sender as MyBackgroundWorker;
            string guid = bw.Guid.ToString();
            ProgressBar pb;
            System.Windows.Forms.Label lbl;

            int processedFiles = e.ProgressPercentage;
            decimal percentageFilesProcessed = Math.Round(Decimal.Multiply(Decimal.Divide(processedFiles, bw.FileIds.Count), 100), 1);

            if (!string.IsNullOrEmpty(guid))
            {
                foreach (Control c in tableLayoutPanel.Controls)
                {
                    if (c.Name == ("pgb" + guid))
                    {
                        pb = (ProgressBar)c;
                        pb.Value = Math.Min(100, (int)percentageFilesProcessed);
                    }
                    else if (c.Name == ("lblProcessed" + guid))
                    {
                        lbl = (System.Windows.Forms.Label)c;
                        lbl.Text = ProcessedLabel(processedFiles, bw.FileIds.Count, percentageFilesProcessed);
                    }
                }
            }
        }

        private static string ProcessedLabel(int processedFiles, int totalFiles, decimal percentageFilesProcessed)
        {
            return string.Format("{0:#,0} of {1:#,0} ({2}%)", processedFiles, totalFiles, percentageFilesProcessed);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MyBackgroundWorker bw = (sender as MyBackgroundWorker);
            string guid = bw.Guid.ToString();

            if (e.Error != null)
            {
                bw.Log("Error: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                bw.Log("Work Canceled");
            }
            else
            {
                bw.Log("Work Completed");
                ProgressBar pb = (ProgressBar)tableLayoutPanel.Controls.Find(("pgb" + guid), true)[0];
                tableLayoutPanel.Controls.Remove(pb);
            }
        }

        private void MigrateFile(DataRow dr)
        {
            string localFilePath = dr["FILE_PATH"].ToString().Replace("//", "\\");

            string vaultFolder = Path.Combine(_settings.DesignsRootPath, dr["CLASS"].ToString(), dr["SUBCLASS"].ToString());

            Dictionary<string, string> fileProps = new Dictionary<string, string>();
            //TODO: get fileProps from Settings.xml

            string[] readACLGroups = dr["FACILITIES_EXTERNAL"].ToString().Split(';');
            string[] writeACLGroups = dr["FACILITIES_INTERNAL"].ToString().Split(';');
            
            //TODO: Not Ready For This Yet
            //UpdateFile(localFilePath, vaultFolder, fileProps, readACLGroups, writeACLGroups);
        }

        /// <summary>
        ///  This is an example of how to load file into Vault
        ///  It may require modifications and testing to ensure it's fully functional
        /// </summary>
        /// <param name="localFilePath">Path to the file on the local file system (FileStore)</param>
        /// <param name "vaultFolder">The Vault Folder to check into.  
        ///     "$/Vault/Designs/Folder/SubFolder
        ///     Build the vaultFolder from the Classification data (Class/SubClass)
        ///     //TODO: Set this via an input or config
        ///     string designsRootPath = "$/Vault/Designs/"; //All files will be added under this directory in Vault
        ///     string classPath String.Format("{0}/{1}", data.CLASS, data.SUBCLASS);
        /// </param>
        /// <param name="fileProps">Dictionary<string,string> of File Properties to be added/updated.
        ///     Key = Vault Property Display Name
        ///     Value = Property Value
        /// </param>
        /// <param name="readACLGroups">Array of ACL Group Names for Read Only access</param>
        /// <param name="writeACLGroups">Array of ACL Group Names for Read/Write access</param>
        private void UpdateFile(string localFilePath, string vaultFolder, Dictionary<string, string> fileProps, string[] readACLGroups, string[] writeACLGroups)
        {
            try
            {
                _vaultUtility.LoginToVault();


                //Step1
                //If the file being migrated has a previous version,
                //we need to look up that file so we can change its state to WIP, then check it out
                //If not previous version, skip to Step4

                //if we have captured the File MasterID from a previous version we can use that to find
                //the latest version when checking in a newer version

                long masterID = -1; //TODO: From previous file
                Autodesk.Connectivity.WebServices.File existingFile = null;

                FileInfo fi = new FileInfo(localFilePath);
                if (!fi.Exists)
                {
                    _logUtility.Log("File does not exist: " + localFilePath);
                    return;
                }

                //Search for file by name
                PropDef[] filePropDefs = _vaultUtility.WebServiceManager.PropertyService.GetPropertyDefinitionsByEntityClassId("FILE");
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
                    Autodesk.Connectivity.WebServices.File[] results = _vaultUtility.WebServiceManager.DocumentService.FindFilesBySearchConditions(new SrchCond[] { fileNameCond }, null, null, false, true, ref bookmark, out status);

                    if (results != null)
                        foundFiles.AddRange(results);
                    else
                        break;
                }

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
                    masterID = existingFile.MasterId;

                    //First we need to get a list of the Lifecycle States on this file
                    //We are using the same lifecycle definition for all files so this should be fairly straight forward
                    LfCycDef[] lcDefs = _vaultUtility.WebServiceManager.DocumentServiceExtensions.GetAllLifeCycleDefinitions();
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
                    IdPair[] wipFileLCStates = _vaultUtility.WebServiceManager.DocumentServiceExtensions.GetLifeCycleStateIdsByFileMasterIds(new long[] { masterID });
                    foreach (var state in wipFileLCStates)
                    {
                        LfCycState lfState = _vaultUtility.WebServiceManager.LifeCycleService.GetLifeCycleStatesByIds(new long[] { state.ValId }).First();
                        if (lfState.DispName == _settings.WipStateName)
                        {
                            _settings.WipStateID = lfState.Id;
                            break;
                        }
                    }

                    //Update the Files Lifecycle State
                    _vaultUtility.WebServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(new long[] { masterID }, new long[] { _settings.WipStateID }, "Data Migration");

                    //Get the latest version now that we have changed the state, as the state change may have created a new version
                    Autodesk.Connectivity.WebServices.File latestFileVersion =
                        _vaultUtility.WebServiceManager.DocumentService.FindLatestFilesByMasterIds(new long[] { masterID }).First();

                    ByteArray ticket = null;
                    //Check out the file, but don't worry about downloading it, we're going to just upload a new version on top of this version
                    _vaultUtility.WebServiceManager.DocumentService.CheckoutFile(
                        latestFileVersion.Id,
                        CheckoutFileOptions.Master,
                        null,
                        null,
                        "Data Migration - Updating File Revision",
                        out ticket);
                }



                //Step2
                long fldrID = -1;
                //TODO: Set this via an input or config
                //Find the Folder we are checking into, if it doesn't exist then we'll create it
                Folder vFolder = _vaultUtility.WebServiceManager.DocumentService.GetFolderByPath(vaultFolder);
                if (vFolder == null)
                {
                    Folder currentFolder = _vaultUtility.WebServiceManager.DocumentService.GetFolderRoot();
                    string currentPath = "";
                    string[] folders = vaultFolder.Split('/');

                    //Recursively build the Vault Folders
                    foreach (var fldr in folders)
                    {
                        currentPath = currentFolder.FullName + "/" + fldr;
                        Folder nextFolder = _vaultUtility.WebServiceManager.DocumentService.GetFolderByPath(currentPath);
                        //If the folder doesn't exist then create it
                        if (nextFolder == null)
                        {
                            nextFolder = _vaultUtility.WebServiceManager.DocumentService.AddFolder(fldr, currentFolder.Id, false);
                        }

                        currentFolder = nextFolder;
                    }

                    vFolder = currentFolder;
                }



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
                if (existingFile != null)
                {
                    //Step 3A: Check In the File
                    newFileRev = _vaultUtility.WebServiceManager.DocumentService.CheckinUploadedFile(
                        masterID,
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
                    newFileRev = _vaultUtility.WebServiceManager.DocumentService.AddUploadedFile(
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




                //Step4
                //Update the Vault File Properties
                List<PropInstParamArray> updateProps = new List<PropInstParamArray>();
                foreach (string propName in fileProps.Keys)
                {
                    PropDef nPropDef = filePropDefs.FirstOrDefault(n => n.DispName == propName);
                    if (nPropDef != null)
                    {
                        PropInstParam propInstParam = new PropInstParam();
                        propInstParam.PropDefId = nPropDef.Id;
                        propInstParam.Val = fileProps[propName];

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
                _vaultUtility.WebServiceManager.DocumentService.UpdateFileProperties(new long[] { masterID }, updateProps.ToArray());


                //Step5
                //Set the file security Groups (Access Control List)
                AccessPermis readAccessPermission = new AccessPermis();
                readAccessPermission.Id = 1;
                readAccessPermission.Val = true;

                AccessPermis writeAccessPermission = new AccessPermis();
                writeAccessPermission.Id = 2;
                writeAccessPermission.Val = true;

                AccessPermis deleteAccessPermission = new AccessPermis();
                deleteAccessPermission.Id = 3;
                deleteAccessPermission.Val = true;

                //Set the Read/Write ACL Groups
                foreach (var aclGroup in writeACLGroups)
                {
                    Group group = _vaultUtility.WebServiceManager.AdminService.GetGroupByName(aclGroup);
                    if (group != null)
                    {
                        ACE ace = new ACE();
                        ace.UserGrpId = group.Id;
                        ace.PermisArray = new AccessPermis[] { readAccessPermission, writeAccessPermission };
                        ACE[] aces = new ACE[1];

                        ACL myAcl = _vaultUtility.WebServiceManager.SecurityService.AddSystemACL(aces);

                        _vaultUtility.WebServiceManager.SecurityService.SetSystemACLs(new long[] { masterID }, myAcl.Id);

                    }
                    else
                    {
                        //TODO: Security Group not found so log the error in the database
                    }
                }

                //Set the Read Only ACL Groups
                foreach (var aclGroup in readACLGroups)
                {
                    Group group = _vaultUtility.WebServiceManager.AdminService.GetGroupByName(aclGroup);
                    if (group != null)
                    {
                        ACE ace = new ACE();
                        ace.UserGrpId = group.Id;

                        ace.PermisArray = new AccessPermis[] { readAccessPermission };

                        ACE[] aces = new ACE[1];
                        aces[0] = ace;

                        ACL myACL = _vaultUtility.WebServiceManager.SecurityService.AddSystemACL(aces);

                        _vaultUtility.WebServiceManager.SecurityService.SetSystemACLs(new long[] { masterID }, myACL.Id);
                    }
                    else
                    {
                        //TODO: Security Group not found so log the error in the database
                    }
                }


                //Step6
                //Set the state of the file to Released
                //Find the LfCycState that matches the To State Name and get its ID
                IdPair[] releasedRildLCStates = _vaultUtility.WebServiceManager.DocumentServiceExtensions.GetLifeCycleStateIdsByFileMasterIds(new long[] { masterID });
                foreach (var state in releasedRildLCStates)
                {
                    LfCycState lfState = _vaultUtility.WebServiceManager.LifeCycleService.GetLifeCycleStatesByIds(new long[] { state.ValId }).First();
                    if (lfState.DispName == _settings.ReleasedStateName)
                    {
                        _settings.ReleasedStateID = lfState.Id;
                        break;
                    }
                }

                //Update the Files Lifecycl State
                _vaultUtility.WebServiceManager.DocumentServiceExtensions.UpdateFileLifeCycleStates(new long[] { masterID }, new long[] { _settings.ReleasedStateID }, "Data Migration - Released new File Revision");

                //Get the latest version now that we have changed the state, as the state change may have created a new version
                Autodesk.Connectivity.WebServices.File updatedFileVersion = _vaultUtility.WebServiceManager.DocumentService.FindLatestFilesByMasterIds(new long[] { masterID }).First();


                //Step7
                //Write the new Vault File data back to the database
                long updatedFileMasterID = updatedFileVersion.MasterId;
                long updatedFileVersionID = updatedFileVersion.Id;
                string _status = "Write migration status back to database";
            }
            catch (Exception ex)
            {
                string str = "UpdateFile(string localFilePath, string vaultFolder, Dictionary<string, string> fileProps, string[] readACLGroups, string[] writeACLGroups)";
                _logUtility.Log(ex.Message);
            }
        }
        
        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewConfig();
        }

        private void ViewConfig()
        {
            SettingsEditor editor = new SettingsEditor(_settings);
            editor.ShowDialog();

            if (editor.IsDirty)
            {
                _settings = editor.Settings;
                _settings.Save();
                _agileUtility.Settings = _settings;
                _vaultUtility.Settings = _settings;
            }

            editor.Dispose();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Migration_FormClosing(object sender, FormClosingEventArgs e)
        {
            _logUtility.Dispose();            
        }
    }
}

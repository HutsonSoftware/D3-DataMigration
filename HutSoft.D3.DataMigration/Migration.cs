using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        private void RefreshAgile_Click(object sender, EventArgs e)
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

        private void Migrate_Click(object sender, EventArgs e)
        {
            try
            {
                if (FlightCheck())
                {
                    //TODO: visual cue of button pushed while migrating.  Tricky to know when threads are done though
                    GetThreadCount();
                    InitializeBackgroundWorkers();
                    SetupTableLayoutPanel();
                    PopulateDistinctFileIds();
                    AssignFileIdsToBackgroundWorkers();
                    RunBackgroundWorkers();
                }
            }
            catch (Exception ex)
            {
                _logUtility.Log("Exception in Migrate_Click: " + ex.Message);
            }
        }

        private bool FlightCheck()
        {
            bool passesFlightCheck = false;
            try
            {
                _vaultUtility.LoginToVault();
                passesFlightCheck = _vaultUtility.FlightCheck();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return passesFlightCheck;
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

                Label labelThread = new System.Windows.Forms.Label()
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

                Label labelProcessed = new System.Windows.Forms.Label()
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

        private void Cancel_Click(object sender, EventArgs e)
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
            Label lbl;

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
                        lbl = (Label)c;
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
                //Step1
                //If the file being migrated has a previous version,
                //we need to look up that file so we can change its state to WIP, then check it out
                //If not previous version, skip to Step4
                //if we have captured the File MasterID from a previous version we can use that to find
                //the latest version when checking in a newer version
                long masterID = -1;
                Autodesk.Connectivity.WebServices.File existingFile = _vaultUtility.GetExistingFile(localFilePath);
                if (existingFile != null)
                {
                    masterID = existingFile.MasterId;

                    //Step2
                    //Find the Folder we are checking into, if it doesn't exist then we'll create it
                    _vaultUtility.VerifyVaultFolderExists(vaultFolder);

                    //Step3
                    //Upload or checkin the file
                    long fldrID = -1;  //TODO: Set this via an input or config
                    _vaultUtility.CheckInFile(localFilePath, existingFile, fldrID);
                }

                //Step4
                //Update the Vault File Properties
                _vaultUtility.UpdateVaultFileProperties(fileProps, masterID);

                //Step5
                //Set the file security Groups (Access Control List)
                _vaultUtility.SetFileSecurityGroups(writeACLGroups, readACLGroups, masterID);

                //Step6
                //Set the state of the file to Released
                //Find the LfCycState that matches the To State Name and get its ID
                Autodesk.Connectivity.WebServices.File updatedFileVersion = _vaultUtility.UpdateLifeCycleStateInfo(masterID);

                //Step7
                //Write the new Vault File data back to the database
                _agileUtility.WriteBackVaultFileInfo(updatedFileVersion.MasterId, updatedFileVersion.Id);
            }
            catch (Exception ex)
            {
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

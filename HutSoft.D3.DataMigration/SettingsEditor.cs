using Autodesk.Connectivity.WebServicesTools;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HutSoft.D3.DataMigration
{
    internal partial class SettingsEditor : Form
    {
        private AgileUtility _agileUtility;
        private VaultUtility _vaultUtility;

        public Settings Settings { get; set; }

        public bool IsDirty { get; set; }

        public SettingsEditor(Settings settings)
        {
            Settings = settings;
            _agileUtility = new AgileUtility(Settings);
            _vaultUtility = new VaultUtility(Settings);
            InitializeComponent();
        }

        private void SettingsEditor_Load(object sender, EventArgs e)
        {
            AgileSQLiteConnectionString.Text = Settings.AgileSQLiteConnectionString;
            AgileOracleConnectionString.Text = Settings.AgileOracleConnectionString;
            VaultServer.Text = Settings.VaultServer;
            VaultInstance.Text = Settings.VaultInstance;
            VaultUserName.Text = Settings.VaultUserName;
            VaultPassword.Text = Settings.VaultPassword;
            LifeCycleDefName.Text = Settings.LifeCycleDefName;
            WipStateName.Text = Settings.WipStateName;
            WipStateID.Text = Settings.WipStateID.ToString();
            ReleasedStateName.Text = Settings.ReleasedStateName;
            ReleasedStateID.Text = Settings.ReleasedStateID.ToString();
            DesignsRootPath.Text = Settings.DesignsRootPath;
        }

        private void TestAgileSQLiteConnection_Click(object sender, EventArgs e)
        {
            TestAgileSQLiteConnection.Text = "Testing...";
            TestAgileSQLiteConnection.Enabled = false;
            TestAgileSQLiteConnection.Parent.Focus();
            
            try
            {
                _agileUtility.TestSQLiteConnection(AgileSQLiteConnectionString.Text);
                errorProvider1.SetError(TestAgileSQLiteConnection, "");
                AgileSQLiteConnectionString.BackColor = SystemColors.Window;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(TestAgileSQLiteConnection, "Connection Test Failed: " + ex.Message);
                AgileSQLiteConnectionString.BackColor = Color.LightPink;
            }
            TestAgileSQLiteConnection.Text = "Test Connection";
            TestAgileSQLiteConnection.Enabled = true;
            TestAgileSQLiteConnection.Focus();
        }

        private void TestAgileOracleConnection_Click(object sender, EventArgs e)
        {
            TestAgileOracleConnection.Text = "Testing...";
            TestAgileOracleConnection.Enabled = false;
            TestAgileOracleConnection.Parent.Focus();
            try
            {
                _agileUtility.TestOracleConnection(AgileOracleConnectionString.Text);
                errorProvider1.SetError(TestAgileOracleConnection, "");
                AgileOracleConnectionString.BackColor = SystemColors.Window;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(TestAgileOracleConnection, "Connection Test Failed: " + ex.Message);
                AgileOracleConnectionString.BackColor = Color.LightPink;
            }
            TestAgileOracleConnection.Text = "Test Connection";
            TestAgileOracleConnection.Enabled = true;
            TestAgileOracleConnection.Focus();
        }

        private void TestVaultConnection_Click(object sender, EventArgs e)
        {
            TestVaultConnection.Text = "Testing...";
            TestVaultConnection.Enabled = false;
            TestVaultConnection.Parent.Focus();
            try
            {
                WebServiceManager svcMgr = _vaultUtility.LoginToVault(VaultServer.Text, VaultInstance.Text, VaultUserName.Text, VaultPassword.Text);
                errorProvider1.SetError(TestVaultConnection, "");
                VaultServer.BackColor = SystemColors.Window;
                VaultInstance.BackColor = SystemColors.Window;
                VaultUserName.BackColor = SystemColors.Window;
                VaultPassword.BackColor = SystemColors.Window;
            }
            catch (Exception ex)
            {
                errorProvider1.SetError(TestVaultConnection, "Connection Test Failed: " + ex.Message);
                VaultServer.BackColor = Color.LightPink;
                VaultInstance.BackColor = Color.LightPink;
                VaultUserName.BackColor = Color.LightPink;
                VaultPassword.BackColor = Color.LightPink;
            }
            TestVaultConnection.Text = "Test Connection";
            TestVaultConnection.Enabled = true;
            TestVaultConnection.Focus();
        }

        private void TexBoxIsNumeric_Validated(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            errorProvider1.SetError(textBox, "");
        }

        private void TextBoxIsNumeric_Validating(object sender, CancelEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!long.TryParse(textBox.Text, out long value))
            {
                errorProvider1.SetError(textBox, "Entry must be a numeric value");
                textBox.SelectAll();
                e.Cancel = true;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void SaveSettings()
        {
            if (Settings.AgileSQLiteConnectionString != AgileSQLiteConnectionString.Text)
            {
                Settings.AgileSQLiteConnectionString = AgileSQLiteConnectionString.Text;
                IsDirty = true;
            }
            if (Settings.AgileOracleConnectionString != AgileOracleConnectionString.Text)
            {
                Settings.AgileOracleConnectionString = AgileOracleConnectionString.Text;
                IsDirty = true;
            }
            if (Settings.VaultServer != VaultServer.Text)
            {
                Settings.VaultServer = VaultServer.Text;
                IsDirty = true;
            }
            if (Settings.VaultInstance != VaultInstance.Text)
            {
                Settings.VaultInstance = VaultInstance.Text;
                IsDirty = true;
            }
            if (Settings.VaultUserName != VaultUserName.Text)
            {
                Settings.VaultUserName = VaultUserName.Text;
                IsDirty = true;
            }
            if (Settings.VaultPassword != VaultPassword.Text)
            {
                Settings.VaultPassword = VaultPassword.Text;
                IsDirty = true;
            }
            if (Settings.LifeCycleDefName != LifeCycleDefName.Text)
            {
                Settings.LifeCycleDefName = LifeCycleDefName.Text;
                IsDirty = true;
            }
            if (Settings.WipStateName != WipStateName.Text)
            {
                Settings.WipStateName = WipStateName.Text;
                IsDirty = true;
            }
            long.TryParse(WipStateID.Text, out long wipStateID);
            if (Settings.WipStateID != wipStateID)
            {
                Settings.WipStateID = wipStateID;
                IsDirty = true;
            }
            if (Settings.ReleasedStateName != ReleasedStateName.Text)
            {
                Settings.ReleasedStateName = ReleasedStateName.Text;
                IsDirty = true;
            }
            long.TryParse(WipStateID.Text, out long releasedStateID);
            if (Settings.ReleasedStateID != releasedStateID)
            {
                Settings.ReleasedStateID = releasedStateID;
                IsDirty = true;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

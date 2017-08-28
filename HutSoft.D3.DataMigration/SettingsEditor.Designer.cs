namespace HutSoft.D3.DataMigration
{
    partial class SettingsEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.DesignsRootPath = new System.Windows.Forms.TextBox();
            this.TestAgileOracleConnection = new System.Windows.Forms.Button();
            this.TestAgileSQLiteConnection = new System.Windows.Forms.Button();
            this.AgileOracleConnectionString = new System.Windows.Forms.TextBox();
            this.AgileSQLiteConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.VerifyVariables = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.LifeCycleDefName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ReleasedStateID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ReleasedStateName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.WipStateID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.WipStateName = new System.Windows.Forms.TextBox();
            this.TestVaultConnection = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.VaultPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.VaultUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.VaultInstance = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.VaultServer = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.DesignsRootPath);
            this.groupBox1.Controls.Add(this.TestAgileOracleConnection);
            this.groupBox1.Controls.Add(this.TestAgileSQLiteConnection);
            this.groupBox1.Controls.Add(this.AgileOracleConnectionString);
            this.groupBox1.Controls.Add(this.AgileSQLiteConnectionString);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(571, 174);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Agile Connection Strings";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 132);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "DesignsRootPath:";
            // 
            // DesignsRootPath
            // 
            this.DesignsRootPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DesignsRootPath.Location = new System.Drawing.Point(120, 129);
            this.DesignsRootPath.Name = "DesignsRootPath";
            this.DesignsRootPath.Size = new System.Drawing.Size(418, 20);
            this.DesignsRootPath.TabIndex = 23;
            // 
            // TestAgileOracleConnection
            // 
            this.TestAgileOracleConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TestAgileOracleConnection.Location = new System.Drawing.Point(415, 100);
            this.TestAgileOracleConnection.Name = "TestAgileOracleConnection";
            this.TestAgileOracleConnection.Size = new System.Drawing.Size(123, 23);
            this.TestAgileOracleConnection.TabIndex = 13;
            this.TestAgileOracleConnection.Text = "Test Connection";
            this.TestAgileOracleConnection.UseVisualStyleBackColor = true;
            this.TestAgileOracleConnection.Click += new System.EventHandler(this.TestAgileOracleConnection_Click);
            // 
            // TestAgileSQLiteConnection
            // 
            this.TestAgileSQLiteConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TestAgileSQLiteConnection.Location = new System.Drawing.Point(415, 45);
            this.TestAgileSQLiteConnection.Name = "TestAgileSQLiteConnection";
            this.TestAgileSQLiteConnection.Size = new System.Drawing.Size(123, 23);
            this.TestAgileSQLiteConnection.TabIndex = 12;
            this.TestAgileSQLiteConnection.Text = "Test Connection";
            this.TestAgileSQLiteConnection.UseVisualStyleBackColor = true;
            this.TestAgileSQLiteConnection.Click += new System.EventHandler(this.TestAgileSQLiteConnection_Click);
            // 
            // AgileOracleConnectionString
            // 
            this.AgileOracleConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgileOracleConnectionString.Location = new System.Drawing.Point(75, 74);
            this.AgileOracleConnectionString.Name = "AgileOracleConnectionString";
            this.AgileOracleConnectionString.Size = new System.Drawing.Size(463, 20);
            this.AgileOracleConnectionString.TabIndex = 3;
            // 
            // AgileSQLiteConnectionString
            // 
            this.AgileSQLiteConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgileSQLiteConnectionString.Location = new System.Drawing.Point(75, 19);
            this.AgileSQLiteConnectionString.Name = "AgileSQLiteConnectionString";
            this.AgileSQLiteConnectionString.Size = new System.Drawing.Size(463, 20);
            this.AgileSQLiteConnectionString.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Oracle:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "SQLite:";
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(394, 517);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 0;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(475, 517);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 1;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.VerifyVariables);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.LifeCycleDefName);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.ReleasedStateID);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.ReleasedStateName);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.WipStateID);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.WipStateName);
            this.groupBox2.Controls.Add(this.TestVaultConnection);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.VaultPassword);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.VaultUserName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.VaultInstance);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.VaultServer);
            this.groupBox2.Location = new System.Drawing.Point(12, 192);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(571, 319);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vault";
            // 
            // VerifyVariables
            // 
            this.VerifyVariables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VerifyVariables.Location = new System.Drawing.Point(415, 285);
            this.VerifyVariables.Name = "VerifyVariables";
            this.VerifyVariables.Size = new System.Drawing.Size(123, 23);
            this.VerifyVariables.TabIndex = 34;
            this.VerifyVariables.Text = "Verify Variables";
            this.VerifyVariables.UseVisualStyleBackColor = true;
            this.VerifyVariables.Click += new System.EventHandler(this.VerifyVariables_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 158);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 13);
            this.label11.TabIndex = 33;
            this.label11.Text = "LifeCycleDefName:";
            // 
            // LifeCycleDefName
            // 
            this.LifeCycleDefName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LifeCycleDefName.Location = new System.Drawing.Point(120, 155);
            this.LifeCycleDefName.Name = "LifeCycleDefName";
            this.LifeCycleDefName.Size = new System.Drawing.Size(418, 20);
            this.LifeCycleDefName.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 262);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(91, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "ReleasedStateID:";
            // 
            // ReleasedStateID
            // 
            this.ReleasedStateID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReleasedStateID.Location = new System.Drawing.Point(120, 259);
            this.ReleasedStateID.Name = "ReleasedStateID";
            this.ReleasedStateID.Size = new System.Drawing.Size(418, 20);
            this.ReleasedStateID.TabIndex = 30;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "ReleasedStateName:";
            // 
            // ReleasedStateName
            // 
            this.ReleasedStateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReleasedStateName.Location = new System.Drawing.Point(120, 233);
            this.ReleasedStateName.Name = "ReleasedStateName";
            this.ReleasedStateName.Size = new System.Drawing.Size(418, 20);
            this.ReleasedStateName.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 210);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "WipStateID:";
            // 
            // WipStateID
            // 
            this.WipStateID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WipStateID.Location = new System.Drawing.Point(120, 207);
            this.WipStateID.Name = "WipStateID";
            this.WipStateID.Size = new System.Drawing.Size(418, 20);
            this.WipStateID.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "WipStateName:";
            // 
            // WipStateName
            // 
            this.WipStateName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WipStateName.Location = new System.Drawing.Point(120, 181);
            this.WipStateName.Name = "WipStateName";
            this.WipStateName.Size = new System.Drawing.Size(418, 20);
            this.WipStateName.TabIndex = 24;
            // 
            // TestVaultConnection
            // 
            this.TestVaultConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TestVaultConnection.Location = new System.Drawing.Point(415, 123);
            this.TestVaultConnection.Name = "TestVaultConnection";
            this.TestVaultConnection.Size = new System.Drawing.Size(123, 23);
            this.TestVaultConnection.TabIndex = 11;
            this.TestVaultConnection.Text = "Test Connection";
            this.TestVaultConnection.UseVisualStyleBackColor = true;
            this.TestVaultConnection.Click += new System.EventHandler(this.TestVaultConnection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Password:";
            // 
            // VaultPassword
            // 
            this.VaultPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultPassword.Location = new System.Drawing.Point(75, 97);
            this.VaultPassword.Name = "VaultPassword";
            this.VaultPassword.Size = new System.Drawing.Size(463, 20);
            this.VaultPassword.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "User Name:";
            // 
            // VaultUserName
            // 
            this.VaultUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultUserName.Location = new System.Drawing.Point(75, 71);
            this.VaultUserName.Name = "VaultUserName";
            this.VaultUserName.Size = new System.Drawing.Size(463, 20);
            this.VaultUserName.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Instance:";
            // 
            // VaultInstance
            // 
            this.VaultInstance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultInstance.Location = new System.Drawing.Point(75, 45);
            this.VaultInstance.Name = "VaultInstance";
            this.VaultInstance.Size = new System.Drawing.Size(463, 20);
            this.VaultInstance.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server:";
            // 
            // VaultServer
            // 
            this.VaultServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VaultServer.Location = new System.Drawing.Point(75, 19);
            this.VaultServer.Name = "VaultServer";
            this.VaultServer.Size = new System.Drawing.Size(463, 20);
            this.VaultServer.TabIndex = 3;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // SettingsEditor
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(595, 549);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.groupBox1);
            this.Name = "SettingsEditor";
            this.Text = "Settings Editor";
            this.Load += new System.EventHandler(this.SettingsEditor_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button TestAgileOracleConnection;
        private System.Windows.Forms.Button TestAgileSQLiteConnection;
        private System.Windows.Forms.TextBox AgileOracleConnectionString;
        private System.Windows.Forms.TextBox AgileSQLiteConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button TestVaultConnection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox VaultPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox VaultUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox VaultInstance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox VaultServer;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox DesignsRootPath;
        private System.Windows.Forms.Button VerifyVariables;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox LifeCycleDefName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ReleasedStateID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ReleasedStateName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox WipStateID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox WipStateName;
    }
}
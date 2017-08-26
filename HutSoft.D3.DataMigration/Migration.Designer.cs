namespace hutsoft.d3.data_migration
{
    partial class Migration
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
            this.nudThreads = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.Migrate = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.gbxAgile = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvAgileStatuses = new System.Windows.Forms.DataGridView();
            this.dgvAgileTop100Sample = new System.Windows.Forms.DataGridView();
            this.RefreshAgile = new System.Windows.Forms.Button();
            this.rbnOracle = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbnSQLite = new System.Windows.Forms.RadioButton();
            this.gbxResults = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogText = new System.Windows.Forms.TextBox();
            this.LogFile = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).BeginInit();
            this.gbxAgile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgileStatuses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgileTop100Sample)).BeginInit();
            this.gbxResults.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudThreads
            // 
            this.nudThreads.Location = new System.Drawing.Point(74, 254);
            this.nudThreads.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThreads.Name = "nudThreads";
            this.nudThreads.Size = new System.Drawing.Size(60, 20);
            this.nudThreads.TabIndex = 0;
            this.nudThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 256);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Threads:";
            // 
            // Migrate
            // 
            this.Migrate.Location = new System.Drawing.Point(159, 251);
            this.Migrate.Name = "Migrate";
            this.Migrate.Size = new System.Drawing.Size(75, 23);
            this.Migrate.TabIndex = 2;
            this.Migrate.Text = "Migrate";
            this.Migrate.UseVisualStyleBackColor = true;
            this.Migrate.Click += new System.EventHandler(this.btnMigrate_Click);
            // 
            // Cancel
            // 
            this.Cancel.Location = new System.Drawing.Point(240, 251);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbxAgile
            // 
            this.gbxAgile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxAgile.Controls.Add(this.label2);
            this.gbxAgile.Controls.Add(this.dgvAgileStatuses);
            this.gbxAgile.Controls.Add(this.dgvAgileTop100Sample);
            this.gbxAgile.Controls.Add(this.RefreshAgile);
            this.gbxAgile.Controls.Add(this.rbnOracle);
            this.gbxAgile.Controls.Add(this.label3);
            this.gbxAgile.Controls.Add(this.rbnSQLite);
            this.gbxAgile.Location = new System.Drawing.Point(12, 27);
            this.gbxAgile.Name = "gbxAgile";
            this.gbxAgile.Size = new System.Drawing.Size(1004, 216);
            this.gbxAgile.TabIndex = 5;
            this.gbxAgile.TabStop = false;
            this.gbxAgile.Text = "Agile";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Statuses:";
            // 
            // dgvStatuses
            // 
            this.dgvAgileStatuses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgileStatuses.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAgileStatuses.Location = new System.Drawing.Point(10, 90);
            this.dgvAgileStatuses.Name = "dgvStatuses";
            this.dgvAgileStatuses.RowHeadersVisible = false;
            this.dgvAgileStatuses.ShowEditingIcon = false;
            this.dgvAgileStatuses.Size = new System.Drawing.Size(196, 116);
            this.dgvAgileStatuses.TabIndex = 5;
            // 
            // dgvAgile
            // 
            this.dgvAgileTop100Sample.AllowUserToAddRows = false;
            this.dgvAgileTop100Sample.AllowUserToDeleteRows = false;
            this.dgvAgileTop100Sample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAgileTop100Sample.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAgileTop100Sample.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvAgileTop100Sample.Location = new System.Drawing.Point(221, 16);
            this.dgvAgileTop100Sample.Name = "dgvAgile";
            this.dgvAgileTop100Sample.RowHeadersVisible = false;
            this.dgvAgileTop100Sample.ShowEditingIcon = false;
            this.dgvAgileTop100Sample.Size = new System.Drawing.Size(777, 190);
            this.dgvAgileTop100Sample.TabIndex = 4;
            // 
            // RefreshAgile
            // 
            this.RefreshAgile.Location = new System.Drawing.Point(10, 39);
            this.RefreshAgile.Name = "RefreshAgile";
            this.RefreshAgile.Size = new System.Drawing.Size(196, 23);
            this.RefreshAgile.TabIndex = 3;
            this.RefreshAgile.Text = "Refresh";
            this.RefreshAgile.UseVisualStyleBackColor = true;
            this.RefreshAgile.Click += new System.EventHandler(this.btnRefreshAgile_Click);
            // 
            // rbnOracle
            // 
            this.rbnOracle.AutoSize = true;
            this.rbnOracle.Location = new System.Drawing.Point(119, 16);
            this.rbnOracle.Name = "rbnOracle";
            this.rbnOracle.Size = new System.Drawing.Size(56, 17);
            this.rbnOracle.TabIndex = 2;
            this.rbnOracle.Text = "Oracle";
            this.rbnOracle.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Source:";
            // 
            // rbnSQLite
            // 
            this.rbnSQLite.AutoSize = true;
            this.rbnSQLite.Checked = true;
            this.rbnSQLite.Location = new System.Drawing.Point(56, 16);
            this.rbnSQLite.Name = "rbnSQLite";
            this.rbnSQLite.Size = new System.Drawing.Size(57, 17);
            this.rbnSQLite.TabIndex = 0;
            this.rbnSQLite.TabStop = true;
            this.rbnSQLite.Text = "SQLite";
            this.rbnSQLite.UseVisualStyleBackColor = true;
            // 
            // gbxResults
            // 
            this.gbxResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxResults.Controls.Add(this.tableLayoutPanel);
            this.gbxResults.Location = new System.Drawing.Point(12, 282);
            this.gbxResults.Name = "gbxResults";
            this.gbxResults.Size = new System.Drawing.Size(547, 390);
            this.gbxResults.TabIndex = 6;
            this.gbxResults.TabStop = false;
            this.gbxResults.Text = "Processing Results";
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoScroll = true;
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(541, 371);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1028, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.configToolStripMenuItem.Text = "Config";
            this.configToolStripMenuItem.Click += new System.EventHandler(this.configToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // LogText
            // 
            this.LogText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogText.Location = new System.Drawing.Point(583, 282);
            this.LogText.Multiline = true;
            this.LogText.Name = "LogText";
            this.LogText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogText.Size = new System.Drawing.Size(432, 389);
            this.LogText.TabIndex = 9;
            // 
            // LogFile
            // 
            this.LogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LogFile.AutoSize = true;
            this.LogFile.Location = new System.Drawing.Point(580, 256);
            this.LogFile.Name = "LogFile";
            this.LogFile.Size = new System.Drawing.Size(47, 13);
            this.LogFile.TabIndex = 10;
            this.LogFile.Text = "Log File:";
            // 
            // Migration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 684);
            this.Controls.Add(this.LogFile);
            this.Controls.Add(this.LogText);
            this.Controls.Add(this.gbxResults);
            this.Controls.Add(this.gbxAgile);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Migrate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudThreads);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Migration";
            this.Text = "Data Migration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Migration_FormClosing);
            this.Load += new System.EventHandler(this.Migration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudThreads)).EndInit();
            this.gbxAgile.ResumeLayout(false);
            this.gbxAgile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgileStatuses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgileTop100Sample)).EndInit();
            this.gbxResults.ResumeLayout(false);
            this.gbxResults.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudThreads;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Migrate;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.GroupBox gbxAgile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvAgileStatuses;
        private System.Windows.Forms.DataGridView dgvAgileTop100Sample;
        private System.Windows.Forms.Button RefreshAgile;
        private System.Windows.Forms.RadioButton rbnOracle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbnSQLite;
        private System.Windows.Forms.GroupBox gbxResults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox LogText;
        private System.Windows.Forms.Label LogFile;
    }
}
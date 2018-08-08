namespace CE.DbConnectionHelper
{
    partial class frmDatabaseActivities
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseActivities));
            this.trvDatabases = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefreshLists = new System.Windows.Forms.ToolStripButton();
            this.tsiDatabase = new System.Windows.Forms.ToolStripDropDownButton();
            this.addUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateAdminLoginTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConnectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backupDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDatabaseFromBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateAllAppLoginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAdminPWToHotelGolf82ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCloseForm = new System.Windows.Forms.ToolStripButton();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblSelectedDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLocation = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlLists = new System.Windows.Forms.Panel();
            this.pnlDatabaseList = new System.Windows.Forms.Panel();
            this.lblDatabaseList = new System.Windows.Forms.Label();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlUpgradesList = new System.Windows.Forms.Panel();
            this.trvUpgrades = new System.Windows.Forms.TreeView();
            this.lblUpgrades = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upgradeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.pnlLists.SuspendLayout();
            this.pnlDatabaseList.SuspendLayout();
            this.pnlUpgradesList.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvDatabases
            // 
            this.trvDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvDatabases.Location = new System.Drawing.Point(0, 28);
            this.trvDatabases.Margin = new System.Windows.Forms.Padding(4);
            this.trvDatabases.Name = "trvDatabases";
            this.trvDatabases.Size = new System.Drawing.Size(251, 334);
            this.trvDatabases.TabIndex = 0;
            this.trvDatabases.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trvDatabases_BeforeSelect);
            this.trvDatabases.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvDatabases_AfterSelect);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(257, 49);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 478);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshLists,
            this.tsiDatabase,
            this.btnCloseForm});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1067, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRefreshLists
            // 
            this.btnRefreshLists.Image = global::CE.DbConnectionHelper.Properties.Resources._112_RefreshArrow_Green_16x16_72;
            this.btnRefreshLists.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshLists.Name = "btnRefreshLists";
            this.btnRefreshLists.Size = new System.Drawing.Size(89, 22);
            this.btnRefreshLists.Text = "Refresh lists";
            this.btnRefreshLists.Click += new System.EventHandler(this.btnRefreshLists_Click);
            // 
            // tsiDatabase
            // 
            this.tsiDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserToolStripMenuItem,
            this.updateAdminLoginTimeToolStripMenuItem,
            this.updateVersionToolStripMenuItem,
            this.testConnectionToolStripMenuItem,
            this.backupDatabaseToolStripMenuItem,
            this.copyDatabaseToolStripMenuItem,
            this.restoreDatabaseFromBackupToolStripMenuItem,
            this.updateAllAppLoginsToolStripMenuItem,
            this.setAdminPWToHotelGolf82ToolStripMenuItem,
            this.upgradeDatabaseToolStripMenuItem});
            this.tsiDatabase.Image = global::CE.DbConnectionHelper.Properties.Resources.Database;
            this.tsiDatabase.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsiDatabase.Name = "tsiDatabase";
            this.tsiDatabase.Size = new System.Drawing.Size(93, 22);
            this.tsiDatabase.Text = "Database...";
            // 
            // addUserToolStripMenuItem
            // 
            this.addUserToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources._157_GetPermission_16x16_72;
            this.addUserToolStripMenuItem.Name = "addUserToolStripMenuItem";
            this.addUserToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.addUserToolStripMenuItem.Text = "Add user login";
            this.addUserToolStripMenuItem.Click += new System.EventHandler(this.addUserToolStripMenuItem_Click);
            // 
            // updateAdminLoginTimeToolStripMenuItem
            // 
            this.updateAdminLoginTimeToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.Calendar_scheduleHS;
            this.updateAdminLoginTimeToolStripMenuItem.Name = "updateAdminLoginTimeToolStripMenuItem";
            this.updateAdminLoginTimeToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.updateAdminLoginTimeToolStripMenuItem.Text = "Update admin lockout";
            this.updateAdminLoginTimeToolStripMenuItem.Click += new System.EventHandler(this.updateAdminLoginTimeToolStripMenuItem_Click);
            // 
            // updateVersionToolStripMenuItem
            // 
            this.updateVersionToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.bulleted_list_options;
            this.updateVersionToolStripMenuItem.Name = "updateVersionToolStripMenuItem";
            this.updateVersionToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.updateVersionToolStripMenuItem.Text = "Update details";
            this.updateVersionToolStripMenuItem.Click += new System.EventHandler(this.updateVersionToolStripMenuItem_Click);
            // 
            // testConnectionToolStripMenuItem
            // 
            this.testConnectionToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.PlayHS;
            this.testConnectionToolStripMenuItem.Name = "testConnectionToolStripMenuItem";
            this.testConnectionToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.testConnectionToolStripMenuItem.Text = "Test connection";
            this.testConnectionToolStripMenuItem.Click += new System.EventHandler(this.testConnectionToolStripMenuItem_Click);
            // 
            // backupDatabaseToolStripMenuItem
            // 
            this.backupDatabaseToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources._1474_Tape_48x48;
            this.backupDatabaseToolStripMenuItem.Name = "backupDatabaseToolStripMenuItem";
            this.backupDatabaseToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.backupDatabaseToolStripMenuItem.Text = "Backup database";
            this.backupDatabaseToolStripMenuItem.Click += new System.EventHandler(this.backupDatabaseToolStripMenuItem_Click);
            // 
            // copyDatabaseToolStripMenuItem
            // 
            this.copyDatabaseToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.clipboard;
            this.copyDatabaseToolStripMenuItem.Name = "copyDatabaseToolStripMenuItem";
            this.copyDatabaseToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.copyDatabaseToolStripMenuItem.Text = "Copy database";
            this.copyDatabaseToolStripMenuItem.Click += new System.EventHandler(this.copyDatabaseToolStripMenuItem_Click);
            // 
            // restoreDatabaseFromBackupToolStripMenuItem
            // 
            this.restoreDatabaseFromBackupToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.Paste;
            this.restoreDatabaseFromBackupToolStripMenuItem.Name = "restoreDatabaseFromBackupToolStripMenuItem";
            this.restoreDatabaseFromBackupToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.restoreDatabaseFromBackupToolStripMenuItem.Text = "Restore database from backup";
            this.restoreDatabaseFromBackupToolStripMenuItem.Click += new System.EventHandler(this.restoreDatabaseFromBackupToolStripMenuItem_Click);
            // 
            // updateAllAppLoginsToolStripMenuItem
            // 
            this.updateAllAppLoginsToolStripMenuItem.Image = global::CE.DbConnectionHelper.Properties.Resources.SqlScript;
            this.updateAllAppLoginsToolStripMenuItem.Name = "updateAllAppLoginsToolStripMenuItem";
            this.updateAllAppLoginsToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.updateAllAppLoginsToolStripMenuItem.Text = "Update all app logins";
            this.updateAllAppLoginsToolStripMenuItem.Click += new System.EventHandler(this.updateAllAppLoginsToolStripMenuItem_Click);
            // 
            // setAdminPWToHotelGolf82ToolStripMenuItem
            // 
            this.setAdminPWToHotelGolf82ToolStripMenuItem.Name = "setAdminPWToHotelGolf82ToolStripMenuItem";
            this.setAdminPWToHotelGolf82ToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.setAdminPWToHotelGolf82ToolStripMenuItem.Text = "Set Admin PW to hotelGolf82";
            this.setAdminPWToHotelGolf82ToolStripMenuItem.Click += new System.EventHandler(this.setAdminPWToHotelGolf82ToolStripMenuItem_Click);
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnCloseForm.Image = global::CE.DbConnectionHelper.Properties.Resources._305_Close_16x16_72;
            this.btnCloseForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(87, 22);
            this.btnCloseForm.Text = "Close Form";
            this.btnCloseForm.ToolTipText = "Close";
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(261, 49);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(806, 478);
            this.pnlBody.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblSelectedDatabase,
            this.lblVersion,
            this.lblLocation});
            this.statusStrip1.Location = new System.Drawing.Point(0, 527);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1067, 27);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblSelectedDatabase
            // 
            this.lblSelectedDatabase.AutoSize = false;
            this.lblSelectedDatabase.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblSelectedDatabase.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.lblSelectedDatabase.Name = "lblSelectedDatabase";
            this.lblSelectedDatabase.Size = new System.Drawing.Size(250, 22);
            this.lblSelectedDatabase.Text = "-";
            this.lblSelectedDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = false;
            this.lblVersion.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblVersion.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(150, 22);
            this.lblVersion.Text = "-";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = false;
            this.lblLocation.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblLocation.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(150, 22);
            this.lblLocation.Text = "-";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLists
            // 
            this.pnlLists.Controls.Add(this.pnlDatabaseList);
            this.pnlLists.Controls.Add(this.splitter2);
            this.pnlLists.Controls.Add(this.pnlUpgradesList);
            this.pnlLists.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLists.Location = new System.Drawing.Point(0, 49);
            this.pnlLists.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLists.Name = "pnlLists";
            this.pnlLists.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlLists.Size = new System.Drawing.Size(257, 478);
            this.pnlLists.TabIndex = 5;
            // 
            // pnlDatabaseList
            // 
            this.pnlDatabaseList.Controls.Add(this.trvDatabases);
            this.pnlDatabaseList.Controls.Add(this.lblDatabaseList);
            this.pnlDatabaseList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDatabaseList.Location = new System.Drawing.Point(3, 2);
            this.pnlDatabaseList.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDatabaseList.Name = "pnlDatabaseList";
            this.pnlDatabaseList.Size = new System.Drawing.Size(251, 362);
            this.pnlDatabaseList.TabIndex = 1;
            // 
            // lblDatabaseList
            // 
            this.lblDatabaseList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDatabaseList.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDatabaseList.Location = new System.Drawing.Point(0, 0);
            this.lblDatabaseList.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDatabaseList.Name = "lblDatabaseList";
            this.lblDatabaseList.Size = new System.Drawing.Size(251, 28);
            this.lblDatabaseList.TabIndex = 0;
            this.lblDatabaseList.Text = "Databases";
            this.lblDatabaseList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(3, 364);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(251, 4);
            this.splitter2.TabIndex = 3;
            this.splitter2.TabStop = false;
            // 
            // pnlUpgradesList
            // 
            this.pnlUpgradesList.Controls.Add(this.trvUpgrades);
            this.pnlUpgradesList.Controls.Add(this.lblUpgrades);
            this.pnlUpgradesList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUpgradesList.Location = new System.Drawing.Point(3, 368);
            this.pnlUpgradesList.Margin = new System.Windows.Forms.Padding(4);
            this.pnlUpgradesList.Name = "pnlUpgradesList";
            this.pnlUpgradesList.Size = new System.Drawing.Size(251, 108);
            this.pnlUpgradesList.TabIndex = 2;
            // 
            // trvUpgrades
            // 
            this.trvUpgrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvUpgrades.Location = new System.Drawing.Point(0, 28);
            this.trvUpgrades.Margin = new System.Windows.Forms.Padding(4);
            this.trvUpgrades.Name = "trvUpgrades";
            this.trvUpgrades.Size = new System.Drawing.Size(251, 80);
            this.trvUpgrades.TabIndex = 0;
            // 
            // lblUpgrades
            // 
            this.lblUpgrades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUpgrades.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUpgrades.Location = new System.Drawing.Point(0, 0);
            this.lblUpgrades.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpgrades.Name = "lblUpgrades";
            this.lblUpgrades.Size = new System.Drawing.Size(251, 28);
            this.lblUpgrades.TabIndex = 0;
            this.lblUpgrades.Text = "Upgrade Scripts";
            this.lblUpgrades.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(89, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // upgradeDatabaseToolStripMenuItem
            // 
            this.upgradeDatabaseToolStripMenuItem.Name = "upgradeDatabaseToolStripMenuItem";
            this.upgradeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.upgradeDatabaseToolStripMenuItem.Text = "Upgrade database";
            this.upgradeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.upgradeDatabaseToolStripMenuItem_Click);
            // 
            // frmDatabaseActivities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlLists);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDatabaseActivities";
            this.Text = "Database Activities";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDatabaseActivities_FormClosing);
            this.Load += new System.EventHandler(this.frmDatabaseActivities_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlLists.ResumeLayout(false);
            this.pnlDatabaseList.ResumeLayout(false);
            this.pnlUpgradesList.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trvDatabases;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.ToolStripButton btnRefreshLists;
        private System.Windows.Forms.ToolStripDropDownButton tsiDatabase;
        private System.Windows.Forms.ToolStripMenuItem addUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateAdminLoginTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateVersionToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblSelectedDatabase;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        private System.Windows.Forms.ToolStripStatusLabel lblLocation;
        private System.Windows.Forms.Panel pnlLists;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel pnlUpgradesList;
        private System.Windows.Forms.TreeView trvUpgrades;
        private System.Windows.Forms.Label lblUpgrades;
        private System.Windows.Forms.Panel pnlDatabaseList;
        private System.Windows.Forms.Label lblDatabaseList;
        private System.Windows.Forms.ToolStripMenuItem testConnectionToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backupDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreDatabaseFromBackupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateAllAppLoginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnCloseForm;
        private System.Windows.Forms.ToolStripMenuItem setAdminPWToHotelGolf82ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upgradeDatabaseToolStripMenuItem;
    }
}
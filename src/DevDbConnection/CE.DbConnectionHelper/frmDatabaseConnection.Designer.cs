namespace CE.DbConnectionHelper
{
    partial class frmDatabaseConnection
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
            System.Windows.Forms.Label machineLabel;
            System.Windows.Forms.Label serverLabel;
            System.Windows.Forms.Label databaseLabel;
            System.Windows.Forms.Label integratedSecurityLabel;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDatabaseConnection));
            this.machineTextBox = new System.Windows.Forms.TextBox();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.databaseTextBox = new System.Windows.Forms.TextBox();
            this.integratedSecurityCheckBox = new System.Windows.Forms.CheckBox();
            this.userIdLabel = new System.Windows.Forms.Label();
            this.userIdTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.dbConnectionViewModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            machineLabel = new System.Windows.Forms.Label();
            serverLabel = new System.Windows.Forms.Label();
            databaseLabel = new System.Windows.Forms.Label();
            integratedSecurityLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.pnlButtons.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbConnectionViewModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // machineLabel
            // 
            machineLabel.AutoSize = true;
            machineLabel.Location = new System.Drawing.Point(17, 15);
            machineLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            machineLabel.Name = "machineLabel";
            machineLabel.Size = new System.Drawing.Size(68, 18);
            machineLabel.TabIndex = 1;
            machineLabel.Text = "Machine:";
            // 
            // serverLabel
            // 
            serverLabel.AutoSize = true;
            serverLabel.Location = new System.Drawing.Point(31, 51);
            serverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            serverLabel.Name = "serverLabel";
            serverLabel.Size = new System.Drawing.Size(55, 18);
            serverLabel.TabIndex = 3;
            serverLabel.Text = "Server:";
            // 
            // databaseLabel
            // 
            databaseLabel.AutoSize = true;
            databaseLabel.Location = new System.Drawing.Point(9, 87);
            databaseLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            databaseLabel.Name = "databaseLabel";
            databaseLabel.Size = new System.Drawing.Size(75, 18);
            databaseLabel.TabIndex = 5;
            databaseLabel.Text = "Database:";
            // 
            // integratedSecurityLabel
            // 
            integratedSecurityLabel.AutoSize = true;
            integratedSecurityLabel.Location = new System.Drawing.Point(263, 14);
            integratedSecurityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            integratedSecurityLabel.Name = "integratedSecurityLabel";
            integratedSecurityLabel.Size = new System.Drawing.Size(133, 18);
            integratedSecurityLabel.TabIndex = 7;
            integratedSecurityLabel.Text = "Integrated Security:";
            // 
            // machineTextBox
            // 
            this.machineTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "Machine", true));
            this.machineTextBox.Location = new System.Drawing.Point(87, 11);
            this.machineTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.machineTextBox.Name = "machineTextBox";
            this.machineTextBox.Size = new System.Drawing.Size(171, 24);
            this.machineTextBox.TabIndex = 2;
            // 
            // serverTextBox
            // 
            this.serverTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "Server", true));
            this.serverTextBox.Location = new System.Drawing.Point(87, 47);
            this.serverTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(171, 24);
            this.serverTextBox.TabIndex = 4;
            // 
            // databaseTextBox
            // 
            this.databaseTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "Database", true));
            this.databaseTextBox.Location = new System.Drawing.Point(87, 83);
            this.databaseTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.databaseTextBox.Name = "databaseTextBox";
            this.databaseTextBox.Size = new System.Drawing.Size(171, 24);
            this.databaseTextBox.TabIndex = 6;
            // 
            // integratedSecurityCheckBox
            // 
            this.integratedSecurityCheckBox.Checked = true;
            this.integratedSecurityCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.integratedSecurityCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.dbConnectionViewModelBindingSource, "IntegratedSecurity", true));
            this.integratedSecurityCheckBox.Location = new System.Drawing.Point(410, 7);
            this.integratedSecurityCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.integratedSecurityCheckBox.Name = "integratedSecurityCheckBox";
            this.integratedSecurityCheckBox.Size = new System.Drawing.Size(202, 33);
            this.integratedSecurityCheckBox.TabIndex = 8;
            this.integratedSecurityCheckBox.UseVisualStyleBackColor = true;
            this.integratedSecurityCheckBox.CheckedChanged += new System.EventHandler(this.integratedSecurityCheckBox_CheckedChanged);
            // 
            // userIdLabel
            // 
            this.userIdLabel.AutoSize = true;
            this.userIdLabel.Location = new System.Drawing.Point(345, 52);
            this.userIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.userIdLabel.Name = "userIdLabel";
            this.userIdLabel.Size = new System.Drawing.Size(59, 18);
            this.userIdLabel.TabIndex = 9;
            this.userIdLabel.Text = "User Id:";
            // 
            // userIdTextBox
            // 
            this.userIdTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "UserId", true));
            this.userIdTextBox.Location = new System.Drawing.Point(410, 48);
            this.userIdTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.userIdTextBox.Name = "userIdTextBox";
            this.userIdTextBox.Size = new System.Drawing.Size(203, 24);
            this.userIdTextBox.TabIndex = 10;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(327, 88);
            this.passwordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(79, 18);
            this.passwordLabel.TabIndex = 11;
            this.passwordLabel.Text = "Password:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "Password", true));
            this.passwordTextBox.Location = new System.Drawing.Point(410, 84);
            this.passwordTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(203, 24);
            this.passwordTextBox.TabIndex = 12;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnTest);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 236);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(625, 59);
            this.pnlButtons.TabIndex = 13;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(128, 5);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(92, 42);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(521, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 42);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancelClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnEditSave_Click);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Controls.Add(this.txtNotes);
            this.pnlDetails.Controls.Add(label1);
            this.pnlDetails.Controls.Add(this.passwordTextBox);
            this.pnlDetails.Controls.Add(this.machineTextBox);
            this.pnlDetails.Controls.Add(this.passwordLabel);
            this.pnlDetails.Controls.Add(machineLabel);
            this.pnlDetails.Controls.Add(this.serverTextBox);
            this.pnlDetails.Controls.Add(this.userIdLabel);
            this.pnlDetails.Controls.Add(serverLabel);
            this.pnlDetails.Controls.Add(this.userIdTextBox);
            this.pnlDetails.Controls.Add(this.databaseTextBox);
            this.pnlDetails.Controls.Add(integratedSecurityLabel);
            this.pnlDetails.Controls.Add(databaseLabel);
            this.pnlDetails.Controls.Add(this.integratedSecurityCheckBox);
            this.pnlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetails.Location = new System.Drawing.Point(0, 0);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(625, 236);
            this.pnlDetails.TabIndex = 14;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 125);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 18);
            label1.TabIndex = 13;
            label1.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.dbConnectionViewModelBindingSource, "Notes", true));
            this.txtNotes.Location = new System.Drawing.Point(87, 122);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNotes.Size = new System.Drawing.Size(526, 107);
            this.txtNotes.TabIndex = 14;
            // 
            // dbConnectionViewModelBindingSource
            // 
            this.dbConnectionViewModelBindingSource.DataSource = typeof(CE.DbConnectionHelper.ViewModels.DbConnectionViewModel);
            // 
            // frmDatabaseConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 295);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDatabaseConnection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Database Connection";
            this.Load += new System.EventHandler(this.frmDatabaseConnection_Load);
            this.pnlButtons.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbConnectionViewModelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource dbConnectionViewModelBindingSource;
        private System.Windows.Forms.TextBox machineTextBox;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.CheckBox integratedSecurityCheckBox;
        private System.Windows.Forms.TextBox userIdTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label userIdLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtNotes;
    }
}
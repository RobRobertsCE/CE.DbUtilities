using CE.DbConnect.Models;
using CE.DbConnectionHelper.Controllers;
using CE.DbConnectionHelper.Dialogs;
using CE.DbConnectionHelper.Extensions;
using CE.DbConnectionHelper.Models;
using CE.DbConnectionHelper.Services;
using CE.DbConnectionHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CE.DbConnectionHelper
{
    public partial class frmDatabaseActivities : Form
    {
        #region fields
        private IList<DbUpgradeScriptViewModel> _upgradeScripts;

        private TreeNode _selectedDatabaseNode = null;

        private bool _userRequestedClose = false;
        #endregion

        #region properties
        internal DbConnectionController Controller { get; set; }

        internal DbUpgradeViewModel UpgradeViewModel { get; set; }

        private Font _unselected;
        internal Font UnselectedDatabaseNodeFont
        {
            get
            {
                if (_unselected == null)
                    _unselected = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                return _unselected;
            }
        }

        private Font _selected;
        internal Font SelectedDatabaseNodeFont
        {
            get
            {
                if (_selected == null)
                    _selected = new Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                return _selected;
            }
        }

        internal DatabaseInstance SelectedDatabase
        {
            get
            {
                if (_selectedDatabaseNode == null)
                    return null;
                else
                    return (DatabaseInstance)_selectedDatabaseNode.Tag;
            }
        }
        #endregion

        #region ctor
        public frmDatabaseActivities()
        {
            InitializeComponent();

            Controller = new DbConnectionController();

            UpgradeViewModel = new DbUpgradeViewModel();
        }
        #endregion

        #region private
        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(this, ex.Message);
        }

        private void frmDatabaseActivities_Load(object sender, EventArgs e)
        {
            PopulateDatabaseList();

            PopulateUpgradesList();
        }

        private void PopulateDatabaseList()
        {
            try
            {
                trvDatabases.SuspendLayout();

                trvDatabases.Nodes.Clear();

                var dbRootNode = trvDatabases.Nodes.Add("Databases");

                foreach (var connection in Controller.Model.Connections.GroupBy(c => c.Machine))
                {
                    var machineNode = new TreeNode(connection.Key);

                    foreach (var serverItem in connection.GroupBy(s => s.Server))
                    {
                        var serverNode = new TreeNode(serverItem.Key);

                        foreach (var databaseItem in serverItem.OrderBy(c => c.Database))
                        {
                            var databaseNode = new TreeNode(databaseItem.Database)
                            {
                                Tag = databaseItem.DbModel
                            };
                            databaseNode.Nodes.Add($"Version {databaseItem.Version}");
                            databaseNode.Nodes.Add($"Location {databaseItem.Location}");

                            if (databaseItem.IsEncrypted)
                                databaseNode.Nodes.Add("*Encrypted*");

                            if (databaseItem.IntegratedSecurity)
                                databaseNode.Nodes.Add("*Integrated Security*");
                            else
                            {
                                databaseNode.Nodes.Add($"UserId {databaseItem.UserId}");
                                databaseNode.Nodes.Add($"Password {databaseItem.Password}");
                            }

                            serverNode.Nodes.Add(databaseNode);
                        }

                        machineNode.Nodes.Add(serverNode);
                        serverNode.Expand();
                    }

                    dbRootNode.Nodes.Add(machineNode);
                    machineNode.Expand();
                    dbRootNode.Expand();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                trvDatabases.ResumeLayout(true);
            }
        }

        private void PopulateUpgradesList()
        {
            try
            {
                trvUpgrades.SuspendLayout();

                trvUpgrades.Nodes.Clear();

                _upgradeScripts = UpgradeViewModel.GetUpgradeScripts();

                var scriptRootNode = trvUpgrades.Nodes.Add("Upgrade Scripts");

                foreach (var majorUpgradeScript in _upgradeScripts.GroupBy(s => s.Version.Major))
                {
                    var majorVersionNode = new TreeNode(majorUpgradeScript.Key.ToString());

                    foreach (var minorUpgradeScript in majorUpgradeScript.GroupBy(s => s.Version.Minor))
                    {
                        var minorVersionNode = new TreeNode($"{majorUpgradeScript.Key.ToString()}.{minorUpgradeScript.Key.ToString()}");

                        foreach (var dbVersionScript in minorUpgradeScript.GroupBy(s => s.Version))
                        {
                            var dbVersionNode = new TreeNode(dbVersionScript.Key.ToString());

                            foreach (var sqlScript in dbVersionScript.OrderBy(s => s.Version))
                            {
                                var sqlScriptNode = new TreeNode(sqlScript.Name)
                                {
                                    Tag = sqlScript
                                };
                                dbVersionNode.Nodes.Add(sqlScriptNode);
                            }
                            minorVersionNode.Nodes.Add(dbVersionNode);
                        }
                        majorVersionNode.Nodes.Add(minorVersionNode);
                    }
                    scriptRootNode.Nodes.Add(majorVersionNode);
                    scriptRootNode.Expand();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
            finally
            {
                trvUpgrades.ResumeLayout(true);
            }
        }

        private void trvDatabases_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;
            if (node.Tag != null)
            {
                if ((node.Tag is DatabaseInstance))
                {
                    if (_selectedDatabaseNode != null)
                    {
                        _selectedDatabaseNode.NodeFont = UnselectedDatabaseNodeFont;
                        _selectedDatabaseNode.BackColor = Color.White;
                    }
                    e.Node.NodeFont = SelectedDatabaseNodeFont;
                    e.Node.BackColor = Color.Yellow;
                    e.Node.EnsureVisible();
                    e.Node.Text = e.Node.Text;
                }
            }
        }

        private void trvDatabases_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = trvDatabases.SelectedNode;

            if (node.Tag != null)
            {
                if (node.Tag is DatabaseInstance)
                {
                    _selectedDatabaseNode = node;
                    DatabaseSelectionChanged((DatabaseInstance)node.Tag);
                }
            }
        }

        private void DatabaseSelectionChanged(DatabaseInstance database)
        {
            tsiDatabase.Enabled = (database != null);

            if (database != null)
            {
                lblSelectedDatabase.Text = $"{database?.Name} on {database?.ServerInstance?.MachineInstance?.Name}\\{database?.ServerInstance?.Name}";
                lblVersion.Text = database?.Version?.ToString();
                lblLocation.Text = database?.Location?.ToString();
            }
            else
            {
                lblSelectedDatabase.Text = "No database selected";
                lblVersion.Text = "-";
                lblLocation.Text = "-";
            }
        }

        private void btnRefreshLists_Click(object sender, EventArgs e)
        {
            PopulateDatabaseList();

            PopulateUpgradesList();
        }

        private void frmDatabaseActivities_FormClosing(object sender, FormClosingEventArgs e)
        {
            HideForm();
            e.Cancel = true;
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideForm();
        }

        private void HideForm()
        {
            this.Hide();
        }
        #endregion

        #region database activities
        private async void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await AddUser();
        }

        private async Task<bool> AddUser()
        {
            var result = false;

            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Add your user as an employee to database {SelectedDatabase.Name}?", "Add user as employee", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;

                var group = await GetSecurityGroupAsync();

                if (group == null)
                    return false;

                result = await Controller.AddUserAsync(SelectedDatabase, group.Id);

                if (result)
                {
                    MessageBox.Show(this, $"User added to {group.Name} group");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async void setAdminPWToHotelGolf82ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await SetAdminPWTohotelGolf82();
        }

        private async Task<bool> SetAdminPWTohotelGolf82()
        {
            var result = false;

            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Set Admin password to hotelGolf82 in database {SelectedDatabase.Name}?", "Add user as employee", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;


                result = await Controller.SetAdminLoginTohotelGolf82Async(SelectedDatabase);

                if (result)
                {
                    MessageBox.Show(this, $"Password reset");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async Task<SecurityGroupModel> GetSecurityGroupAsync()
        {
            SecurityGroupModel model = null;

            try
            {
                var securityGroups = await Controller.GetSecurityGroupsAsync(SelectedDatabase);

                var viewModel = new SecurityGroupSelectionViewModel(securityGroups);

                var securityGroupsDialog = new SecurityGroupSelectionView(viewModel);

                var securityGroupsResult = securityGroupsDialog.ShowDialog(this);

                if (securityGroupsResult == DialogResult.OK)
                {
                    model = securityGroupsDialog.ViewModel.SelectedItem;

                    Console.WriteLine($"Selected Group {model.Name}");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return model;
        }

        private async void updateAdminLoginTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateAdminLogin();
        }

        private async Task<bool> UpdateAdminLogin()
        {
            var result = false;

            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Update admin lockout date for database {SelectedDatabase.Name}?", "Update admin lockout date", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;

                var newTimestamp = await Controller.UpdateAdminLoginAsync(SelectedDatabase);

                MessageBox.Show(this, $"Admin account unlocked until {newTimestamp.Value}");

                result = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async void updateVersionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await UpdateVersion();
        }

        private async Task<bool> UpdateVersion()
        {
            var result = false;

            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Update details for database {SelectedDatabase.Name}?", "Update details", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;

                result = await Controller.UpdateModelDetailsAsync(SelectedDatabase);

                if (result)
                {
                    MessageBox.Show(this, "Details updated");
                }
                else
                {
                    MessageBox.Show(this, "***** Detail update FAILED *****");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async void testConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await TestConnection();
        }

        private async Task<bool> TestConnection()
        {
            var result = false;

            try
            {
                result = await Controller.TestConnectionAsync(SelectedDatabase);

                if (result)
                {
                    MessageBox.Show(this, "Connection test passed");
                }
                else
                {
                    MessageBox.Show(this, "***** Connection test FAILED *****");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async void backupDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await BackupDatabaseAsync();
        }

        private async Task<bool> BackupDatabaseAsync()
        {
            var result = false;

            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Backup database {SelectedDatabase.Name}?", "Backup database", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;

                var backupFileFullPath = await Controller.BackupDatabaseAsync(SelectedDatabase);

                result = (!String.IsNullOrEmpty(backupFileFullPath));

                if (result)
                {
                    MessageBox.Show(this, "Database backup complete");
                }
                else
                {
                    MessageBox.Show(this, "***** Database backup FAILED *****");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return result;
        }

        private async void copyDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await CopyDatabaseWithNameAsync();
        }

        private async Task<bool> CopyDatabaseWithNameAsync()
        {
            try
            {
                var activityConfirmation = MessageBox.Show(this, $"Copy database {SelectedDatabase.Name}?", "Copy database", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (activityConfirmation != DialogResult.OK)
                    return false;

                var dialog = new InputDialog();

                var result = dialog.ShowDialog(this, "Enter the name for the new database", "New Database Name", $"{SelectedDatabase.Name}Copy");
                if (result != DialogResult.OK)
                {
                    return false;
                }

                var newDatabaseName = dialog.Response;

                var copiedDatabaseName = await Controller.CopyDatabaseAsync(SelectedDatabase, newDatabaseName);

                if (String.IsNullOrEmpty(copiedDatabaseName))
                {
                    MessageBox.Show(this, "***** Database copy FAILED *****");
                    return false;
                }

                var addNewConnectionResponse = MessageBox.Show(this, "Add as a new connection?", "Add new connection?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (addNewConnectionResponse == DialogResult.Cancel)
                    return true;
                else if (addNewConnectionResponse == DialogResult.Yes)
                {
                    // add the new copy to the list.
                    var newConnection = SelectedDatabase.Copy(copiedDatabaseName).ToViewModel();

                    Controller.Model.Connections.Add(newConnection);
                    Controller.Model.CurrentConnection = newConnection;

                    Controller.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return false;
        }

        private async void restoreDatabaseFromBackupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await RestoreDatabaseFromBackupAsync();
        }

        private async Task<string> RestoreDatabaseFromBackupAsync()
        {
            try
            {
                string newDatabaseName = string.Empty;

                var newDatabaseResponse = MessageBox.Show(this, $"Restore to a new database using credentials from {SelectedDatabase.Name}?", "Restore from backup", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (newDatabaseResponse == DialogResult.Cancel)
                    return String.Empty;
                else if (newDatabaseResponse == DialogResult.Yes)
                {
                    var dialog = new InputDialog();

                    var result = dialog.ShowDialog(this, "Enter the name for the new database", "New Database Name", $"{SelectedDatabase.Name}Copy");
                    if (result != DialogResult.OK)
                    {
                        return String.Empty;
                    }

                    newDatabaseName = dialog.Response;
                }

                var backupSelectionDialog = new OpenFileDialog()
                {
                    Filter = "Backups (*.bak)|*.bak|All Files (*.*)|*.*",
                    FilterIndex = 0,
                    InitialDirectory = CEDatabaseService.BackupDirectory
                };

                var backupSelectionResponse = backupSelectionDialog.ShowDialog(this);
                if (backupSelectionResponse != DialogResult.OK)
                {
                    return String.Empty;
                }
                var backupFileFullPath = backupSelectionDialog.FileName;

                var restoredDatabaseName = await Controller.RestoreDatabaseAsync(SelectedDatabase, backupFileFullPath, newDatabaseName);

                if (String.IsNullOrEmpty(restoredDatabaseName))
                {
                    MessageBox.Show(this, "***** Database copy FAILED *****");
                    return String.Empty;
                }
                else
                {
                    MessageBox.Show(this, $"Database restored to {newDatabaseName}");
                }

                var addNewConnectionResponse = MessageBox.Show(this, "Add as a new connection?", "Add new connection?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (newDatabaseResponse == DialogResult.Cancel)
                    return newDatabaseName;
                else if (newDatabaseResponse == DialogResult.Yes)
                {
                    // add the new copy to the list.
                    var newConnection = SelectedDatabase.Copy(restoredDatabaseName).ToViewModel();

                    Controller.Model.Connections.Add(newConnection);
                    Controller.Model.CurrentConnection = newConnection;

                    Controller.SaveChanges();

                    return newDatabaseName;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return String.Empty;
        }

        private void updateAllAppLoginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Sync all employee logins for all databases. (Add card? PIN?)
            UpdateAllLogins();
        }

        private void UpdateAllLogins()
        {

        }
        #endregion

        private void upgradeDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpgradeDatabase();
        }

        private void UpgradeDatabase()
        {
            try
            {
                var upgradeDialog = new UpgradeDatabaseView() { Database = SelectedDatabase };

                upgradeDialog.ShowDialog(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
    }
}
/*
 Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports System.Security
Imports System.Security.Cryptography
Imports System.Windows.Forms
Imports AdvantageShared.Security

Imports AdvCommon.AdvCommonCtrls
Imports AdvCommon.My.Resources

Public Class Authentication

    Public Const VendorAdminEmpNo As Integer = 3
    Public Const MaxFailedLoginAttempts As Integer = 5
    Public Const MinPasswordLength As Integer = 7
    Public Const PasswordHistoryCount As Integer = 4

    Public Shared ReadOnly LockoutResetTime As TimeSpan = TimeSpan.FromMinutes(30)
    Public Shared ReadOnly FailedLoginResetTime As TimeSpan = TimeSpan.FromMinutes(30)
    Public Shared ReadOnly MaxPasswordAge As TimeSpan = TimeSpan.FromDays(90)

#Region "Pin Handling"

    Public Shared Function VerifyPin(empNo As Integer, pin As String) As Boolean
        If String.IsNullOrEmpty(pin) Then
            Throw New ArgumentNullException("pin")
        End If

        Try
            Dim hashString As String
            Dim saltString As String

            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Return False
                End If

                Using cmd As New SqlCommand("SELECT PinHash, PinSalt FROM Employees WHERE EmpNo = @EmpNo", cn)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                    Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If Not dr.Read() Then
                            Return False
                        End If

                        hashString = dr.GetString(0)
                        saltString = dr.GetString(1)
                    End Using
                End Using
            End Using

            Return VerifyPassphrase(saltString, hashString, pin)
        Catch ex As Exception
            Logger.LogException(ex, "Error Verifying Pin")
            Return False
        End Try
    End Function

    Public Shared Sub UpdatePin(empNo As Integer, pin As SecureString, mustReset As Boolean)
        If pin Is Nothing Then
            Throw New ArgumentNullException("pin")
        End If

        Using zeroableString As New ZeroableString(pin)
            UpdatePin(empNo, zeroableString.ExtractString(), mustReset)
        End Using
    End Sub

    Public Shared Sub UpdatePin(empNo As Integer, pin As String, mustReset As Boolean)
        If String.IsNullOrEmpty(pin) Then
            Throw New ArgumentNullException("pin")
        End If

        Using cn As New ConnectionInfo()
            If Not cn.Open() Then
                Throw New ApplicationException("Database Connection Error")
            End If

            cn.BeginTransaction()
            UpdatePin(cn, empNo, pin, mustReset)
            cn.Commit()
        End Using
    End Sub

    Public Shared Sub UpdatePin(cn As ConnectionInfo, empNo As Integer, pin As SecureString, mustReset As Boolean)
        If pin Is Nothing Then
            Throw New ArgumentNullException("pin")
        End If

        Using zeroableString As New ZeroableString(pin)
            UpdatePin(cn, empNo, zeroableString.ExtractString(), mustReset)
        End Using
    End Sub

    Public Shared Sub UpdatePin(cn As ConnectionInfo, empNo As Integer, pin As String, mustReset As Boolean)
        If cn Is Nothing Then
            Throw New ArgumentNullException("cn")
        End If
        If String.IsNullOrEmpty(pin) Then
            Throw New ArgumentNullException("pin")
        End If

        Try
            Dim salt As Byte() = Array.CreateInstance(GetType(Byte), 20)
            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider()
            rng.GetBytes(salt)

            Dim pinBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(pin)
            Dim pinPlusSalt As Byte() = Array.CreateInstance(GetType(Byte), salt.Length + pinBytes.Length)
            Array.Copy(salt, pinPlusSalt, salt.Length)
            Array.Copy(pinBytes, 0, pinPlusSalt, salt.Length, pinBytes.Length)

            Dim sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider()
            Using cmd As New SqlCommand("UPDATE Employees SET PinHash = @PinHash, PinSalt = @PinSalt, MustChangePin = @MustChangePin WHERE EmpNo = @EmpNo", cn, cn.Trans)
                cmd.Parameters.Add("@PinHash", SqlDbType.Char, 28).Value = Convert.ToBase64String(sha1.ComputeHash(pinPlusSalt))
                cmd.Parameters.Add("@PinSalt", SqlDbType.Char, 28).Value = Convert.ToBase64String(salt)
                cmd.Parameters.Add("@MustChangePin", SqlDbType.Bit).Value = mustReset
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw Logger.CreateException(ex, Function() Authentication_UpdatePin_Error_Setting_Pin)
        End Try
    End Sub

    Public Shared Function CheckMustChangePin(empNo As Integer) As Boolean
        If empNo = VendorAdminEmpNo Then
            Return False
        End If

        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Return False
                End If

                Using cmd As New SqlCommand("SELECT MustChangePin FROM Employees WHERE EmpNo = @EmpNo", cn)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                    Dim res As Object = cmd.ExecuteScalar()
                    If res Is Nothing OrElse IsDBNull(res) Then
                        Return False
                    Else
                        Return CBool(res)
                    End If
                End Using
            End Using
        Catch ex As Exception
            Logger.LogException(ex, "Error Checking For Required Pin Change")
            Return False
        End Try
    End Function

#End Region

#Region "Password Handling"

    Public Shared Function VerifyPassword(empNo As Integer, password As String) As Boolean
        If String.IsNullOrEmpty(password) Then
            Throw New ArgumentNullException("password")
        End If

        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Return False
                End If

                Return VerifyPassword(cn, empNo, password)
            End Using
        Catch ex As Exception
            Logger.LogException(ex, "Error Verifying Password")
            Return False
        End Try
    End Function

    Public Shared Function VerifyPassword(cn As ConnectionInfo, empNo As Integer, password As String) As Boolean
        If String.IsNullOrEmpty(password) Then
            Throw New ArgumentNullException("password")
        End If

        Try
            Dim hashString As String
            Dim saltString As String

            Using cmd As New SqlCommand("SELECT PasswordHash, PasswordSalt FROM Employees WHERE EmpNo = @EmpNo", cn)
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not dr.Read() Then
                        Return False
                    End If

                    If dr.IsDBNull(0) OrElse dr.IsDBNull(1) Then
                        Return False
                    End If

                    hashString = dr.GetString(0)
                    saltString = dr.GetString(1)
                End Using
            End Using

            Return VerifyPassphrase(saltString, hashString, password)
        Catch ex As Exception
            Logger.LogException(ex, "Error Verifying Password")
            Return False
        End Try
    End Function

    Public Shared Sub UpdatePassword(empNo As Integer, password As SecureString, mustReset As Boolean)
        If password Is Nothing Then
            Throw New ArgumentNullException("password")
        End If

        Using zeroableString As New ZeroableString(password)
            UpdatePassword(empNo, zeroableString.ExtractString(), mustReset)
        End Using
    End Sub

    Public Shared Sub UpdatePassword(empNo As Integer, password As String, mustReset As Boolean)
        If String.IsNullOrEmpty(password) Then
            Throw New ArgumentNullException("password")
        End If

        Using cn As New ConnectionInfo()
            If Not cn.Open() Then
                Throw New ApplicationException("Database Connection Error")
            End If

            cn.BeginTransaction()
            UpdatePassword(cn, empNo, password, mustReset)
            cn.Commit()
        End Using
    End Sub

    Public Shared Sub UpdatePassword(cn As ConnectionInfo, empNo As Integer, password As SecureString, mustReset As Boolean)
        If password Is Nothing Then
            Throw New ArgumentNullException("password")
        End If

        Using zeroableString As New ZeroableString(password)
            UpdatePassword(cn, empNo, zeroableString.ExtractString(), mustReset)
        End Using
    End Sub

    Public Shared Sub UpdatePassword(cn As ConnectionInfo, empNo As Integer, password As String, mustReset As Boolean)
        If cn Is Nothing Then
            Throw New ArgumentNullException("cn")
        End If
        If String.IsNullOrEmpty(password) Then
            Throw New ArgumentNullException("password")
        End If

        Try
            Dim salt As Byte() = Array.CreateInstance(GetType(Byte), 20)
            Dim rng As New System.Security.Cryptography.RNGCryptoServiceProvider()
            rng.GetBytes(salt)

            Dim paswordBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)
            Dim passwordPlusSalt As Byte() = Array.CreateInstance(GetType(Byte), salt.Length + paswordBytes.Length)
            Array.Copy(salt, passwordPlusSalt, salt.Length)
            Array.Copy(paswordBytes, 0, passwordPlusSalt, salt.Length, paswordBytes.Length)

            Using cmd As New SqlCommand("UPDATE EmployeePasswordHistory SET SequenceNo = SequenceNo + 1 WHERE EmpNo = @EmpNo", cn, cn.Trans)
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                cmd.ExecuteNonQuery()
            End Using

            ' Keep one less than password history count, because the new password counts and is stored in Employees table
            Using cmd As New SqlCommand("DELETE FROM EmployeePasswordHistory WHERE EmpNo = @EmpNo AND SequenceNo > @PasswordHistoryCount - 1", cn, cn.Trans)
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                cmd.Parameters.Add("@PasswordHistoryCount", SqlDbType.Int).Value = PasswordHistoryCount
                cmd.ExecuteNonQuery()
            End Using

            Using cmd As New SqlCommand("INSERT INTO EmployeePasswordHistory (EmpNo, SequenceNo, PasswordHash, PasswordSalt) " & _
                                        "SELECT EmpNo, 1, PasswordHash, PasswordSalt " &
                                        "FROM Employees WHERE EmpNo = @EmpNo " & _
                                        "AND PasswordHash IS NOT NULL AND PasswordSalt IS NOT NULL", cn, cn.Trans)
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                cmd.ExecuteNonQuery()
            End Using

            Dim sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider()
            Using cmd As New SqlCommand("UPDATE Employees SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, MustChangePassword = @MustChangePassword, LastPasswordChange = GETDATE() " & _
                                        "WHERE EmpNo = @EmpNo", cn, cn.Trans)
                cmd.Parameters.Add("@PasswordHash", SqlDbType.Char, 28).Value = Convert.ToBase64String(sha1.ComputeHash(passwordPlusSalt))
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.Char, 28).Value = Convert.ToBase64String(salt)
                cmd.Parameters.Add("@MustChangePassword", SqlDbType.Bit).Value = mustReset
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw Logger.CreateException(ex, Function() Authentication_UpdatePassword_Error_Setting_Password)
        End Try
    End Sub

    Public Shared Function ValidateNewPassword(empNo As Integer, password As String) As Boolean
        If String.IsNullOrEmpty(password) Then
            Throw New ArgumentNullException("password")
        End If

        Using cn As New ConnectionInfo()
            If Not cn.Open() Then
                Throw New DatabaseConnectionException()
            End If

            Return ValidateNewPassword(cn, empNo, password)
        End Using
    End Function

    Public Shared Function ValidateNewPassword(cn As ConnectionInfo, empNo As Integer, password As String) As Boolean
        ' First check against current password
        If VerifyPassword(cn, empNo, password) Then
            Return False
        End If

        ' Then old passwords
        Using cmd As New SqlCommand("SELECT PasswordHash, PasswordSalt FROM EmployeePasswordHistory WHERE EmpNo = @EmpNo", cn)
            cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

            Using dr As SqlDataReader = cmd.ExecuteReader()
                While dr.Read()
                    If VerifyPassphrase(dr.GetString(1), dr.GetString(0), password) Then
                        Return False
                    End If
                End While
            End Using
        End Using

        Return True
    End Function

    Public Shared Function CheckMustChangePassword(empNo As Integer) As Boolean
        If empNo = VendorAdminEmpNo Then
            Return False
        End If

        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Return False
                End If

                Using cmd As New SqlCommand("SELECT MustChangePassword, LastPasswordChange, GETDATE() FROM Employees WHERE EmpNo = @EmpNo", cn)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                    Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If Not dr.Read() Then
                            Return False
                        End If

                        If dr.GetBoolean(0) Then
                            ' MustChangePassword is true
                            Return True
                        End If

                        If dr.IsDBNull(1) Then
                            ' No last change date
                            Return False
                        End If

                        Return (dr.GetDateTime(2) - dr.GetDateTime(1)) > MaxPasswordAge
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Logger.LogException(ex, "Error Checking For Required Password Change")
            Return False
        End Try
    End Function

    Public Shared Function CheckLockout(empNo As Integer) As Boolean
        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Return False
                End If

                Using cmd As New SqlCommand("SELECT LockedOutUntil, GETDATE() FROM Employees WHERE EmpNo = @EmpNo", cn)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                    Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If Not dr.Read() OrElse dr.IsDBNull(0) Then
                            Return False
                        End If

                        ' if LockedOutUntil is greater than current time, return True to indicate the account is still locked out
                        Return dr.GetDateTime(0) > dr.GetDateTime(1)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Logger.LogException(ex, "Error Checking For Lockout")
            Return False
        End Try
    End Function

    Public Shared Sub ClearLockout(empNo As Integer)
        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Throw New ApplicationException("Database Connection Error")
                End If

                ClearLockout(cn, empNo)
            End Using
        Catch ex As Exception
            Throw Logger.CreateException(ex, Function() Authentication_Error_Clearing_Lockout)
        End Try
    End Sub

    Public Shared Sub ClearLockout(cn As ConnectionInfo, empNo As Integer)
        Try
            Using cmd As New SqlCommand("UPDATE Employees SET LockedOutUntil = NULL, FailedLoginAttempts = 0 WHERE EmpNo = @EmpNo", cn, cn.Trans)
                cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw Logger.CreateException(ex, Function() Authentication_Error_Clearing_Lockout)
        End Try
    End Sub

    Public Shared Function IncrementFailedLoginCount(empNo As Integer) As Boolean
        Try
            Dim lockedOut As Boolean = False

            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Throw New ApplicationException("Database Connection Error")
                End If

                cn.BeginTransaction(IsolationLevel.RepeatableRead)

                Dim lastFailed As DateTime?
                Dim failCount As Integer
                Dim currentTime As DateTime

                Using cmd As New SqlCommand("SELECT LastFailedLogin, FailedLoginAttempts, GETDATE() FROM Employees WHERE EmpNo = @EmpNo", cn, cn.Trans)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo

                    Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                        If Not dr.Read() Then
                            ' Employee not found
                            Exit Function
                        End If

                        lastFailed = NullHelper.DBNullToNullable(Of DateTime)(dr, 0)
                        failCount = dr.GetInt32(1)
                        currentTime = dr.GetDateTime(2)
                    End Using
                End Using

                If lastFailed.HasValue Then
                    If currentTime - lastFailed.Value >= FailedLoginResetTime Then
                        failCount = 0
                    End If
                Else
                    failCount = 0
                End If

                If failCount < 0 Then
                    failCount = 0
                End If

                Using cmd As New SqlCommand("UPDATE Employees SET LastFailedLogin = GETDATE(), FailedLoginAttempts = @FailCount WHERE EmpNo = @EmpNo;" & _
                                            "SELECT FailedLoginAttempts FROM Employees WHERE EmpNo = @EmpNo", cn, cn.Trans)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                    cmd.Parameters.Add("@FailCount", SqlDbType.Int).Value = failCount + 1
                    cmd.ExecuteNonQuery()
                End Using

                If failCount + 1 >= MaxFailedLoginAttempts Then
                    Using cmd As New SqlCommand("UPDATE Employees SET LockedOutUntil = @LockedOutUntil WHERE EmpNo = @EmpNo", cn, cn.Trans)
                        cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                        cmd.Parameters.Add("@LockedOutUntil", SqlDbType.DateTime).Value = currentTime.Add(LockoutResetTime)
                        cmd.ExecuteNonQuery()
                    End Using

                    lockedOut = True
                End If

                cn.Commit()
            End Using

            Return lockedOut
        Catch ex As Exception
            Logger.LogException(ex, "Error Incrementing Failed Login Count")
        End Try
    End Function

    Public Shared Sub ResetFailedLoginCount(empNo As Integer)
        Try
            Using cn As New ConnectionInfo()
                If Not cn.Open() Then
                    Throw New ApplicationException("Database Connection Error")
                End If

                Using cmd As New SqlCommand("UPDATE Employees SET LastFailedLogin = NULL, FailedLoginAttempts = 0 WHERE EmpNo = @EmpNo", cn, cn.Trans)
                    cmd.Parameters.Add("@EmpNo", SqlDbType.Int).Value = empNo
                    cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Logger.LogException(ex, "Error Resetting Failed Login Count")
        End Try
    End Sub

    Public Shared Function ValidatePasswordStrength(owner As IWin32Window, password As String) As Boolean
        If password Is Nothing OrElse password.Length < MinPasswordLength Then
            DisplayMessage(owner, Function() String.Format(Authentication_The_new_password_must_be_at_least_characters_long, MinPasswordLength))
            Return False
        End If

        Dim hasLetter As Boolean = False
        Dim hasNumber As Boolean = False
        For Each ch As Char In password
            If Char.IsLetter(ch) Then
                hasLetter = True
            ElseIf Char.IsDigit(ch) Then
                hasNumber = True
            End If
        Next

        If Not hasLetter OrElse Not hasNumber Then
            DisplayMessage(owner, Function() Authentication_The_new_password_must_contain_at_least_1_letter_and_1_number)
            Return False
        End If

        Return True
    End Function

#End Region

#Region "Admin Account Handling"

    Public Shared Function CheckAdminUnlocked(cn As ConnectionInfo) As Boolean
        Try
            Dim curTime As DateTime
            Dim untilTime As DateTime

            Using cmd As New SqlCommand("SELECT OptionValue, GETDATE() FROM AppOptions WHERE OptionName = 'AdminUnlockedUntil'", cn, cn.Trans)
                Using dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleRow)
                    If Not dr.Read() Then
                        Return False
                    End If

                    Dim str As String = dr.GetString(0)
                    curTime = dr.GetDateTime(1)

                    If Not DateTime.TryParseExact(str, "u", Globalization.CultureInfo.InvariantCulture.DateTimeFormat, Globalization.DateTimeStyles.None, untilTime) Then
                        Return False
                    End If
                End Using
            End Using

            If untilTime > curTime.AddDays(7) Then
                ' Admin account has been unlocked too far into the future, set it back

                Using cmd As New SqlCommand("DELETE FROM AppOptions WHERE OptionName = 'AdminUnlockedUntil'", cn, cn.Trans)
                    cmd.ExecuteNonQuery()
                End Using

                Return False
            End If

            Return untilTime > curTime
        Catch ex As Exception
            Logger.LogException(ex, "Error Checking Admin Unlock")
            Return False
        End Try
    End Function

    Public Shared Sub UnlockAdminUntil(cn As ConnectionInfo, timeSpan As TimeSpan, ByRef info As AdminUnlockInfo)
        Try
            Dim internalTrans As Boolean = False

            If cn.Trans Is Nothing Then
                cn.BeginTransaction()
                internalTrans = True
            End If

            Dim curTime As DateTime
            Using cmd As New SqlCommand("SELECT GETDATE()", cn, cn.Trans)
                curTime = cmd.ExecuteScalar()
            End Using

            Using cmd As New SqlCommand("UPDATE AppOptions SET OptionValue = @OptionValue WHERE OptionName = 'AdminUnlockedUntil';" & _
                                        "IF @@ROWCOUNT = 0 INSERT INTO AppOptions (KeyNo, OptionName, OptionValue, TypeName) " & _
                                        "VALUES (1, 'AdminUnlockedUntil', @OptionValue, 'System.String')", cn, cn.Trans)
                cmd.Parameters.Add("@OptionValue", SqlDbType.VarChar, 50).Value = curTime.Add(timeSpan).ToString("u")
                cmd.ExecuteNonQuery()
            End Using

            info = GenerateAdminUnlockInfo()
            UpdatePassword(cn, VendorAdminEmpNo, info.NewPassword, False)
            UpdatePin(cn, VendorAdminEmpNo, info.NewPin, False)
            ClearLockout(cn, VendorAdminEmpNo)

            If internalTrans Then
                cn.Commit()
            End If
        Catch ex As Exception
            Throw Logger.CreateException(ex, Function() Authentication_Error_Unlocking_Admin_Account)
        End Try
    End Sub

    Private Shared Function GenerateAdminUnlockInfo() As AdminUnlockInfo
        Dim info As New AdminUnlockInfo()

        Dim bytes(15) As Byte

        Dim handle As GCHandle
        Runtime.CompilerServices.RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(
            Sub()
                ' ReSharper disable once RedundantAssignment
                handle = GCHandle.Alloc(bytes, GCHandleType.Pinned)

                Dim rand As New RNGCryptoServiceProvider()
                rand.GetBytes(bytes)

                info.NewPin = New SecureString()
                info.NewPassword = New SecureString()

                For i = 1 To 3
                    Dim chNumeric = BitConverter.ToUInt32(bytes, i * 4) Mod 26

                    info.NewPassword.AppendChar(Chr(Asc("a"c) + chNumeric))
                Next

                Dim pinNumeric = BitConverter.ToUInt32(bytes, 0) Mod 10000
                For i = 3 To 0 Step -1
                    Dim digit = Chr(Asc("0"c) + ((pinNumeric \ Math.Pow(10, i)) Mod 10))

                    info.NewPassword.AppendChar(digit)
                    info.NewPin.AppendChar(digit)
                Next

                info.NewPin.MakeReadOnly()
                info.NewPassword.MakeReadOnly()
            End Sub,
            Sub()
                If handle.IsAllocated Then
                    For i = 0 To bytes.Length - 1
                        bytes(i) = 0
                    Next

                    handle.Free()
                End If
            End Sub,
            Nothing
        )

        Return info
    End Function

#End Region

#Region "Helpers"

    Private Shared Function VerifyPassphrase(saltString As String, hashString As String, password As String) As Boolean
        Dim hash As Byte() = Convert.FromBase64String(hashString)
        Dim salt As Byte() = Convert.FromBase64String(saltString)
        Dim passwordBytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)

        Dim passwordPlusSalt() As Byte = Array.CreateInstance(GetType(Byte), passwordBytes.Length + salt.Length)
        Array.Copy(salt, passwordPlusSalt, salt.Length)
        Array.Copy(passwordBytes, 0, passwordPlusSalt, salt.Length, passwordBytes.Length)

        Dim sha1 As New System.Security.Cryptography.SHA1CryptoServiceProvider()
        Return CompareBytes(hash, sha1.ComputeHash(passwordPlusSalt))
    End Function

    Private Shared Function CompareBytes(array1 As Byte(), array2 As Byte()) As Boolean
        If array1 Is Nothing OrElse array2 Is Nothing OrElse array1.Length <> array2.Length Then
            Return False
        End If

        For i As Integer = 0 To array1.Length - 1
            If array1(i) <> array2(i) Then
                Return False
            End If
        Next

        Return True
    End Function

#End Region

End Class

 * */

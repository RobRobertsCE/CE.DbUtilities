using CE.DbConnectionHelper.Controllers;
using CE.DbConnectionHelper.Extensions;
using CE.DbConnectionHelper.ViewModels;
using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CE.Windows.Controls;

namespace CE.DbConnectionHelper
{
    public partial class frmDatabaseConnections : Form
    {
        #region fields
        private DbConnectionController _controller;
        private bool _loadingData = false;
        #endregion

        #region properties
        private Version _branchVersion;
        protected Version BranchVersion
        {
            get
            {
                if (_branchVersion == null)
                {
                    _branchVersion = GetBranchVersion();
                }

                return _branchVersion;
            }
        }
        #endregion

        #region ctor / form load
        public frmDatabaseConnections()
        {
            InitializeComponent();
        }
        private void frmDatabaseConnections_Load(object sender, EventArgs e)
        {
            _controller = new DbConnectionController();

            DisplayBranchVersion();

            RefreshViewModel();

            _controller.Model.Connections.ListChanged += Connections_ListChanged;
            _controller.Model.PropertyChanged += Model_PropertyChanged;
        }
        #endregion

        #region private
        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(this, ex.Message);
        }
        private Version GetBranchVersion()
        {
            try
            {
                var upgradeModel = new DbUpgradeViewModel();
                return upgradeModel.BranchVersion;
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }

            return null;
        }
        private void DisplayBranchVersion()
        {
            try
            {
                if (BranchVersion == null)
                {
                    lblBranchVersion.Text = $"*** Error Reading Branch Version ***";
                }
                else
                {
                    lblBranchVersion.Text = $"Repo Branch Version: {BranchVersion}";
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        private void RefreshViewModel()
        {
            //SetListBoxBindings();

            SetListViewBindings();

            lblCurrentConnection.Text = $"{_controller.SavedPfsConnection}";
        }
        private void SetListViewBindings()
        {
            blvConnections.DataSource = _controller.Model.Connections.OrderBy(c => c.Machine).ThenBy(c => c.Server).ThenBy(c => c.Database).ToList();

            foreach (ListViewItem lvi in blvConnections.Items)
            {
                lvi.Selected = (lvi.Tag == _controller.Model.CurrentConnection);
            }
            blvConnections.Select();
        }
        private void SetActivePfsConnection()
        {
            foreach (ListViewItem lvi in blvConnections.Items)
            {
                lvi.Selected = (lvi.Tag == _controller.Model.CurrentConnection);
            }
            blvConnections.Select();
        }

        private void Connections_ListChanged(object sender, ListChangedEventArgs e)
        {
            RefreshViewModel();
        }
        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_controller.Model.CurrentConnection))
            {
                SetActivePfsConnection();
            }
        }

        private void blvConnections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingData)
                return;

            if (blvConnections.SelectedItems.Count > 0)
            {
                var item = blvConnections.SelectedItems[0] as ListViewItem;
                var selected = (DbConnectionViewModel)item.Tag;
                SelectedConnectionChanged(selected);
            }
        }

        private void SelectedConnectionChanged(DbConnectionViewModel selected)
        {
            try
            {
                _controller.Model.CurrentConnection = selected;

                if (_controller.Model.CurrentConnection.DatabaseFullName != _controller.SavedPfsConnection)
                {
                    lblCurrentConnection.BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    lblCurrentConnection.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        #region actions
        /*** hide/show controls ***/
        private void toolbarToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.toolStrip1.Visible = toolbarToolStripMenuItem.Checked;
        }
        private void buttonPanelToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            this.pnlButtons.Visible = buttonPanelToolStripMenuItem.Checked;
        }

        /*** Add ***/
        private void new_Click(object sender, EventArgs e)
        {
            AddConnection();
        }
        private void AddConnection()
        {
            try
            {
                var newConnection = _controller.GetNewConnection();

                var dialog = new frmDatabaseConnection()
                {
                    Model = newConnection,
                    Controller = _controller
                };

                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    _controller.SaveNewConnection(dialog.Model);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Edit ***/
        private void edit_Click(object sender, EventArgs e)
        {
            EditConnection();
        }
        private void EditConnection()
        {
            try
            {
                var dialog = new frmDatabaseConnection()
                {
                    Model = this._controller.Model.CurrentConnection,
                    Controller = _controller
                };

                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    _controller.UpdateConnection(dialog.Model);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Copy ***/
        private void copy_Click(object sender, EventArgs e)
        {
            CopyConnection();
        }
        private void CopyConnection()
        {
            try
            {
                var newConnection = _controller.Model.CurrentConnection.Copy();

                var dialog = new frmDatabaseConnection()
                {
                    Model = newConnection,
                    Controller = _controller
                };

                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    _controller.SaveNewConnection(dialog.Model);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Delete ***/
        private void delete_Click(object sender, EventArgs e)
        {
            DeleteConnection();
        }
        private void DeleteConnection()
        {
            try
            {
                _controller.DeleteConnection(_controller.Model.CurrentConnection);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Save ***/
        private void save_Click(object sender, EventArgs e)
        {
            ApplyChanges();
        }
        private void ApplyChanges()
        {
            try
            {
                _controller.SaveChanges();

                SetListViewBindings();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Hide Form ***/
        private void frmDatabaseConnections_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
            Hide();
        }
        private void close_Click(object sender, EventArgs e)
        {
            CloseForm();
        }
        private void CloseForm()
        {
            try
            {
                if (_controller.Model.CurrentConnection == null)
                {
                    MessageBox.Show(this, "Must have an active connection selected.", "No Active Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (_controller.HasChanges)
                {
                    var promptResult = MessageBox.Show(this, "Save changes?", "Pending Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (promptResult == DialogResult.Yes)
                    {
                        ApplyChanges();
                    }
                    else if (promptResult == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                this.Hide();
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Test Connection ***/
        private async void testConnection_Click(object sender, EventArgs e)
        {
            await TestConnection();
        }
        private async Task TestConnection()
        {
            try
            {
                pnlDetails.Enabled = false;
                pnlButtons.Enabled = false;

                var taskResult = await _controller.TestConnectionAsync(_controller.Model.CurrentConnection);

                if (taskResult)
                {
                    MessageBox.Show(this, "Connection test successful");
                }
                else
                {
                    MessageBox.Show(this, "Connection test failed");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
                MessageBox.Show(this, $"Connection test failed: {ex.Message}");
            }
            finally
            {
                pnlDetails.Enabled = true;
                pnlButtons.Enabled = true;
            }
        }

        /*** Update Details ***/
        private async void updateDetails_Click(object sender, EventArgs e)
        {
            await UpdateDetailsAsync();
        }
        private async Task UpdateDetailsAsync()
        {
            try
            {
                await _controller.UpdateModelDetailsAsync(_controller.Model.CurrentConnection);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        /*** Upgrade Database ***/
        private void upgrade_Click(object sender, EventArgs e)
        {
            UpgradeDatabase();
        }
        private void UpgradeDatabase()
        {

            try
            {
                var upgradeModel = new DbUpgradeViewModel();
                Version targetVersion = upgradeModel.BranchVersion;
                lblBranchVersion.Text = $"Branch Version: {targetVersion}";

                var prompt = MessageBox.Show($"Upgrade to version {targetVersion}?", "Confirm Upgrade", MessageBoxButtons.OKCancel);

                if (prompt == DialogResult.Cancel)
                    return;

                MessageBox.Show("TODO");
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }
        #endregion
        #endregion
    }
}

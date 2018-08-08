using CE.DbConnectionHelper.Controllers;
using CE.DbConnectionHelper.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CE.DbConnectionHelper
{
    public partial class frmDatabaseConnection : Form
    {
        internal DbConnectionViewModel Model { get; set; }
        internal DbConnectionController Controller { get; set; }

        public frmDatabaseConnection()
        {
            InitializeComponent();
        }

        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            MessageBox.Show(this, ex.Message);
        }

        private void integratedSecurityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            userIdTextBox.Visible = (!integratedSecurityCheckBox.Checked);
            passwordTextBox.Visible = (!integratedSecurityCheckBox.Checked);
            userIdLabel.Visible = (!integratedSecurityCheckBox.Checked);
            passwordLabel.Visible = (!integratedSecurityCheckBox.Checked);
        }

        private void btnEditSave_Click(object sender, EventArgs e)
        {            
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmDatabaseConnection_Load(object sender, EventArgs e)
        {
            dbConnectionViewModelBindingSource.DataSource = Model;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            await TestConnection();
        }

        private async Task TestConnection()
        {
            try
            {
                pnlDetails.Enabled = false;
                pnlButtons.Enabled = false;

                var taskResult = await Controller.TestConnectionAsync(Model);

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
    }
}

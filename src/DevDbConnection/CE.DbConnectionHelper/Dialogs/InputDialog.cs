using System;
using System.Windows.Forms;

namespace CE.DbConnectionHelper.Dialogs
{
    public partial class InputDialog : Form
    {
        public string Response { get; private set; } = String.Empty;

        internal InputDialog()
        {
            InitializeComponent();

            Text = "";
            lblPrompt.Text = "";
            txtResponse.Text = "";
        }

        private void txtResponse_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Response = txtResponse.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Response = txtResponse.Text;
            DialogResult = DialogResult.OK;
        }

        public DialogResult ShowDialog(IWin32Window parent, string prompt)
        {
            return ShowDialog(parent, prompt, "", "");
        }
        public DialogResult ShowDialog(IWin32Window parent, string prompt, string title)
        {
            return ShowDialog(parent, prompt, title, "");
        }
        public DialogResult ShowDialog(IWin32Window parent, string prompt, string title, string defaultResponse)
        {
            Text = title;
            lblPrompt.Text = prompt;
            txtResponse.Text = defaultResponse;

            return base.ShowDialog(parent);
        }

        private void InputDialog_Activated(object sender, EventArgs e)
        {
            txtResponse.Focus();
            txtResponse.SelectAll();
        }
    }
}

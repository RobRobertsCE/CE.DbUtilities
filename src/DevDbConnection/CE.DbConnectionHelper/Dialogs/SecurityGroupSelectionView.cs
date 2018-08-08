using CE.DbConnectionHelper.ViewModels;
using ReactiveUI;
using System;
using System.Data;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace CE.DbConnectionHelper.Dialogs
{
    public partial class SecurityGroupSelectionView
        : Form, IViewFor<SecurityGroupSelectionViewModel>
    {
        #region properties
        public SecurityGroupSelectionViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (SecurityGroupSelectionViewModel)value;
        }

        SecurityGroupSelectionViewModel IViewFor<SecurityGroupSelectionViewModel>.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = value; }
        }
        #endregion

        #region ctor
        public SecurityGroupSelectionView()
            : this(new SecurityGroupSelectionViewModel())
        {

        }

        public SecurityGroupSelectionView(SecurityGroupSelectionViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;

            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.SecurityGroups, v => v.lstSecurityGroups.DataSource));
                d(this.OneWayBind(ViewModel, vm => vm.SelectedItem.Description, v => v.lblDescription.Text));

                d(Observable.FromEventPattern<EventHandler, EventArgs>(
                        ev => lstSecurityGroups.SelectedIndexChanged += ev,
                        ev => lstSecurityGroups.SelectedIndexChanged -= ev)
                        .Select(x => lstSecurityGroups.SelectedItem)
                        .BindTo(this, x => x.ViewModel.SelectedItem));

                d(Observable.FromEventPattern<EventHandler, EventArgs>(
                        ev => lstSecurityGroups.SelectedIndexChanged += ev,
                        ev => lstSecurityGroups.SelectedIndexChanged -= ev)
                        .Select(x => (lstSecurityGroups.SelectedItem != null))
                        .BindTo(this, x => x.ViewModel.HasSelection));

                d(this.Bind(ViewModel, vm => vm.HasSelection, v => v.btnOk.Enabled));

                lstSecurityGroups.SelectedIndex = -1;
            });
        }
        #endregion

        #region private
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
    }
}

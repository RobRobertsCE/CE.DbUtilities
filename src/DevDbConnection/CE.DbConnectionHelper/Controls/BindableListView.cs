using CE.DbConnectionHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace CE.Windows.Controls
{
    public partial class BindableListView : System.Windows.Forms.ListView
    {
        #region properties
        public CurrencyManager CurrencyManager { get; set; } = null;
        public DataView DataView { get; set; } = null;

        private object _dataSource = null;
        [Bindable(true)]
        [TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
        [Category("Data")]
        public Object DataSource
        {
            get
            {
                return _dataSource;
            }
            set
            {
                _dataSource = value;
                UpdateBindings();
            }
        }

        private String _dataMember;
        [Bindable(true)]
        [Category("Data")]
        [Editor("System.Windows.Forms.Design.DataMemberFieldEditor, System.Design", "System.Drawing.Design.UITypeEditor, System.Drawing")]
        [RefreshProperties(RefreshProperties.All)]
        public String DataMember
        {
            get
            {
                return _dataMember;
            }
            set
            {
                _dataMember = value;
                UpdateBindings();
            }
        }

        [Browsable(false)]
        public new SortOrder Sorting
        {
            get
            {
                return base.Sorting;
            }
            set
            {
                base.Sorting = value;
            }
        }
        #endregion

        #region ctor
        public BindableListView()
        {
            InitializeComponent();

            base.SelectedIndexChanged += new EventHandler(
                       bindableListView_SelectedIndexChanged);
            base.ColumnClick += new ColumnClickEventHandler(
                               bindableListView_ColumnClick);
        }
        #endregion

        #region private events
        private void bindableListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (Sorting == SortOrder.None || Sorting == SortOrder.Descending)
            {
                Sorting = SortOrder.Ascending;
                //DataView.Sort = this.Columns[0].Text + " ASC";
            }
            else if (Sorting == SortOrder.Ascending)
            {
                Sorting = SortOrder.Descending;
                //DataView.Sort = this.Columns[0].Text + " DESC";
            }
        }
        private void bindableListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndices.Count > 0)
            {
                if (CurrencyManager != null)
                {
                    CurrencyManager.Position = base.SelectedIndices[0];
                }
            }
        }
        private void currencyManager_CurrentChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndices.Count > 0)
            {
                if (CurrencyManager != null)
                {
                    CurrencyManager.Position = base.SelectedIndices[0];
                }
            }
        }
        #endregion

        #region private
        private void UpdateBindings()
        {
            //Clear the existing list
            Items.Clear();
            //This implementation assumes the DataSource is a DataSet
            if (DataSource is DataSet)
            {
                DataSet ds = DataSource as DataSet;
                DataTable dt = ds.Tables[0];

                if (dt != null)
                {

                    //get the Binding Context for the Form
                    CurrencyManager = (CurrencyManager)BindingContext[ds,
                                      ds.Tables[0].TableName];

                    //add an event handler for the Currency Manager       
                    CurrencyManager.CurrentChanged += new EventHandler(currencyManager_CurrentChanged);

                    //Get the Currency Manager Collection as a DataView
                    DataView = (DataView)CurrencyManager.List;

                    //Create the column header based on the DataMember
                    Columns.Add(DataMember, ClientRectangle.Width - 17,
                                              HorizontalAlignment.Left);
                    //Add each Row as a ListViewItem        
                    foreach (DataRow dr in dt.Rows)
                    {
                        ListViewItem lvi = new ListViewItem(
                                                dr[DataMember].ToString());
                        lvi.Tag = dr;
                        Items.Add(lvi);
                    }
                    //Set the Sorting property as Ascending
                    Sorting = SortOrder.Ascending;

                    //Sort the DataView on the DataMember          
                    DataView.Sort = this.Columns[0].Text + " ASC";
                }
            }
            else if (DataSource is BindingList<DbConnectionViewModel>)
            {
                try
                {

                    BindingList<DbConnectionViewModel> ds = DataSource as BindingList<DbConnectionViewModel>;

                    if (ds != null)
                    {

                        //get the Binding Context for the Form
                        CurrencyManager = (CurrencyManager)BindingContext[ds];

                        //add an event handler for the Currency Manager       
                        CurrencyManager.CurrentChanged += new EventHandler(currencyManager_CurrentChanged);

                        //Add each Row as a ListViewItem        
                        foreach (DbConnectionViewModel item in ds)
                        {
                            ListViewItem lvi = new ListViewItem(item.DatabaseFullName);
                            lvi.SubItems.Add(item.Version);
                            lvi.SubItems.Add(item.Location);
                            lvi.SubItems.Add(item.SqlServerVersion);

                            lvi.Tag = item;
                            Items.Add(lvi);
                        }
                        //Set the Sorting property as Ascending
                        Sorting = SortOrder.Ascending;

                        //Sort the DataView on the DataMember          
                        //DataView.Sort = this.Columns[0].Text + " ASC";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else if (DataSource is IList<DbConnectionViewModel>)
            {
                try
                {

                    IList<DbConnectionViewModel> ds = DataSource as IList<DbConnectionViewModel>;

                    if (ds != null)
                    {

                        //get the Binding Context for the Form
                        CurrencyManager = (CurrencyManager)BindingContext[ds];

                        //add an event handler for the Currency Manager       
                        CurrencyManager.CurrentChanged += new EventHandler(currencyManager_CurrentChanged);

                        //Add each Row as a ListViewItem        
                        foreach (DbConnectionViewModel item in ds)
                        {
                            ListViewItem lvi = new ListViewItem(item.DatabaseFullName);
                            lvi.SubItems.Add(item.Version);
                            lvi.SubItems.Add(item.Location);
                            lvi.SubItems.Add(item.SqlServerVersion);
                            lvi.Tag = item;
                            Items.Add(lvi);
                        }

                        //Set the Sorting property as Ascending
                        Sorting = SortOrder.Ascending;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                //If no source is defined, Currency Manager is null  
                CurrencyManager = null;
            }
        }
        #endregion
    }
}

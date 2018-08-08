using System.ComponentModel;

namespace CE.DbConnectionHelper.ViewModels
{
    public class DbConnectionsViewModel
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private DbConnectionViewModel _currentConnection;
        public DbConnectionViewModel CurrentConnection
        {
            get
            {
                return _currentConnection;
            }
            set
            {
                _currentConnection = value;
                OnPropertyChanged(nameof(CurrentConnection));
            }
        }

        public BindingList<DbConnectionViewModel> Connections { get; set; }

        public DbConnectionsViewModel()
        {
            Connections = new BindingList<DbConnectionViewModel>();
            Connections.AllowNew = true;
            Connections.AllowRemove = true;
            Connections.RaiseListChangedEvents = true;
            Connections.AllowEdit = true;
        }
    }
}

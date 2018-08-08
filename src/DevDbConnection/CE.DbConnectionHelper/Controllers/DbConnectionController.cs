using CE.DbConnect.Data;
using CE.DbConnect.Models;
using CE.DbConnectionHelper.Extensions;
using CE.DbConnectionHelper.Models;
using CE.DbConnectionHelper.Services;
using CE.DbConnectionHelper.ViewModels;
using CE.PfsConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CE.DbConnectionHelper.Controllers
{
    class DbConnectionController : IDbConnectionController
    {
        #region fields
        private DevelopmentContext _dbContext = null;
        private PfsConnectFile _pfsConnectFile;
        private string _initialActiveConnectionKey;
        #endregion

        #region properties
        public DbConnectionsViewModel Model { get; set; }

        public bool HasChanges
        {
            get
            {
                return _dbContext.ChangeTracker.HasChanges() || PfsConnectionChanged;
            }
        }

        protected bool PfsConnectionChanged
        {
            get
            {
                return (_initialActiveConnectionKey != _pfsConnectFile.Connection?.KeyId);
            }
        }

        public string CurrentPfsConnection { get; set; }

        public string SavedPfsConnection { get; set; }
        #endregion

        #region ctor
        public DbConnectionController()
        {
            InitializeController();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        private void InitializeController()
        {
            _dbContext = new DevelopmentContext();

            Model = new DbConnectionsViewModel();

            LoadPfsConnect();

            SyncDbConnections();

            LoadDbViewModel();
        }
        #endregion

        #region public
        public void SaveChanges()
        {
            try
            {
                _pfsConnectFile.SavePfsConnectFile();

                LoadPfsConnect();

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void ResetViewModel()
        {
            try
            {
                InitializeController();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public DbConnectionViewModel GetNewConnection()
        {
            var newDbModel = new DatabaseInstance();
            var newModel = new DbConnectionViewModel() { DbModel = newDbModel };
            Model.CurrentConnection = newModel;
            return newModel;
        }

        public void SaveNewConnection(DbConnectionViewModel viewModel)
        {
            try
            {
                var databaseModel = new DatabaseInstance()
                {
                    DatabaseId = Guid.NewGuid(),
                    Name = viewModel.Database,
                    ServerId = viewModel.ServerId,
                    SqlCredentialId = viewModel.SqlCredentialId
                };

                viewModel.DbModel = databaseModel;

                SyncDbModelFromViewModel(viewModel);

                _dbContext.SaveChanges();

                LoadDbViewModel();

                Model.Connections.ResetBindings();

                Model.CurrentConnection = Model.Connections.FirstOrDefault(c => c.DatabaseId == databaseModel.DatabaseId);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public void UpdateConnection(DbConnectionViewModel viewModel)
        {
            try
            {
                var databaseModel = _dbContext.Databases.FirstOrDefault(d => d.DatabaseId == viewModel.DatabaseId);

                viewModel.DbModel = databaseModel;

                SyncDbModelFromViewModel(viewModel);

                _dbContext.SaveChanges();

                Model.Connections.ResetBindings();

                Model.CurrentConnection = Model.Connections.FirstOrDefault(c => c.DatabaseId == databaseModel.DatabaseId);
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public void DeleteConnection(DbConnectionViewModel model)
        {
            try
            {
                _dbContext.Databases.Remove(model.DbModel);
                Model.Connections.Remove(model);

                Model.Connections.ResetBindings();

            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        public async Task<bool> UpdateModelDetailsAsync(DbConnectionViewModel model)
        {
            return await UpdateModelDetailsAsync(model.DbModel);
        }

        public async Task<bool> UpdateModelDetailsAsync(DatabaseInstance databaseModel)
        {
            var cnString = databaseModel.GetSqlConnectionString();

            var service = new CEDatabaseService();

            var canConnect = await service.TestConnectionAsync(cnString);

            if (!canConnect)
                return false;

            var existingModel = _dbContext.Databases.Include("Serverinstance").FirstOrDefault(d => d.DatabaseId == databaseModel.DatabaseId);

            existingModel.ServerInstance.SqlVersion = await service.GetSqlServerVersionAsync(cnString);
            existingModel.Version = await service.GetDbVersionAsync(cnString);
            existingModel.Location = await service.GetLocationNameAsync(cnString);

            await _dbContext.SaveChangesAsync();

            existingModel.Location = databaseModel.Location;
            existingModel.Version = databaseModel.Version;

            Model.Connections.ResetBindings();

            return true;
        }

        public async Task<bool> TestConnectionAsync(DbConnectionViewModel model)
        {

            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.TestConnectionAsync(cnString);
        }
        public async Task<bool> TestConnectionAsync(DatabaseInstance model)
        {

            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.TestConnectionAsync(cnString);
        }

        public async Task<bool> AddUserAsync(DbConnectionViewModel model, int groupId)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            var empNo = await service.ResetUserAsync(cnString, groupId);

            return empNo.HasValue;
        }
        public async Task<bool> AddUserAsync(DatabaseInstance model, int groupId)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            var empNo = await service.ResetUserAsync(cnString, groupId);

            return empNo.HasValue;
        }

        public async Task<IList<SecurityGroupModel>> GetSecurityGroupsAsync(DatabaseInstance model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.GetSecurityGroupsAsync(cnString);
        }

        public async Task<string> CopyDatabaseAsync(DbConnectionViewModel model)
        {
            return await CopyDatabaseAsync(model, model.Database);
        }
        public async Task<string> CopyDatabaseAsync(DbConnectionViewModel model, string newDatabaseName)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.CopyDatabaseAsync(model.Database, newDatabaseName, cnString);
        }
        public async Task<string> CopyDatabaseAsync(DatabaseInstance model)
        {
            return await CopyDatabaseAsync(model, model.Name);
        }
        public async Task<string> CopyDatabaseAsync(DatabaseInstance model, string newDatabaseName)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.CopyDatabaseAsync(model.Name, newDatabaseName, cnString);
        }

        public async Task<string> RestoreDatabaseAsync(DbConnectionViewModel model, string backupFileFullPath)
        {
            return await RestoreDatabaseAsync(model, backupFileFullPath, model.Database);
        }
        public async Task<string> RestoreDatabaseAsync(DbConnectionViewModel model, string backupFileFullPath, string newDatabaseName)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.RestoreDatabaseAsync(backupFileFullPath, model.Database, newDatabaseName, cnString);
        }
        public async Task<string> RestoreDatabaseAsync(DatabaseInstance model, string backupFileFullPath)
        {
            return await RestoreDatabaseAsync(model, backupFileFullPath, model.Name);
        }
        public async Task<string> RestoreDatabaseAsync(DatabaseInstance model, string backupFileFullPath, string newDatabaseName)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.RestoreDatabaseAsync(backupFileFullPath, model.Name, newDatabaseName, cnString);
        }

        public async Task<string> BackupDatabaseAsync(DbConnectionViewModel model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.BackupDatabaseAsync(model.Database, cnString);
        }
        public async Task<string> BackupDatabaseAsync(DatabaseInstance model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.BackupDatabaseAsync(model.Name, cnString);
        }

        public async Task<DateTime?> UpdateAdminLoginAsync(DatabaseInstance model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.UpdateAdminLockoutAsync(cnString);
        }
        public async Task<DateTime?> UpdateAdminLoginAsync(DbConnectionViewModel model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.UpdateAdminLockoutAsync(cnString);
        }


        public async Task<bool> SetAdminLoginTohotelGolf82Async(DatabaseInstance model)
        {
            var cnString = model.GetSqlConnectionString();

            var service = new CEDatabaseService();

            return await service.ResetAdminPasswordAsync(cnString);
        }
        #endregion

        #region private
        private void ExceptionHandler(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw ex;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.CurrentConnection))
            {
                SetActivePfsConnection();
            }
        }

        private void LoadPfsConnect()
        {
            _pfsConnectFile = PfsConnectFile.LoadPfsConnectFile();
            _initialActiveConnectionKey = _pfsConnectFile.Connection?.KeyId;
            SavedPfsConnection = $"{_pfsConnectFile.Connection?.DataSource}.{_pfsConnectFile.Connection?.Catalog}";
        }

        private IList<DatabaseInstance> GetDatabaseList()
        {
            var databases = _dbContext.
                    Databases.
                    Include("SqlCredential").
                    Include("ServerInstance").
                    Include("ServerInstance.MachineInstance").
                    Where(d => d.ServerInstance != null && d.SqlCredential != null && d.ServerInstance.MachineInstance != null).
                    OrderBy(d => d.ServerInstance.Name).
                    ThenBy(d => d.Name).ToList();

            return databases;
        }

        private void LoadDbViewModel()
        {
            try
            {
                var databases = GetDatabaseList();

                Model.Connections.Clear();

                foreach (var databaseModel in databases)
                {
                    var connectionModel = new DbConnectionViewModel()
                    {
                        UserId = databaseModel.SqlCredential.UserId,
                        Password = databaseModel.SqlCredential.Password,
                        IntegratedSecurity = databaseModel.SqlCredential.UseIntegratedSecurity,
                        Machine = databaseModel.ServerInstance.MachineInstance.Name,
                        Server = databaseModel.ServerInstance.Name,
                        Database = databaseModel.Name,
                        DatabaseId = databaseModel.DatabaseId,
                        Version = databaseModel.Version,
                        ServerId = databaseModel.ServerId,
                        MachineId = databaseModel.ServerInstance.MachineId,
                        SqlCredentialId = databaseModel.SqlCredentialId,
                        IsEncrypted = databaseModel.SqlCredential.UseEncryption,
                        Location = databaseModel.Location,
                        Notes = databaseModel.Notes,
                        DbModel = databaseModel
                    };

                    Model.Connections.Add(connectionModel);

                    if (IsCurrentPfsConnection(connectionModel))
                    {
                        Model.CurrentConnection = connectionModel;
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private void SyncDbModelFromViewModel(DbConnectionViewModel viewModel)
        {
            try
            {
                var machineModel = _dbContext.Machines.FirstOrDefault(m => m.Name == viewModel.Machine);

                if (machineModel == null)
                {
                    machineModel = new MachineInstance()
                    {
                        MachineId = Guid.NewGuid(),
                        Name = viewModel.Machine
                    };
                    _dbContext.Machines.Add(machineModel);
                }

                viewModel.MachineId = machineModel.MachineId;

                var serverModel = _dbContext.Servers.FirstOrDefault(s => s.Name == viewModel.Server && s.MachineInstance.Name == viewModel.Machine);

                if (serverModel == null)
                {
                    serverModel = new ServerInstance()
                    {
                        ServerId = Guid.NewGuid(),
                        Name = viewModel.Machine,
                        MachineId = machineModel.MachineId
                    };

                    _dbContext.Servers.Add(serverModel);
                }

                viewModel.ServerId = serverModel.ServerId;

                var credentialModel = _dbContext.
                    Credentials.
                    FirstOrDefault(s => s.UserId == viewModel.UserId &&
                        s.Password == viewModel.Password &&
                        s.UseEncryption == viewModel.IsEncrypted &&
                        s.UseIntegratedSecurity == viewModel.IntegratedSecurity);

                if (credentialModel == null)
                {
                    credentialModel = new SqlCredentials()
                    {
                        SqlCredentialId = Guid.NewGuid(),
                        UserId = viewModel.UserId,
                        Password = viewModel.Password,
                        UseEncryption = viewModel.IsEncrypted,
                        UseIntegratedSecurity = viewModel.IntegratedSecurity
                    };

                    _dbContext.Credentials.Add(credentialModel);
                }

                viewModel.SqlCredentialId = credentialModel.SqlCredentialId;

                var databaseModel = _dbContext.Databases.FirstOrDefault(s => s.DatabaseId == viewModel.DatabaseId);

                if (databaseModel == null)
                {
                    databaseModel = new DatabaseInstance()
                    {
                        DatabaseId = Guid.NewGuid(),
                        Name = viewModel.Database,
                        ServerId = serverModel.ServerId,
                        SqlCredentialId = credentialModel.SqlCredentialId
                    };

                    _dbContext.Databases.Add(databaseModel);

                    viewModel.DatabaseId = databaseModel.DatabaseId;
                }
                else
                {
                    databaseModel.Name = viewModel.Database;
                    databaseModel.ServerId = viewModel.ServerId;
                    databaseModel.SqlCredentialId = viewModel.SqlCredentialId;
                    databaseModel.Location = viewModel.Location;
                    databaseModel.Version = viewModel.Version;
                    databaseModel.Notes = viewModel.Notes;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void SyncDbConnections()
        {
            try
            {
                foreach (var pfsConnection in _pfsConnectFile.Sql2000.Connections)
                {
                    var model = Model.Connections.FirstOrDefault(c => c.KeyId == pfsConnection.KeyId);

                    if (model == null)
                    {
                        var instanceNames = pfsConnection.DataSource.Split('\\');
                        var machineName = instanceNames[0];
                        var sqlInstance = instanceNames[1];
                        var machineModel = _dbContext.Machines.FirstOrDefault(m => m.Name == machineName);
                        if (machineModel == null)
                        {
                            machineModel = new DbConnect.Models.MachineInstance()
                            {
                                MachineId = Guid.NewGuid(),
                                Name = machineName
                            };
                            _dbContext.Machines.Add(machineModel);
                        }

                        var serverModel = _dbContext.Servers.FirstOrDefault(s => s.Name == sqlInstance && s.MachineId == machineModel.MachineId);
                        if (serverModel == null)
                        {
                            serverModel = new DbConnect.Models.ServerInstance()
                            {
                                ServerId = Guid.NewGuid(),
                                Name = sqlInstance,
                                MachineId = machineModel.MachineId
                            };

                            _dbContext.Servers.Add(serverModel);
                        }

                        var credentialModel = _dbContext.Credentials.FirstOrDefault(s => s.UserId == pfsConnection.UserID && s.Password == pfsConnection.Password);
                        if (credentialModel == null)
                        {
                            credentialModel = new DbConnect.Models.SqlCredentials()
                            {
                                SqlCredentialId = Guid.NewGuid(),
                                UserId = pfsConnection.UserID,
                                Password = pfsConnection.Password,
                                UseEncryption = (pfsConnection.PasswordEncryption == 1),
                                UseIntegratedSecurity = (pfsConnection.IntegratedSecurity == 1)
                            };

                            _dbContext.Credentials.Add(credentialModel);
                        }

                        var databaseModel = _dbContext.Databases.FirstOrDefault(s => s.Name == pfsConnection.Catalog && s.ServerId == serverModel.ServerId && s.SqlCredentialId == credentialModel.SqlCredentialId);
                        if (databaseModel == null)
                        {
                            databaseModel = new DbConnect.Models.DatabaseInstance()
                            {
                                DatabaseId = Guid.NewGuid(),
                                Name = pfsConnection.Catalog,
                                ServerId = serverModel.ServerId,
                                SqlCredentialId = credentialModel.SqlCredentialId
                            };

                            _dbContext.Databases.Add(databaseModel);
                        }
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private bool IsCurrentPfsConnection(DbConnectionViewModel model)
        {
            return ((_pfsConnectFile.Connection.KeyId == model.KeyId));
        }

        private void SetActivePfsConnection()
        {
            if (Model.CurrentConnection != null && Model.CurrentConnection.DatabaseId != Guid.Empty)
            {
                _pfsConnectFile.Sql2000.ClearActiveConnections();

                var existing = _pfsConnectFile.Sql2000
                   .Connections
                   .FirstOrDefault(c => c.KeyId == Model.CurrentConnection.KeyId);

                if (existing == null)
                {
                    existing = new PfsDbConnection()
                    {
                        DataSource = Model.CurrentConnection.GetServerInstanceName(),
                        Catalog = Model.CurrentConnection.Database,
                        UserID = Model.CurrentConnection.UserId,
                        Password = Model.CurrentConnection.Password,
                        IntegratedSecurity = 0,
                        PasswordEncryption = 0,
                        IsActive = true
                    };

                    _pfsConnectFile.Sql2000.Connections.Add(existing);
                }
                else
                {
                    existing.IsActive = true;
                }
            }
        }
        #endregion
    }
}

using CE.DbConnect.Models;
using CE.DbConnectionHelper.ViewModels;
using System;
using System.Data.SqlClient;

namespace CE.DbConnectionHelper.Extensions
{
    internal static class DbConnectionViewModelExtensions
    {
        public static string GetSqlConnectionString(this DbConnectionViewModel model)
        {
            var cnStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = model.GetServerInstanceName(),
                InitialCatalog = model.Database,
                ConnectTimeout = 15,
                IntegratedSecurity = model.IntegratedSecurity,
                Encrypt = model.IsEncrypted
            };

            if (!model.IntegratedSecurity)
            {
                cnStringBuilder.UserID = model.UserId;
                cnStringBuilder.Password = model.Password;
            }

            return cnStringBuilder.ToString();
        }

        public static string GetServerInstanceName(this DbConnectionViewModel model)
        {
            return $"{model.Machine}\\{model.Server}";
        }

        public static DbConnectionViewModel Copy(this DbConnectionViewModel model)
        {
            var databaseModel = new DatabaseInstance()
            {
                DatabaseId = Guid.NewGuid(),
                Name = model.DbModel.Name,
                ServerInstance = model.DbModel.ServerInstance,
                SqlCredential = model.DbModel.SqlCredential,
                Version = model.DbModel.Version
            };
            var newModel = new DbConnectionViewModel()
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
                DbModel = databaseModel
            };

            return newModel;
        }
    }
}

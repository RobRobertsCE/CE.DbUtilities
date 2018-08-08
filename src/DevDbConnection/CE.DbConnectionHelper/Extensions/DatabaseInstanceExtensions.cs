using CE.DbConnect.Models;
using CE.DbConnectionHelper.ViewModels;
using System;
using System.Data.SqlClient;

namespace CE.DbConnectionHelper.Extensions
{
    internal static class DatabaseInstanceExtensions
    {
        public static string GetSqlConnectionString(this DatabaseInstance model)
        {
            var cnStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = model.GetServerInstanceName(),
                InitialCatalog = model.Name,
                ConnectTimeout = 15,
                IntegratedSecurity = model.SqlCredential.UseIntegratedSecurity,
                Encrypt = model.SqlCredential.UseEncryption
            };

            if (!model.SqlCredential.UseIntegratedSecurity)
            {
                cnStringBuilder.UserID = model.SqlCredential.UserId;
                cnStringBuilder.Password = model.SqlCredential.Password;
            }

            return cnStringBuilder.ToString();
        }

        public static string GetServerInstanceName(this DatabaseInstance model)
        {
            return $"{model.ServerInstance.MachineInstance.Name}\\{model.ServerInstance.Name}";
        }

        public static DatabaseInstance Copy(this DatabaseInstance model)
        {
            return model.Copy($"{model.Name}Copy");
        }
        public static DatabaseInstance Copy(this DatabaseInstance model, string newDatabaseName)
        {
            var databaseModel = new DatabaseInstance()
            {
                DatabaseId = Guid.NewGuid(),
                Name = newDatabaseName,
                ServerInstance = model.ServerInstance,
                SqlCredential = model.SqlCredential,
                Location = model.Location,
                Version = model.Version,
                Notes = model.Notes
            };

            return databaseModel;
        }

        public static DbConnectionViewModel ToViewModel(this DatabaseInstance databaseModel)
        {
            return new DbConnectionViewModel()
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
                Notes = databaseModel.Notes,
                DbModel = databaseModel
            };
        }
    }
}

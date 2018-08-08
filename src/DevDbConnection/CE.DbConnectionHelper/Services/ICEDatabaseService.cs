using CE.DbConnectionHelper.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CE.DbConnectionHelper.Services
{
    public interface ICEDatabaseService
    {
        Task<string> BackupDatabaseAsync(string databaseName, string cnString);
        Task<string> CopyDatabaseAsync(string databaseName, string cnString);
        Task<string> CopyDatabaseAsync(string databaseName, string newDatabaseName, string cnString);
        Task<string> GetDbVersionAsync(string cnString);
        Task<string> GetLocationNameAsync(string cnString);
        Task<string> GetSqlServerVersionAsync(string cnString);
        Task<IList<SecurityGroupModel>> GetSecurityGroupsAsync(string cnString);
        Task<bool> IsUserInDatabaseAsync(string cnString);
        Task<int?> ResetAdminPasswordAsync(string cnString, int groupId);
        Task<string> RestoreDatabaseAsync(string backupFileFullPath, string databaseName, string cnString);
        Task<string> RestoreDatabaseAsync(string backupFileFullPath, string databaseName, string newDatabaseName, string cnString);
        Task<bool> TestConnectionAsync(string cnString);
        Task<DateTime?> UpdateAdminLockoutAsync(string cnString);
    }
}
using CE.DbConnectionHelper.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace CE.DbConnectionHelper.Services
{
    class CEDatabaseService : ICEDatabaseService
    {
        #region consts
        public const string BackupDirectory = @"C:\DatabaseBackups";
        private const string MyEmpFirstName = "Rob";
        private const string MyEmpLastName = "Roberts";
        private const string MyEmpPwSalt = "ghHr11jiyCd+ry4NkjgXSnuKh5k=";
        private const string MyEmpPwHash = "TLy06xsjL1QP+hu2JNj6YIqRaNo=";
        private const string MyEmpPinSalt = "jpQhOv/HON01i6yl5Be1zkaYqj8=";
        private const string MyEmpPinHash = "kdAOvLxjUsTxNTZ86djSb4ZsmNY=";
        private const string UpdateMyEmployee = @"UPDATE [dbo].[Employees]
           SET 
               [CreditLimit] = 500
              ,[PasswordSalt] = @pwsalt
              ,[PasswordHash] = @pwhash
              ,[PinSalt] = @pinsalt
              ,[PinHash] = @pinhash
              ,[MustChangePassword] = 0
              ,[MustChangePin] = 0
              ,[FailedLoginAttempts] = 0
              ,[LastFailedLogin] = null
              ,[LockedOutUntil] = null
         WHERE EmpNo = @empno";

        // password = 'hotelGolf82'; pin = 4382
        private const string AddMeAsEmployee = @"INSERT INTO [dbo].[Employees]
           ([LastName]
           ,[FirstName]
           ,[MiddleInitial]
           ,[Address]
           ,[City]
           ,[State]
           ,[ZipCode]
           ,[HomePhone]
           ,[CellPhone]
           ,[EMailAddress]
           ,[DateOfBirth]
           ,[DateOfHire]
           ,[DateOfTerminate]
           ,[EmpStatus]
           ,[SecurityLevel]
           ,[SSN]
           ,[ServerID]
           ,[Picture]
           ,[TimeCreated]
           ,[CreatedBy_EmpNo]
           ,[QuickBooks_ListID]
           ,[DefaultPayRate]
           ,[AltEmpNo]
           ,[CreditLimit]
           ,[EMailMessages]
           ,[EmpID]
           ,[FingerprintTemplate]
           ,[Salaried]
           ,[ShiftBreakApplies]
           ,[AvailabilityNotes]
           ,[PasswordSalt]
           ,[PasswordHash]
           ,[PinSalt]
           ,[PinHash]
           ,[MustChangePassword]
           ,[MustChangePin]
           ,[LastPasswordChange]
           ,[FailedLoginAttempts]
           ,[LastFailedLogin]
           ,[LockedOutUntil]
           ,[TimeModified]
           ,[EditSequence]
           ,[Address2]
           ,[Address3]
           ,[Address4]
           ,[Address5]
           ,[CustomAddress]
           ,[CountryCode]
           ,[Instructor]
           ,[InstructorBio])
     VALUES
           (@lastname
           ,@firstname
           ,''
           ,'1234 Southern Dr.'
           ,'Efland'
           ,'NC'
           ,'27243'
           ,'111-222-3333'
           ,'111-222-3333'
           ,'rroberts@centeredgesoftware.com'
           ,'1967-01-24 00:00:00.000'
           ,'1997-01-24 00:00:00.000'
           ,null
           ,1
           ,6
           ,'123-45-6789'
           ,0
           ,null
           ,GetDate()
           ,3
           ,null
           ,3.50
           ,4382
           ,500.00
           ,0
           ,NewId()
           ,null
           ,0
           ,0
           ,null
           ,@pwsalt
           ,@pwhash
           ,@pinsalt
           ,@pinhash
           ,0
           ,0
           ,'2018-06-28 15:39:07.963'
           ,0
           ,null
           ,null
           ,null
           ,null
           ,''
           ,'Efland, NC 27243  + US'
           ,''
           ,''
           ,0
           ,'US'
           ,0
           ,'<BODY scroll=auto></BODY>')";
        #endregion

        #region public
        public async Task<bool> TestConnectionAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT GetDate()", cn))
                {
                    await cmd.ExecuteScalarAsync();
                    return true;
                }
            }
        }
        public async Task<DateTime?> UpdateAdminLockoutAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE AppOptions SET OptionValue = @newTimestamp WHERE OptionName = 'AdminUnlockedUntil'; SELECT OptionValue FROM AppOptions WHERE OptionName = 'AdminUnlockedUntil'", cn))
                {
                    var newTimestamp = DateTime.Now.AddDays(1);

                    var newTimestampString = newTimestamp.ToString("u", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);

                    cmd.Parameters.Add(new SqlParameter("@newTimestamp", newTimestampString));

                    var savedTimestamp = await cmd.ExecuteScalarAsync();

                    DateTime verificationTimestamp;

                    if (DateTime.TryParseExact(savedTimestamp.ToString(), "u", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat, System.Globalization.DateTimeStyles.None, out verificationTimestamp))
                    {
                        if (newTimestamp.ToShortDateString() == verificationTimestamp.ToShortDateString())
                        {
                            return newTimestamp;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public async Task<bool> IsUserInDatabaseAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();
                var empNo = await GetUserEmployeeNumberAsync(cn);
                return empNo.HasValue;
            }
        }
        public async Task<int?> ResetAdminPasswordAsync(string cnString, int empNo)
        {
            //int empNo = 3;

            //using (SqlConnection cn = new SqlConnection(cnString))
            //{
            //    cn.InfoMessage += sqlConnectionn_InfoMessage;
            //    cn.Open();

            //    await UpdateUserLoginAsync(cn, empNo);
            //}

            //return true;
            throw new NotImplementedException();
        }
        public async Task<bool> ResetAdminPasswordAsync(string cnString)
        {
            int empNo = 3;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();

                await UpdateUserLoginAsync(cn, empNo);
            }

            return true;
        }
        public async Task<int?> ResetUserAsync(string cnString, int groupId)
        {
            int? empNo = null;

            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();

                empNo = await GetUserEmployeeNumberAsync(cn);

                if (!empNo.HasValue)
                {
                    empNo = await InsertUserLoginAsync(cn);
                }
                else
                {
                    var userIsInSync = false;

                    using (SqlCommand cmd = new SqlCommand("SELECT PasswordSalt, PasswordHash, PinSalt, PinHash FROM Employees Where EmpNo = @empno", cn))
                    {
                        cmd.Parameters.Add(new SqlParameter("@empno", empNo.Value));

                        using (var dr = await cmd.ExecuteReaderAsync())
                        {
                            dr.Read();

                            userIsInSync = ((MyEmpPwSalt == dr.GetString(1).Trim()) && (MyEmpPwHash == dr.GetString(2).Trim()) &&
                                (MyEmpPinSalt == dr.GetString(3).Trim()) && (MyEmpPinHash == dr.GetString(4).Trim()));
                        }
                    }

                    if (!userIsInSync)
                        await UpdateUserLoginAsync(cn, empNo.Value);

                    if (!empNo.HasValue)
                    {
                        throw new Exception("Failed to add or update employee");
                    }

                    await AddUserToAdministrationGroupAsync(cn, empNo.Value, groupId);
                }
            }

            return empNo;
        }
        public async Task<IList<SecurityGroupModel>> GetSecurityGroupsAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();
                return await GetSecurityGroupsListAsync(cn);
            }
        }

        public async Task<string> BackupDatabaseAsync(string databaseName, string cnString)
        {
            string backupFileFullPath = null;

            try
            {

                using (SqlConnection cn = new SqlConnection(cnString))
                {
                    cn.InfoMessage += sqlConnectionn_InfoMessage;
                    var timestamp = DateTime.Now.ToString("yyyy MM dd hhmm tt");
                    var backupFileName = $"{databaseName}.{timestamp}.bak";
                    if (!Directory.Exists(BackupDirectory))
                    {
                        Directory.CreateDirectory(BackupDirectory);
                    }
                    backupFileFullPath = Path.Combine(BackupDirectory, backupFileName);

                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand($"BACKUP DATABASE {databaseName} TO DISK = '{backupFileFullPath}'", cn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

            return backupFileFullPath;
        }
        public async Task<string> CopyDatabaseAsync(string databaseName, string cnString)
        {
            return await CopyDatabaseAsync(databaseName, $"{databaseName}Copy", cnString);
        }
        public async Task<string> CopyDatabaseAsync(string databaseName, string newDatabaseName, string cnString)
        {
            var backupFileFullPath = await BackupDatabaseAsync(databaseName, cnString);

            if (String.IsNullOrEmpty(backupFileFullPath))
            {
                throw new Exception($"Error copying database: Backup of {databaseName} failed.");
            }

            string restoreResult = await RestoreDatabaseAsync(backupFileFullPath, databaseName, newDatabaseName, cnString);

            return String.IsNullOrEmpty(restoreResult) ? null : newDatabaseName;
        }
        public async Task<string> RestoreDatabaseAsync(string backupFileFullPath, string databaseName, string cnString)
        {
            return await RestoreDatabaseAsync(backupFileFullPath, databaseName, databaseName, cnString);
        }
        public async Task<string> RestoreDatabaseAsync(string backupFileFullPath, string databaseName, string newDatabaseName, string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.InfoMessage += sqlConnectionn_InfoMessage;
                cn.Open();

                // Get the file paths from the backup.

                string dataLogicalName = null;
                string dataPhysicalPath = null;
                string logLogicalName = null;
                string logPhysicalPath = null;

                using (SqlCommand cmd = new SqlCommand($"RESTORE FILELISTONLY FROM DISK = '{backupFileFullPath}'", cn))
                {
                    using (var dr = await cmd.ExecuteReaderAsync())
                    {
                        // data file
                        while (dr.Read())
                        {
                            // LogicalName, PhysicalName, Type
                            var fileType = dr.GetString(2);
                            if (fileType == "D")
                            {
                                dataLogicalName = dr.GetString(0);
                                dataPhysicalPath = dr.GetString(1);
                            }
                            else if (fileType == "L")
                            {
                                logLogicalName = dr.GetString(0);
                                logPhysicalPath = dr.GetString(1);
                            }
                        }
                    }
                }

                string restoreCommandString = $"RESTORE DATABASE {newDatabaseName} FROM DISK = '{backupFileFullPath}'";
                string newDataPhysicalPath = null;
                string newLogPhysicalPath = null;

                var parameters = new List<SqlParameter>();

                if (databaseName != newDatabaseName)
                {

                    // Set the file paths for the restore
                    newDataPhysicalPath = Path.Combine(Path.GetDirectoryName(dataPhysicalPath), $"{newDatabaseName}{Path.GetExtension(dataPhysicalPath)}");
                    newLogPhysicalPath = Path.Combine(Path.GetDirectoryName(logPhysicalPath), $"{newDatabaseName}{Path.GetExtension(logPhysicalPath)}");

                    restoreCommandString += $" WITH MOVE '{dataLogicalName}' TO '{newDataPhysicalPath}', MOVE '{logLogicalName}' TO '{newLogPhysicalPath}'";
                }
                else
                {
                    newDataPhysicalPath = dataPhysicalPath;
                    newLogPhysicalPath = logPhysicalPath;
                }

                Console.WriteLine(restoreCommandString);

                using (SqlCommand cmd = new SqlCommand(restoreCommandString, cn))
                {
                    await cmd.ExecuteNonQueryAsync();

                    return newDatabaseName;
                }
            }
        }

        public async Task<string> GetDbVersionAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 Version FROM VersionInfo ORDER BY UpgradeDate DESC", cn))
                {
                    return (string)await cmd.ExecuteScalarAsync();
                }
            }
        }
        public async Task<string> GetSqlServerVersionAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT @@VERSION", cn))
                {
                    return (string)await cmd.ExecuteScalarAsync();
                }
            }
        }
        public async Task<string> GetLocationNameAsync(string cnString)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 LocName FROM ApplicationInfo", cn))
                {
                    return (string)await cmd.ExecuteScalarAsync();
                }
            }
        }
        #endregion

        #region protected
        protected virtual async Task<bool> UpdateUserLoginAsync(string cnString, int empNo)
        {
            using (SqlConnection cn = new SqlConnection(cnString))
            {
                return await UpdateUserLoginAsync(cn, empNo);
            }
        }
        protected virtual async Task<bool> UpdateUserLoginAsync(SqlConnection cn, int empNo)
        {
            using (SqlCommand cmd = new SqlCommand(UpdateMyEmployee, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@pwsalt", MyEmpPwSalt ));
                cmd.Parameters.Add(new SqlParameter("@pwhash", MyEmpPwHash));
                cmd.Parameters.Add(new SqlParameter("@pinsalt", MyEmpPinSalt));
                cmd.Parameters.Add(new SqlParameter("@pinhash", MyEmpPinHash));
                cmd.Parameters.Add(new SqlParameter("@empno", empNo));

                var result = await cmd.ExecuteNonQueryAsync();

                return (result == 1);
            }
        }
        protected virtual async Task<int?> InsertUserLoginAsync(SqlConnection cn)
        {
            using (SqlCommand cmd = new SqlCommand(AddMeAsEmployee, cn))
            {
                cmd.Parameters.Add(new SqlParameter("@firstname", MyEmpFirstName));
                cmd.Parameters.Add(new SqlParameter("@lastname", MyEmpLastName));
                cmd.Parameters.Add(new SqlParameter("@pwsalt", MyEmpPwSalt));
                cmd.Parameters.Add(new SqlParameter("@pwhash", MyEmpPwHash));
                cmd.Parameters.Add(new SqlParameter("@pinsalt", MyEmpPinSalt));
                cmd.Parameters.Add(new SqlParameter("@pinhash", MyEmpPinHash));

                var result = await cmd.ExecuteNonQueryAsync();

                return await GetUserEmployeeNumberAsync(cn);
            }
        }
        protected virtual async Task<int?> GetUserEmployeeNumberAsync(SqlConnection cn)
        {
            int? empNo = null;

            using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 EmpNo FROM Employees Where FirstName = @firstname AND LastName = @lastname", cn))
            {
                cmd.Parameters.Add(new SqlParameter("@firstname", MyEmpFirstName));
                cmd.Parameters.Add(new SqlParameter("@lastname", MyEmpLastName));

                var result = await cmd.ExecuteScalarAsync();

                if (result != null)
                    empNo = (int)result;
            }

            return empNo;
        }
        protected virtual async Task<IList<SecurityGroupModel>> GetSecurityGroupsListAsync(SqlConnection cn)
        {
            var models = new List<SecurityGroupModel>();

            using (SqlCommand cmd = new SqlCommand("SELECT [GroupID], [GroupName], [Description] FROM [dbo].[SecurityGroups] WHERE [Enabled] = 1", cn))
            {
                var dr = await cmd.ExecuteReaderAsync();

                while (dr.Read())
                {
                    models.Add(new SecurityGroupModel()
                    {
                        Id = dr.GetInt32(0),
                        Name = dr.GetString(1)
                    });
                }
            }

            return models;
        }
        protected virtual async Task<int?> GetAdministrationGroupIdAsync(SqlConnection cn)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT GroupId FROM SecurityGroups WHERE GroupName = 'Administration' OR GroupName = 'General Manager'", cn))
            {
                return (int?)await cmd.ExecuteScalarAsync();
            }
        }
        protected virtual async Task<bool> AddUserToAdministrationGroupAsync(SqlConnection cn, int empNo, int groupId)
        {
            bool userIsInAdminGroup = false;

            using (SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM SecurityGroupMembers WHERE GroupId = {groupId} AND EmpNo = {empNo}", cn))
            {
                var existingRowCount = (int)await cmd.ExecuteScalarAsync();
                userIsInAdminGroup = (existingRowCount == 1);
            }

            if (!userIsInAdminGroup)
            {
                using (SqlCommand cmd = new SqlCommand($"INSERT INTO SecurityGroupMembers (GroupId, EmpNo) VALUES ({groupId}, {empNo})", cn))
                {
                    var result = await cmd.ExecuteNonQueryAsync();
                    return (result == 1);
                }
            }
            else
            {
                return true;
            }
        }
        protected virtual void sqlConnectionn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Console.WriteLine(e.Message);

            foreach (SqlError sqlError in e.Errors)
            {
                Console.WriteLine(sqlError.Message);
                Console.WriteLine($"{sqlError.Server} {sqlError.Procedure} {sqlError.Number} {sqlError.LineNumber} {sqlError.Source}  {sqlError.Class} {sqlError.State}");
            }
        }
        #endregion
    }
}

using CE.DbConnect.Models;
using System;

namespace CE.DbConnectionHelper.ViewModels
{
    public class DbConnectionViewModel
    {
        public string Machine { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Version { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool IsEncrypted { get; set; }
        public Guid DatabaseId { get; set; }
        public Guid? ServerId { get; set; }
        public Guid? MachineId { get; set; }
        public Guid? SqlCredentialId { get; set; }

        internal DatabaseInstance DbModel { get; set; }

        public string KeyId
        {
            get
            {
                return $"{Machine?.ToUpper()}\\{Server?.ToUpper()}:{Database?.ToUpper()}:{UserId?.ToUpper()}:{Password?.ToUpper()}";
            }
        }

        public string DatabaseFullName
        {
            get
            {
                return $"{Machine}\\{Server}.{Database}";
            }
        }

        public string Title
        {
            get
            {
                return $"{DatabaseFullName,-48} CE:{Version,-12} {Location,-24} SQL:{DbModel.ServerInstance.SqlVersion}";
            }
        }

        public string SqlServerVersion
        {
            get
            {
                if (DbModel == null)
                    return "";
                else
                {
                    var fullVersion = DbModel.ServerInstance?.SqlVersion;
                    var express = fullVersion.Contains("Express") ? " [Express]" : "";
                    var firstPeriodLocation = fullVersion.IndexOf('.');
                    var truncateLocation = fullVersion.IndexOf(')', firstPeriodLocation);
                    var sqlVersion = fullVersion.Substring(0, truncateLocation + 1);
                    sqlVersion = sqlVersion.Replace("-", express);
                    return sqlVersion;
                }
            }
        }
    }
}

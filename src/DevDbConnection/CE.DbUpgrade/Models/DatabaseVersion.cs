using CE.DbUpgrade.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CE.DbUpgrade.Models
{
    class DatabaseVersion : IDatabaseVersion, IVersioned
    {
        #region properties
        public Version Version { get; set; }
        public IList<IDatabaseVersionScript> SqlScripts { get; set; }
        public bool HasPendingDbVersions
        {
            get
            {
                return SqlScripts.Any(s => s.Version.Build >= 100);
            }
        }
        #endregion

        #region ctor
        public DatabaseVersion(int major, int minor, int dbVersion)
        {
            Version = new Version(major, minor, dbVersion);

            SqlScripts = new List<IDatabaseVersionScript>();
        }
        #endregion
    }
}
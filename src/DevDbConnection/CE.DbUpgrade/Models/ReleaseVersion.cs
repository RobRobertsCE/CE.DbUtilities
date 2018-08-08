using System;
using System.Collections.Generic;
using System.Linq;

namespace CE.DbUpgrade.Models
{
    class ReleaseVersion : IReleaseVersion, IVersioned
    {
        private string _classFileContent;

        #region properties
        public Version Version { get; set; }
        public IList<IDatabaseVersion> DatabaseVersions { get; set; }
        public bool HasPendingDbVersions
        {
            get
            {
                return DatabaseVersions.Any(s => s.HasPendingDbVersions);
            }
        }
        #endregion

        #region ctor
        public ReleaseVersion(int major, int minor)
        {
            Version = new Version(major, minor);

            DatabaseVersions = new List<IDatabaseVersion>();
        }
        public ReleaseVersion(int major, int minor, string classFileContent)
        {
            Version = new Version(major, minor);

            DatabaseVersions = new List<IDatabaseVersion>();
            _classFileContent = classFileContent;
        }
        // TODO: Parse class file content
        #endregion
    }
}
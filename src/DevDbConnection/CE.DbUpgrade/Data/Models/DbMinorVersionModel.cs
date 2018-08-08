using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CE.DbUpgrade.Data.Models
{
    class DbMinorVersionModel
         : DbVersionBase
    {
        #region properties
        public FileSystemInfo ClassFile
        {
            get
            {
                return Files.FirstOrDefault(f => f.Extension == ".vb");
            }
        }
        public IList<DbVersionModel> DbVersions { get; set; }
        #endregion

        #region ctor
        public DbMinorVersionModel(string rootDirectory)
            : base(rootDirectory)
        {
            foreach (var dbVersionDirectory in RootDirectory.GetDirectories())
            {
                var dbVersion = new DbVersionModel(dbVersionDirectory);

                DbVersions.Add(dbVersion);
            }
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CE.DbUpgrade.Data.Models
{
    class DbVersionModel
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
        public IList<FileSystemInfo> SqlScripts
        {
            get
            {
                return Files.Where(f => f.Extension == ".sql").ToList();
            }
        }
        #endregion

        #region ctor
        public DbVersionModel(string rootDirectory)
            : base(rootDirectory)
        {

        }
        public DbVersionModel(DirectoryInfo rootDirectoryInfo)
          : base(rootDirectoryInfo)
        {

        }
        #endregion
    }
}

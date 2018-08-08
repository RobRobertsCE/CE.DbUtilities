using System;
using System.Collections.Generic;
using System.IO;

namespace CE.DbUpgrade.Data.Models
{
    abstract class DbVersionBase
    {
        #region properties
        protected virtual DirectoryInfo RootDirectory { get; set; }
        protected virtual IList<FileSystemInfo> Files { get; set; }
        public Version Version { get; set; }
        #endregion

        #region ctor
        protected DbVersionBase(string rootDirectory)
            : this(new DirectoryInfo(rootDirectory))
        {
        }
        protected DbVersionBase(DirectoryInfo rootDirectoryInfo)
        {
            if (!Directory.Exists(rootDirectoryInfo.FullName))
                throw new ArgumentException($"Directory '{rootDirectoryInfo.FullName}' does not exist");

            RootDirectory = rootDirectoryInfo;

            Files = RootDirectory.GetFileSystemInfos();
        }
        #endregion
    }
}

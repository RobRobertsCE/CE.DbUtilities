using System;

namespace CE.DbUpgrade.Models
{
    class DatabaseVersionCallbackScript
         : DatabaseVersionScript, IDatabaseVersionScript
    {
        #region properties
        public Version ParentVersion { get; set; }
        public override bool IsCallbackVersion { get { return true; } }
        #endregion

        #region ctor
        public DatabaseVersionCallbackScript()
            : base()
        {
        }
        public DatabaseVersionCallbackScript(Version parentVersion, string name)
            : base(name)
        {
            ParentVersion = parentVersion == null ? throw new ArgumentNullException(nameof(parentVersion)) : parentVersion;
        }
        #endregion
    }
}

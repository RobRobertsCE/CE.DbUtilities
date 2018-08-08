using System;

namespace CE.DbConnectionHelper.ViewModels
{
    class DbUpgradeScriptViewModel
    {
        public Version Version { get; set; }
        public string Script { get; set; }
        public string Name { get; set; }
        public int Result { get; set; }
    }
}

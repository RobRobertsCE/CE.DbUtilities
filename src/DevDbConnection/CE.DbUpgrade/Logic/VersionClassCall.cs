using System;

namespace CE.DbUpgrade.Logic
{
    public class VersionClassCall
    {
        public Version Version { get; private set; }
        public string Line { get; private set; }

        public VersionClassCall(string line)
        {
            // New Upgrade18_2_0(),
            var lineSections = line.Replace("New Upgrade", "").Replace("()", "").Replace(",", "").Split('_');

            var major = int.Parse(lineSections[0]);
            var minor = int.Parse(lineSections[1]);
            var dbVersion = int.Parse(lineSections[2]);

            Version = new Version(major, minor, dbVersion);
        }
    }
}

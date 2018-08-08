using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CE.DbUpgrade.Logic
{
    static class ReleaseClassFileParser
    {
        static IList<VersionClassCall> ParseReleaseClassFile(string filePath, Version version)
        {
            var versionClassCalls = new List<VersionClassCall>();

            var lines = File.ReadAllLines(filePath);

            var token = $"New Upgrade{version.Major}_{version.Minor}";

            foreach (var line in lines.Select(l => l.Trim()))
            {
                if (line.Trim().StartsWith(token))
                {
                    versionClassCalls.Add(new VersionClassCall(line));
                }
            }

            return versionClassCalls;
        }
    }
}

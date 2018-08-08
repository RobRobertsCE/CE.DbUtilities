using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CE.DbUpgrade.Logic
{
    static class DatabaseVersionClassFileParser
    {
        internal static IList<SqlScriptCall> ParseDatabaseVersionClassFile(string filePath, Version version)
        {
            var sqlScriptCalls = new List<SqlScriptCall>();

            var fileName = $"Upgrade.{version.ToString()}.vb";
            var fullFilePath = Path.Combine(filePath, fileName);

            var lines = File.ReadAllLines(fullFilePath);

            if (!lines.Any(l => l.Contains("DataPumpHelper")))
            {
                var token = $"SetMessage(";

                for (int i = 0; i < lines.Count() - 1; i++)
                {
                    var line = lines[i];
                    if (line.Trim().StartsWith(token))
                    {
                        if (lines[i + 1].Trim() != "ProcessSecurityRights()")
                        {
                            sqlScriptCalls.Add(new SqlScriptCall(new List<string>() { line, lines[i + 1] }));
                        }
                        i++;
                    }
                }
            }
            return sqlScriptCalls;
        }
    }
}

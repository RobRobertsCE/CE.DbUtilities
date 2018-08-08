using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CE.DbConnectionHelper.ViewModels
{
    class DbUpgradeViewModel
    {
        public const string UpgradeScriptDirectory = @"C:\Users\rroberts\Source\Repos\Advantage\src\AdvUpgrade\AdvUpgrade";
        public const string UpgradeClassFileTemplate = @"C:\Users\rroberts\Source\Repos\Advantage\src\AdvUpgrade\AdvUpgrade\{0}.{1}\Upgrade.{0}.{1}.{2}.vb";
        public const string CallbackScriptFileTemplate = @"C:\Users\rroberts\Source\Repos\Advantage\src\AdvUpgrade\AdvUpgrade\{1}.{2}\{0}.{1}.{2}.{3}.sql";
        public const string BranchVersionFile = @"C:\Users\rroberts\Source\Repos\Advantage\src\Version.txt";

        public Version BranchVersion
        {
            get
            {
                var content = File.ReadAllText(BranchVersionFile);
                return new Version(content);
            }
        }

        public IList<DbUpgradeScriptViewModel> GetUpgradeScripts()
        {
            return GetUpgradeScripts(new Version(16, 1, 0, 0), BranchVersion);
        }

        public IList<DbUpgradeScriptViewModel> GetUpgradeScripts(Version startingVersion)
        {
            return GetUpgradeScripts(startingVersion, BranchVersion);
        }

        public IList<DbUpgradeScriptViewModel> GetUpgradeScripts(Version startingVersion, Version targetVersion)
        {
            var upgrades = new List<DbUpgradeScriptViewModel>();

            var scriptMinorDirectories = new DirectoryInfo(UpgradeScriptDirectory);

            foreach (var scriptMinorDirectory in scriptMinorDirectories.GetDirectories().Where(d => d.Name.Contains('.')))
            {
                var minorDirectoryNameVersion = new Version(scriptMinorDirectory.Name);

                if (minorDirectoryNameVersion >= startingVersion && minorDirectoryNameVersion <= targetVersion)
                {
                    var scriptDbVersionDirectories = scriptMinorDirectory.GetDirectories();

                    foreach (var scriptDbVersionDirectory in scriptMinorDirectory.GetDirectories().Where(d => d.Name.Contains('.')))
                    {
                        var versionDirectoryNameVersion = new Version(scriptDbVersionDirectory.Name);

                        if (versionDirectoryNameVersion >= startingVersion && minorDirectoryNameVersion <= targetVersion)
                        {
                            var scriptDbVersionFiles = scriptDbVersionDirectory.GetFiles("*.sql");

                            foreach (var scriptDbVersionFile in scriptDbVersionFiles)
                            {
                                var upgradeScriptModel = new DbUpgradeScriptViewModel()
                                {
                                    Version = versionDirectoryNameVersion,
                                    Script = File.ReadAllText(scriptDbVersionFile.FullName),
                                    Name = scriptDbVersionFile.Name.Substring(0, scriptDbVersionFile.Name.IndexOf("."))
                                };

                                upgrades.Add(upgradeScriptModel);
                            }

                            // get callbacks
                            var versionClassFile = String.Format(UpgradeClassFileTemplate, minorDirectoryNameVersion.Major, minorDirectoryNameVersion.Minor, minorDirectoryNameVersion.Revision);

                            if (File.Exists(versionClassFile))
                            {
                                foreach (var line in File.ReadAllLines(versionClassFile))
                                {
                                    if (line.Contains("ProcessQueryByNameAndVersion"))
                                    {
                                        var lineTokens = line.Split('"');
                                        var callbackVersion = new Version(lineTokens[3]);

                                        var callbackScript = String.Format(CallbackScriptFileTemplate,
                                            lineTokens[1],
                                            callbackVersion.Major,
                                            callbackVersion.Minor,
                                            callbackVersion.Revision);

                                        var upgradeScriptModel = new DbUpgradeScriptViewModel()
                                        {
                                            Version = versionDirectoryNameVersion,
                                            Script = File.ReadAllText(callbackScript)
                                        };

                                        upgrades.Add(upgradeScriptModel);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return upgrades;
        }

        public virtual async Task<bool> ApplyUpgradeScriptsAsync(string cnString, IList<DbUpgradeScriptViewModel> upgrades)
        {
            var result = false;

            try
            {
                using (var cn = new SqlConnection(cnString))
                {
                    cn.Open();

                    SqlTransaction trans = cn.BeginTransaction("Upgrade DB");

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = cn;
                        cmd.Transaction = trans;

                        try
                        {
                            foreach (var upgrade in upgrades)
                            {
                                cmd.CommandText = upgrade.Script;
                                var upgradeResult = await cmd.ExecuteNonQueryAsync();
                                upgrade.Result = upgradeResult;
                            }

                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                            Console.WriteLine("  Message: {0}", ex.Message);

                            try
                            {
                                trans.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                // This catch block will handle any errors that may have occurred 
                                // on the server that would cause the rollback to fail, such as 
                                // a closed connection.
                                Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                                Console.WriteLine("  Message: {0}", ex2.Message);
                            }
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Upgrade Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }

            return result;
        }
    }
}

using CE.DbUpgrade.Logic;
using CE.DbUpgrade.Models;
using CE.DbUpgrade.Ports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CE.DbUpgrade.Data
{
    public class DbUpgradeRepository : IDbUpgradeRepository
    {
        #region consts
        private const string DefaultVersionFile = @"src\version.txt";
        private const string DefaultReleaseDirectory = @"src\AdvUpgrade\AdvUpgrade";
        private const string DefaultRepositoryDirectory = @"C:\Users\rroberts\Source\Repos\Advantage\";
        #endregion

        #region properties
        protected string RepositoryRoot { get; set; } = DefaultRepositoryDirectory;
        protected string VersionFilePath { get; set; }
        protected string ReleaseDirectoryRoot { get; set; }
        #endregion

        #region ctor
        public DbUpgradeRepository()
            : this(DefaultRepositoryDirectory)
        {
        }
        public DbUpgradeRepository(string repositoryDirectory)
            : this(DefaultRepositoryDirectory,
                  Path.Combine(repositoryDirectory, DefaultVersionFile),
                  Path.Combine(repositoryDirectory, DefaultReleaseDirectory))
        {
        }
        public DbUpgradeRepository(string repositoryDirectory,
                                    string versionFile,
                                    string releaseDirectory)
        {
            RepositoryRoot = repositoryDirectory;
            VersionFilePath = versionFile;
            ReleaseDirectoryRoot = releaseDirectory;
        }
        #endregion

        #region public
        public virtual Version GetBranchVersion()
        {
            var versionFileContent = File.ReadAllText(VersionFilePath).Trim();

            return Version.Parse(versionFileContent);
        }
        public virtual void SetBranchVersion(Version version)
        {
            if (version == null)
                throw new ArgumentException("No version specified");

            File.WriteAllText(VersionFilePath, version.ToString());
        }

        public virtual IList<IReleaseVersion> GetReleases()
        {
            return GetReleaseVersions();
        }
        public virtual IList<IReleaseVersion> GetReleases(Version minimumVersion)
        {
            return GetReleaseVersions(minimumVersion);
        }

        public virtual IReleaseVersion GetVersion(int major, int minor)
        {
            return GetReleaseVersion(major, minor);
        }
        public virtual IDatabaseVersion GetVersion(int major, int minor, int dbVersion)
        {
            return GetDatabaseVersion(major, minor, dbVersion);
        }

        public virtual void SaveVersion(IReleaseVersion version)
        {
            throw new NotImplementedException();
        }
        public virtual void SaveVersion(IDatabaseVersion version)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region protected
        #region load version
        protected virtual IList<IReleaseVersion> GetReleaseVersions()
        {
            return GetReleaseVersions(new Version(15, 0));
        }
        protected virtual IList<IReleaseVersion> GetReleaseVersions(Version minimumVersion)
        {
            IList<IReleaseVersion> releases = new List<IReleaseVersion>();
            var rootDirectoryInfo = new DirectoryInfo(ReleaseDirectoryRoot);

            foreach (var releaseVersionPath in rootDirectoryInfo.EnumerateDirectories().Where(d => d.Name.Contains('.')).ToList())
            {
                var version = GetMinorVersionFromPath(releaseVersionPath.Name);

                if (version.Major >= minimumVersion.Major)
                {
                    if (version.Major > minimumVersion.Major || (version.Major == minimumVersion.Major && version.Minor >= minimumVersion.Minor))
                    {
                        var release = GetReleaseVersion(version, releaseVersionPath.FullName);

                        releases.Add(release);
                    }
                }
            }
            return releases;
        }

        protected virtual IReleaseVersion GetReleaseVersion(int major, int minor)
        {
            IReleaseVersion release = new ReleaseVersion(major, minor);
            release.DatabaseVersions = GetDatabaseVersions(major, minor);
            return release;
        }
        protected virtual IReleaseVersion GetReleaseVersion(Version version, string releaseVersionPath)
        {
            IReleaseVersion release = new ReleaseVersion(version.Major, version.Minor);

            release.DatabaseVersions = GetDatabaseVersions(releaseVersionPath);

            return release;
        }

        protected virtual IList<IDatabaseVersion> GetDatabaseVersions(int major, int minor)
        {
            var releaseVersionPath = Path.Combine(ReleaseDirectoryRoot, $"{major}.{minor}");
            return GetDatabaseVersions(releaseVersionPath);
        }
        protected virtual IList<IDatabaseVersion> GetDatabaseVersions(string releaseVersionPath)
        {
            var databaseVersions = new List<IDatabaseVersion>();
            foreach (var databaseVersionPath in Directory.GetDirectories(releaseVersionPath))
            {
                var databaseVersion = GetDatabaseVersion(databaseVersionPath);
                databaseVersions.Add(databaseVersion);
            }
            return databaseVersions;
        }

        protected virtual IDatabaseVersion GetDatabaseVersion(int major, int minor, int dbVersion)
        {
            IDatabaseVersion databaseVersion = new DatabaseVersion(major, minor, dbVersion);

            var releaseVersionPath = Path.Combine(ReleaseDirectoryRoot, $"{major}.{minor}");
            var databaseVersionPath = Path.Combine(releaseVersionPath, $"{major}.{minor}.{dbVersion}");

            var sqlScriptCalls = DatabaseVersionClassFileParser.ParseDatabaseVersionClassFile(databaseVersionPath, databaseVersion.Version);

            databaseVersion.SqlScripts = GetDatabaseVersionScripts(major, minor, dbVersion, sqlScriptCalls);

            return databaseVersion;
        }
        protected virtual IDatabaseVersion GetDatabaseVersion(string databaseVersionPath)
        {
            var version = GetDbVersionFromPath(databaseVersionPath);

            IDatabaseVersion databaseVersion = new DatabaseVersion(version.Major, version.Minor, version.Build);

            var sqlScriptCalls = DatabaseVersionClassFileParser.ParseDatabaseVersionClassFile(databaseVersionPath, databaseVersion.Version);

            databaseVersion.SqlScripts = GetDatabaseVersionScripts(databaseVersionPath, sqlScriptCalls);

            return databaseVersion;
        }

        protected virtual IList<IDatabaseVersionScript> GetDatabaseVersionScripts(int major, int minor, int dbVersion, IList<SqlScriptCall> sqlScriptCalls)
        {
            var releaseVersionPath = Path.Combine(ReleaseDirectoryRoot, $"{major}.{minor}");
            var databaseVersionPath = Path.Combine(releaseVersionPath, $"{major}.{minor}.{dbVersion}");

            var scripts = GetDatabaseVersionScripts(databaseVersionPath, sqlScriptCalls);

            if (sqlScriptCalls.Count > 0)
                throw new Exception($"{sqlScriptCalls.Count} unaccounted for script call(s)");

            return scripts;
        }
        protected virtual IList<IDatabaseVersionScript> GetDatabaseVersionScripts(string databaseVersionPath, IList<SqlScriptCall> sqlScriptCalls)
        {
            var scripts = new List<IDatabaseVersionScript>();

            try
            {
                var scriptFiles = Directory.GetFiles(databaseVersionPath, "*.sql");

                if (scriptFiles.Count() > 0)
                {
                    foreach (var sqlScriptPath in Directory.GetFiles(databaseVersionPath, "*.sql"))
                    {
                        scripts.Add(GetDatabaseVersionScript(sqlScriptPath, sqlScriptCalls));
                    }
                }

                // all remaining script calls should be callbacks.
                if (sqlScriptCalls.Any(s => s.CallbackVersion.Major == 0))
                {
                    throw new Exception($"Could not find a call to {sqlScriptCalls.FirstOrDefault(s => s.CallbackVersion.Major == 0)?.Name} from the class file.");
                }

                foreach (var callback in sqlScriptCalls.Where(s => s.CallbackVersion.Major == 0).ToList())
                {
                    scripts.Add(GetDatabaseVersionScript(callback));

                    sqlScriptCalls.Remove(callback);
                }

                if (sqlScriptCalls.Count > 0)
                    throw new Exception($"{sqlScriptCalls.Count} unaccounted for script call(s)");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return scripts;
        }
        protected virtual IDatabaseVersionScript GetDatabaseVersionScript(SqlScriptCall sqlScriptCallback)
        {
            // ProcessQueryByNameAndVersion("Areas_Sold", "18.1.48")

            var releaseVersionPath = Path.Combine(ReleaseDirectoryRoot, $"{sqlScriptCallback.CallbackVersion.Major}.{sqlScriptCallback.CallbackVersion.Minor}");
            var databaseVersionPath = Path.Combine(releaseVersionPath, $"{sqlScriptCallback.CallbackVersion.Major}.{sqlScriptCallback.CallbackVersion.Minor}.{sqlScriptCallback.CallbackVersion.Build}");
            var databaseVersionScriptPath = Path.Combine(databaseVersionPath, $"{sqlScriptCallback.Name}.{sqlScriptCallback.CallbackVersion.Major}.{sqlScriptCallback.CallbackVersion.Minor}.{sqlScriptCallback.CallbackVersion.Build}.sql");

            var fileName = Path.GetFileName(databaseVersionScriptPath);

            var script = new DatabaseVersionScript(fileName);

            return script;
        }

        protected virtual IDatabaseVersionScript GetDatabaseVersionScript(string databaseVersionScriptPath, IList<SqlScriptCall> sqlScriptCalls)
        {

            var fileName = Path.GetFileName(databaseVersionScriptPath);

            var script = new DatabaseVersionScript(fileName);

            try
            {
                var scriptCall = sqlScriptCalls.FirstOrDefault(s => s.Name == script.Target);

                if (scriptCall == null)
                {
                    throw new Exception($"Could not find a call to {script.Target} from the class file.");
                }
                script.ActionDescription = scriptCall.Action;

                sqlScriptCalls.Remove(scriptCall);

                script.SqlStatement = File.ReadAllText(databaseVersionScriptPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return script;
        }
        #endregion

        #region save version
        protected virtual void SaveReleaseVersions(IList<IReleaseVersion> releaseVersions)
        {
            foreach (var releaseVersion in releaseVersions)
            {
                SaveReleaseVersion(releaseVersion);
            }
        }
        protected virtual void SaveReleaseVersion(IReleaseVersion releaseVersion)
        {
            // Save the release version here

            SaveDatabaseVersions(releaseVersion.DatabaseVersions);
        }
        protected virtual void SaveDatabaseVersions(IList<IDatabaseVersion> databaseVersions)
        {
            foreach (var databaseVersion in databaseVersions)
            {
                SaveDatabaseVersion(databaseVersion);
            }
        }
        protected virtual void SaveDatabaseVersion(IDatabaseVersion databaseVersion)
        {
            // Save the database version here

            SaveDatabaseVersionScripts(databaseVersion.SqlScripts);
        }

        protected virtual void SaveDatabaseVersionScripts(IList<IDatabaseVersionScript> scripts)
        {
            foreach (var script in scripts)
            {
                SaveDatabaseVersionScript(script);
            }
        }
        protected virtual void SaveDatabaseVersionScript(IDatabaseVersionScript script)
        {
            // Save the database version script here
        }
        #endregion
        #endregion

        #region private
        private Version GetMinorVersionFromPath(string path)
        {
            var pathSections = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var versionSections = pathSections.LastOrDefault().Split('.');

            var major = int.Parse(versionSections[0]);
            var minor = int.Parse(versionSections[1]);

            return new Version(major, minor);
        }
        private Version GetDbVersionFromPath(string path)
        {
            var pathSections = new List<string>(path.Split(Path.DirectorySeparatorChar));
            var versionSections = pathSections.LastOrDefault().Split('.');

            var major = int.Parse(versionSections[0]);
            var minor = int.Parse(versionSections[1]);
            var dbVersion = int.Parse(versionSections[2]);

            return new Version(major, minor, dbVersion);
        }
        #endregion

    }
}

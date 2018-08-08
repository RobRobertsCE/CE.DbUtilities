using CE.DbUpgrade.Models;
using System;
using System.Collections.Generic;

namespace CE.DbUpgrade.Ports
{
    public interface IDbUpgradeRepository
    {
        Version GetBranchVersion();
        void SetBranchVersion(Version version);
        IList<IReleaseVersion> GetReleases();
        IList<IReleaseVersion> GetReleases(Version minimumVersion);
        IReleaseVersion GetVersion(int major, int minor);
        IDatabaseVersion GetVersion(int major, int minor, int dbVersion);
        void SaveVersion(IDatabaseVersion version);
        void SaveVersion(IReleaseVersion version);
    }
}
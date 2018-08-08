using System;
using System.Collections.Generic;

namespace CE.DbUpgrade.Models
{
    public interface IReleaseVersion
    {
        IList<IDatabaseVersion> DatabaseVersions { get; set; }
        bool HasPendingDbVersions { get; }
        Version Version { get; set; }
    }
}
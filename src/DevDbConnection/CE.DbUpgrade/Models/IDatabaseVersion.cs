using System;
using System.Collections.Generic;

namespace CE.DbUpgrade.Models
{
    public interface IDatabaseVersion
    {
        bool HasPendingDbVersions { get; }
        IList<IDatabaseVersionScript> SqlScripts { get; set; }
        Version Version { get; set; }
    }
}
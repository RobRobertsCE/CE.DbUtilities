using System;

namespace CE.DbUpgrade.Models
{
    public interface IVersioned
    {
        Version Version { get; }
    }
}

using System;

namespace CE.DbUpgrade.Models
{
    public interface IDatabaseVersionScript
    {
        bool IsCallbackVersion { get; }
        string Name { get; }
        string Target { get; }
        Version Version { get; }
        string ActionDescription { get; set; }
        string SqlStatement { get; set; }
    }
}
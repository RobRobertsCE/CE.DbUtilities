using System.Data.Entity;
using CE.DbConnect.Models;

namespace CE.DbConnect.Data
{
    public interface IDevelopmentContext
    {
        DbSet<SqlCredentials> Credentials { get; set; }
        DbSet<DatabaseInstance> Databases { get; set; }
        DbSet<MachineInstance> Machines { get; set; }
        DbSet<ServerInstance> Servers { get; set; }
    }
}
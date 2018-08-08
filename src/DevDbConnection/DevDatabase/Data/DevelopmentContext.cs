using CE.DbConnect.Models;
using System.Data.Entity;

namespace CE.DbConnect.Data
{
    public class DevelopmentContext
        : DbContext, IDevelopmentContext
    {
        public const string cn = @"Server=ROB-PC\SQLEXPRESS;Database=Development;User Id=sa;Password=sql;";

        public virtual DbSet<DatabaseInstance> Databases { get; set; }
        public virtual DbSet<MachineInstance> Machines { get; set; }
        public virtual DbSet<ServerInstance> Servers { get; set; }
        public virtual DbSet<SqlCredentials> Credentials { get; set; }

        public DevelopmentContext()
            : base(cn)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<DevelopmentContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}

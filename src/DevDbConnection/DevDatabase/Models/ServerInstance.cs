using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CE.DbConnect.Models
{
    [Table("Servers")]
    public class ServerInstance
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ServerId { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public String SqlVersion { get; set; }
        [ForeignKey("MachineInstance")]
        public virtual Guid MachineId { get; set; }
        public virtual MachineInstance MachineInstance { get; set; }
        public virtual IList<DatabaseInstance> Databases { get; set; }

        public ServerInstance()
        {
            Databases = new List<DatabaseInstance>();
        }
    }
}

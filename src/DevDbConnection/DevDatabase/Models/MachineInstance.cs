using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CE.DbConnect.Models
{
    [Table("Machines")]
    public class MachineInstance
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid MachineId { get; set; }
        public string Name { get; set; }
        public virtual IList<ServerInstance> ServerInstances { get; set; }

        public MachineInstance()
        {
            ServerInstances = new List<ServerInstance>();
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CE.DbConnect.Models
{
    [Table("Databases")]
    public class DatabaseInstance
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid DatabaseId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        [ForeignKey("ServerInstance")]
        public virtual Guid? ServerId { get; set; }
        public virtual ServerInstance ServerInstance { get; set; }
        [ForeignKey("SqlCredential")]
        public virtual Guid? SqlCredentialId { get; set; }
        public virtual SqlCredentials SqlCredential { get; set; }
    }
}

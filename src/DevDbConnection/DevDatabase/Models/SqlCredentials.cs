using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CE.DbConnect.Models
{
    [Table("Credentials")]
    public class SqlCredentials
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid SqlCredentialId { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool UseIntegratedSecurity { get; set; }
        public bool UseEncryption { get; set; }
    }
}

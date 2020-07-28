using EFTest.Configs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Entities
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        [Key]
        public int UserId { get; set; }
        [Column("user_name")]
        [Required]
        [StringLength(40)]
        public string UserName { get; set; }
        [Column("password")]
        [Required]
        [StringLength(40)]
        public string Password { get; set; }
        [Column("user_type")]
        public int UserType { get; set; }
    }
}

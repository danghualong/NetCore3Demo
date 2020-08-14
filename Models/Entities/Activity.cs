using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Entities
{
    [Table("activity")]
    public class Activity
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }
        [Column("activity_name",TypeName ="varchar")]
        [Required]
        [StringLength(40)]
        public string ActivityName { get; set; }
        [Column("remark", TypeName = "varchar")]
        [StringLength(100)]
        public string Summary { get; set; }
        [Column("status", TypeName = "tinyint")]
        public int Status { get; set; } = 0;
    }
}

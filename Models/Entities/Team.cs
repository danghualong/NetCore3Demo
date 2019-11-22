using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EFTest.Models.Entities
{
    [Table("team")]
    public class Team
    {
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("activity_id")]
        public int ActivityId { get; set; }
        [Column("team_name", TypeName = "varchar")]
        [Required]
        [StringLength(20)]
        public string TeamName { get; set; }
        [Column("team_order")]
        public int Order { get; set; } = 0;
        [Column("remark", TypeName = "varchar")]
        [StringLength(100)]
        public string Summary { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnAPI.Modal
{
    public class CreateUser
    {
        [Column("code")]
        [StringLength(50)]
        [Unicode(false)]
        public string Code { get; set; } 

        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } 

        [Column("email")]
        [StringLength(100)]
        [Unicode(false)]
        public string? Email { get; set; }

        [Column("phone")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }

        [Column("password")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Password { get; set; }

        [Column("isactive")]
        public bool? Isactive { get; set; }

        [Column("role")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Role { get; set; }
    }
}

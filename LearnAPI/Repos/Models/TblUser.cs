using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LearnAPI.Repos.Models;

[Table("tbl_user")]
public partial class TblUser
{
    [Key]
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("email")]
    [StringLength(50)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(50)]
    public string? Phone { get; set; }

    [Column("password")]
    [StringLength(50)]
    public string? Password { get; set; }

    [Column("isactive")]
    [StringLength(50)]
    public string? Isactive { get; set; }

    [Column("role")]
    [StringLength(50)]
    public string? Role { get; set; }
}

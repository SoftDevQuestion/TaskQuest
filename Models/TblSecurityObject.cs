using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityObject", Schema = "mdm")]
public partial class TblSecurityObject
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    [StringLength(128)]
    public string? ViewName { get; set; }

    public bool IsActive { get; set; }
}

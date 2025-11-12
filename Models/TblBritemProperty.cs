using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblBRItemProperties", Schema = "mdm")]
[Index("BritemId", Name = "ix_tblBRItemProperties_BRItem_ID")]
[Index("Muid", Name = "ux_tblBRItemProperties_MUID", IsUnique = true)]
public partial class TblBritemProperty
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BRItem_ID")]
    public int BritemId { get; set; }

    [Column("PropertyType_ID")]
    public int PropertyTypeId { get; set; }

    [Column("PropertyName_ID")]
    public int PropertyNameId { get; set; }

    [StringLength(999)]
    public string Value { get; set; } = null!;

    public int Sequence { get; set; }

    public bool IsLeftHandSide { get; set; }

    [Column("Parent_ID")]
    public int? ParentId { get; set; }

    public bool? SuppressText { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("BritemId")]
    [InverseProperty("TblBritemProperties")]
    public virtual TblBritem Britem { get; set; } = null!;
}

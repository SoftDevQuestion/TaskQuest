using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblBRItem", Schema = "mdm")]
[Index("BritemAppliesToId", Name = "ix_tblBRItem_BRItemAppliesTo_ID")]
[Index("BrlogicalOperatorGroupId", Name = "ix_tblBRItem_BRLogicalOperatorGroup_ID")]
[Index("Muid", Name = "ux_tblBRItem_MUID", IsUnique = true)]
public partial class TblBritem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BRLogicalOperatorGroup_ID")]
    public int BrlogicalOperatorGroupId { get; set; }

    [Column("BRItemAppliesTo_ID")]
    public int BritemAppliesToId { get; set; }

    public int Sequence { get; set; }

    public string ItemText { get; set; } = null!;

    [StringLength(250)]
    public string? AnchorName { get; set; }

    public int? AnchorAttributeType { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("BritemAppliesToId")]
    [InverseProperty("TblBritems")]
    public virtual TblBritemTypeAppliesTo BritemAppliesTo { get; set; } = null!;

    [ForeignKey("BrlogicalOperatorGroupId")]
    [InverseProperty("TblBritems")]
    public virtual TblBrlogicalOperatorGroup BrlogicalOperatorGroup { get; set; } = null!;

    [InverseProperty("Britem")]
    public virtual ICollection<TblBritemProperty> TblBritemProperties { get; set; } = new List<TblBritemProperty>();
}

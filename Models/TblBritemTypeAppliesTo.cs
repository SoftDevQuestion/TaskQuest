using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblBRItemTypeAppliesTo", Schema = "mdm")]
[Index("ApplyToId", Name = "ix_tblBRItemTypeAppliesTo_ApplyTo_ID")]
[Index("BritemTypeId", Name = "ix_tblBRItemTypeAppliesTo_BRItemType_ID")]
public partial class TblBritemTypeAppliesTo
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BRItemType_ID")]
    public int BritemTypeId { get; set; }

    [Column("ApplyTo_ID")]
    public int ApplyToId { get; set; }

    public int? Sequence { get; set; }

    [ForeignKey("ApplyToId")]
    [InverseProperty("TblBritemTypeAppliesTos")]
    public virtual TblListRelationship ApplyTo { get; set; } = null!;

    [InverseProperty("BritemAppliesTo")]
    public virtual ICollection<TblBritem> TblBritems { get; set; } = new List<TblBritem>();
}

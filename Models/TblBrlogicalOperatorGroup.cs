using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblBRLogicalOperatorGroup", Schema = "mdm")]
[Index("BusinessRuleId", Name = "ix_tblBRLogicalOperatorGroup_BusinessRule_ID")]
[Index("Muid", Name = "ux_tblBRLogicalOperatorGroup_MUID", IsUnique = true)]
public partial class TblBrlogicalOperatorGroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("LogicalOperator_ID")]
    public int? LogicalOperatorId { get; set; }

    [Column("Parent_ID")]
    public int? ParentId { get; set; }

    [Column("BusinessRule_ID")]
    public int BusinessRuleId { get; set; }

    public int Sequence { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("BusinessRuleId")]
    [InverseProperty("TblBrlogicalOperatorGroups")]
    public virtual TblBrbusinessRule BusinessRule { get; set; } = null!;

    [InverseProperty("BrlogicalOperatorGroup")]
    public virtual ICollection<TblBritem> TblBritems { get; set; } = new List<TblBritem>();
}

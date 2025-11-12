using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblBRBusinessRule", Schema = "mdm")]
[Index("EntityId", "MemberTypeId", Name = "ix_tblBRBusinessRule_Entity_ID_MemberType_ID")]
[Index("Muid", Name = "ux_tblBRBusinessRule_MUID", IsUnique = true)]
public partial class TblBrbusinessRule
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    public string? RuleConditionText { get; set; }

    public string? RuleActionText { get; set; }

    public string? RuleElseActionText { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [Column("Status_ID")]
    public int StatusId { get; set; }

    public int? Priority { get; set; }

    [Column("NotificationGroupID")]
    public int? NotificationGroupId { get; set; }

    [Column("NotificationUserID")]
    public int? NotificationUserId { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblBrbusinessRules")]
    public virtual TblEntity Entity { get; set; } = null!;

    [InverseProperty("BusinessRule")]
    public virtual ICollection<TblBrlogicalOperatorGroup> TblBrlogicalOperatorGroups { get; set; } = new List<TblBrlogicalOperatorGroup>();
}

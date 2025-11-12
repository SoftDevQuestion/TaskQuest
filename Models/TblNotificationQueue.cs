using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblNotificationQueue", Schema = "mdm")]
[Index("MemberTypeId", Name = "ix_tblNotificationQueue_MemberType_ID")]
[Index("ModelId", Name = "ix_tblNotificationQueue_Model_ID")]
[Index("NotificationTypeId", Name = "ix_tblNotificationQueue_NotificationType_ID")]
[Index("VersionId", Name = "ix_tblNotificationQueue_Version_ID")]
public partial class TblNotificationQueue
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("NotificationType_ID")]
    public int NotificationTypeId { get; set; }

    [Column("Version_ID")]
    public int? VersionId { get; set; }

    [Column("Model_ID")]
    public int? ModelId { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Member_ID")]
    public int? MemberId { get; set; }

    [Column("MemberType_ID")]
    public byte? MemberTypeId { get; set; }

    public string Message { get; set; } = null!;

    [Column("BRBusinessRule_ID")]
    public int? BrbusinessRuleId { get; set; }

    [StringLength(255)]
    public string? PriorityRank { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("DueDTM")]
    [Precision(3)]
    public DateTime? DueDtm { get; set; }

    [Column("SentDTM")]
    [Precision(3)]
    public DateTime? SentDtm { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("TblNotificationQueues")]
    public virtual TblModel? Model { get; set; }

    [ForeignKey("VersionId")]
    [InverseProperty("TblNotificationQueues")]
    public virtual TblModelVersion? Version { get; set; }
}

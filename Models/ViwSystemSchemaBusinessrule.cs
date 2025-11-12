using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaBusinessrule
{
    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [Column("BusinessRule_ID")]
    public int BusinessRuleId { get; set; }

    [Column("BusinessRule_MUID")]
    public Guid BusinessRuleMuid { get; set; }

    [Column("BusinessRule_Name")]
    [StringLength(50)]
    public string BusinessRuleName { get; set; } = null!;

    [Column("BusinessRule_Description")]
    [StringLength(255)]
    public string? BusinessRuleDescription { get; set; }

    [Column("BusinessRule_RuleConditionText")]
    public string? BusinessRuleRuleConditionText { get; set; }

    [Column("BusinessRule_RuleActionText")]
    public string? BusinessRuleRuleActionText { get; set; }

    [Column("BusinessRule_RuleElseActionText")]
    public string? BusinessRuleRuleElseActionText { get; set; }

    [Column("BusinessRule_StatusID")]
    public int BusinessRuleStatusId { get; set; }

    [Column("BusinessRule_StatusName")]
    [StringLength(250)]
    public string BusinessRuleStatusName { get; set; } = null!;

    [Column("BusinessRule_Priority")]
    public int? BusinessRulePriority { get; set; }

    [Column("BusinessRule_NotificationGroupID")]
    public int? BusinessRuleNotificationGroupId { get; set; }

    [Column("BusinessRule_NotificationGroupMUID")]
    public Guid? BusinessRuleNotificationGroupMuid { get; set; }

    [Column("BusinessRule_NotificationGroupName")]
    [StringLength(355)]
    public string? BusinessRuleNotificationGroupName { get; set; }

    [Column("BusinessRule_NotificationUserID")]
    public int? BusinessRuleNotificationUserId { get; set; }

    [Column("BusinessRule_NotificationUserMUID")]
    public Guid? BusinessRuleNotificationUserMuid { get; set; }

    [Column("BusinessRule_NotificationUserName")]
    [StringLength(100)]
    public string? BusinessRuleNotificationUserName { get; set; }

    [Column("BusinessRule_LastChgTS")]
    public byte[] BusinessRuleLastChgTs { get; set; } = null!;

    [Column("BusinessRule_CreatedUserID")]
    public int BusinessRuleCreatedUserId { get; set; }

    [Column("BusinessRule_CreatedUserMUID")]
    public Guid? BusinessRuleCreatedUserMuid { get; set; }

    [Column("BusinessRule_CreatedUserName")]
    [StringLength(100)]
    public string? BusinessRuleCreatedUserName { get; set; }

    [Column("BusinessRule_DateCreated")]
    [Precision(3)]
    public DateTime BusinessRuleDateCreated { get; set; }

    [Column("BusinessRule_UpdatedUserID")]
    public int BusinessRuleUpdatedUserId { get; set; }

    [Column("BusinessRule_UpdatedUserMUID")]
    public Guid? BusinessRuleUpdatedUserMuid { get; set; }

    [Column("BusinessRule_UpdatedUserName")]
    [StringLength(100)]
    public string? BusinessRuleUpdatedUserName { get; set; }

    [Column("BusinessRule_DateUpdated")]
    [Precision(3)]
    public DateTime BusinessRuleDateUpdated { get; set; }
}

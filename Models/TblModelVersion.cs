using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblModelVersion", Schema = "mdm")]
[Index("VersionFlagId", Name = "ix_tblModelVersion_VersionFlag_ID")]
[Index("Muid", Name = "ux_tblModelVersion_MUID", IsUnique = true)]
[Index("ModelId", "Name", Name = "ux_tblModelVersion_Model_ID_Name", IsUnique = true)]
public partial class TblModelVersion
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    [Column("Display_ID")]
    public int DisplayId { get; set; }

    [Column("VersionFlag_ID")]
    public int? VersionFlagId { get; set; }

    [Column("AsOfVersion_ID")]
    public int? AsOfVersionId { get; set; }

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

    [ForeignKey("ModelId")]
    [InverseProperty("TblModelVersions")]
    public virtual TblModel Model { get; set; } = null!;

    [InverseProperty("Version")]
    public virtual ICollection<TblEvent> TblEvents { get; set; } = new List<TblEvent>();

    [InverseProperty("Version")]
    public virtual ICollection<TblNotificationQueue> TblNotificationQueues { get; set; } = new List<TblNotificationQueue>();

    [InverseProperty("Version")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();

    [InverseProperty("ModelVersion")]
    public virtual ICollection<TblSubscriptionView> TblSubscriptionViews { get; set; } = new List<TblSubscriptionView>();

    [InverseProperty("SourceVersion")]
    public virtual ICollection<TblSyncRelationship> TblSyncRelationshipSourceVersions { get; set; } = new List<TblSyncRelationship>();

    [InverseProperty("TargetVersion")]
    public virtual ICollection<TblSyncRelationship> TblSyncRelationshipTargetVersions { get; set; } = new List<TblSyncRelationship>();

    [ForeignKey("VersionFlagId")]
    [InverseProperty("TblModelVersions")]
    public virtual TblModelVersionFlag? VersionFlag { get; set; }
}

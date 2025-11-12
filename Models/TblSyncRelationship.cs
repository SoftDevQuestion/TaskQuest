using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSyncRelationship", Schema = "mdm")]
[Index("SourceEntityId", Name = "ix_tblSyncRelationship_SourceEntity_ID")]
[Index("SourceVersionId", Name = "ix_tblSyncRelationship_SourceVersion_ID")]
[Index("TargetVersionId", "TargetEntityId", Name = "ux_tblSyncRelationship_TargetVersion_ID_TargetEntity_ID", IsUnique = true)]
public partial class TblSyncRelationship
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("SourceVersion_ID")]
    public int SourceVersionId { get; set; }

    [Column("SourceEntity_ID")]
    public int SourceEntityId { get; set; }

    [Column("TargetVersion_ID")]
    public int TargetVersionId { get; set; }

    [Column("TargetEntity_ID")]
    public int TargetEntityId { get; set; }

    public bool TargetEntityNameIsAliased { get; set; }

    public int? RefreshFrequencyInHours { get; set; }

    [Column("LastSuccessfulSyncDTM")]
    public DateTime? LastSuccessfulSyncDtm { get; set; }

    [MaxLength(8)]
    public byte[]? LastSuccessfulSyncTimestamp { get; set; }

    [Column("LastSyncAttemptDTM")]
    public DateTime? LastSyncAttemptDtm { get; set; }

    public byte LastSyncAttemptStatus { get; set; }

    public string? LastSyncAttemptErrorInfo { get; set; }

    [Column("EnterDTM")]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("LastChgDTM")]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [ForeignKey("SourceEntityId")]
    [InverseProperty("TblSyncRelationshipSourceEntities")]
    public virtual TblEntity SourceEntity { get; set; } = null!;

    [ForeignKey("SourceVersionId")]
    [InverseProperty("TblSyncRelationshipSourceVersions")]
    public virtual TblModelVersion SourceVersion { get; set; } = null!;

    [ForeignKey("TargetEntityId")]
    [InverseProperty("TblSyncRelationshipTargetEntities")]
    public virtual TblEntity TargetEntity { get; set; } = null!;

    [ForeignKey("TargetVersionId")]
    [InverseProperty("TblSyncRelationshipTargetVersions")]
    public virtual TblModelVersion TargetVersion { get; set; } = null!;
}

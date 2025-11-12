using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblStgBatch", Schema = "mdm")]
[Index("EntityId", Name = "ix_tblStgBatch_Entity_ID")]
public partial class TblStgBatch
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("OriginalBatch_ID")]
    public int? OriginalBatchId { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("MemberType_ID")]
    public byte? MemberTypeId { get; set; }

    [StringLength(50)]
    public string? BatchTag { get; set; }

    [Column("ExternalSystem_ID")]
    public int? ExternalSystemId { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    public int? TotalMemberCount { get; set; }

    public int? ErrorMemberCount { get; set; }

    public int? TotalMemberAttributeCount { get; set; }

    public int? ErrorMemberAttributeCount { get; set; }

    public int? TotalMemberRelationshipCount { get; set; }

    public int? ErrorMemberRelationshipCount { get; set; }

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    [Column("LastRunStartDTM", TypeName = "datetime")]
    public DateTime? LastRunStartDtm { get; set; }

    [Column("LastRunStartUserID")]
    public int? LastRunStartUserId { get; set; }

    [Column("LastRunEndDTM", TypeName = "datetime")]
    public DateTime? LastRunEndDtm { get; set; }

    [Column("LastRunEndUserID")]
    public int? LastRunEndUserId { get; set; }

    [Column("LastClearedDTM", TypeName = "datetime")]
    public DateTime? LastClearedDtm { get; set; }

    [Column("LastClearedUserID")]
    public int? LastClearedUserId { get; set; }

    [Column("EnterDTM", TypeName = "datetime")]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblStgBatches")]
    public virtual TblEntity? Entity { get; set; }
}

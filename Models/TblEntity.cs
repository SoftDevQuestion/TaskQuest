using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblEntity", Schema = "mdm")]
[Index("Muid", Name = "ux_tblEntity_MUID", IsUnique = true)]
[Index("ModelId", "Name", Name = "ux_tblEntity_Model_ID_Name", IsUnique = true)]
public partial class TblEntity
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

    [StringLength(128)]
    public string EntityTable { get; set; } = null!;

    [StringLength(128)]
    public string? HierarchyTable { get; set; }

    [StringLength(128)]
    public string? HierarchyParentTable { get; set; }

    [StringLength(128)]
    public string? CollectionTable { get; set; }

    [StringLength(128)]
    public string? CollectionMemberTable { get; set; }

    public bool IsBase { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("EnterVersionID")]
    public int EnterVersionId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgVersionID")]
    public int LastChgVersionId { get; set; }

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [StringLength(60)]
    public string? StagingBase { get; set; }

    [StringLength(65)]
    public string? StagingLeafName { get; set; }

    [StringLength(73)]
    public string? StagingConsolidatedName { get; set; }

    [StringLength(73)]
    public string? StagingRelationshipName { get; set; }

    public byte DataCompression { get; set; }

    public byte TransactionLogType { get; set; }

    public bool RequireApproval { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("TblEntities")]
    public virtual TblModel Model { get; set; } = null!;

    [InverseProperty("DomainEntity")]
    public virtual ICollection<TblAttribute> TblAttributeDomainEntities { get; set; } = new List<TblAttribute>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblAttribute> TblAttributeEntities { get; set; } = new List<TblAttribute>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblAttributeGroup> TblAttributeGroups { get; set; } = new List<TblAttributeGroup>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblBrbusinessRule> TblBrbusinessRules { get; set; } = new List<TblBrbusinessRule>();

    [InverseProperty("Entity")]
    public virtual TblCodeGenInfo? TblCodeGenInfo { get; set; }

    [InverseProperty("Entity")]
    public virtual ICollection<TblEvent> TblEvents { get; set; } = new List<TblEvent>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblHierarchy> TblHierarchies { get; set; } = new List<TblHierarchy>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblIndex> TblIndices { get; set; } = new List<TblIndex>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblStgBatch> TblStgBatches { get; set; } = new List<TblStgBatch>();

    [InverseProperty("Entity")]
    public virtual ICollection<TblSubscriptionView> TblSubscriptionViews { get; set; } = new List<TblSubscriptionView>();

    [InverseProperty("SourceEntity")]
    public virtual ICollection<TblSyncRelationship> TblSyncRelationshipSourceEntities { get; set; } = new List<TblSyncRelationship>();

    [InverseProperty("TargetEntity")]
    public virtual ICollection<TblSyncRelationship> TblSyncRelationshipTargetEntities { get; set; } = new List<TblSyncRelationship>();
}

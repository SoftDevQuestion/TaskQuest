using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSubscriptionView", Schema = "mdm")]
[Index("DerivedHierarchyId", Name = "ix_tblSubscriptionView_DerivedHierarchy_ID")]
[Index("EntityId", Name = "ix_tblSubscriptionView_Entity_ID")]
[Index("ModelVersionId", Name = "ix_tblSubscriptionView_ModelVersion_ID")]
[Index("ModelId", Name = "ix_tblSubscriptionView_Model_ID")]
public partial class TblSubscriptionView
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("DerivedHierarchy_ID")]
    public int? DerivedHierarchyId { get; set; }

    [Column("ViewFormat_ID")]
    public int ViewFormatId { get; set; }

    [Column("ModelVersion_ID")]
    public int? ModelVersionId { get; set; }

    [Column("ModelVersionFlag_ID")]
    public int? ModelVersionFlagId { get; set; }

    [StringLength(128)]
    public string Name { get; set; } = null!;

    public int? Levels { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IncludeSoftDeletedMembers { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int? EnterUserId { get; set; }

    [Column("EnterVersionID")]
    public int EnterVersionId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int? LastChgUserId { get; set; }

    [Column("LastChgVersionID")]
    public int LastChgVersionId { get; set; }

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [ForeignKey("DerivedHierarchyId")]
    [InverseProperty("TblSubscriptionViews")]
    public virtual TblDerivedHierarchy? DerivedHierarchy { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblSubscriptionViews")]
    public virtual TblEntity? Entity { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("TblSubscriptionViews")]
    public virtual TblModel Model { get; set; } = null!;

    [ForeignKey("ModelVersionId")]
    [InverseProperty("TblSubscriptionViews")]
    public virtual TblModelVersion? ModelVersion { get; set; }
}

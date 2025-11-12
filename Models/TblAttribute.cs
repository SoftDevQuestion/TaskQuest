using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblAttribute", Schema = "mdm")]
[Index("DomainEntityId", "EntityId", "MemberTypeId", "Id", Name = "ix_tblAttribute_DomainEntity_DomainEntity_Entity_ID_MemberType_ID_ID")]
[Index("DomainEntityId", Name = "ix_tblAttribute_DomainEntity_ID")]
[Index("EntityId", "MemberTypeId", "AttributeTypeId", Name = "ix_tblAttribute_Entity_ID_MemberType_ID_AttributeType_ID")]
[Index("FilterHierarchyDetailId", Name = "ix_tblAttribute_FilterHierarchyDetail_ID")]
[Index("FilterParentAttributeId", Name = "ix_tblAttribute_FilterParentAttribute_ID")]
[Index("MemberTypeId", Name = "ix_tblAttribute_MemberType_ID")]
[Index("SourceId", Name = "ix_tblAttribute_Source_ID")]
[Index("EntityId", "MemberTypeId", "DisplayName", Name = "ux_tblAttribute_Entity_ID_MemberType_ID_DisplayName", IsUnique = true)]
[Index("EntityId", "MemberTypeId", "Name", Name = "ux_tblAttribute_Entity_ID_MemberType_ID_Name", IsUnique = true)]
[Index("EntityId", "MemberTypeId", "TableColumn", Name = "ux_tblAttribute_Entity_ID_MemberType_ID_TableColumn", IsUnique = true)]
[Index("Muid", Name = "ux_tblAttribute_MUID", IsUnique = true)]
public partial class TblAttribute
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Source_ID")]
    public int? SourceId { get; set; }

    [Column("Source_LastChgTS")]
    [MaxLength(8)]
    public byte[]? SourceLastChgTs { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [StringLength(250)]
    public string DisplayName { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(128)]
    public string TableColumn { get; set; } = null!;

    [Column("AttributeType_ID")]
    public byte AttributeTypeId { get; set; }

    [Column("DataType_ID")]
    public byte DataTypeId { get; set; }

    public int? DataTypeInformation { get; set; }

    [Column("InputMask_ID")]
    public int InputMaskId { get; set; }

    public int DisplayWidth { get; set; }

    public int SortOrder { get; set; }

    [Column("DomainEntity_ID")]
    public int? DomainEntityId { get; set; }

    public int ChangeTrackingGroup { get; set; }

    [Column("FilterParentAttribute_ID")]
    public int? FilterParentAttributeId { get; set; }

    [Column("FilterHierarchyDetail_ID")]
    public int? FilterHierarchyDetailId { get; set; }

    public bool IsCode { get; set; }

    public bool IsName { get; set; }

    public bool IsSystem { get; set; }

    public bool IsReadOnly { get; set; }

    public bool IsRequired { get; set; }

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

    [ForeignKey("DomainEntityId")]
    [InverseProperty("TblAttributeDomainEntities")]
    public virtual TblEntity? DomainEntity { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblAttributeEntities")]
    public virtual TblEntity Entity { get; set; } = null!;

    [ForeignKey("FilterHierarchyDetailId")]
    [InverseProperty("TblAttributes")]
    public virtual TblDerivedHierarchyDetail? FilterHierarchyDetail { get; set; }

    [ForeignKey("FilterParentAttributeId")]
    [InverseProperty("InverseFilterParentAttribute")]
    public virtual TblAttribute? FilterParentAttribute { get; set; }

    [InverseProperty("FilterParentAttribute")]
    public virtual ICollection<TblAttribute> InverseFilterParentAttribute { get; set; } = new List<TblAttribute>();

    [InverseProperty("Source")]
    public virtual ICollection<TblAttribute> InverseSource { get; set; } = new List<TblAttribute>();

    [ForeignKey("SourceId")]
    [InverseProperty("InverseSource")]
    public virtual TblAttribute? Source { get; set; }

    [InverseProperty("Attribute")]
    public virtual ICollection<TblAttributeGroupDetail> TblAttributeGroupDetails { get; set; } = new List<TblAttributeGroupDetail>();

    [InverseProperty("ManyToManyChildAttribute")]
    public virtual ICollection<TblDerivedHierarchyDetail> TblDerivedHierarchyDetails { get; set; } = new List<TblDerivedHierarchyDetail>();
}

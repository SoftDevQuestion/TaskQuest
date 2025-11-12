using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaAttribute
{
    [Column("Model_ID")]
    public int? ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid? ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string? ModelName { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid? EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string? EntityName { get; set; }

    [Column("Attribute_ID")]
    public int? AttributeId { get; set; }

    [Column("Attribute_MUID")]
    public Guid? AttributeMuid { get; set; }

    [Column("Attribute_Name")]
    [StringLength(100)]
    public string? AttributeName { get; set; }

    [Column("Attribute_Description")]
    [StringLength(500)]
    public string? AttributeDescription { get; set; }

    [Column("Attribute_DisplayName")]
    [StringLength(250)]
    public string? AttributeDisplayName { get; set; }

    [Column("Attribute_DisplayWidth")]
    public int? AttributeDisplayWidth { get; set; }

    [Column("Attribute_ChangeTrackingGroup")]
    public int? AttributeChangeTrackingGroup { get; set; }

    [Column("Attribute_Column")]
    [StringLength(128)]
    public string? AttributeColumn { get; set; }

    [Column("Attribute_IsSystem")]
    public bool? AttributeIsSystem { get; set; }

    [Column("Attribute_IsReadOnly")]
    public bool? AttributeIsReadOnly { get; set; }

    [Column("Attribute_IsCode")]
    public bool? AttributeIsCode { get; set; }

    [Column("Attribute_IsName")]
    public bool? AttributeIsName { get; set; }

    [Column("Attribute_MemberType_ID")]
    public byte? AttributeMemberTypeId { get; set; }

    [Column("Attribute_MemberType_Name")]
    [StringLength(16)]
    public string? AttributeMemberTypeName { get; set; }

    [Column("Attribute_Type_ID")]
    public byte? AttributeTypeId { get; set; }

    [Column("Attribute_Type_Name")]
    [StringLength(8)]
    public string? AttributeTypeName { get; set; }

    [Column("Attribute_DBAEntity_ID")]
    public int? AttributeDbaentityId { get; set; }

    [Column("Attribute_DBAEntity_MUID")]
    public Guid? AttributeDbaentityMuid { get; set; }

    [Column("Attribute_DBAEntity_Name")]
    [StringLength(50)]
    public string? AttributeDbaentityName { get; set; }

    [Column("Attribute_DBAEntity_IsHierarchyEnabled")]
    public int? AttributeDbaentityIsHierarchyEnabled { get; set; }

    [Column("FilterParentAttribute_ID")]
    public int? FilterParentAttributeId { get; set; }

    [Column("FilterParentAttribute_MUID")]
    public Guid? FilterParentAttributeMuid { get; set; }

    [Column("FilterParentAttribute_Name")]
    [StringLength(100)]
    public string? FilterParentAttributeName { get; set; }

    [Column("FilterParentHierarchy_ID")]
    public int? FilterParentHierarchyId { get; set; }

    [Column("FilterParentHierarchy_MUID")]
    public Guid? FilterParentHierarchyMuid { get; set; }

    [Column("FilterParentHierarchy_Name")]
    [StringLength(50)]
    public string? FilterParentHierarchyName { get; set; }

    [Column("FilterParentHierarchy_LevelNumber")]
    public int? FilterParentHierarchyLevelNumber { get; set; }

    [Column("Attribute_DataType_ID")]
    public byte? AttributeDataTypeId { get; set; }

    [Column("Attribute_DataType_Name")]
    [StringLength(8)]
    public string? AttributeDataTypeName { get; set; }

    [Column("Attribute_DataType_Information")]
    public int? AttributeDataTypeInformation { get; set; }

    [Column("Attribute_DataMask_ID")]
    public int? AttributeDataMaskId { get; set; }

    [Column("Attribute_DataMask_Name")]
    [StringLength(250)]
    public string? AttributeDataMaskName { get; set; }

    [Column("Entity_PhysicalTableName")]
    [StringLength(128)]
    public string? EntityPhysicalTableName { get; set; }

    [Column("Attribute_SortOrder")]
    public int? AttributeSortOrder { get; set; }

    [Column("EnteredUser_ID")]
    public int? EnteredUserId { get; set; }

    [Column("EnteredUser_MUID")]
    public Guid? EnteredUserMuid { get; set; }

    [Column("EnteredUser_UserName")]
    [StringLength(100)]
    public string? EnteredUserUserName { get; set; }

    [Column("EnteredUser_DTM")]
    [Precision(3)]
    public DateTime? EnteredUserDtm { get; set; }

    [Column("LastChgUser_ID")]
    public int? LastChgUserId { get; set; }

    [Column("LastChgUser_MUID")]
    public Guid? LastChgUserMuid { get; set; }

    [Column("LastChgUser_UserName")]
    [StringLength(100)]
    public string? LastChgUserUserName { get; set; }

    [Column("LastChgUser_DTM")]
    [Precision(3)]
    public DateTime? LastChgUserDtm { get; set; }

    [Column("Attribute_FullyQualifiedName")]
    [StringLength(1035)]
    public string? AttributeFullyQualifiedName { get; set; }
}

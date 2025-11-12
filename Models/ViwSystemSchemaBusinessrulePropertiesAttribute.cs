using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaBusinessrulePropertiesAttribute
{
    [Column("Property_ID")]
    public int PropertyId { get; set; }

    [Column("BusinessRule_ID")]
    public int BusinessRuleId { get; set; }

    [Column("BusinessRule_MUID")]
    public Guid BusinessRuleMuid { get; set; }

    [Column("BusinessRule_Name")]
    [StringLength(50)]
    public string BusinessRuleName { get; set; } = null!;

    [Column("BusinessRule_Status")]
    public int BusinessRuleStatus { get; set; }

    [Column("Item_ID")]
    public int ItemId { get; set; }

    [Column("Item_MUID")]
    public Guid ItemMuid { get; set; }

    [Column("Item_AppliesTo_ID")]
    public int ItemAppliesToId { get; set; }

    [Column("PropertyType_ID")]
    public int PropertyTypeId { get; set; }

    [Column("Property_Value")]
    [StringLength(999)]
    public string PropertyValue { get; set; } = null!;

    [Column("Property_Sequence")]
    public int PropertySequence { get; set; }

    [Column("Property_IsLeftHandSide")]
    public bool PropertyIsLeftHandSide { get; set; }

    [Column("Property_Parent_ID")]
    public int? PropertyParentId { get; set; }

    [Column("Property_Parent_PropertyType_ID")]
    public int? PropertyParentPropertyTypeId { get; set; }

    [Column("Property_Parent_PropertyName_ID")]
    public int? PropertyParentPropertyNameId { get; set; }

    [Column("Model_ID")]
    public int? ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid? ModelMuid { get; set; }

    [Column("Attribute_Entity_ID")]
    public int? AttributeEntityId { get; set; }

    [Column("Attribute_Entity_MUID")]
    public Guid? AttributeEntityMuid { get; set; }

    [Column("Attribute_Entity_Name")]
    [StringLength(50)]
    public string? AttributeEntityName { get; set; }

    [Column("Attribute_ID")]
    public int? AttributeId { get; set; }

    [Column("Attribute_MUID")]
    public Guid? AttributeMuid { get; set; }

    [Column("Attribute_Name")]
    [StringLength(100)]
    public string? AttributeName { get; set; }

    [Column("Attribute_Column")]
    [StringLength(128)]
    public string? AttributeColumn { get; set; }

    [Column("Attribute_MemberType_ID")]
    public byte? AttributeMemberTypeId { get; set; }

    [Column("Attribute_DBAEntity_ID")]
    public int? AttributeDbaentityId { get; set; }

    [Column("Attribute_DBAEntity_MUID")]
    public Guid? AttributeDbaentityMuid { get; set; }

    [Column("Attribute_DBAEntity_Name")]
    [StringLength(50)]
    public string? AttributeDbaentityName { get; set; }

    [Column("Attribute_ChangeTrackingGroup")]
    public int? AttributeChangeTrackingGroup { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaBusinessruleProperty
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

    [Column("LogicalOperator_ID")]
    public int LogicalOperatorId { get; set; }

    [Column("LogicalOperator_MUID")]
    public Guid LogicalOperatorMuid { get; set; }

    [Column("LogicalOperator_Name")]
    [StringLength(250)]
    public string LogicalOperatorName { get; set; } = null!;

    [Column("Item_ID")]
    public int ItemId { get; set; }

    [Column("Item_MUID")]
    public Guid ItemMuid { get; set; }

    [Column("Item_Text")]
    public string ItemText { get; set; } = null!;

    [Column("Item_Sequence")]
    public int ItemSequence { get; set; }

    [Column("Item_AppliesTo_ID")]
    public int ItemAppliesToId { get; set; }

    [Column("PropertyType_ID")]
    public int PropertyTypeId { get; set; }

    [Column("PropertyType_Name")]
    [StringLength(250)]
    public string PropertyTypeName { get; set; } = null!;

    [Column("PropertyName_ID")]
    public int PropertyNameId { get; set; }

    [Column("PropertyName_Name")]
    [StringLength(250)]
    public string PropertyNameName { get; set; } = null!;

    [Column("Property_Value")]
    [StringLength(999)]
    public string PropertyValue { get; set; } = null!;

    [Column("Property_Sequence")]
    public int PropertySequence { get; set; }

    [Column("Property_IsLeftHandSide")]
    public bool PropertyIsLeftHandSide { get; set; }

    [Column("Property_Parent_ID")]
    public int? PropertyParentId { get; set; }

    [Column("Property_SuppressText")]
    public bool? PropertySuppressText { get; set; }
}

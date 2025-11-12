using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemBusinessrulesHierarchyChangevalueInheritance
{
    [Column("ModelID")]
    public int ModelId { get; set; }

    [Column("EntityID")]
    public int EntityId { get; set; }

    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    [StringLength(100)]
    public string AttributeName { get; set; } = null!;

    [StringLength(128)]
    public string AttributeColumnName { get; set; } = null!;

    [Column("HierarchyID")]
    public int HierarchyId { get; set; }

    [StringLength(50)]
    public string HierarchyName { get; set; } = null!;
}

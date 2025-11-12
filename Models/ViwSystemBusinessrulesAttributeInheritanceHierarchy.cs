using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemBusinessrulesAttributeInheritanceHierarchy
{
    [Column("ParentModelID")]
    public int? ParentModelId { get; set; }

    [Column("ParentEntityID")]
    public int? ParentEntityId { get; set; }

    [StringLength(50)]
    public string? ParentEntityName { get; set; }

    [StringLength(100)]
    public string? ParentAttributeName { get; set; }

    [StringLength(128)]
    public string? ParentAttributeColumnName { get; set; }

    [Column("ChildEntityID")]
    public int? ChildEntityId { get; set; }

    [StringLength(50)]
    public string? ChildEntityName { get; set; }

    [StringLength(100)]
    public string? ChildAttributeName { get; set; }

    [StringLength(128)]
    public string? ChildAttributeColumnName { get; set; }

    [Column("Attribute_MemberType_ID")]
    public byte? AttributeMemberTypeId { get; set; }
}

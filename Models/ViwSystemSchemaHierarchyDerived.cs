using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaHierarchyDerived
{
    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Hierarchy_ID")]
    public int HierarchyId { get; set; }

    [Column("Hierarchy_MUID")]
    public Guid HierarchyMuid { get; set; }

    [Column("Hierarchy_Name")]
    [StringLength(50)]
    public string HierarchyName { get; set; } = null!;

    [Column("Hierarchy_AnchorNullRecursions")]
    public bool HierarchyAnchorNullRecursions { get; set; }

    [Column("Hierarchy_Label")]
    [StringLength(111)]
    public string HierarchyLabel { get; set; } = null!;

    [Column("EnteredUser_ID")]
    public int EnteredUserId { get; set; }

    [Column("EnteredUser_MUID")]
    public Guid? EnteredUserMuid { get; set; }

    [Column("EnteredUser_UserName")]
    [StringLength(100)]
    public string? EnteredUserUserName { get; set; }

    [Column("EnteredUser_DTM")]
    [Precision(3)]
    public DateTime EnteredUserDtm { get; set; }

    [Column("LastChgUser_ID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgUser_MUID")]
    public Guid? LastChgUserMuid { get; set; }

    [Column("LastChgUser_UserName")]
    [StringLength(100)]
    public string? LastChgUserUserName { get; set; }

    [Column("LastChgUser_DTM")]
    [Precision(3)]
    public DateTime LastChgUserDtm { get; set; }
}

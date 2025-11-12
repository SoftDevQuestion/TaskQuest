using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaHierarchyDerivedLevel
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

    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [Column("ForeignType_ID")]
    public int ForeignTypeId { get; set; }

    [Column("ForeignType_Name")]
    [StringLength(10)]
    public string? ForeignTypeName { get; set; }

    [Column("Foreign_ID")]
    public int? ForeignId { get; set; }

    [Column("Foreign_MUID")]
    public Guid? ForeignMuid { get; set; }

    [Column("Foreign_Name")]
    [StringLength(100)]
    public string? ForeignName { get; set; }

    [Column("ForeignEntity_ID")]
    public int? ForeignEntityId { get; set; }

    [Column("ForeignEntity_MUID")]
    public Guid? ForeignEntityMuid { get; set; }

    [Column("ForeignEntity_Name")]
    [StringLength(50)]
    public string? ForeignEntityName { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid? EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string? EntityName { get; set; }

    [Column("ManyToManyChildAttribute_ID")]
    public int? ManyToManyChildAttributeId { get; set; }

    [Column("ManyToManyChildAttribute_MUID")]
    public Guid? ManyToManyChildAttributeMuid { get; set; }

    [Column("ManyToManyChildAttribute_Name")]
    [StringLength(100)]
    public string? ManyToManyChildAttributeName { get; set; }

    public int? LevelNumber { get; set; }

    [Column("Level_ID")]
    public int? LevelId { get; set; }

    public bool IsLevelVisible { get; set; }

    [Column("MemberType_ID")]
    public int MemberTypeId { get; set; }

    [Column("MemberType_Name")]
    [StringLength(12)]
    public string MemberTypeName { get; set; } = null!;

    public int IsRecursive { get; set; }

    [Column("Detail_EnteredUser_ID")]
    public int DetailEnteredUserId { get; set; }

    [Column("Detail_EnteredUser_MUID")]
    public Guid? DetailEnteredUserMuid { get; set; }

    [Column("Detail_EnteredUser_UserName")]
    [StringLength(100)]
    public string? DetailEnteredUserUserName { get; set; }

    [Column("Detail_EnteredUser_DTM")]
    [Precision(3)]
    public DateTime DetailEnteredUserDtm { get; set; }

    [Column("Detail_LastChgUser_ID")]
    public int DetailLastChgUserId { get; set; }

    [Column("Detail_LastChgUser_MUID")]
    public Guid? DetailLastChgUserMuid { get; set; }

    [Column("Detail_LastChgUser_UserName")]
    [StringLength(100)]
    public string? DetailLastChgUserUserName { get; set; }

    [Column("Detail_LastChgUser_DTM")]
    [Precision(3)]
    public DateTime DetailLastChgUserDtm { get; set; }
}

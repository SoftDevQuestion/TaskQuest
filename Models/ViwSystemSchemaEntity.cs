using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaEntity
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsBase { get; set; }

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

    [StringLength(60)]
    public string? StagingBase { get; set; }

    [StringLength(65)]
    public string? StagingLeafTable { get; set; }

    [StringLength(73)]
    public string? StagingConsolidatedTable { get; set; }

    [StringLength(73)]
    public string? StagingRelationshipTable { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

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

    public int IsCodeGenerationEnabled { get; set; }

    public int? CodeGenerationSeed { get; set; }

    public byte DataCompression { get; set; }

    public byte TransactionLogType { get; set; }

    public bool RequireApproval { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaIndex
{
    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    [Column("Index_ID")]
    public int IndexId { get; set; }

    [Column("Index_MUID")]
    public Guid IndexMuid { get; set; }

    [Column("Index_Name")]
    [StringLength(50)]
    public string IndexName { get; set; } = null!;

    [Column("SysIndex_ID")]
    public int? SysIndexId { get; set; }

    [Column("Index_IsUnique")]
    public bool IndexIsUnique { get; set; }

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

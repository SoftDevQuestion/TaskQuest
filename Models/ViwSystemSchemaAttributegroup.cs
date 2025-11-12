using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaAttributegroup
{
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string EntityName { get; set; } = null!;

    [Column("Entity_MUID")]
    public Guid EntityMuid { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [Column("MemberType_Name")]
    [StringLength(16)]
    public string MemberTypeName { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool IsNameCodeFrozen { get; set; }

    public bool IsSystem { get; set; }

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

    [StringLength(1035)]
    public string FullName { get; set; } = null!;
}

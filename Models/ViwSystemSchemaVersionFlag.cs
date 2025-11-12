using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaVersionFlag
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    public bool? IsCommittedOnly { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column("AssignedVersion_ID")]
    public int AssignedVersionId { get; set; }

    [Column("AssignedVersion_MUID")]
    public Guid AssignedVersionMuid { get; set; }

    [Column("AssignedVersion_Name")]
    [StringLength(50)]
    public string AssignedVersionName { get; set; } = null!;

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

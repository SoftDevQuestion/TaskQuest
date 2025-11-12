using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaSystemsetting
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(100)]
    public string SettingName { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [StringLength(250)]
    public string Description { get; set; } = null!;

    [Column("SettingType_ID")]
    public byte SettingTypeId { get; set; }

    [Column("SettingType_Name")]
    [StringLength(8)]
    public string? SettingTypeName { get; set; }

    [Column("DataType_ID")]
    public byte DataTypeId { get; set; }

    [Column("DataType_Name")]
    [StringLength(8)]
    public string? DataTypeName { get; set; }

    public string SettingValue { get; set; } = null!;

    [StringLength(50)]
    public string? SettingValueDomainName { get; set; }

    [StringLength(200)]
    public string? MinValue { get; set; }

    [StringLength(200)]
    public string? MaxValue { get; set; }

    public bool IsVisible { get; set; }

    public int DisplaySequence { get; set; }

    [Column("SystemSettingGroup_ID")]
    public int SystemSettingGroupId { get; set; }
}

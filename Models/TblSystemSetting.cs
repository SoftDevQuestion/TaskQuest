using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSystemSetting", Schema = "mdm")]
[Index("SettingName", Name = "ix_tblSystemSetting_SettingName", IsUnique = true)]
[Index("SystemSettingGroupId", Name = "ix_tblSystemSetting_SystemSettingGroup_ID")]
[Index("Muid", Name = "ux_tblSystemSetting_MUID", IsUnique = true)]
public partial class TblSystemSetting
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public bool IsVisible { get; set; }

    [StringLength(100)]
    public string SettingName { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [StringLength(250)]
    public string Description { get; set; } = null!;

    [Column("SettingType_ID")]
    public byte SettingTypeId { get; set; }

    [Column("DataType_ID")]
    public byte DataTypeId { get; set; }

    [StringLength(200)]
    public string? MinValue { get; set; }

    [StringLength(200)]
    public string? MaxValue { get; set; }

    [StringLength(50)]
    public string? ListCode { get; set; }

    [Column("EnterDTM", TypeName = "datetime")]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("LastChgDTM", TypeName = "datetime")]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("SystemSettingGroup_ID")]
    public int SystemSettingGroupId { get; set; }

    public int DisplaySequence { get; set; }

    [ForeignKey("SystemSettingGroupId")]
    [InverseProperty("TblSystemSettings")]
    public virtual TblSystemSettingGroup SystemSettingGroup { get; set; } = null!;
}

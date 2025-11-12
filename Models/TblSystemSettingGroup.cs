using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSystemSettingGroup", Schema = "mdm")]
[Index("Muid", Name = "ux_tblSystemSettingGroup_MUID", IsUnique = true)]
public partial class TblSystemSettingGroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(50)]
    public string GroupName { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [StringLength(250)]
    public string Description { get; set; } = null!;

    public int DisplaySequence { get; set; }

    [InverseProperty("SystemSettingGroup")]
    public virtual ICollection<TblSystemSetting> TblSystemSettings { get; set; } = new List<TblSystemSetting>();
}

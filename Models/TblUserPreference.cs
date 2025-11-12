using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblUserPreference", Schema = "mdm")]
[Index("UserId", "PreferenceName", Name = "ix_tblUserPreference_User_ID_PreferenceName", IsUnique = true)]
public partial class TblUserPreference
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string PreferenceName { get; set; } = null!;

    public string PreferenceValue { get; set; } = null!;

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TblUserPreferences")]
    public virtual TblUser User { get; set; } = null!;
}

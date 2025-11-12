using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblUser", Schema = "mdm")]
[Index("StatusId", Name = "ix_tblUser_Status_ID")]
[Index("Muid", Name = "ux_tblUser_MUID", IsUnique = true)]
[Index("Sid", Name = "ux_tblUser_SID", IsUnique = true)]
[Index("UserName", Name = "ux_tblUser_UserName", IsUnique = true)]
public partial class TblUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    [Column("SID")]
    [StringLength(250)]
    public string? Sid { get; set; }

    [StringLength(100)]
    public string UserName { get; set; } = null!;

    [StringLength(256)]
    public string DisplayName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? EmailAddress { get; set; }

    [Column("LastLoginDTM")]
    [Precision(3)]
    public DateTime? LastLoginDtm { get; set; }

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

    [InverseProperty("User")]
    public virtual ICollection<TblNotificationUser> TblNotificationUsers { get; set; } = new List<TblNotificationUser>();

    [InverseProperty("User")]
    public virtual ICollection<TblUserGroupAssignment> TblUserGroupAssignments { get; set; } = new List<TblUserGroupAssignment>();

    [InverseProperty("User")]
    public virtual ICollection<TblUserPreference> TblUserPreferences { get; set; } = new List<TblUserPreference>();
}

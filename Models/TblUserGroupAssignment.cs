using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblUserGroupAssignment", Schema = "mdm")]
[Index("Muid", Name = "ux_tblUserGroupAssignment_MUID", IsUnique = true)]
[Index("UserGroupId", "UserId", Name = "ux_tblUserGroupAssignment_UserGroup_ID_User_ID", IsUnique = true)]
[Index("UserId", "UserGroupId", Name = "ux_tblUserGroupAssignment_User_ID_UserGroup_ID", IsUnique = true)]
public partial class TblUserGroupAssignment
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("UserGroup_ID")]
    public int UserGroupId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

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
    [InverseProperty("TblUserGroupAssignments")]
    public virtual TblUser User { get; set; } = null!;

    [ForeignKey("UserGroupId")]
    [InverseProperty("TblUserGroupAssignments")]
    public virtual TblUserGroup UserGroup { get; set; } = null!;
}

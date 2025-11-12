using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblUserGroup", Schema = "mdm")]
[Index("UserGroupTypeId", Name = "ix_tblUserGroup_UserGroupType_ID")]
[Index("Muid", Name = "ux_tblUserGroup_MUID", IsUnique = true)]
[Index("Name", Name = "ux_tblUserGroup_Name", IsUnique = true)]
[Index("Sid", Name = "ux_tblUserGroup_SID", IsUnique = true)]
public partial class TblUserGroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("UserGroupType_ID")]
    public byte UserGroupTypeId { get; set; }

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    [Column("SID")]
    [StringLength(250)]
    public string? Sid { get; set; }

    [StringLength(355)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

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

    [InverseProperty("UserGroup")]
    public virtual ICollection<TblUserGroupAssignment> TblUserGroupAssignments { get; set; } = new List<TblUserGroupAssignment>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityRole", Schema = "mdm")]
[Index("Muid", Name = "ux_tblSecurityRole_MUID", IsUnique = true)]
public partial class TblSecurityRole
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(115)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
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

    [InverseProperty("Role")]
    public virtual ICollection<TblSecurityAccessControl> TblSecurityAccessControls { get; set; } = new List<TblSecurityAccessControl>();

    [InverseProperty("Role")]
    public virtual ICollection<TblSecurityRoleAccessFunctional> TblSecurityRoleAccessFunctionals { get; set; } = new List<TblSecurityRoleAccessFunctional>();

    [InverseProperty("Role")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();

    [InverseProperty("Role")]
    public virtual ICollection<TblSecurityRoleAccess> TblSecurityRoleAccesses { get; set; } = new List<TblSecurityRoleAccess>();
}

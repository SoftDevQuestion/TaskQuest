using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityRoleAccessFunctional", Schema = "mdm")]
[Index("FunctionalPrivilegeId", Name = "ix_tblSecurityRoleAccessFunctional_FunctionalPrivilege_ID")]
[Index("RoleId", Name = "ix_tblSecurityRoleAccessFunctional_Role_ID")]
public partial class TblSecurityRoleAccessFunctional
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("FunctionalPrivilege_ID")]
    public byte FunctionalPrivilegeId { get; set; }

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

    [ForeignKey("FunctionalPrivilegeId")]
    [InverseProperty("TblSecurityRoleAccessFunctionals")]
    public virtual TblSecurityPrivilegeFunctional FunctionalPrivilege { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("TblSecurityRoleAccessFunctionals")]
    public virtual TblSecurityRole Role { get; set; } = null!;
}

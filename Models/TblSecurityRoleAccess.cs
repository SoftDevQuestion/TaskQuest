using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityRoleAccess", Schema = "mdm")]
[Index("ObjectId", "SecurableId", "PrivilegeId", Name = "ix_tblSecurityRoleAccess_Object_ID_Securable_ID_Privilege_ID")]
[Index("ModelId", "SecurableId", "ObjectId", "RoleId", Name = "ux_tblSecurityRoleAccess_Model_ID_Securable_ID_Object_ID_Role_ID", IsUnique = true)]
public partial class TblSecurityRoleAccess
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Privilege_ID")]
    public int PrivilegeId { get; set; }

    public byte AccessPermission { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Securable_ID")]
    public int SecurableId { get; set; }

    [Column("Object_ID")]
    public int ObjectId { get; set; }

    [StringLength(250)]
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

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("ModelId")]
    [InverseProperty("TblSecurityRoleAccesses")]
    public virtual TblModel Model { get; set; } = null!;

    [ForeignKey("PrivilegeId")]
    [InverseProperty("TblSecurityRoleAccesses")]
    public virtual TblSecurityPrivilege Privilege { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("TblSecurityRoleAccesses")]
    public virtual TblSecurityRole Role { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityRoleAccesscontrol
{
    [Column("RoleAccess_ID")]
    public int RoleAccessId { get; set; }

    [Column("RoleAccess_MUID")]
    public Guid RoleAccessMuid { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Principal_ID")]
    public int PrincipalId { get; set; }

    [Column("Principal_MUID")]
    public Guid? PrincipalMuid { get; set; }

    [Column("PrincipalType_ID")]
    public int PrincipalTypeId { get; set; }

    [Column("Principal_Name")]
    [StringLength(355)]
    public string? PrincipalName { get; set; }

    [Column("Object_ID")]
    public int ObjectId { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    public int IsModelAdministrator { get; set; }

    [Column("Securable_ID")]
    public int SecurableId { get; set; }

    [Column("Securable_MUID")]
    public Guid? SecurableMuid { get; set; }

    [Column("Securable_Name")]
    [StringLength(250)]
    public string? SecurableName { get; set; }

    [Column("Privilege_ID")]
    public int PrivilegeId { get; set; }

    public byte AccessPermission { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("EnterUserMUID")]
    public Guid? EnterUserMuid { get; set; }

    [StringLength(256)]
    public string? EnterUserName { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgUserMUID")]
    public Guid? LastChgUserMuid { get; set; }

    [StringLength(256)]
    public string? LastChgUserName { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }
}

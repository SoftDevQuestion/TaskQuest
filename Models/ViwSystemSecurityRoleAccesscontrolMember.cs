using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityRoleAccesscontrolMember
{
    [Column("RoleAccess_ID")]
    public int RoleAccessId { get; set; }

    [Column("RoleAccess_MUID")]
    public Guid RoleAccessMuid { get; set; }

    [Column("Privilege_ID")]
    public int PrivilegeId { get; set; }

    public byte AccessPermission { get; set; }

    [Column("Model_ID")]
    public int? ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid? ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string? ModelName { get; set; }

    public int IsModelAdministrator { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    [Column("Version_MUID")]
    public Guid? VersionMuid { get; set; }

    [Column("Version_Name")]
    [StringLength(50)]
    public string? VersionName { get; set; }

    [Column("HierarchyType_ID")]
    public byte HierarchyTypeId { get; set; }

    [Column("Hierarchy_ID")]
    public int? HierarchyId { get; set; }

    [Column("Hierarchy_MUID")]
    public Guid? HierarchyMuid { get; set; }

    [Column("Hierarchy_Name")]
    [StringLength(50)]
    public string? HierarchyName { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid? EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string? EntityName { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [Column("Member_ID")]
    public int MemberId { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("PrincipalType_ID")]
    public int PrincipalTypeId { get; set; }

    [Column("Principal_ID")]
    public int PrincipalId { get; set; }

    [Column("Principal_MUID")]
    public Guid? PrincipalMuid { get; set; }

    [Column("Principal_Name")]
    [StringLength(355)]
    public string? PrincipalName { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("EnterUserMUID")]
    public Guid? EnterUserMuid { get; set; }

    [StringLength(256)]
    public string? EnterUser { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgUserMUID")]
    public Guid? LastChgUserMuid { get; set; }

    [StringLength(256)]
    public string? LastChgUser { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }
}

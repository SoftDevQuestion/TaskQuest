using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityRoleAccessMember", Schema = "mdm")]
[Index("PrivilegeId", Name = "ix_tblSecurityRoleAccessMember_Privilege_ID")]
[Index("RoleId", Name = "ix_tblSecurityRoleAccessMember_Role_ID")]
[Index("EntityId", "VersionId", "RoleId", "ExplicitHierarchyId", "DerivedHierarchyId", "MemberTypeId", "MemberId", Name = "ux_tblSecurityRoleAccessMember_Entity_ID_Version_ID_Role_ID_ExplicitHierarchy_ID_DerivedHierarchy_ID_MemberType_ID_Member_ID", IsUnique = true)]
public partial class TblSecurityRoleAccessMember
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Privilege_ID")]
    public int PrivilegeId { get; set; }

    public byte AccessPermission { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("HierarchyType_ID")]
    public byte HierarchyTypeId { get; set; }

    [Column("ExplicitHierarchy_ID")]
    public int? ExplicitHierarchyId { get; set; }

    [Column("DerivedHierarchy_ID")]
    public int? DerivedHierarchyId { get; set; }

    [Column("Hierarchy_ID")]
    public int? HierarchyId { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [Column("Member_ID")]
    public int MemberId { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    public bool IsInitialized { get; set; }

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

    [ForeignKey("DerivedHierarchyId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblDerivedHierarchy? DerivedHierarchy { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblEntity Entity { get; set; } = null!;

    [ForeignKey("ExplicitHierarchyId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblHierarchy? ExplicitHierarchy { get; set; }

    [ForeignKey("PrivilegeId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblSecurityPrivilege Privilege { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblSecurityRole Role { get; set; } = null!;

    [ForeignKey("VersionId")]
    [InverseProperty("TblSecurityRoleAccessMembers")]
    public virtual TblModelVersion Version { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
[Table("tblviwSystemSecurityRole", Schema = "mdm")]
public partial class TblviwSystemSecurityRole
{
    [Column("Role_ID")]
    public int? RoleId { get; set; }

    [Column("Model_ID")]
    public int? ModelId { get; set; }

    [Column("Model_PrivilegeID")]
    public int? ModelPrivilegeId { get; set; }

    [Column("Model_AccessPermission")]
    public int? ModelAccessPermission { get; set; }

    [Column("Model_IsExplicit")]
    public int? ModelIsExplicit { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Entity_PrivilegeID")]
    public int? EntityPrivilegeId { get; set; }

    [Column("Entity_AccessPermission")]
    public int? EntityAccessPermission { get; set; }

    [Column("Entity_IsExplicit")]
    public int? EntityIsExplicit { get; set; }

    [Column("MemberType_ID")]
    public int? MemberTypeId { get; set; }

    [Column("MemberType_PrivilegeID")]
    public int? MemberTypePrivilegeId { get; set; }

    [Column("MemberType_AccessPermission")]
    public int? MemberTypeAccessPermission { get; set; }

    [Column("MemberType_IsExplicit")]
    public int? MemberTypeIsExplicit { get; set; }

    [Column("Attribute_ID")]
    public int? AttributeId { get; set; }

    [Column("Attribute_PrivilegeID")]
    public int? AttributePrivilegeId { get; set; }

    [Column("Attribute_AccessPermission")]
    public int? AttributeAccessPermission { get; set; }

    [Column("Attribute_IsExplicit")]
    public int? AttributeIsExplicit { get; set; }
}

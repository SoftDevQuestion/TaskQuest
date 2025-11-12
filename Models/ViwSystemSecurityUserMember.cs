using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityUserMember
{
    [Column("User_ID")]
    public int? UserId { get; set; }

    [Column("Privilege_ID")]
    public int? PrivilegeId { get; set; }

    public int? AccessPermission { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("Hierarchy_ID")]
    public int? HierarchyId { get; set; }

    [Column("HierarchyType_ID")]
    public byte HierarchyTypeId { get; set; }

    [Column("Member_ID")]
    public int MemberId { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    public bool IsMapped { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityRoleMember
{
    [Column("Role_ID")]
    public int RoleId { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    public int LeafOnly { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    [Column("Hierarchy_ID")]
    public int HierarchyId { get; set; }

    [Column("HierarchyType_ID")]
    public int HierarchyTypeId { get; set; }

    public bool IsInitialized { get; set; }
}

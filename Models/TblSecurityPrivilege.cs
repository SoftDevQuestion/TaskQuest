using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityPrivilege", Schema = "mdm")]
public partial class TblSecurityPrivilege
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(15)]
    public string Code { get; set; } = null!;

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Description { get; set; }

    [InverseProperty("Privilege")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();

    [InverseProperty("Privilege")]
    public virtual ICollection<TblSecurityRoleAccess> TblSecurityRoleAccesses { get; set; } = new List<TblSecurityRoleAccess>();
}

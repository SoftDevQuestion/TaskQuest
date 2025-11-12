using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblHierarchy", Schema = "mdm")]
[Index("Muid", Name = "ux_tblHierarchy_MUID", IsUnique = true)]
[Index("EntityId", "Name", Name = "ux_tblHierarchy_Name", IsUnique = true)]
public partial class TblHierarchy
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public bool IsMandatory { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("EnterVersionID")]
    public int EnterVersionId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgVersionID")]
    public int LastChgVersionId { get; set; }

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [ForeignKey("EntityId")]
    [InverseProperty("TblHierarchies")]
    public virtual TblEntity Entity { get; set; } = null!;

    [InverseProperty("ExplicitHierarchy")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();
}

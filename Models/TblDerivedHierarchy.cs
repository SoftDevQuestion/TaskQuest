using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblDerivedHierarchy", Schema = "mdm")]
[Index("Muid", Name = "ux_tblDerivedHierarchy_MUID", IsUnique = true)]
[Index("ModelId", "Name", Name = "ux_tblDerivedHierarchy_Model_ID_Name", IsUnique = true)]
public partial class TblDerivedHierarchy
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public bool AnchorNullRecursions { get; set; }

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

    [ForeignKey("ModelId")]
    [InverseProperty("TblDerivedHierarchies")]
    public virtual TblModel Model { get; set; } = null!;

    [InverseProperty("DerivedHierarchy")]
    public virtual ICollection<TblDerivedHierarchyDetail> TblDerivedHierarchyDetails { get; set; } = new List<TblDerivedHierarchyDetail>();

    [InverseProperty("DerivedHierarchy")]
    public virtual ICollection<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; } = new List<TblSecurityRoleAccessMember>();

    [InverseProperty("DerivedHierarchy")]
    public virtual ICollection<TblSubscriptionView> TblSubscriptionViews { get; set; } = new List<TblSubscriptionView>();
}

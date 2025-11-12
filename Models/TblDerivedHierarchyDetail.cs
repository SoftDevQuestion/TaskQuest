using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblDerivedHierarchyDetail", Schema = "mdm")]
[Index("Muid", Name = "ck_tblDerivedHierarchyDetail_MUID", IsUnique = true)]
[Index("DerivedHierarchyId", "ForeignTypeId", "ForeignId", "LevelId", Name = "ix_tblDerivedHierarchyDetail_DerivedHierarchy_ID_ForeignType_ID_Foreign_ID_Level_ID")]
[Index("ManyToManyChildAttributeId", Name = "ix_tblDerivedHierarchyDetail_ManyToManyChildAttribute_ID")]
public partial class TblDerivedHierarchyDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("DerivedHierarchy_ID")]
    public int DerivedHierarchyId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [Column("ForeignParent_ID")]
    public int? ForeignParentId { get; set; }

    [Column("Foreign_ID")]
    public int? ForeignId { get; set; }

    [Column("ForeignType_ID")]
    public byte ForeignTypeId { get; set; }

    [Column("ManyToManyChildAttribute_ID")]
    public int? ManyToManyChildAttributeId { get; set; }

    [Column("Level_ID")]
    public int? LevelId { get; set; }

    public bool IsVisible { get; set; }

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

    [ForeignKey("DerivedHierarchyId")]
    [InverseProperty("TblDerivedHierarchyDetails")]
    public virtual TblDerivedHierarchy DerivedHierarchy { get; set; } = null!;

    [ForeignKey("ManyToManyChildAttributeId")]
    [InverseProperty("TblDerivedHierarchyDetails")]
    public virtual TblAttribute? ManyToManyChildAttribute { get; set; }

    [InverseProperty("FilterHierarchyDetail")]
    public virtual ICollection<TblAttribute> TblAttributes { get; set; } = new List<TblAttribute>();
}

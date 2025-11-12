using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblAttributeGroup", Schema = "mdm")]
[Index("MemberTypeId", Name = "ix_tblAttributeGroup_MemberType_ID")]
[Index("EntityId", "MemberTypeId", "Name", Name = "ux_tblAttributeGroup_Entity_ID_MemberType_ID_Name", IsUnique = true)]
[Index("Muid", Name = "ux_tblAttributeGroup_MUID", IsUnique = true)]
public partial class TblAttributeGroup
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("MemberType_ID")]
    public byte MemberTypeId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public int SortOrder { get; set; }

    public bool FreezeNameCode { get; set; }

    public bool IsSystem { get; set; }

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

    [ForeignKey("EntityId")]
    [InverseProperty("TblAttributeGroups")]
    public virtual TblEntity Entity { get; set; } = null!;

    [InverseProperty("AttributeGroup")]
    public virtual ICollection<TblAttributeGroupDetail> TblAttributeGroupDetails { get; set; } = new List<TblAttributeGroupDetail>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblAttributeGroupDetail", Schema = "mdm")]
[Index("AttributeId", Name = "ix_tblAttributeGroupDetail_Attribute_ID")]
[Index("AttributeGroupId", "AttributeId", Name = "ux_tblAttributeGroupDetail_AttributeGroup_ID_Attribute_ID", IsUnique = true)]
[Index("Muid", Name = "ux_tblAttributeGroupDetail_MUID", IsUnique = true)]
public partial class TblAttributeGroupDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("AttributeGroup_ID")]
    public int AttributeGroupId { get; set; }

    [Column("Attribute_ID")]
    public int AttributeId { get; set; }

    public int SortOrder { get; set; }

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

    [ForeignKey("AttributeId")]
    [InverseProperty("TblAttributeGroupDetails")]
    public virtual TblAttribute Attribute { get; set; } = null!;

    [ForeignKey("AttributeGroupId")]
    [InverseProperty("TblAttributeGroupDetails")]
    public virtual TblAttributeGroup AttributeGroup { get; set; } = null!;
}

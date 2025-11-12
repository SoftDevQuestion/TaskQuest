using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblIndex", Schema = "mdm")]
[Index("SysIndexId", Name = "ix_tblIndex_SysIndex_ID")]
[Index("EntityId", "Name", Name = "ux_tblIndex_Entity_ID_Name", IsUnique = true)]
[Index("Muid", Name = "ux_tblIndex_MUID", IsUnique = true)]
public partial class TblIndex
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("SysIndex_ID")]
    public int? SysIndexId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    public bool IsUnique { get; set; }

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

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [ForeignKey("EntityId")]
    [InverseProperty("TblIndices")]
    public virtual TblEntity Entity { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblModelVersionFlag", Schema = "mdm")]
[Index("Muid", Name = "ux_tblModelVersionFlag_MUID", IsUnique = true)]
[Index("ModelId", "Name", Name = "ux_tblModelVersionFlag_Model_ID_Name", IsUnique = true)]
public partial class TblModelVersionFlag
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

    [StringLength(500)]
    public string? Description { get; set; }

    [Column("Status_ID")]
    public byte StatusId { get; set; }

    [Column("CommittedOnly_ID")]
    public byte? CommittedOnlyId { get; set; }

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

    [ForeignKey("ModelId")]
    [InverseProperty("TblModelVersionFlags")]
    public virtual TblModel Model { get; set; } = null!;

    [InverseProperty("VersionFlag")]
    public virtual ICollection<TblModelVersion> TblModelVersions { get; set; } = new List<TblModelVersion>();
}

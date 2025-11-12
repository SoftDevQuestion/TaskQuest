using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblFile", Schema = "mdm")]
[Index("SourceId", Name = "ix_tblFile_Source_ID")]
[Index("Muid", Name = "ux_tblFile_MUID", IsUnique = true)]
public partial class TblFile
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Source_ID")]
    public int? SourceId { get; set; }

    [Column("Source_LastChgTS")]
    [MaxLength(8)]
    public byte[]? SourceLastChgTs { get; set; }

    [StringLength(250)]
    public string FileName { get; set; } = null!;

    [StringLength(200)]
    public string FileContentType { get; set; } = null!;

    public byte[]? FileContent { get; set; }

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

    [InverseProperty("Source")]
    public virtual ICollection<TblFile> InverseSource { get; set; } = new List<TblFile>();

    [ForeignKey("SourceId")]
    [InverseProperty("InverseSource")]
    public virtual TblFile? Source { get; set; }
}

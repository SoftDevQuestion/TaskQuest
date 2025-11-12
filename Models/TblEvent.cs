using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblEvent", Schema = "mdm")]
[Index("VersionId", "EntityId", Name = "ix_tblEvent_Version_ID_Entity_ID")]
[Index("Muid", Name = "ux_tblEvent_MUID", IsUnique = true)]
public partial class TblEvent
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [Column("Version_ID")]
    public int? VersionId { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [StringLength(100)]
    public string EventName { get; set; } = null!;

    [Column("EventStatus_ID")]
    public byte EventStatusId { get; set; }

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

    public string? ErrorMsg { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblEvents")]
    public virtual TblEntity? Entity { get; set; }

    [ForeignKey("VersionId")]
    [InverseProperty("TblEvents")]
    public virtual TblModelVersion? Version { get; set; }
}

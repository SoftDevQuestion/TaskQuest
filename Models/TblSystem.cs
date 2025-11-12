using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSystem", Schema = "mdm")]
public partial class TblSystem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(250)]
    public string SchemaVersion { get; set; } = null!;

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    [StringLength(250)]
    public string EnterUserId { get; set; } = null!;

    [Column("LastChgUserID")]
    [StringLength(250)]
    public string LastChgUserId { get; set; } = null!;

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }
}

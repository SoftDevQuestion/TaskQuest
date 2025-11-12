using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblDBUpgradeHistory", Schema = "mdm")]
public partial class TblDbupgradeHistory
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("DBVersion")]
    public int Dbversion { get; set; }

    [StringLength(250)]
    public string EnterUser { get; set; } = null!;

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }
}

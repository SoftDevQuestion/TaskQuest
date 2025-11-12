using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblDBErrors", Schema = "mdm")]
public partial class TblDberror
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Language_ID")]
    public int LanguageId { get; set; }

    [StringLength(4000)]
    public string Text { get; set; } = null!;

    [StringLength(250)]
    public string Category { get; set; } = null!;

    [StringLength(4000)]
    public string? Comment { get; set; }
}

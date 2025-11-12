using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblLocalizedStrings", Schema = "mdm")]
public partial class TblLocalizedString
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int LanguageCode { get; set; }

    [StringLength(100)]
    public string ResourceName { get; set; } = null!;

    public string? LocalizedValue { get; set; }
}

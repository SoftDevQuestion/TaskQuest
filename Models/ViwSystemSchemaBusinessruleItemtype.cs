using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaBusinessruleItemtype
{
    [Column("AppliesTo_ID")]
    public int AppliesToId { get; set; }

    [Column("ApplyTo_ID")]
    public int ApplyToId { get; set; }

    [Column("ApplyToCategoryID")]
    public int ApplyToCategoryId { get; set; }

    [StringLength(50)]
    public string ApplyToCategory { get; set; } = null!;

    [Column("BRTypeID")]
    public int BrtypeId { get; set; }

    [Column("BRSubTypeID")]
    public int BrsubTypeId { get; set; }

    [Column("BRSubTypeIsVisible")]
    public int BrsubTypeIsVisible { get; set; }

    [Column("BRItemType_ID")]
    public int BritemTypeId { get; set; }

    public int DisplaySequence { get; set; }

    public int DisplaySubSequence { get; set; }
}

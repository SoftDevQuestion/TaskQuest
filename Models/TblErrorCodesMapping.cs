using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblErrorCodesMapping", Schema = "mdm")]
public partial class TblErrorCodesMapping
{
    [Key]
    public int Bitmask { get; set; }

    public int UniqueErrorCode { get; set; }
}

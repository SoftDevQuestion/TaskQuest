using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblCodeGenInfo", Schema = "mdm")]
public partial class TblCodeGenInfo
{
    [Key]
    public int EntityId { get; set; }

    public int Seed { get; set; }

    public long? LargestCodeValue { get; set; }

    [ForeignKey("EntityId")]
    [InverseProperty("TblCodeGenInfo")]
    public virtual TblEntity Entity { get; set; } = null!;
}

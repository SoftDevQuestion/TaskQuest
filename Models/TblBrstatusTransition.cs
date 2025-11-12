using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[PrimaryKey("ActionId", "CurrentStatusId")]
[Table("tblBRStatusTransition", Schema = "mdm")]
public partial class TblBrstatusTransition
{
    [Key]
    [Column("Action_ID")]
    public int ActionId { get; set; }

    [Key]
    [Column("CurrentStatus_ID")]
    public int CurrentStatusId { get; set; }

    [Column("NewStatus_ID")]
    public int NewStatusId { get; set; }
}

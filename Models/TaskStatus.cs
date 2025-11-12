using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("TaskStatus")]
public partial class TaskStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(50)]
    public string? StatusName { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}

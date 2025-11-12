using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

public partial class Task
{
    [Key]
    [Column("TaskID")]
    public int TaskId { get; set; }

    [Column("ProjectID")]
    public int ProjectId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? DueDate { get; set; }

    [StringLength(20)]
    public string? Priority { get; set; }

    [Column("StatusID")]
    public int StatusId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("Tasks")]
    public virtual Project Project { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Tasks")]
    public virtual TaskStatus Status { get; set; } = null!;
}

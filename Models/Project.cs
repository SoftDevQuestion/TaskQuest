using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

public partial class Project
{
    [Key]
    [Column("ProjectID")]
    public int ProjectId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string ProjectName { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Project")]
    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    [ForeignKey("UserId")]
    [InverseProperty("Projects")]
    public virtual User User { get; set; } = null!;
}

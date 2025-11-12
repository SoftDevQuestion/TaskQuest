using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Index("Username", Name = "UQ__Users__536C85E4DE6B9732", IsUnique = true)]
[Index("Email", Name = "UQ__Users__A9D10534A4025ACD", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("UserID")]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(50)]
    public string Username { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblListRelationshipType", Schema = "mdm")]
public partial class TblListRelationshipType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("ListRelationshipType")]
    public virtual ICollection<TblListRelationship> TblListRelationships { get; set; } = new List<TblListRelationship>();
}

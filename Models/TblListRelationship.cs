using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblListRelationship", Schema = "mdm")]
[Index("ListRelationshipTypeId", Name = "ix_tblListRelationship_ListRelationshipType_ID")]
public partial class TblListRelationship
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string ParentListCode { get; set; } = null!;

    [Column("Parent_ID")]
    public int ParentId { get; set; }

    [StringLength(50)]
    public string ChildListCode { get; set; } = null!;

    [Column("Child_ID")]
    public int ChildId { get; set; }

    [Column("ListRelationshipType_ID")]
    public int ListRelationshipTypeId { get; set; }

    [ForeignKey("ListRelationshipTypeId")]
    [InverseProperty("TblListRelationships")]
    public virtual TblListRelationshipType ListRelationshipType { get; set; } = null!;

    [InverseProperty("ApplyTo")]
    public virtual ICollection<TblBritemTypeAppliesTo> TblBritemTypeAppliesTos { get; set; } = new List<TblBritemTypeAppliesTo>();
}

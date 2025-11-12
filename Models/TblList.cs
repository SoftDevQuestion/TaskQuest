using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblList", Schema = "mdm")]
[Index("ListCode", "ListOption", Name = "ix_tblList_ListCode_ListOption", IsUnique = true)]
[Index("ListCode", "OptionId", "GroupId", Name = "ix_tblList_ListCode_OptionID_Group_ID", IsUnique = true)]
public partial class TblList
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string ListCode { get; set; } = null!;

    [StringLength(50)]
    public string ListName { get; set; } = null!;

    public int Seq { get; set; }

    [StringLength(250)]
    public string ListOption { get; set; } = null!;

    [Column("OptionID")]
    public int OptionId { get; set; }

    public bool IsVisible { get; set; }

    [Column("Group_ID")]
    public int GroupId { get; set; }
}

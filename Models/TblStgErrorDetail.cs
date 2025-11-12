using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblStgErrorDetail", Schema = "mdm")]
[Index("BatchId", "Code", Name = "ix_tblStgErrorDetail_Batch_ID_Code")]
[Index("Id", Name = "ix_tblStgErrorDetail_ID")]
public partial class TblStgErrorDetail
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Batch_ID")]
    public int BatchId { get; set; }

    [StringLength(250)]
    public string? Code { get; set; }

    [StringLength(100)]
    public string AttributeName { get; set; } = null!;

    [StringLength(2000)]
    public string? AttributeValue { get; set; }

    public int UniqueErrorCode { get; set; }
}

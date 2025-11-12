using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwEntityStagingBatchesAllProcessedExceptCleared
{
    [Column("Batch_ID")]
    public int BatchId { get; set; }

    [StringLength(50)]
    public string? BatchTag { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("MemberTypeID")]
    public byte? MemberTypeId { get; set; }

    [Column("Version_MUID")]
    public Guid VersionMuid { get; set; }

    [StringLength(50)]
    public string VersionName { get; set; } = null!;

    public byte Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProcessStart { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProcessEnd { get; set; }

    [Column("ProcessUserID")]
    public int? ProcessUserId { get; set; }

    public int? TotalRows { get; set; }

    public int? RowsInError { get; set; }

    [Column("Model_ID")]
    public int ModelId { get; set; }
}

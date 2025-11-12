using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
[Table("tblDataQualityOperationsState", Schema = "mdm")]
public partial class TblDataQualityOperationsState
{
    [Column("CreateDTM")]
    [Precision(3)]
    public DateTime CreateDtm { get; set; }

    public Guid? OperationId { get; set; }

    public string? SerializedOperation { get; set; }
}

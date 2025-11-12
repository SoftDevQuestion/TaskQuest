using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaEntityMembertype
{
    [Column("Entity_ID")]
    public int EntityId { get; set; }

    [Column("MemberType_ID")]
    public int MemberTypeId { get; set; }
}

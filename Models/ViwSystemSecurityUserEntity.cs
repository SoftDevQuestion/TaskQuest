using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityUserEntity
{
    [Column("User_ID")]
    public int UserId { get; set; }

    public int? IsSyncTarget { get; set; }

    [Column("ID")]
    public int Id { get; set; }

    [Column("Privilege_ID")]
    public int? PrivilegeId { get; set; }

    public int? AccessPermission { get; set; }
}

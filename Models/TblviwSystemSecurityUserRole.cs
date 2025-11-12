using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
[Table("tblviwSystemSecurityUserRole", Schema = "mdm")]
public partial class TblviwSystemSecurityUserRole
{
    [Column("User_ID")]
    public int? UserId { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    public int IsUserGroupAssignment { get; set; }
}

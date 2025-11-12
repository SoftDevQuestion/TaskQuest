using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemUsergroupUser
{
    [Column("UserGroup_ID")]
    public int UserGroupId { get; set; }

    [Column("UserGroup_MUID")]
    public Guid UserGroupMuid { get; set; }

    [Column("UserGroup_SID")]
    [StringLength(250)]
    public string? UserGroupSid { get; set; }

    [Column("UserGroup_Name")]
    [StringLength(355)]
    public string UserGroupName { get; set; } = null!;

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("User_MUID")]
    public Guid UserMuid { get; set; }

    [Column("User_SID")]
    [StringLength(250)]
    public string? UserSid { get; set; }

    [Column("User_Name")]
    [StringLength(100)]
    public string UserName { get; set; } = null!;
}

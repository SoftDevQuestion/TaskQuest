using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSecurityUserFunction
{
    [Column("RoleAccess_ID")]
    public int RoleAccessId { get; set; }

    [Column("RoleAccess_MUID")]
    public Guid RoleAccessMuid { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [Column("User_MUID")]
    public Guid? UserMuid { get; set; }

    [StringLength(355)]
    public string? UserName { get; set; }

    [Column("Principal_ID")]
    public int PrincipalId { get; set; }

    [Column("Principal_MUID")]
    public Guid? PrincipalMuid { get; set; }

    [Column("PrincipalType_ID")]
    public int PrincipalTypeId { get; set; }

    [Column("Principal_Name")]
    [StringLength(355)]
    public string? PrincipalName { get; set; }

    [Column("Function_ID")]
    public byte FunctionId { get; set; }

    [Column("Function_Name")]
    [StringLength(50)]
    public string FunctionName { get; set; } = null!;

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("EnterUserMUID")]
    public Guid? EnterUserMuid { get; set; }

    [StringLength(100)]
    public string? EnterUserName { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("LastChgUserMUID")]
    public Guid? LastChgUserMuid { get; set; }

    [StringLength(100)]
    public string? LastChgUserName { get; set; }
}

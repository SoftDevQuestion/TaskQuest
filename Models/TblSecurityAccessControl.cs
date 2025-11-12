using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityAccessControl", Schema = "mdm")]
[Index("PrincipalTypeId", "PrincipalId", Name = "ix_tblSecurityAccessControl_PrincipalType_ID_Principal_ID")]
[Index("RoleId", Name = "ix_tblSecurityAccessControl_Role_ID")]
public partial class TblSecurityAccessControl
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PrincipalType_ID")]
    public int PrincipalTypeId { get; set; }

    [Column("Principal_ID")]
    public int PrincipalId { get; set; }

    [Column("Role_ID")]
    public int RoleId { get; set; }

    [StringLength(110)]
    public string? Description { get; set; }

    [Column("EnterDTM")]
    [Precision(3)]
    public DateTime EnterDtm { get; set; }

    [Column("EnterUserID")]
    public int EnterUserId { get; set; }

    [Column("LastChgDTM")]
    [Precision(3)]
    public DateTime LastChgDtm { get; set; }

    [Column("LastChgUserID")]
    public int LastChgUserId { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("TblSecurityAccessControls")]
    public virtual TblSecurityRole Role { get; set; } = null!;
}

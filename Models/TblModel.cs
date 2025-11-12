using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblModel", Schema = "mdm")]
[Index("Muid", Name = "ux_tblModel_MUID", IsUnique = true)]
[Index("Name", Name = "ux_tblModel_Name", IsUnique = true)]
public partial class TblModel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("MUID")]
    public Guid Muid { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(500)]
    public string? Description { get; set; }

    public int? LogRetentionDays { get; set; }

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

    [Column("LastChgTS")]
    public byte[] LastChgTs { get; set; } = null!;

    [InverseProperty("Model")]
    public virtual ICollection<TblDerivedHierarchy> TblDerivedHierarchies { get; set; } = new List<TblDerivedHierarchy>();

    [InverseProperty("Model")]
    public virtual ICollection<TblEntity> TblEntities { get; set; } = new List<TblEntity>();

    [InverseProperty("Model")]
    public virtual ICollection<TblModelVersionFlag> TblModelVersionFlags { get; set; } = new List<TblModelVersionFlag>();

    [InverseProperty("Model")]
    public virtual ICollection<TblModelVersion> TblModelVersions { get; set; } = new List<TblModelVersion>();

    [InverseProperty("Model")]
    public virtual ICollection<TblNotificationQueue> TblNotificationQueues { get; set; } = new List<TblNotificationQueue>();

    [InverseProperty("Model")]
    public virtual ICollection<TblSecurityRoleAccess> TblSecurityRoleAccesses { get; set; } = new List<TblSecurityRoleAccess>();

    [InverseProperty("Model")]
    public virtual ICollection<TblSubscriptionView> TblSubscriptionViews { get; set; } = new List<TblSubscriptionView>();
}

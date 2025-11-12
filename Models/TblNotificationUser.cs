using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblNotificationUsers", Schema = "mdm")]
[Index("UserId", Name = "ix_tblNotificationUsers_User_ID")]
public partial class TblNotificationUser
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Notification_ID")]
    public int NotificationId { get; set; }

    [Column("User_ID")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("TblNotificationUsers")]
    public virtual TblUser User { get; set; } = null!;
}

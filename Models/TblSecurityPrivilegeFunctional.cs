using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Table("tblSecurityPrivilegeFunctional", Schema = "mdm")]
public partial class TblSecurityPrivilegeFunctional
{
    [Key]
    [Column("ID")]
    public byte Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("FunctionalPrivilege")]
    public virtual ICollection<TblSecurityRoleAccessFunctional> TblSecurityRoleAccessFunctionals { get; set; } = new List<TblSecurityRoleAccessFunctional>();
}

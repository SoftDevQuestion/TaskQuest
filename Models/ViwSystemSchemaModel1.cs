using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

[Keyless]
public partial class ViwSystemSchemaModel1
{
    [Column("Model_ID")]
    public int ModelId { get; set; }

    [Column("Model_MUID")]
    public Guid ModelMuid { get; set; }

    [Column("Model_Name")]
    [StringLength(50)]
    public string ModelName { get; set; } = null!;

    [Column("Model_Label")]
    [StringLength(258)]
    public string? ModelLabel { get; set; }

    [Column("Model_Description")]
    [StringLength(500)]
    public string? ModelDescription { get; set; }

    [Column("Version_ID")]
    public int VersionId { get; set; }

    [Column("Version_MUID")]
    public Guid VersionMuid { get; set; }

    [Column("Version_Name")]
    [StringLength(50)]
    public string VersionName { get; set; } = null!;

    [Column("Version_Number")]
    public int VersionNumber { get; set; }

    [Column("Version_Description")]
    [StringLength(500)]
    public string? VersionDescription { get; set; }

    [Column("Version_UserIDCreated")]
    public int VersionUserIdcreated { get; set; }

    [Column("Version_UserNameCreated")]
    [StringLength(100)]
    public string? VersionUserNameCreated { get; set; }

    [Column("Version_DateCreated")]
    [Precision(3)]
    public DateTime VersionDateCreated { get; set; }

    [Column("Version_UserIDUpdated")]
    public int VersionUserIdupdated { get; set; }

    [Column("Version_UserNameUpdated")]
    [StringLength(100)]
    public string? VersionUserNameUpdated { get; set; }

    [Column("Version_DateUpdated")]
    [Precision(3)]
    public DateTime VersionDateUpdated { get; set; }

    [Column("Version_StatusID")]
    public int VersionStatusId { get; set; }

    [Column("Version_Status")]
    [StringLength(250)]
    public string VersionStatus { get; set; } = null!;

    [Column("Version_FlagID")]
    public int? VersionFlagId { get; set; }

    [Column("Version_FlagName")]
    [StringLength(50)]
    public string? VersionFlagName { get; set; }

    [Column("Version_SourceVersionID")]
    public int? VersionSourceVersionId { get; set; }

    [Column("Entity_ID")]
    public int? EntityId { get; set; }

    [Column("Entity_MUID")]
    public Guid? EntityMuid { get; set; }

    [Column("Entity_Name")]
    [StringLength(50)]
    public string? EntityName { get; set; }

    [Column("Entity_Label")]
    [StringLength(517)]
    public string EntityLabel { get; set; } = null!;

    [Column("Entity_IsBase")]
    public bool? EntityIsBase { get; set; }

    [Column("Entity_HasHierarchy")]
    public int EntityHasHierarchy { get; set; }

    [Column("Entity_UserIDCreated")]
    public int? EntityUserIdcreated { get; set; }

    [Column("Entity_UserNameCreated")]
    [StringLength(100)]
    public string? EntityUserNameCreated { get; set; }

    [Column("Entity_DateCreated")]
    [Precision(3)]
    public DateTime? EntityDateCreated { get; set; }

    [Column("Entity_UserIDUpdated")]
    public int? EntityUserIdupdated { get; set; }

    [Column("Entity_UserNameUpdated")]
    [StringLength(100)]
    public string? EntityUserNameUpdated { get; set; }

    [Column("Entity_DateUpdated")]
    [Precision(3)]
    public DateTime? EntityDateUpdated { get; set; }

    [Column("Entity_VersionIDUpdated")]
    public int? EntityVersionIdupdated { get; set; }

    [Column("Entity_Table")]
    [StringLength(128)]
    public string? EntityTable { get; set; }

    [Column("Entity_HierarchyTable")]
    [StringLength(128)]
    public string? EntityHierarchyTable { get; set; }

    [Column("Entity_HierarchyParentTable")]
    [StringLength(128)]
    public string? EntityHierarchyParentTable { get; set; }

    [Column("Entity_CollectionTable")]
    [StringLength(128)]
    public string? EntityCollectionTable { get; set; }

    [Column("Entity_CollectionMemberTable")]
    [StringLength(128)]
    public string? EntityCollectionMemberTable { get; set; }

    [Column("Hierarchy_ID")]
    public int? HierarchyId { get; set; }

    [Column("Hierarchy_MUID")]
    public Guid? HierarchyMuid { get; set; }

    [Column("Hierarchy_Name")]
    [StringLength(50)]
    public string? HierarchyName { get; set; }

    [Column("Hierarchy_Label")]
    [StringLength(526)]
    public string HierarchyLabel { get; set; } = null!;

    [Column("HierarchyType_ID")]
    public int? HierarchyTypeId { get; set; }

    [Column("Hierarchy_IsMandatory")]
    public bool? HierarchyIsMandatory { get; set; }

    [Column("Hierarchy_UserIDCreated")]
    public int? HierarchyUserIdcreated { get; set; }

    [Column("Hierarchy_UserNameCreated")]
    [StringLength(100)]
    public string? HierarchyUserNameCreated { get; set; }

    [Column("Hierarchy_DateCreated")]
    [Precision(3)]
    public DateTime? HierarchyDateCreated { get; set; }

    [Column("Hierarchy_UserIDUpdated")]
    public int? HierarchyUserIdupdated { get; set; }

    [Column("Hierarchy_UserNameUpdated")]
    [StringLength(100)]
    public string? HierarchyUserNameUpdated { get; set; }

    [Column("Hierarchy_DateUpdated")]
    [Precision(3)]
    public DateTime? HierarchyDateUpdated { get; set; }

    [Column("Hierarchy_VersionIDUpdated")]
    public int? HierarchyVersionIdupdated { get; set; }

    [Column("MemberType_ID")]
    public int MemberTypeId { get; set; }

    [Column("MemberType_Name")]
    [StringLength(12)]
    public string MemberTypeName { get; set; } = null!;

    [Column("MemberType_Label")]
    [StringLength(776)]
    public string MemberTypeLabel { get; set; } = null!;
}

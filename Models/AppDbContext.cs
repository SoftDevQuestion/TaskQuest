using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TaskQuest.Web.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<TblAttribute> TblAttributes { get; set; }

    public virtual DbSet<TblAttributeGroup> TblAttributeGroups { get; set; }

    public virtual DbSet<TblAttributeGroupDetail> TblAttributeGroupDetails { get; set; }

    public virtual DbSet<TblBrbusinessRule> TblBrbusinessRules { get; set; }

    public virtual DbSet<TblBritem> TblBritems { get; set; }

    public virtual DbSet<TblBritemProperty> TblBritemProperties { get; set; }

    public virtual DbSet<TblBritemTypeAppliesTo> TblBritemTypeAppliesTos { get; set; }

    public virtual DbSet<TblBrlogicalOperatorGroup> TblBrlogicalOperatorGroups { get; set; }

    public virtual DbSet<TblBrstatusTransition> TblBrstatusTransitions { get; set; }

    public virtual DbSet<TblCodeGenInfo> TblCodeGenInfos { get; set; }

    public virtual DbSet<TblDataQualityOperationsState> TblDataQualityOperationsStates { get; set; }

    public virtual DbSet<TblDberror> TblDberrors { get; set; }

    public virtual DbSet<TblDbupgradeHistory> TblDbupgradeHistories { get; set; }

    public virtual DbSet<TblDerivedHierarchy> TblDerivedHierarchies { get; set; }

    public virtual DbSet<TblDerivedHierarchyDetail> TblDerivedHierarchyDetails { get; set; }

    public virtual DbSet<TblEntity> TblEntities { get; set; }

    public virtual DbSet<TblErrorCodesMapping> TblErrorCodesMappings { get; set; }

    public virtual DbSet<TblEvent> TblEvents { get; set; }

    public virtual DbSet<TblExternalSystem> TblExternalSystems { get; set; }

    public virtual DbSet<TblFile> TblFiles { get; set; }

    public virtual DbSet<TblHierarchy> TblHierarchies { get; set; }

    public virtual DbSet<TblIndex> TblIndices { get; set; }

    public virtual DbSet<TblList> TblLists { get; set; }

    public virtual DbSet<TblListRelationship> TblListRelationships { get; set; }

    public virtual DbSet<TblListRelationshipType> TblListRelationshipTypes { get; set; }

    public virtual DbSet<TblLocalizedString> TblLocalizedStrings { get; set; }

    public virtual DbSet<TblModel> TblModels { get; set; }

    public virtual DbSet<TblModelVersion> TblModelVersions { get; set; }

    public virtual DbSet<TblModelVersionFlag> TblModelVersionFlags { get; set; }

    public virtual DbSet<TblNotificationQueue> TblNotificationQueues { get; set; }

    public virtual DbSet<TblNotificationUser> TblNotificationUsers { get; set; }

    public virtual DbSet<TblSecurityAccessControl> TblSecurityAccessControls { get; set; }

    public virtual DbSet<TblSecurityObject> TblSecurityObjects { get; set; }

    public virtual DbSet<TblSecurityPrivilege> TblSecurityPrivileges { get; set; }

    public virtual DbSet<TblSecurityPrivilegeFunctional> TblSecurityPrivilegeFunctionals { get; set; }

    public virtual DbSet<TblSecurityRole> TblSecurityRoles { get; set; }

    public virtual DbSet<TblSecurityRoleAccess> TblSecurityRoleAccesses { get; set; }

    public virtual DbSet<TblSecurityRoleAccessFunctional> TblSecurityRoleAccessFunctionals { get; set; }

    public virtual DbSet<TblSecurityRoleAccessMember> TblSecurityRoleAccessMembers { get; set; }

    public virtual DbSet<TblStgBatch> TblStgBatches { get; set; }

    public virtual DbSet<TblStgErrorDetail> TblStgErrorDetails { get; set; }

    public virtual DbSet<TblSubscriptionView> TblSubscriptionViews { get; set; }

    public virtual DbSet<TblSyncRelationship> TblSyncRelationships { get; set; }

    public virtual DbSet<TblSystem> TblSystems { get; set; }

    public virtual DbSet<TblSystemSetting> TblSystemSettings { get; set; }

    public virtual DbSet<TblSystemSettingGroup> TblSystemSettingGroups { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    public virtual DbSet<TblUserGroup> TblUserGroups { get; set; }

    public virtual DbSet<TblUserGroupAssignment> TblUserGroupAssignments { get; set; }

    public virtual DbSet<TblUserPreference> TblUserPreferences { get; set; }

    public virtual DbSet<TblViwSystemSchemaAttribute> TblViwSystemSchemaAttributes { get; set; }

    public virtual DbSet<TblviwSystemSecurityRole> TblviwSystemSecurityRoles { get; set; }

    public virtual DbSet<TblviwSystemSecurityUserAttribute> TblviwSystemSecurityUserAttributes { get; set; }

    public virtual DbSet<TblviwSystemSecurityUserMemberType> TblviwSystemSecurityUserMemberTypes { get; set; }

    public virtual DbSet<TblviwSystemSecurityUserRole> TblviwSystemSecurityUserRoles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<ViwEntityStagingBatchesAllProcessed> ViwEntityStagingBatchesAllProcesseds { get; set; }

    public virtual DbSet<ViwEntityStagingBatchesAllProcessedExceptCleared> ViwEntityStagingBatchesAllProcessedExceptCleareds { get; set; }

    public virtual DbSet<ViwSystemBusinessrulesAttributeInheritanceHierarchy> ViwSystemBusinessrulesAttributeInheritanceHierarchies { get; set; }

    public virtual DbSet<ViwSystemBusinessrulesHierarchyChangevalueInheritance> ViwSystemBusinessrulesHierarchyChangevalueInheritances { get; set; }

    public virtual DbSet<ViwSystemSchemaAttribute> ViwSystemSchemaAttributes { get; set; }

    public virtual DbSet<ViwSystemSchemaAttributegroup> ViwSystemSchemaAttributegroups { get; set; }

    public virtual DbSet<ViwSystemSchemaAttributesOriginal> ViwSystemSchemaAttributesOriginals { get; set; }

    public virtual DbSet<ViwSystemSchemaBusinessrule> ViwSystemSchemaBusinessrules { get; set; }

    public virtual DbSet<ViwSystemSchemaBusinessruleItemtype> ViwSystemSchemaBusinessruleItemtypes { get; set; }

    public virtual DbSet<ViwSystemSchemaBusinessrulePropertiesAttribute> ViwSystemSchemaBusinessrulePropertiesAttributes { get; set; }

    public virtual DbSet<ViwSystemSchemaBusinessruleProperty> ViwSystemSchemaBusinessruleProperties { get; set; }

    public virtual DbSet<ViwSystemSchemaEntity> ViwSystemSchemaEntities { get; set; }

    public virtual DbSet<ViwSystemSchemaEntityMembertype> ViwSystemSchemaEntityMembertypes { get; set; }

    public virtual DbSet<ViwSystemSchemaHierarchyDerived> ViwSystemSchemaHierarchyDeriveds { get; set; }

    public virtual DbSet<ViwSystemSchemaHierarchyDerivedLevel> ViwSystemSchemaHierarchyDerivedLevels { get; set; }

    public virtual DbSet<ViwSystemSchemaHierarchyExplicit> ViwSystemSchemaHierarchyExplicits { get; set; }

    public virtual DbSet<ViwSystemSchemaIndex> ViwSystemSchemaIndexes { get; set; }

    public virtual DbSet<ViwSystemSchemaModel> ViwSystemSchemaModels { get; set; }

    public virtual DbSet<ViwSystemSchemaModel1> ViwSystemSchemaModels1 { get; set; }

    public virtual DbSet<ViwSystemSchemaSystemsetting> ViwSystemSchemaSystemsettings { get; set; }

    public virtual DbSet<ViwSystemSchemaVersion> ViwSystemSchemaVersions { get; set; }

    public virtual DbSet<ViwSystemSchemaVersionFlag> ViwSystemSchemaVersionFlags { get; set; }

    public virtual DbSet<ViwSystemSecurityRole> ViwSystemSecurityRoles { get; set; }

    public virtual DbSet<ViwSystemSecurityRoleAccesscontrol> ViwSystemSecurityRoleAccesscontrols { get; set; }

    public virtual DbSet<ViwSystemSecurityRoleAccesscontrolFunctional> ViwSystemSecurityRoleAccesscontrolFunctionals { get; set; }

    public virtual DbSet<ViwSystemSecurityRoleAccesscontrolMember> ViwSystemSecurityRoleAccesscontrolMembers { get; set; }

    public virtual DbSet<ViwSystemSecurityRoleMember> ViwSystemSecurityRoleMembers { get; set; }

    public virtual DbSet<ViwSystemSecurityRoleOriginal> ViwSystemSecurityRoleOriginals { get; set; }

    public virtual DbSet<ViwSystemSecurityUser> ViwSystemSecurityUsers { get; set; }

    public virtual DbSet<ViwSystemSecurityUserAttribute> ViwSystemSecurityUserAttributes { get; set; }

    public virtual DbSet<ViwSystemSecurityUserAttributeOriginal> ViwSystemSecurityUserAttributeOriginals { get; set; }

    public virtual DbSet<ViwSystemSecurityUserAttributegroup> ViwSystemSecurityUserAttributegroups { get; set; }

    public virtual DbSet<ViwSystemSecurityUserEntity> ViwSystemSecurityUserEntities { get; set; }

    public virtual DbSet<ViwSystemSecurityUserFunction> ViwSystemSecurityUserFunctions { get; set; }

    public virtual DbSet<ViwSystemSecurityUserHierarchy> ViwSystemSecurityUserHierarchies { get; set; }

    public virtual DbSet<ViwSystemSecurityUserHierarchyDerived> ViwSystemSecurityUserHierarchyDeriveds { get; set; }

    public virtual DbSet<ViwSystemSecurityUserIndex> ViwSystemSecurityUserIndices { get; set; }

    public virtual DbSet<ViwSystemSecurityUserMember> ViwSystemSecurityUserMembers { get; set; }

    public virtual DbSet<ViwSystemSecurityUserMembertype> ViwSystemSecurityUserMembertypes { get; set; }

    public virtual DbSet<ViwSystemSecurityUserMembertypeOriginal> ViwSystemSecurityUserMembertypeOriginals { get; set; }

    public virtual DbSet<ViwSystemSecurityUserModel> ViwSystemSecurityUserModels { get; set; }

    public virtual DbSet<ViwSystemSecurityUserRole> ViwSystemSecurityUserRoles { get; set; }

    public virtual DbSet<ViwSystemSecurityUserRoleOriginal> ViwSystemSecurityUserRoleOriginals { get; set; }

    public virtual DbSet<ViwSystemUsergroupUser> ViwSystemUsergroupUsers { get; set; }

    public virtual DbSet<ViwSystemVersion> ViwSystemVersions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS01;Database=TaskQuestDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__761ABED05CFDEA83");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Users");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__Tasks__7C6949D1E63F96CC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Priority).HasDefaultValue("Normal");

            entity.HasOne(d => d.Project).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Projects");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Status");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TaskStat__C8EE20437A5F0ED8");
        });

        modelBuilder.Entity<TblAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblAttribute");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.DomainEntity).WithMany(p => p.TblAttributeDomainEntities).HasConstraintName("fk_tblAttribute_tblEntity_DomainEntity_ID");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblAttributeEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblAttribute_tblEntity_Entity_ID");

            entity.HasOne(d => d.FilterHierarchyDetail).WithMany(p => p.TblAttributes).HasConstraintName("fk_tblAttribute_tblDerivedHierarchyDetail_FilterHierarchyDetail_ID");

            entity.HasOne(d => d.FilterParentAttribute).WithMany(p => p.InverseFilterParentAttribute).HasConstraintName("fk_tblAttribute_tblAttribute_FilterParentAttribute_ID");

            entity.HasOne(d => d.Source).WithMany(p => p.InverseSource).HasConstraintName("fk_tblAttribute_tblAttribute_Source_ID");
        });

        modelBuilder.Entity<TblAttributeGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblAttributeGroup");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblAttributeGroups).HasConstraintName("fk_tblAttributeGroup_tblEntity_Entity_ID");
        });

        modelBuilder.Entity<TblAttributeGroupDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblAttributeGroupDetail");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.AttributeGroup).WithMany(p => p.TblAttributeGroupDetails).HasConstraintName("fk_tblAttributeGroupDetail_tblAttribute_AttributeGroup_ID");

            entity.HasOne(d => d.Attribute).WithMany(p => p.TblAttributeGroupDetails).HasConstraintName("fk_tblAttributeGroupDetail_tblAttribute_AttributeID");
        });

        modelBuilder.Entity<TblBrbusinessRule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblBRBusinessRule");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblBrbusinessRules).HasConstraintName("fk_tblBRBusinessRule_tblEntity_Entity_ID");
        });

        modelBuilder.Entity<TblBritem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblBRItem");

            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.BritemAppliesTo).WithMany(p => p.TblBritems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblBRItem_tblBRItemTypeAppliesTo_BRItemAppliesTo_ID");

            entity.HasOne(d => d.BrlogicalOperatorGroup).WithMany(p => p.TblBritems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblBRItem_tblLogicalOperatorGroup_BRLogicalOperatorGroup_ID");
        });

        modelBuilder.Entity<TblBritemProperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblBRItemProperties");

            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Britem).WithMany(p => p.TblBritemProperties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblBRItemProperties_tblBRItem_BRItem_ID");
        });

        modelBuilder.Entity<TblBritemTypeAppliesTo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblBRItemTypeAppliesTo");

            entity.HasOne(d => d.ApplyTo).WithMany(p => p.TblBritemTypeAppliesTos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblBRItemTypeAppliesTo_tblListRelationship_ApplyTo_ID");
        });

        modelBuilder.Entity<TblBrlogicalOperatorGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblBRLogicalOperatorGroup");

            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.BusinessRule).WithMany(p => p.TblBrlogicalOperatorGroups)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblBRLogicalOperatorGroup_tblBRBusinessRule_BusinessRule_ID");
        });

        modelBuilder.Entity<TblBrstatusTransition>(entity =>
        {
            entity.HasKey(e => new { e.ActionId, e.CurrentStatusId }).HasName("pk_tblBRStatusTransition");
        });

        modelBuilder.Entity<TblCodeGenInfo>(entity =>
        {
            entity.Property(e => e.EntityId).ValueGeneratedNever();

            entity.HasOne(d => d.Entity).WithOne(p => p.TblCodeGenInfo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tblCodeGenInfo_tblEntity");
        });

        modelBuilder.Entity<TblDataQualityOperationsState>(entity =>
        {
            entity.Property(e => e.CreateDtm).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<TblDberror>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblDBErrors");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LanguageId).HasDefaultValue(1033);
        });

        modelBuilder.Entity<TblDbupgradeHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblDBUpgradeHistory");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnterUser).HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<TblDerivedHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblDerivedHierarchy");

            entity.Property(e => e.AnchorNullRecursions).HasDefaultValue(true);
            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Model).WithMany(p => p.TblDerivedHierarchies).HasConstraintName("fk_tblDerivedHierarchy_tblModel_Model_ID");
        });

        modelBuilder.Entity<TblDerivedHierarchyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblDerivedHierarchyDetail");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.DerivedHierarchy).WithMany(p => p.TblDerivedHierarchyDetails).HasConstraintName("fk_tblDerivedHierarchyDetail_tblDerivedHierarchy_DerivedHierarchy_ID");

            entity.HasOne(d => d.ManyToManyChildAttribute).WithMany(p => p.TblDerivedHierarchyDetails)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tblDerivedHierarchyDetail_ManyToManyChildAttribute_ID_tblAttribute_ID");
        });

        modelBuilder.Entity<TblEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblEntity");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StagingConsolidatedName).HasComputedColumnSql("([StagingBase]+'_Consolidated')", false);
            entity.Property(e => e.StagingLeafName).HasComputedColumnSql("([StagingBase]+'_Leaf')", false);
            entity.Property(e => e.StagingRelationshipName).HasComputedColumnSql("([StagingBase]+'_Relationship')", false);
            entity.Property(e => e.TransactionLogType).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Model).WithMany(p => p.TblEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblEntity_tblModel_Model_ID");
        });

        modelBuilder.Entity<TblErrorCodesMapping>(entity =>
        {
            entity.HasKey(e => e.Bitmask).HasName("pk_tblErrorCodeMapping");

            entity.Property(e => e.Bitmask).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblEvent");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EventStatusId).HasDefaultValue((byte)1);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblEvents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tblEvent_Entity_ID");

            entity.HasOne(d => d.Version).WithMany(p => p.TblEvents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tblEvent_Version_ID");
        });

        modelBuilder.Entity<TblExternalSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblExternalSystem");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblFile");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Source).WithMany(p => p.InverseSource).HasConstraintName("fk_tblFile_tblFile_Source_ID");
        });

        modelBuilder.Entity<TblHierarchy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblHierarchy");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsMandatory).HasDefaultValue(true);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblHierarchies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblHierarchy_tblEntity_Entity_ID");
        });

        modelBuilder.Entity<TblIndex>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblIndex");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblIndices)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblIndex_tblEntity_Entity_ID");
        });

        modelBuilder.Entity<TblList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblList");
        });

        modelBuilder.Entity<TblListRelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblListRelationship");

            entity.HasOne(d => d.ListRelationshipType).WithMany(p => p.TblListRelationships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblListRelationship_tblListRelationshipType_ListRelationshipType_ID");
        });

        modelBuilder.Entity<TblListRelationshipType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblListRelationshipType");
        });

        modelBuilder.Entity<TblModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblModel");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblModelVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblModelVersion");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StatusId).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Model).WithMany(p => p.TblModelVersions).HasConstraintName("fk_tblModelVersion_tblModel_Model_ID");

            entity.HasOne(d => d.VersionFlag).WithMany(p => p.TblModelVersions).HasConstraintName("fk_tblModelVersion_tblModelVersionFlag_VersionFlag_ID");
        });

        modelBuilder.Entity<TblModelVersionFlag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblModelVersionFlag");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StatusId).HasDefaultValue((byte)1);

            entity.HasOne(d => d.Model).WithMany(p => p.TblModelVersionFlags).HasConstraintName("fk_tblModelVersionFlag_tblModel_Model_ID");
        });

        modelBuilder.Entity<TblNotificationQueue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblNotificationQueue");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Model).WithMany(p => p.TblNotificationQueues).HasConstraintName("fk_tblNotificationQueue_tblModel_Model_ID");

            entity.HasOne(d => d.Version).WithMany(p => p.TblNotificationQueues).HasConstraintName("fk_tblNotificationQueue_tblModelVersion_Version_ID");
        });

        modelBuilder.Entity<TblNotificationUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblNotificationUsers");

            entity.HasOne(d => d.User).WithMany(p => p.TblNotificationUsers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblNotificationUsers_tblUser_UserID");
        });

        modelBuilder.Entity<TblSecurityAccessControl>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityAccessControl");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnterUserId).HasDefaultValue(-1);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgUserId).HasDefaultValue(-1);
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Role).WithMany(p => p.TblSecurityAccessControls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSecurityAccessControl_tblSecurityRole_Role_ID");
        });

        modelBuilder.Entity<TblSecurityObject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityObject");

            entity.Property(e => e.Code).IsFixedLength();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<TblSecurityPrivilege>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityPrivilege");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TblSecurityPrivilegeFunctional>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityPrivilegeFunctional");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<TblSecurityRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityRole");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnterUserId).HasDefaultValue(-1);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgUserId).HasDefaultValue(-1);
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblSecurityRoleAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityRoleAccess");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnterUserId).HasDefaultValue(-1);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgUserId).HasDefaultValue(-1);
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Model).WithMany(p => p.TblSecurityRoleAccesses).HasConstraintName("fk_tblSecurityRoleAccess_tblModel_Model_ID");

            entity.HasOne(d => d.Privilege).WithMany(p => p.TblSecurityRoleAccesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSecurityRoleAccess_tblSecurityPrivilege_Privilege_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.TblSecurityRoleAccesses).HasConstraintName("fk_tblSecurityRoleAccess_tblSecurityRole_Role_ID");
        });

        modelBuilder.Entity<TblSecurityRoleAccessFunctional>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityRoleAccessFunctional");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.FunctionalPrivilege).WithMany(p => p.TblSecurityRoleAccessFunctionals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSecurityRoleAccessFunctional_tblSecurityFunctionalPrivilege_FunctionalPrivilege_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.TblSecurityRoleAccessFunctionals).HasConstraintName("fk_tblSecurityRoleAccessFunctional_tblSecurityRole_Role_ID");
        });

        modelBuilder.Entity<TblSecurityRoleAccessMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSecurityRoleAccessMember");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.HierarchyId).HasComputedColumnSql("(case [HierarchyType_ID] when (0) then [ExplicitHierarchy_ID] when (1) then [DerivedHierarchy_ID]  end)", false);
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.DerivedHierarchy).WithMany(p => p.TblSecurityRoleAccessMembers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tblSecurityRoleAccessMember_tblHierarchy_DerivedHierarchy_ID");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblSecurityRoleAccessMembers).HasConstraintName("fk_tblSecurityRoleAccessMember_tblEntity_Entity_ID");

            entity.HasOne(d => d.ExplicitHierarchy).WithMany(p => p.TblSecurityRoleAccessMembers)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_tblSecurityRoleAccessMember_tblHierarchy_ExplicitHierarchy_ID");

            entity.HasOne(d => d.Privilege).WithMany(p => p.TblSecurityRoleAccessMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSecurityRoleAccessMember_tblSecurityPrivilege_Privilege_ID");

            entity.HasOne(d => d.Role).WithMany(p => p.TblSecurityRoleAccessMembers).HasConstraintName("fk_tblSecurityRoleAccessMember_tblSecurityRole_Role_ID");

            entity.HasOne(d => d.Version).WithMany(p => p.TblSecurityRoleAccessMembers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSecurityRoleAccessMember_tblModelVersion_Version_ID");
        });

        modelBuilder.Entity<TblStgBatch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblStgBatch");

            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblStgBatches).HasConstraintName("fk_tblStgBatch_tblEntity_Entity_ID");
        });

        modelBuilder.Entity<TblStgErrorDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblStgErrorDetail");
        });

        modelBuilder.Entity<TblSubscriptionView>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSubscriptionView");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.DerivedHierarchy).WithMany(p => p.TblSubscriptionViews).HasConstraintName("fk_tblSubscriptionView_tblDerivedHierarchy");

            entity.HasOne(d => d.Entity).WithMany(p => p.TblSubscriptionViews).HasConstraintName("fk_tblSubscriptionView_tblEntity");

            entity.HasOne(d => d.Model).WithMany(p => p.TblSubscriptionViews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSubscriptionView_tblModel");

            entity.HasOne(d => d.ModelVersion).WithMany(p => p.TblSubscriptionViews).HasConstraintName("fk_tblSubscriptionView_tblModelVersion");
        });

        modelBuilder.Entity<TblSyncRelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSyncRelationship");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();

            entity.HasOne(d => d.SourceEntity).WithMany(p => p.TblSyncRelationshipSourceEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSyncRelationship_tblEntity_SourceEntity_ID");

            entity.HasOne(d => d.SourceVersion).WithMany(p => p.TblSyncRelationshipSourceVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSyncRelationship_tblVersion_SourceVersion_ID");

            entity.HasOne(d => d.TargetEntity).WithMany(p => p.TblSyncRelationshipTargetEntities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSyncRelationship_tblEntity_TargetEntity_ID");

            entity.HasOne(d => d.TargetVersion).WithMany(p => p.TblSyncRelationshipTargetVersions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSyncRelationship_tblVersion_TargetVersion_ID");
        });

        modelBuilder.Entity<TblSystem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSystem");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EnterUserId).HasDefaultValueSql("(suser_sname())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgUserId).HasDefaultValueSql("(suser_sname())");
        });

        modelBuilder.Entity<TblSystemSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSystemSetting");

            entity.Property(e => e.DataTypeId).HasDefaultValue((byte)1);
            entity.Property(e => e.DisplaySequence).HasDefaultValue(1);
            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.SettingTypeId).HasDefaultValue((byte)1);
            entity.Property(e => e.SystemSettingGroupId).HasDefaultValue(1);

            entity.HasOne(d => d.SystemSettingGroup).WithMany(p => p.TblSystemSettings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tblSystemSetting_tblSystemSettingGroup");
        });

        modelBuilder.Entity<TblSystemSettingGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblSystemSettingGroup");

            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblUser");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblUserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblUserGroup");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblUserGroupAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblUserGroupAssignment");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Muid).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.UserGroup).WithMany(p => p.TblUserGroupAssignments).HasConstraintName("fk_tblUserGroupAssignment_tblUserGroup_UserGroupID");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserGroupAssignments).HasConstraintName("fk_tblUserGroupAssignment_tblUser_UserID");
        });

        modelBuilder.Entity<TblUserPreference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tblUserPreference");

            entity.Property(e => e.EnterDtm).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LastChgDtm).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.User).WithMany(p => p.TblUserPreferences).HasConstraintName("fk_tblUserPreference_tblUserPreference_User_ID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC47C875A6");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ViwEntityStagingBatchesAllProcessed>(entity =>
        {
            entity.ToView("viw_EntityStagingBatchesAllProcessed", "mdm");
        });

        modelBuilder.Entity<ViwEntityStagingBatchesAllProcessedExceptCleared>(entity =>
        {
            entity.ToView("viw_EntityStagingBatchesAllProcessedExceptCleared", "mdm");
        });

        modelBuilder.Entity<ViwSystemBusinessrulesAttributeInheritanceHierarchy>(entity =>
        {
            entity.ToView("viw_SYSTEM_BUSINESSRULES_ATTRIBUTE_INHERITANCE_HIERARCHY", "mdm");
        });

        modelBuilder.Entity<ViwSystemBusinessrulesHierarchyChangevalueInheritance>(entity =>
        {
            entity.ToView("viw_SYSTEM_BUSINESSRULES_HIERARCHY_CHANGEVALUE_INHERITANCE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaAttribute>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_ATTRIBUTES", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaAttributegroup>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_ATTRIBUTEGROUPS", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaAttributesOriginal>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_ATTRIBUTES_ORIGINAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaBusinessrule>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_BUSINESSRULES", "mdm");

            entity.Property(e => e.BusinessRuleLastChgTs)
                .IsRowVersion()
                .IsConcurrencyToken();
        });

        modelBuilder.Entity<ViwSystemSchemaBusinessruleItemtype>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_BUSINESSRULE_ITEMTYPES", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaBusinessrulePropertiesAttribute>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_BUSINESSRULE_PROPERTIES_ATTRIBUTES", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaBusinessruleProperty>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_BUSINESSRULE_PROPERTIES", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaEntity>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_ENTITY", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaEntityMembertype>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_ENTITY_MEMBERTYPE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaHierarchyDerived>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_HIERARCHY_DERIVED", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaHierarchyDerivedLevel>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_HIERARCHY_DERIVED_LEVELS", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaHierarchyExplicit>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_HIERARCHY_EXPLICIT", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaIndex>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_INDEXES", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaModel>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_MODEL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaModel1>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_MODELS", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaSystemsetting>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_SYSTEMSETTINGS", "mdm");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<ViwSystemSchemaVersion>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_VERSION", "mdm");
        });

        modelBuilder.Entity<ViwSystemSchemaVersionFlag>(entity =>
        {
            entity.ToView("viw_SYSTEM_SCHEMA_VERSION_FLAGS", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRole>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRoleAccesscontrol>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE_ACCESSCONTROL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRoleAccesscontrolFunctional>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE_ACCESSCONTROL_FUNCTIONAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRoleAccesscontrolMember>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE_ACCESSCONTROL_MEMBER", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRoleMember>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE_MEMBER", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityRoleOriginal>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_ROLE_ORIGINAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUser>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserAttribute>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ATTRIBUTE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserAttributeOriginal>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ATTRIBUTE_ORIGINAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserAttributegroup>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ATTRIBUTEGROUP", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserEntity>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ENTITY", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserFunction>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_FUNCTION", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserHierarchy>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_HIERARCHY", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserHierarchyDerived>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_HIERARCHY_DERIVED", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserIndex>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_INDEX", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserMember>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_MEMBER", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserMembertype>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_MEMBERTYPE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserMembertypeOriginal>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_MEMBERTYPE_ORIGINAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserModel>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_MODEL", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserRole>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ROLE", "mdm");
        });

        modelBuilder.Entity<ViwSystemSecurityUserRoleOriginal>(entity =>
        {
            entity.ToView("viw_SYSTEM_SECURITY_USER_ROLE_ORIGINAL", "mdm");
        });

        modelBuilder.Entity<ViwSystemUsergroupUser>(entity =>
        {
            entity.ToView("viw_SYSTEM_USERGROUP_USERS", "mdm");
        });

        modelBuilder.Entity<ViwSystemVersion>(entity =>
        {
            entity.ToView("viw_SYSTEM_VERSION", "mdm");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

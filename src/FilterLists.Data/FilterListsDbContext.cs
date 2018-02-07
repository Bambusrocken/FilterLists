﻿using System.Linq;
using FilterLists.Data.Entities;
using FilterLists.Data.Entities.Junctions;
using FilterLists.Data.EntityTypeConfigurations;
using FilterLists.Data.EntityTypeConfigurations.Junctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FilterLists.Data
{
    public class FilterListsDbContext : DbContext
    {
        public FilterListsDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyConfigurationsEntities(modelBuilder);
            ApplyConfigurationsJunctions(modelBuilder);
            SelectivelyOmitConventions(modelBuilder);
        }

        //TODO: update when official API supported
        //https://github.com/aspnet/EntityFrameworkCore/issues/10855#issuecomment-363219253
        //https://github.com/aspnet/EntityFrameworkCore/issues/7605
        private static void SelectivelyOmitConventions(ModelBuilder modelBuilder)
        {
            ((Model)modelBuilder.Model).ConventionDispatcher.StartBatch();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            foreach (var index in entityType.GetIndexes().ToList())
                if (index.DeclaringEntityType.Name.Contains("Junctions"))
                    entityType.RemoveIndex(index.Properties);
        }

        #region Entities

        private static void ApplyConfigurationsEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FilterListTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LicenseTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MaintainerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SnapshotTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SoftwareTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SyntaxTypeConfiguration());
        }

        public DbSet<FilterList> FilterLists { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Maintainer> Maintainers { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Software> Software { get; set; }
        public DbSet<Syntax> Syntaxes { get; set; }

        #endregion

        #region Junctions

        private static void ApplyConfigurationsJunctions(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FilterListLanguageTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FilterListMaintainerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ForkTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MergeTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SnapshotRuleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SoftwareSyntaxTypeConfiguration());
        }

        public DbSet<FilterListLanguage> FilterListLanguages { get; set; }
        public DbSet<FilterListMaintainer> FilterListMaintainers { get; set; }
        public DbSet<Fork> Forks { get; set; }
        public DbSet<Merge> Merges { get; set; }
        public DbSet<SnapshotRule> SnapshotRules { get; set; }
        public DbSet<SoftwareSyntax> SoftwareSyntaxes { get; set; }

        #endregion
    }
}
﻿using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IMP.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }
        #region  Dbsets

        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicantHistory> ApplicantHistories { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockCampaign> BlockCampaigns { get; set; }
        public DbSet<BlockPlatform> BlockPlatforms { get; set; }
        public DbSet<BlockType> BlockTypes { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignActivity> CampaignActivities { get; set; }
        public DbSet<CampaignMember> CampaignMembers { get; set; }
        public DbSet<CampaignType> CampaignTypes { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<InfluencerPlatform> InfluencerPlatforms { get; set; }
        public DbSet<MemberActivity> MemberActivities { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PaymentInfor> PaymentInfors { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<RankLevel> RankLevels { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<VoucherCode> VoucherCodes { get; set; }
        public DbSet<VoucherTransaction> VoucherTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }

        #endregion  Dbsets


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            //foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //            entry.Entity.Created = _dateTime.NowUtc;
            //            entry.Entity.CreatedBy = _authenticatedUser.UserId;
            //            break;
            //        case EntityState.Modified:
            //            entry.Entity.LastModified = _dateTime.NowUtc;
            //            entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
            //            break;
            //    }
            //}
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.IsDelete = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Configure ForeignKey
            foreach (var relationShip in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationShip.DeleteBehavior = DeleteBehavior.ClientCascade;
            }

            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }

            //Application User
            builder.Entity<ApplicationUser>().HasIndex("UserName").IsUnique();
            base.OnModelCreating(builder);
        }
    }
}

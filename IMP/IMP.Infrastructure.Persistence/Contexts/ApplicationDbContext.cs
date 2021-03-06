using IMP.Application.Interfaces;
using IMP.Domain.Common;
using IMP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        #region  db sets

        public DbSet<ActivityType> ActivityTypes { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<BlockItem> BlockItems { get; set; }
        public DbSet<BlockType> BlockTypes { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<InfluencerConfiguration> InfluencerConfigurations { get; set; }
        public DbSet<InfluencerConfigurationLocation> InfluencerConfigurationLocations { get; set; }
        public DbSet<TargetConfiguration> TargetCustomerConfigurations { get; set; }
        public DbSet<TargetConfigurationLocation> TargetConfigurationLocations { get; set; }
        public DbSet<CampaignActivity> CampaignActivities { get; set; }
        public DbSet<CampaignMember> CampaignMembers { get; set; }
        public DbSet<CampaignImage> CampaignImages { get; set; }
        public DbSet<CampaignType> CampaignTypes { get; set; }
        public DbSet<CampaignVoucher> CampaignVouchers { get; set; }
        public DbSet<CampaignReward> CampaignRewards { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<InfluencerPlatform> InfluencerPlatforms { get; set; }
        public DbSet<MemberActivity> MemberActivities { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Bank> Banks { get; set; }

        public DbSet<PaymentInfor> PaymentInfors { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<RankLevel> RankLevels { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<VoucherCode> VoucherCodes { get; set; }
        public DbSet<VoucherTransaction> VoucherTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<Otp> Otps { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VoucherCodeApplicationUser> VoucherCodeApplicationUsers { get; set; }
        public DbSet<ActivityComment> ActivityComments { get; set; }
        public DbSet<EvidenceType> EvidenceTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<VoucherInfluencer> VoucherInfluencers { get; set; }

        #endregion


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.IsDeleted = false;
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
            #region Convert DateTime to UTC
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
            #endregion

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
            //builder.Entity<ApplicationUser>().HasIndex("UserName").IsUnique();

            builder.Entity<WalletTransaction>()
                    .HasOne<Wallet>(wt => wt.WalletFrom)
                    .WithMany(w => w.FromTransactions)
                    .HasForeignKey(wt => wt.WalletFromId);

            builder.Entity<WalletTransaction>()
                    .HasOne<Wallet>(wt => wt.WalletTo)
                    .WithMany(w => w.ToTransactions)
                    .HasForeignKey(wt => wt.WalletToId);

            builder.Entity<WalletTransaction>()
                    .HasOne<ApplicationUser>(wt => wt.Sender)
                    .WithMany(a => a.TransactionsSent)
                    .HasForeignKey(wt => wt.SenderId);

            builder.Entity<WalletTransaction>()
                   .HasOne<ApplicationUser>(wt => wt.Receiver)
                   .WithMany(a => a.TransactionsReceived)
                   .HasForeignKey(wt => wt.ReceiverId);


            // Filter deleted
            builder.EntitiesOfType<ISoftDeletable>(builder =>
            {
                var param = Expression.Parameter(builder.Metadata.ClrType, "p");
                var body = Expression.Equal(Expression.Property(param, nameof(ISoftDeletable.IsDeleted)), Expression.Constant(false));
                builder.HasQueryFilter(Expression.Lambda(body, param));
            });

            base.OnModelCreating(builder);
        }
    }
}
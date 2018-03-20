using CMSCore.Data.EF.Configurations;
using CMSCore.Data.EF.Extensions;
using CMSCore.Data.Entities;
using CMSCore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace CMSCore.Data.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        public DbSet<Entities.Action> Actions { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

        public DbSet<Function> Functions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// Announcements
        /// </summary>
        public DbSet<Announcement> Announcements { set; get; }
        public DbSet<AnnouncementUser> AnnouncementUsers { set; get; }

        /// <summary>
        /// Advertistment
        /// </summary>
        public DbSet<AdvertistmentPage> AdvertistmentPages { get; set; }
        public DbSet<Advertistment> Advertistments { get; set; }
        public DbSet<AdvertistmentPosition> AdvertistmentPositions { get; set; }

        /// <summary>
        /// Product
        /// </summary>
        public DbSet<Blog> Bills { set; get; }
        public DbSet<BillDetail> BillDetails { set; get; }
        public DbSet<Color> Colors { set; get; }

        public DbSet<Product> Products { set; get; }
        public DbSet<ProductCategory> ProductCategories { set; get; }
        public DbSet<ProductImage> ProductImages { set; get; }

        public DbSet<ProductQuantity> ProductQuantities { set; get; }
        public DbSet<ProductTag> ProductTags { set; get; }
        public DbSet<Size> Sizes { set; get; }

        public DbSet<WholePrice> WholePrices { get; set; }

        /// <summary>
        /// Blog
        /// </summary>
        public DbSet<Blog> Blogs { set; get; }
        public DbSet<BlogTag> BlogTags { set; get; }
       
        /// <summary>
        /// System
        /// </summary>
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<Footer> Footers { set; get; }
        public DbSet<Language> Languages { set; get; }

        public DbSet<Page> Pages { set; get; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        /// <summary>
        /// Manager
        /// </summary>
        public DbSet<Feedback> Feedbacks { set; get; }
        public DbSet<Slide> Slides { set; get; }
        public DbSet<Tag> Tags { set; get; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            #region Identity Config

            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
                .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
                .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });

            #endregion Identity Config

            builder.AddConfiguration(new TagConfiguration());
            builder.AddConfiguration(new BlogTagConfiguration());
            builder.AddConfiguration(new ContactDetailConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new PageConfiguration());
            builder.AddConfiguration(new FooterConfiguration());
            builder.AddConfiguration(new ProductTagConfiguration());
            builder.AddConfiguration(new SystemConfigConfiguration());
            builder.AddConfiguration(new AdvertistmentPositionConfiguration());
            builder.AddConfiguration(new AdvertistmentPageConfiguration());
            builder.AddConfiguration(new FunctionConfiguration());
            builder.AddConfiguration(new ActionConfiguration());
            builder.AddConfiguration(new AnnouncementConfiguration());

            //base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as IDateTracking;
                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.DateModified = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new AppDbContext(builder.Options);
        }
    }
}
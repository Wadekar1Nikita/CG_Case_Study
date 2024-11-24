using Microsoft.EntityFrameworkCore;
using bidder;
using claim;
using farmer;
using bidding;

namespace ContextFile
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Bidder> Bidders { get; set; }
        public DbSet<ClaimInsurance> ClaimInsurances { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Insurance> Insurances { get; set; }
        public DbSet<SoldHistory> SoldHistories { get; set; }
        public DbSet<Bidding> Biddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.AEmail)
                .IsUnique();  

            
            modelBuilder.Entity<Bidder>()
                .HasIndex(b => b.Email)
                .IsUnique(); 

            
            modelBuilder.Entity<Crop>()
                .HasOne(c => c.Farmer)  
                .WithMany() 
                .HasForeignKey(c => c.FarmerID)
                .OnDelete(DeleteBehavior.Cascade); 

            
            
            modelBuilder.Entity<Bidding>()
                .HasOne(b => b.Crop)  
                .WithMany()  
                .HasForeignKey(b => b.CropID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bidding>()
                .HasOne(b => b.Bidder)  
                .WithMany()  
                .HasForeignKey(b => b.BidderID)
                .OnDelete(DeleteBehavior.Cascade);

            // Insurance Model Configuration
    //         modelBuilder.Entity<Insurance>()
    // .HasOne(i => i.Farmer)
    // .WithMany(f => f.Insurances)
    // .HasForeignKey(i => i.FarmerID)
    // .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Insurance>()
            .Property(i => i.PremiumAmount)
            .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Crop>()
            .Property(c => c.BasePrice)
            .HasColumnType("decimal(18, 2)"); 
    
            modelBuilder.Entity<Insurance>()
            .Property(i => i.PremiumAmount)
            .HasColumnType("decimal(18, 2)");
    
            modelBuilder.Entity<Insurance>()
            .Property(i => i.PremiumRateForSeason)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Insurance>()
            .Property(i => i.SumInsured)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SoldHistory>()
            .Property(s => s.MSP)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SoldHistory>()
            .Property(s => s.SoldPrice)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<SoldHistory>()
            .Property(s => s.TotalPrice)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Bidding>()
            .Property(b => b.BidAmount)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ClaimInsurance>()
            .Property(c => c.ClaimAmount)
            .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ClaimInsurance>()
            .HasOne(ci => ci.Insurance)  
            .WithMany() 
            .HasForeignKey(ci => ci.InsuranceID)
            .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Admin>()
            .Property(a => a.AEmail)
            .IsRequired()
            .HasMaxLength(100);
    
            modelBuilder.Entity<Admin>()
            .Property(a => a.APassword)
            .IsRequired()
            .HasMaxLength(100);

            // SoldHistory Model Configuration
    //         modelBuilder.Entity<SoldHistory>()
    // .HasOne(sh => sh.Crop)
    // .WithMany(c => c.SoldHistories)
    // .HasForeignKey(sh => sh.CropID)
    // .OnDelete(DeleteBehavior.Restrict);  // Change from Cascade to Restrict

    //  modelBuilder.Entity<SoldHistory>()
    //     .HasOne(sh => sh.Crop)
    //     .WithMany()  // No navigation property from Crop to SoldHistory
    //     .HasForeignKey(sh => sh.CropID)
    //     .OnDelete(DeleteBehavior.Restrict);  // Keep Cascade on Bidding

    // modelBuilder.Entity<SoldHistory>()
    //     .HasOne(sh => sh.Crop)
    //     .WithMany()
    //     .HasForeignKey(sh => sh.CropID)
    //     .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SoldHistory>()
            .HasOne(sh => sh.Crop)
            .WithMany() 
            .HasForeignKey(sh => sh.CropID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SoldHistory>()
            .HasOne(sh => sh.Bidding)
            .WithMany() 
            .HasForeignKey(sh => sh.BidID)
            .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Bidding>()
            .HasOne(b => b.Bidder)  
            .WithMany()  
            .HasForeignKey(b => b.BidderID) 
            .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}

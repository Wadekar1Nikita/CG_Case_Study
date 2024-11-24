﻿// <auto-generated />
using System;
using ContextFile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LatestUpdate.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241115035404_update12345")]
    partial class update12345
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Admin", b =>
                {
                    b.Property<int>("AdminID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminID"));

                    b.Property<string>("AEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("APassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AdminID");

                    b.HasIndex("AEmail")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Crop", b =>
                {
                    b.Property<int>("CropID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CropID"));

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CropName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CropType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("FarmerID")
                        .HasColumnType("int");

                    b.Property<string>("FertilizerType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("CropID");

                    b.HasIndex("FarmerID");

                    b.ToTable("Crops");
                });

            modelBuilder.Entity("Insurance", b =>
                {
                    b.Property<int>("InsuranceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InsuranceID"));

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CropName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FarmerID")
                        .HasColumnType("int");

                    b.Property<string>("PolicyName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("PremiumAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PremiumRateForSeason")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Season")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("SumInsured")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("InsuranceID");

                    b.HasIndex("FarmerID");

                    b.ToTable("Insurances");
                });

            modelBuilder.Entity("SoldHistory", b =>
                {
                    b.Property<int>("SoldID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SoldID"));

                    b.Property<int>("BidID")
                        .HasColumnType("int");

                    b.Property<int>("BiddingBidID")
                        .HasColumnType("int");

                    b.Property<int>("CropID")
                        .HasColumnType("int");

                    b.Property<decimal>("MSP")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("SoldPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("SoldID");

                    b.HasIndex("BiddingBidID");

                    b.HasIndex("CropID");

                    b.ToTable("SoldHistories");
                });

            modelBuilder.Entity("bidder.Bidder", b =>
                {
                    b.Property<int>("BidderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidderID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Adharno")
                        .HasColumnType("bigint");

                    b.Property<long>("BankAccountNo")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IFSCCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PAN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TraderLicence")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BidderID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Bidders");
                });

            modelBuilder.Entity("bidding.Bidding", b =>
                {
                    b.Property<int>("BidID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BidID"));

                    b.Property<string>("AuctionResult")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("BidAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BidderID")
                        .HasColumnType("int");

                    b.Property<int?>("BidderID1")
                        .HasColumnType("int");

                    b.Property<int>("CropID")
                        .HasColumnType("int");

                    b.HasKey("BidID");

                    b.HasIndex("BidderID");

                    b.HasIndex("BidderID1");

                    b.HasIndex("CropID");

                    b.ToTable("Biddings");
                });

            modelBuilder.Entity("claim.ClaimInsurance", b =>
                {
                    b.Property<int>("ClaimID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClaimID"));

                    b.Property<decimal>("ClaimAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ClaimReason")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("DateOfLoss")
                        .HasColumnType("datetime2");

                    b.Property<int>("InsuranceID")
                        .HasColumnType("int");

                    b.HasKey("ClaimID");

                    b.HasIndex("InsuranceID");

                    b.ToTable("ClaimInsurances");
                });

            modelBuilder.Entity("farmer.Farmer", b =>
                {
                    b.Property<int>("FarmerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FarmerID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("Adharno")
                        .HasColumnType("bigint");

                    b.Property<string>("Area")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("BankAccountNo")
                        .HasColumnType("bigint");

                    b.Property<string>("Certificate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("IFSCCode")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("LandAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PAN")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("FarmerID");

                    b.ToTable("Farmers");
                });

            modelBuilder.Entity("Crop", b =>
                {
                    b.HasOne("farmer.Farmer", "Farmer")
                        .WithMany()
                        .HasForeignKey("FarmerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farmer");
                });

            modelBuilder.Entity("Insurance", b =>
                {
                    b.HasOne("farmer.Farmer", "Farmer")
                        .WithMany()
                        .HasForeignKey("FarmerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farmer");
                });

            modelBuilder.Entity("SoldHistory", b =>
                {
                    b.HasOne("bidding.Bidding", "Bidding")
                        .WithMany()
                        .HasForeignKey("BiddingBidID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Crop", "Crop")
                        .WithMany()
                        .HasForeignKey("CropID")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("Bidding");

                    b.Navigation("Crop");
                });

            modelBuilder.Entity("bidding.Bidding", b =>
                {
                    b.HasOne("bidder.Bidder", "Bidder")
                        .WithMany()
                        .HasForeignKey("BidderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("bidder.Bidder", null)
                        .WithMany("Biddings")
                        .HasForeignKey("BidderID1");

                    b.HasOne("Crop", "Crop")
                        .WithMany()
                        .HasForeignKey("CropID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bidder");

                    b.Navigation("Crop");
                });

            modelBuilder.Entity("claim.ClaimInsurance", b =>
                {
                    b.HasOne("Insurance", "Insurance")
                        .WithMany()
                        .HasForeignKey("InsuranceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Insurance");
                });

            modelBuilder.Entity("bidder.Bidder", b =>
                {
                    b.Navigation("Biddings");
                });
#pragma warning restore 612, 618
        }
    }
}

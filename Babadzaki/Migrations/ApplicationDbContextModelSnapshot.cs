﻿// <auto-generated />
using System;
using Babadzaki.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Babadzaki.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Babadzaki.Models.Email", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("Babadzaki.Models.Filter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("Babadzaki.Models.SeasonCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SeasonCollections");
                });

            modelBuilder.Entity("Babadzaki.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SeasonCollectionId")
                        .HasColumnType("int");

                    b.Property<int>("edition")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SeasonCollectionId");

                    b.ToTable("Tokens", t =>
                        {
                            t.HasTrigger("UpdateTotalTokensNumAfterUpdate");
                        });
                });

            modelBuilder.Entity("Babadzaki.Models.TokensFilters", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FilterId")
                        .HasColumnType("int");

                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("FilterId");

                    b.HasIndex("TokenId");

                    b.ToTable("TokensFilters");
                });

            modelBuilder.Entity("Babadzaki.Models.Token", b =>
                {
                    b.HasOne("Babadzaki.Models.SeasonCollection", "SeasonCollection")
                        .WithMany("Tokens")
                        .HasForeignKey("SeasonCollectionId");

                    b.Navigation("SeasonCollection");
                });

            modelBuilder.Entity("Babadzaki.Models.TokensFilters", b =>
                {
                    b.HasOne("Babadzaki.Models.Filter", "Filter")
                        .WithMany("TokensFilters")
                        .HasForeignKey("FilterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Babadzaki.Models.Token", "Token")
                        .WithMany("TokensFilters")
                        .HasForeignKey("TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Filter");

                    b.Navigation("Token");
                });

            modelBuilder.Entity("Babadzaki.Models.Filter", b =>
                {
                    b.Navigation("TokensFilters");
                });

            modelBuilder.Entity("Babadzaki.Models.SeasonCollection", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Babadzaki.Models.Token", b =>
                {
                    b.Navigation("TokensFilters");
                });
#pragma warning restore 612, 618
        }
    }
}

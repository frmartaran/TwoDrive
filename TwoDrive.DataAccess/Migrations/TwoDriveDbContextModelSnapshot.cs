﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwoDrive.DataAccess;

namespace TwoDrive.DataAccess.Migrations
{
    [DbContext(typeof(TwoDriveDbContext))]
    partial class TwoDriveDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TwoDrive.Domain.Claim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ElementId");

                    b.Property<int>("Type");

                    b.Property<int?>("WriterId");

                    b.HasKey("Id");

                    b.HasIndex("ElementId");

                    b.HasIndex("WriterId");

                    b.ToTable("Claim");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.Element", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name");

                    b.Property<int?>("OwnerId");

                    b.Property<int?>("ParentFolderId");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Elements");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Element");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.Modification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("ElementId");

                    b.Property<int>("type");

                    b.HasKey("Id");

                    b.HasIndex("ElementId");

                    b.ToTable("Modifications");
                });

            modelBuilder.Entity("TwoDrive.Domain.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("Token");

                    b.Property<int?>("WriterId");

                    b.HasKey("Id");

                    b.HasIndex("WriterId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("TwoDrive.Domain.Writer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FriendId");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("FriendId");

                    b.ToTable("Writers");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.File", b =>
                {
                    b.HasBaseType("TwoDrive.Domain.FileManagement.Element");

                    b.HasDiscriminator().HasValue("File");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.Folder", b =>
                {
                    b.HasBaseType("TwoDrive.Domain.FileManagement.Element");

                    b.HasDiscriminator().HasValue("Folder");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.TxtFile", b =>
                {
                    b.HasBaseType("TwoDrive.Domain.FileManagement.File");

                    b.Property<string>("Content");

                    b.HasDiscriminator().HasValue("TxtFile");
                });

            modelBuilder.Entity("TwoDrive.Domain.Claim", b =>
                {
                    b.HasOne("TwoDrive.Domain.FileManagement.Element", "Element")
                        .WithMany()
                        .HasForeignKey("ElementId");

                    b.HasOne("TwoDrive.Domain.Writer")
                        .WithMany("Claims")
                        .HasForeignKey("WriterId");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.Element", b =>
                {
                    b.HasOne("TwoDrive.Domain.Writer", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.HasOne("TwoDrive.Domain.FileManagement.Folder", "ParentFolder")
                        .WithMany("FolderChildren")
                        .HasForeignKey("ParentFolderId");
                });

            modelBuilder.Entity("TwoDrive.Domain.FileManagement.Modification", b =>
                {
                    b.HasOne("TwoDrive.Domain.FileManagement.Element", "ElementModified")
                        .WithMany()
                        .HasForeignKey("ElementId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TwoDrive.Domain.Session", b =>
                {
                    b.HasOne("TwoDrive.Domain.Writer", "Writer")
                        .WithMany()
                        .HasForeignKey("WriterId");
                });

            modelBuilder.Entity("TwoDrive.Domain.Writer", b =>
                {
                    b.HasOne("TwoDrive.Domain.Writer", "Friend")
                        .WithMany("Friends")
                        .HasForeignKey("FriendId");
                });
#pragma warning restore 612, 618
        }
    }
}

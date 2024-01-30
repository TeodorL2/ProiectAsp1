﻿// <auto-generated />
using System;
using Drive.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Drive.Migrations
{
    [DbContext(typeof(DriveContext))]
    partial class DriveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Drive.Data.Models.BaseDirectory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Author")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<string>("dir")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Author");

                    b.ToTable("BaseDirectorys");
                });

            modelBuilder.Entity("Drive.Data.Models.DirectoryDesc", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DirectoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId")
                        .IsUnique();

                    b.ToTable("DirectoryDescs");
                });

            modelBuilder.Entity("Drive.Data.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Drive.Data.Models.UserAccessToBaseDirectory", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BaseDirectoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("HasReadPermission")
                        .HasColumnType("bit");

                    b.Property<bool>("HasWritePermission")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "BaseDirectoryId");

                    b.HasIndex("BaseDirectoryId");

                    b.ToTable("UserAccessToBaseDirectorys");
                });

            modelBuilder.Entity("Drive.Data.Models.BaseDirectory", b =>
                {
                    b.HasOne("Drive.Data.Models.User", "User")
                        .WithMany("BaseDirectorys")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Drive.Data.Models.DirectoryDesc", b =>
                {
                    b.HasOne("Drive.Data.Models.BaseDirectory", "BaseDirectory")
                        .WithOne("DirectoryDesc")
                        .HasForeignKey("Drive.Data.Models.DirectoryDesc", "DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseDirectory");
                });

            modelBuilder.Entity("Drive.Data.Models.UserAccessToBaseDirectory", b =>
                {
                    b.HasOne("Drive.Data.Models.BaseDirectory", "BaseDirectory")
                        .WithMany("UserAccessToBaseDirectorys")
                        .HasForeignKey("BaseDirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Drive.Data.Models.User", "User")
                        .WithMany("UserAccessToBaseDirectorys")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BaseDirectory");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Drive.Data.Models.BaseDirectory", b =>
                {
                    b.Navigation("DirectoryDesc")
                        .IsRequired();

                    b.Navigation("UserAccessToBaseDirectorys");
                });

            modelBuilder.Entity("Drive.Data.Models.User", b =>
                {
                    b.Navigation("BaseDirectorys");

                    b.Navigation("UserAccessToBaseDirectorys");
                });
#pragma warning restore 612, 618
        }
    }
}
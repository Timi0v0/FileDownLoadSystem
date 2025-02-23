﻿// <auto-generated />
using System;
using FileDownLoadSystem.Core.EFDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileDownLoadSystem.Core.Migrations
{
    [DbContext(typeof(FileDownLoadSystemDbContext))]
    partial class FileDownLoadSystemDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FileModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<long>("ClickCount")
                        .HasColumnType("bigint");

                    b.Property<long>("DownloadCount")
                        .HasColumnType("bigint");

                    b.Property<string>("FileDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("FileIconUrl")
                        .HasColumnType("longtext");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("FileTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("FileModel");
                });

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FilePackages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FileId")
                        .HasColumnType("int");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PackageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("FilePackages");
                });

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FileTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FileTypeName")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("FileTypes");
                });

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FileWebConfigs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PublishTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("FileWebConfigs");
                });

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FilePackages", b =>
                {
                    b.HasOne("FileDownLoadSystem.Entity.FileInfo.FileModel", null)
                        .WithMany("FilePackages")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FileDownLoadSystem.Entity.FileInfo.FileModel", b =>
                {
                    b.Navigation("FilePackages");
                });
#pragma warning restore 612, 618
        }
    }
}

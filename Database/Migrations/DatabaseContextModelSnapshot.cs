﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("Database.Entities.InputHtmlFileEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_on");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_on");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("FileGuid")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("text")
                        .HasColumnName("guid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER")
                        .HasColumnName("is_deleted");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER")
                        .HasColumnName("status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_on");

                    b.HasKey("Id");

                    b.ToTable("input_html_files");
                });

            modelBuilder.Entity("Database.Entities.OutputPdfFileEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("created_on");

                    b.Property<DateTime?>("DeletedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("deleted_on");

                    b.Property<long>("InputHtmlFileId")
                        .HasColumnType("INTEGER")
                        .HasColumnName("input_html_file_id");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("TEXT")
                        .HasColumnName("updated_on");

                    b.HasKey("Id");

                    b.HasIndex("InputHtmlFileId");

                    b.ToTable("output_pdf_files");
                });

            modelBuilder.Entity("Database.Entities.OutputPdfFileEntity", b =>
                {
                    b.HasOne("Database.Entities.InputHtmlFileEntity", "InputHtmlFile")
                        .WithMany()
                        .HasForeignKey("InputHtmlFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InputHtmlFile");
                });
#pragma warning restore 612, 618
        }
    }
}

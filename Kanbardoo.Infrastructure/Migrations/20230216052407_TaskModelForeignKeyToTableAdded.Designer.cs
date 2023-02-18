﻿// <auto-generated />
using System;
using Kanbardoo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20230216052407_TaskModelForeignKeyToTableAdded")]
    partial class TaskModelForeignKeyToTableAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Board", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BackgroundImageUrl")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FinishDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("StatusID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("StatusID");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.BoardStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("BoardStatuses");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Active"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Closed"
                        });
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTask", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AssigneeID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int>("StatusID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TableID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("AssigneeID");

                    b.HasIndex("StatusID");

                    b.HasIndex("TableID");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardID")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("BoardID");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.TaskStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("TaskStatuses");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "New"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Active"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Closed"
                        });
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 46920,
                            CreationDate = new DateTime(2023, 2, 16, 5, 24, 7, 391, DateTimeKind.Utc).AddTicks(2619),
                            Name = "piotrpuszk"
                        });
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Board", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.BoardStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTask", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.User", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.TaskStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.Table", "Table")
                        .WithMany("Tasks")
                        .HasForeignKey("TableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Status");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Table", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.Board", "Board")
                        .WithMany("Tables")
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Board", b =>
                {
                    b.Navigation("Tables");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Table", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
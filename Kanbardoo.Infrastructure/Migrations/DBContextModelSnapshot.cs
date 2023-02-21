﻿// <auto-generated />
using System;
using Kanbardoo.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kanbardoo.Infrastructure.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Invitation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("BoardID");

                    b.HasIndex("UserID");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanBoard", b =>
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

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanBoardStatus", b =>
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

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanClaim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Claims");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Owner"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Member"
                        });
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanRoleClaim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClaimID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ClaimID");

                    b.HasIndex("RoleID");

                    b.ToTable("RolesClaims");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTable", b =>
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

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTaskStatus", b =>
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

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUser", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            ID = 46920,
                            CreationDate = new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PasswordHash = new byte[0],
                            PasswordSalt = new byte[0],
                            UserName = "piotrpuszk"
                        });
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserBoard", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("BoardID");

                    b.HasIndex("UserID");

                    b.ToTable("UserBoards");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserClaim", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClaimID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ClaimID");

                    b.HasIndex("UserID");

                    b.ToTable("UsersClaims");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserRole", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoleID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("RoleID");

                    b.HasIndex("UserID");

                    b.ToTable("UsersRoles");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserTable", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TableID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("TableID");

                    b.HasIndex("UserID");

                    b.ToTable("UserTables");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserTask", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("TaskID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("TaskID");

                    b.HasIndex("UserID");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.Invitation", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanBoard", "Board")
                        .WithMany()
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanBoard", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanBoardStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanRoleClaim", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanClaim", "Claim")
                        .WithMany("Roles")
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanRole", "Role")
                        .WithMany("Claims")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claim");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTable", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanBoard", "Board")
                        .WithMany("Tables")
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTask", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanTaskStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanTable", "Table")
                        .WithMany("Tasks")
                        .HasForeignKey("TableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assignee");

                    b.Navigation("Status");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserBoard", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanBoard", "Board")
                        .WithMany()
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserClaim", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanClaim", "Claim")
                        .WithMany("Users")
                        .HasForeignKey("ClaimID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Claim");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserRole", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserTable", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanTable", "Table")
                        .WithMany()
                        .HasForeignKey("TableID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Table");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUserTask", b =>
                {
                    b.HasOne("Kanbardoo.Domain.Entities.KanTask", "Task")
                        .WithMany()
                        .HasForeignKey("TaskID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kanbardoo.Domain.Entities.KanUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanBoard", b =>
                {
                    b.Navigation("Tables");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanClaim", b =>
                {
                    b.Navigation("Roles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanRole", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanTable", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Kanbardoo.Domain.Entities.KanUser", b =>
                {
                    b.Navigation("Claims");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}

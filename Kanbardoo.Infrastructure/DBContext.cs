using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;

namespace Kanbardoo.Infrastructure;
public class DBContext : DbContext
{
    public DbSet<KanBoard> Boards { get; set; }
    public DbSet<KanBoardStatus> BoardStatuses { get; set; }
    public DbSet<KanTaskStatus> TaskStatuses { get; set; }
    public DbSet<KanTable> Tables { get; set; }
    public DbSet<KanTask> Tasks { get; set; }
    public DbSet<KanUser> Users { get; set; }
    public DbSet<KanClaim> Claims { get; set; }
    public DbSet<KanRole> Roles { get; set; }
    public DbSet<KanUserClaim> UsersClaims { get; set; }
    public DbSet<KanUserBoardRole> UserBoardsRoles { get; set; }
    public DbSet<KanRoleClaim> RolesClaims { get; set; }
    public DbSet<KanUserBoard> UserBoards { get; set; }
    public DbSet<KanUserTable> UserTables { get; set; }
    public DbSet<KanUserTask> UserTasks { get; set; }
    public DbSet<Invitation> Invitations { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<KanUserClaim>()
            .HasOne(e => e.User)
            .WithMany(e => e.Claims)
            .HasForeignKey(e => e.UserID);

        modelBuilder.Entity<KanUserClaim>()
            .HasOne(e => e.Claim)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.ClaimID);

        modelBuilder.Entity<KanUserBoardRole>()
            .HasOne(e => e.User)
            .WithMany(e => e.Roles)
            .HasForeignKey(e => e.UserID);

        modelBuilder.Entity<KanUserBoardRole>()
            .HasOne(e => e.Role)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.RoleID);

        modelBuilder.Entity<KanRoleClaim>()
            .HasOne(e => e.Role)
            .WithMany(e => e.Claims)
            .HasForeignKey(e => e.RoleID);

        modelBuilder.Entity<KanRoleClaim>()
            .HasOne(e => e.Claim)
            .WithMany(e => e.Roles)
            .HasForeignKey(e => e.ClaimID);

        modelBuilder.Entity<KanTable>()
            .HasOne(e => e.Board)
            .WithMany(e => e.Tables);

        modelBuilder.Entity<KanClaim>()
            .HasData(new List<KanClaim>()
            { 
                new KanClaim()
                {
                    ID = 1,
                    Name = "Admin",
                },
                new KanClaim()
                {
                    ID = 2,
                    Name = "Owner",
                },
                new KanClaim()
                {
                    ID = 3,
                    Name = "Member",
                }
            });

        modelBuilder.Entity<KanRole>()
            .HasData(new List<KanRole>()
            {
                new KanRole()
                {
                    ID = KanRoleID.Admin,
                    Name = "Admin",
                },
                new KanRole()
                {
                    ID = KanRoleID.Owner,
                    Name = "Owner",
                },
                new KanRole()
                {
                    ID = KanRoleID.Member,
                    Name = "Member",
                }
            });

        modelBuilder.Entity<KanBoardStatus>()
        .HasData
        (
            new List<KanBoardStatus>()
            {
                new KanBoardStatus()
                {
                    ID = KanBoardStatusId.Active,
                    Name = nameof(KanBoardStatusId.Active),
                },
                new KanBoardStatus()
                {
                    ID = KanBoardStatusId.Closed,
                    Name = nameof(KanBoardStatusId.Closed),
                }
            }
        );

        modelBuilder.Entity<KanTaskStatus>().HasData
        (
            new List<KanTaskStatus>()
            {
                new KanTaskStatus()
                {
                    ID = KanTaskStatusId.New,
                    Name = nameof(KanTaskStatusId.New),
                },
                new KanTaskStatus()
                {
                    ID = KanTaskStatusId.Active,
                    Name = nameof(KanTaskStatusId.Active),
                },
                new KanTaskStatus()
                {
                    ID = KanTaskStatusId.Closed,
                    Name = nameof(KanTaskStatusId.Closed),
                }
            }
        );

        var creationDate = new DateTime(2023, 1, 1);
        creationDate = creationDate.ToUniversalTime();
        modelBuilder.Entity<KanUser>()
            .HasData
            (
                new List<KanUser>()
                {
                    new KanUser()
                    {
                        ID = 46920,
                        UserName = "piotrpuszk",
                        CreationDate = creationDate,
                    }
                }
            );
    } 
}
using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<KanUserRole> UsersRoles { get; set; }

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

        modelBuilder.Entity<KanUserRole>()
            .HasOne(e => e.User)
            .WithMany(e => e.Roles)
            .HasForeignKey(e => e.UserID);

        modelBuilder.Entity<KanUserRole>()
            .HasOne(e => e.Role)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.RoleID);

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

        modelBuilder.Entity<KanUser>()
            .HasData
            (
                new List<KanUser>()
                {
                    new KanUser()
                    {
                        ID = 46920,
                        UserName = "piotrpuszk",
                        CreationDate = new DateTime(2023, 2, 1),
                    }
                }
            );
    } 
}

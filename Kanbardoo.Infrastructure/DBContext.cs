using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Models;
using Microsoft.EntityFrameworkCore;
using KanTask = Kanbardoo.Domain.Entities.KanTask;
using TaskStatus = Kanbardoo.Domain.Entities.TaskStatus;

namespace Kanbardoo.Infrastructure;
public class DBContext : DbContext
{
    public DbSet<Board> Boards { get; set; }
    public DbSet<BoardStatus> BoardStatuses { get; set; }
    public DbSet<TaskStatus> TaskStatuses { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<KanTask> Tasks { get; set; }
    public DbSet<User> Users { get; set; }

    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Table>()
            .HasOne(e => e.Board)
            .WithMany(e => e.Tables);


        modelBuilder.Entity<BoardStatus>()
        .HasData
        (
            new List<BoardStatus>()
            {
                new BoardStatus()
                {
                    ID = BoardStatusId.Active,
                    Name = nameof(BoardStatusId.Active),
                },
                new BoardStatus()
                {
                    ID = BoardStatusId.Closed,
                    Name = nameof(BoardStatusId.Closed),
                }
            }
        );

        modelBuilder.Entity<TaskStatus>().HasData
        (
            new List<TaskStatus>()
            {
                new TaskStatus()
                {
                    ID = TaskStatusId.New,
                    Name = nameof(TaskStatusId.New),
                },
                new TaskStatus()
                {
                    ID = TaskStatusId.Active,
                    Name = nameof(TaskStatusId.Active),
                },
                new TaskStatus()
                {
                    ID = TaskStatusId.Closed,
                    Name = nameof(TaskStatusId.Closed),
                }
            }
        );

        modelBuilder.Entity<User>()
            .HasData
            (
                new List<User>()
                {
                    new User()
                    {
                        ID = 46920,
                        Name = "piotrpuszk",
                        CreationDate = DateTime.UtcNow,
                    }
                }
            );
    } 
}

using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Kanbardoo.Infrastructure;
using Kanbardoo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((context, config) => 
    config
    .WriteTo.File("logging.txt")
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAddBoardUseCase, AddBoardUseCase>();
builder.Services.AddScoped<IGetBoardUseCase, GetBoardUseCase>();
builder.Services.AddScoped<IDeleteBoardUseCase, DeleteBoardUseCase>();
builder.Services.AddScoped<IUpdateBoardUseCase, UpdateBoardUseCase>();

builder.Services.AddScoped<IAddTableUseCase, AddTableUseCase>();
builder.Services.AddScoped<IGetTableUseCase, GetTableUseCase>();
builder.Services.AddScoped<IDeleteTableUseCase, DeleteTableUseCase>();
builder.Services.AddScoped<IUpdateTableUseCase, UpdateTableUseCase>();

builder.Services.AddScoped<IAddTaskUseCase, AddTaskUseCase>();
builder.Services.AddScoped<IGetTaskUseCase, GetTaskUseCase>();
builder.Services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
builder.Services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<BoardFiltersValidator>();
builder.Services.AddScoped<BoardToUpdateValidator>();
builder.Services.AddScoped<BoardIdToDeleteValidator>();
builder.Services.AddScoped<NewBoardValidator>();

builder.Services.AddScoped<TableToUpdateValidator>();
builder.Services.AddScoped<TableIDToDelete>();
builder.Services.AddScoped<NewTableValidator>();

builder.Services.AddScoped<KanTaskValidator>();
builder.Services.AddScoped<NewTaskValidator>();
builder.Services.AddScoped<KanTaskIdToDeleteValidator>();

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

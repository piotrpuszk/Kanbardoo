using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Domain.Repositories;
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

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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

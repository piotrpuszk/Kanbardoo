using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Contracts;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Application.UserClaimsUseCases;
using Kanbardoo.Application.UserContracts;
using Kanbardoo.Domain.Repositories;
using Kanbardoo.Domain.Validators;
using Kanbardoo.Infrastructure;
using Kanbardoo.Infrastructure.Repositories;
using Kanbardoo.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(e =>
    {
        e.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        e.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        e.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(e =>
    {
        e.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenSecretKey"]!)),
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Host.UseSerilog((context, config) =>
    config
    .WriteTo.File("logging.txt")
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(e => 
{
    e.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    e.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

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

builder.Services.AddScoped<ISignUpUseCase, SignUpUseCase>();
builder.Services.AddScoped<ISignInUseCase, SignInUseCase>();

builder.Services.AddScoped<IAddClaimToUserUseCase, AddClaimToUserUseCase>();

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IUserClaimsRepository, UserClaimsRepository>();
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

builder.Services.AddScoped<SignInValidator>();
builder.Services.AddScoped<SignUpValidator>();

builder.Services.AddScoped<AddClaimToUserValidator>();

builder.Services.AddScoped<ICreateToken, TokenService>();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

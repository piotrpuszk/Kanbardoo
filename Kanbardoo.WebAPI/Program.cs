using Kanbardoo.Application.Authorization.Policies;
using Kanbardoo.Application.Authorization.PolicyContracts;
using Kanbardoo.Application.Authorization.RequirementHandlers;
using Kanbardoo.Application.BoardUseCases;
using Kanbardoo.Application.Contracts;
using Kanbardoo.Application.Contracts.BoardContracts;
using Kanbardoo.Application.Contracts.InvitationContrats;
using Kanbardoo.Application.Contracts.TableContracts;
using Kanbardoo.Application.Contracts.TaskContracts;
using Kanbardoo.Application.Contracts.TaskStatusContracts;
using Kanbardoo.Application.Contracts.UserClaimsContracts;
using Kanbardoo.Application.Contracts.UserContracts;
using Kanbardoo.Application.InvitationUseCases;
using Kanbardoo.Application.TableUseCases;
using Kanbardoo.Application.TaskStatusUseCases;
using Kanbardoo.Application.TaskUseCases;
using Kanbardoo.Application.UserClaimsUseCases;
using Kanbardoo.Application.UserContracts;
using Kanbardoo.Domain.Authorization;
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

builder.Services.AddAuthorization(e =>
{
    e.AddPolicy(PolicyName.Admin, e => e.RequireClaim(KanClaimName.Admin));
});

builder.Services.AddScoped<IBoardMembershipPolicy, BoardMembershipPolicy>();
builder.Services.AddScoped<IBoardOwnershipPolicy, BoardOwnershipPolicy>();
builder.Services.AddScoped<ITableMembershipPolicy, TableMembershipPolicy>();
builder.Services.AddScoped<ITaskMembershipPolicy, TaskMembershipPolicy>();

builder.Services.AddScoped<BoardMembershipRequirementHandler>();
builder.Services.AddScoped<BoardOwnershipRequirementHandler>();
builder.Services.AddScoped<TableMembershipRequirementHandler>();
builder.Services.AddScoped<TaskMembershipRequirementHandler>();

builder.Services.AddHttpContextAccessor();

builder.Host.UseSerilog((context, config) =>
    config
    .WriteTo.Console()
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
builder.Services.AddScoped<IGetBoardMembersUseCase, GetBoardMembersUseCase>();

builder.Services.AddScoped<IAddTableUseCase, AddTableUseCase>();
builder.Services.AddScoped<IGetTableUseCase, GetTableUseCase>();
builder.Services.AddScoped<IDeleteTableUseCase, DeleteTableUseCase>();
builder.Services.AddScoped<IUpdateTableUseCase, UpdateTableUseCase>();

builder.Services.AddScoped<IAddTaskUseCase, AddTaskUseCase>();
builder.Services.AddScoped<IGetTaskUseCase, GetTaskUseCase>();
builder.Services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
builder.Services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();

builder.Services.AddScoped<IGetTaskStatusesUseCase, GetTaskStatusesUseCase>();

builder.Services.AddScoped<ISignUpUseCase, SignUpUseCase>();
builder.Services.AddScoped<ISignInUseCase, SignInUseCase>();
builder.Services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();

builder.Services.AddScoped<IAddClaimToUserUseCase, AddClaimToUserUseCase>();
builder.Services.AddScoped<IRevokeClaimFromUserUseCase, RevokeClaimFromUserUseCase>();

builder.Services.AddScoped<IInviteUserUseCase, InviteUserUseCase>();
builder.Services.AddScoped<IGetInvitationsUseCase, GetInvitationsUseCase>();
builder.Services.AddScoped<ICancelInvitationUseCase, CancelInvitationUseCase>();
builder.Services.AddScoped<IAcceptInvitationUseCase, AcceptInvitationUseCase>();
builder.Services.AddScoped<IDeclineInvitationUseCase, DeclineInvitationUseCase>();

builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IUserClaimsRepository, UserClaimsRepository>();
builder.Services.AddScoped<IUserBoardsRepository, UserBoardsRepository>();
builder.Services.AddScoped<IUserTablesRepository, UserTablesRepository>();
builder.Services.AddScoped<IUserTasksRepository, UserTasksRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserBoardRolesRepository, UserBoardRolesRepository>();
builder.Services.AddScoped<IResourceIdConverterRepository, ResourceIdConverterRepository>();
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

builder.Services.AddScoped<KanUserClaimValidator>();
builder.Services.AddScoped<NewKanUserClaimValidator>();

builder.Services.AddScoped<NewInvitationValidator>();
builder.Services.AddScoped<CancelInvitationValidator>();
builder.Services.AddScoped<AcceptInvitationValidator>();
builder.Services.AddScoped<DeclineInvitationValidator>();

builder.Services.AddScoped<ICreateToken, TokenService>();

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), options => 
    {
        options.SetPostgresVersion(new Version(@"9.2.8"));
    });
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

if (app.Environment.IsDevelopment())
{
    app.UseCors(e =>
    {
        e.AllowAnyHeader();
        e.AllowAnyMethod();
        e.AllowAnyOrigin();
    });
}

app.UseAuthentication();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToController("Index", "Fallback");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var context = services.GetRequiredService<DBContext>();
await context.Database.MigrateAsync();
await context.Database.EnsureCreatedAsync();

app.Run();
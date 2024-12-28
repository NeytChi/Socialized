using System.Text;
using Domain.Appeals;
using Domain.Appeals.Messages;
using Domain.Appeals.Replies;
using Domain.AutoPosting;
using Domain.GettingSubscribes;
using Domain.InstagramAccounts;
using Domain.Packages;
using Domain.Users;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UseCases.Admins;
using UseCases.AutoPosts.AutoPostFiles;
using UseCases.Users;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<Context>(options =>
        options.UseMySql(connectionString,
            new MySqlServerVersion(new Version(8, 0, 36))));

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IAdminManager, AdminManager>();
builder.Services.AddTransient<IAdminEmailManager, AdminEmailManager>();
builder.Services.AddTransient<IUsersManager, UsersManager>();
builder.Services.AddTransient<IUserLoginManager, UserLoginManager>();
builder.Services.AddTransient<IUserPasswordRecoveryManager, UserPasswordRecoveryManager>();

// UserPasswordRecoveryManager


builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<IAppealFileRepository, AppealFileRepository>();
builder.Services.AddTransient<IAppealMessageReplyRepository, AppealMessageReplyRepository>();
builder.Services.AddTransient<IAppealMessageRepository, AppealMessageRepository>();
builder.Services.AddTransient<IAppealRepository, AppealRepository>();
builder.Services.AddTransient<IAutoPostFileRepository, AutoPostFileRepository>();
builder.Services.AddTransient<IAutoPostRepository, AutoPostRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IDiscountRepository, DiscountRepository>();
builder.Services.AddTransient<IIGAccountRepository, IGAccountRepository>();
builder.Services.AddTransient<IForServerAccessCountingRepository, IGAccountRepository>();
builder.Services.AddTransient<IPackageAccessRepository, PackageAccessRepository>();
builder.Services.AddTransient<IServiceAccessRepository, ServiceAccessRepository>();
builder.Services.AddTransient<ITaskDataRepository, TaskDataRepository>();
builder.Services.AddTransient<ITaskGsRepository, TaskGsRepository>();
builder.Services.AddTransient<ITaskGettingSubscribesRepository, TaskGsRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddSingleton(Log.Logger);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOption =>
{
    var key = "config.GetValue<string>(\"JwtConfig:Key\")";
    var keyBytes = Encoding.ASCII.GetBytes(key);
    jwtOption.SaveToken = true;
    jwtOption.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true
    };
});



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

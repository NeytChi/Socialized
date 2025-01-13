using System.Text;
using Core;
using Core.FileControl;
using Domain.Admins;
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
using UseCases.AutoPosts;
using UseCases.AutoPosts.AutoPostFiles;
using UseCases.InstagramAccounts;
using UseCases.InstagramApi;
using UseCases.InstagramApi.MockingApi;
using UseCases.Packages;
using UseCases.Users;
using WebAPI.Middleware;

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
builder.Services.AddSingleton(Log.Logger);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAdminManager, AdminManager>();
builder.Services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IAdminEmailManager, AdminEmailManager>();
builder.Services.AddSingleton<ISmtpSender, SmtpSender>(); 
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddSingleton<IPackageManager, PackageManager>();
builder.Services.AddTransient<IServiceAccessRepository, ServiceAccessRepository>();
builder.Services.AddTransient<IPackageAccessRepository, PackageAccessRepository>();
builder.Services.AddTransient<IDiscountRepository, DiscountRepository>();
builder.Services.AddTransient<IForServerAccessCountingRepository, IGAccountRepository>();

builder.Services.AddSingleton<IAutoPostManager, AutoPostManager>();
builder.Services.AddTransient<IAutoPostRepository, AutoPostRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IIGAccountRepository, IGAccountRepository>();
builder.Services.AddSingleton<IAutoPostFileManager, AutoPostFileManager>();
builder.Services.AddTransient<IFileManager, AwsUploader>();
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
builder.Services.AddTransient<IAutoPostFileRepository, AutoPostFileRepository>();
builder.Services.AddTransient<IAutoPostFileSave, AutoPostFileSave>();



builder.Services.AddSingleton<IIGAccountManager, CreateIGAccountManager>();
builder.Services.AddTransient<ILoginApi, LoginApi>();
builder.Services.AddTransient<IChallengeRequiredAccount, ChallengeRequiredAccount>();
builder.Services.AddTransient<IGetChallengeRequireVerifyMethod, GetChallengeRequireVerifyMethod>();
builder.Services.AddTransient<IVerifyCodeForChallengeRequire, VerifyCodeForChallengeRequire>();
builder.Services.AddTransient<ILoginSessionManager, LoginSessionManager>();
builder.Services.AddTransient<IRecoverySessionManager, RecoverySessionManager>();
builder.Services.AddSingleton<ProfileCondition>();
builder.Services.AddTransient<IGetChallengeRequireVerifyMethod, GetChallengeRequireVerifyMethod>();
builder.Services.AddTransient<IGetStateData, GetStateData>();
builder.Services.AddTransient<ISaveSessionManager, SaveSessionManager>();

/// TODO: Write all dependencies for each controller

builder.Services.AddTransient<IPackageAccessRepository, PackageAccessRepository>();
builder.Services.AddTransient<IUsersManager, UsersManager>();
builder.Services.AddTransient<IUserLoginManager, UserLoginManager>();
builder.Services.AddTransient<IUserPasswordRecoveryManager, UserPasswordRecoveryManager>();


builder.Services.AddTransient<IAppealFileRepository, AppealFileRepository>();
builder.Services.AddTransient<IAppealMessageReplyRepository, AppealMessageReplyRepository>();
builder.Services.AddTransient<IAppealMessageRepository, AppealMessageRepository>();
builder.Services.AddTransient<IAppealRepository, AppealRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IIGAccountRepository, IGAccountRepository>();
builder.Services.AddTransient<ITaskDataRepository, TaskDataRepository>();
builder.Services.AddTransient<ITaskGsRepository, TaskGsRepository>();
builder.Services.AddTransient<ITaskGettingSubscribesRepository, TaskGsRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

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

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
using FfmpegConverter;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using UseCases.Admins;
using UseCases.Appeals;
using UseCases.Appeals.Messages;
using UseCases.Appeals.Replies;
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

builder.Services.AddDbContext<Context>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36))));

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

builder.Services.AddScoped<IAdminManager, AdminManager>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminEmailManager, AdminEmailManager>();
builder.Services.AddScoped<ISmtpSender, SmtpSender>(); 

builder.Services.AddScoped<IPackageManager, PackageManager>();
builder.Services.AddScoped<IServiceAccessRepository, ServiceAccessRepository>();
builder.Services.AddScoped<IPackageAccessRepository, PackageAccessRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<IForServerAccessCountingRepository, IGAccountRepository>();

builder.Services.AddScoped<IAutoPostManager, AutoPostManager>();
builder.Services.AddScoped<IAutoPostRepository, AutoPostRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IIGAccountRepository, IGAccountRepository>();
builder.Services.AddScoped<IAutoPostFileManager, AutoPostFileManager>();
builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
builder.Services.AddScoped<IFileManager, AwsUploader>();
builder.Services.AddScoped<IAutoPostFileRepository, AutoPostFileRepository>();
builder.Services.AddScoped<IAutoPostFileSave, AutoPostFileSave>();
builder.Services.AddScoped<IFileConverter, FileConverterMocking>();
builder.Services.AddScoped<IFileMover, FileMover>();

builder.Services.AddScoped<IIGAccountManager, CreateIGAccountManager>();
builder.Services.AddScoped<ILoginApi, LoginApi>();
builder.Services.AddScoped<IChallengeRequiredAccount, ChallengeRequiredAccount>();
builder.Services.AddScoped<IGetChallengeRequireVerifyMethod, GetChallengeRequireVerifyMethod>();
builder.Services.AddScoped<IVerifyCodeForChallengeRequire, VerifyCodeForChallengeRequire>();
builder.Services.AddScoped<ILoginSessionManager, LoginSessionManager>();
builder.Services.AddScoped<IRecoverySessionManager, RecoverySessionManager>();
builder.Services.AddScoped<ProfileCondition>();
builder.Services.AddScoped<IGetChallengeRequireVerifyMethod, GetChallengeRequireVerifyMethod>();
builder.Services.AddScoped<IGetStateData, GetStateData>();
builder.Services.AddScoped<ISaveSessionManager, SaveSessionManager>();
builder.Services.AddScoped<IDeleteIgAccountManager, DeleteIgAccountManager>();
builder.Services.AddScoped<ITaskGettingSubscribesRepository, TaskGsRepository>();
builder.Services.AddScoped<ISmsVerifyIgAccountManager, SmsVerifyIgAccountManager>();

builder.Services.AddScoped<IUsersManager, UsersManager>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailMessanger, EmailMessanger>();
builder.Services.AddScoped<IUserLoginManager, UserLoginManager>();
builder.Services.AddScoped<IUserPasswordRecoveryManager, UserPasswordRecoveryManager>();

builder.Services.AddScoped<IAppealManager, AppealManager>();
builder.Services.AddScoped<IAppealRepository, AppealRepository>();
builder.Services.AddScoped<IAppealMessageRepository, AppealMessageRepository>();

builder.Services.AddScoped<IAppealFileManager, AppealFileManager>();
builder.Services.AddScoped<IAppealFileRepository, AppealFileRepository>();

builder.Services.AddScoped<IAppealMessageManager, AppealMessageManager>();

builder.Services.AddScoped<IAppealMessageReplyManager, AppealMessageReplyManager>();
builder.Services.AddScoped<IAppealMessageReplyRepository, AppealMessageReplyRepository>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwtOption =>
{
    var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
    var keyBytes = Encoding.ASCII.GetBytes(key);
    jwtOption.SaveToken = true;
    jwtOption.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "SocializedAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scopeDatabase  = app.Services.CreateScope())
{
    var dbContext = scopeDatabase.ServiceProvider.GetRequiredService<Context>();

    dbContext.Database.EnsureCreated();
}

app.MapSwagger().RequireAuthorization();

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

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UseCases.Users;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
// Add services to the container.
builder.Services.AddSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUsersManager, UsersManager>();
builder.Services.AddTransient<IUserLoginManager, UserLoginManager>();
builder.Services.AddTransient<IUserPasswordRecoveryManager, UserPasswordRecoveryManager>();
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

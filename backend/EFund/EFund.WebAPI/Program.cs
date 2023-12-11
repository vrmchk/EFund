using System.Text;
using EFund.BLL.Services;
using EFund.BLL.Services.Auth.Auth;
using EFund.BLL.Services.Auth.Interfaces;
using EFund.BLL.Services.Interfaces;
using EFund.Common.Models.Configs;
using EFund.DAL.Contexts;
using EFund.DAL.Entities;
using EFund.DAL.Repositories;
using EFund.DAL.Repositories.Interfaces;
using EFund.Email.Services;
using EFund.Email.Services.Interfaces;
using EFund.Hangfire;
using EFund.Hangfire.Abstractions;
using EFund.Mapping.Profiles;
using EFund.Validation.Auth;
using EFund.Validation.Extensions;
using EFund.WebAPI.Extensions;
using FluentEmail.MailKitSmtp;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Configs
var appDataConfig = new AppDataConfig();
var jwtConfig = new JwtConfig();
var emailConfig = new EmailConfig();
var googleConfig = new GoogleConfig();
var hangfireConfig = new HangfireConfig();
var monobankConfig = new MonobankConfig();

builder.Services.AddConfigs(builder.Configuration, opt =>
    opt.AddConfig<AppDataConfig>(out appDataConfig, pathProperties: config => config.AppDataPath)
        .AddConfig<JwtConfig>(out jwtConfig)
        .AddConfig<EmailConfig>(out emailConfig, pathProperties: config => config.TemplatesPath)
        .AddConfig<AuthConfig>()
        .AddConfig<GoogleConfig>(out googleConfig)
        .AddConfig<HangfireConfig>(out hangfireConfig)
        .AddConfig<EncryptionConfig>()
        .AddConfig<MonobankConfig>(out monobankConfig, "ApiClients:Monobank"));

//DbContext
var dbContextLoggerFactory = LoggerFactory.Create(cfg => cfg.AddConsole());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLoggerFactory(dbContextLoggerFactory));

//Hangfire
builder.Services.AddHangfire(cfg =>
    cfg.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();
builder.Services.AddScoped<IHangfireService, HangfireService>();

//Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IUserCleanerService, UserCleanerService>();

//Email
builder.Services.AddFluentEmail(emailConfig.DefaultEmail)
    .AddRazorRenderer(emailConfig.TemplatesPath)
    .AddMailKitSender(new SmtpClientOptions
    {
        Server = emailConfig.SmtpServer,
        Port = emailConfig.SmtpPort,
        User = emailConfig.DefaultEmail,
        Password = emailConfig.Password,
        UseSsl = false,
        RequiresAuthentication = true,
    });

builder.Services.AddScoped<IEmailSender, EmailSender>();

//Utility
builder.Services.AddCors();

//Mapper
builder.Services.AddAutoMapper(typeof(UserProfile));

//Validators
builder.Services.AddValidatorServiceFromAssemblyContaining<SignInDTOValidator>();

//Logger
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Map(
        evt => evt.Level,
        (level, wt) =>
            wt.File(
                $@"{appDataConfig.AppDataPath}\{appDataConfig.LogDirectory}\{level}-{DateTime.Today:yyyy-MM-dd}.log"))
    .CreateLogger();
builder.Logging.AddSerilog(logger, dispose: true);

//Auth
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
    ValidIssuer = jwtConfig.Issuer,
    ValidAudience = jwtConfig.Audience,
    ClockSkew = jwtConfig.ClockSkew
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = tokenValidationParameters;
    })
    .AddGoogle(GoogleDefaults.AuthenticationScheme, opt =>
    {
        opt.ClientId = googleConfig.ClientId;
        opt.ClientSecret = googleConfig.ClientSecret;
    });

//Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EFund API", Version = "v1" });

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.MigrateDatabase();

await app.ValidateConfigsAsync();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "EFund Dashboard",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = hangfireConfig.User, Pass = hangfireConfig.Password
        }
    }
});

app.MapHangfireDashboard();

app.SetupHangfire(hangfireConfig);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyOrigin()
    .AllowAnyMethod());

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
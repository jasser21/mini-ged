using System;
using System.Text;
using Hangfire;
using Hangfire.Storage.SQLite;
using Meilisearch;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniGED.API.Config;
using MiniGED.API.Data;
using MiniGED.API.Interfaces;
using MiniGED.API.Services;
using static MiniGED.API.Config.LoginUserWithRefreshToken;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MiniGed API",
        Description = "An ASP.NET Core Web API for managing electronic documents",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var HangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
// Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// JWT
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<LoginUserWithRefreshToken>();

builder.Services.AddScoped<IDocumentTextExtractor,DocumentTextExtractor>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddSingleton<MeilisearchClient>(sp =>
{ 
    var url = builder.Configuration["Meilisearch:Url"];
    var masterKey = builder.Configuration["Meilisearch:MasterKey"];
    return new MeilisearchClient(url, masterKey);
});

builder.Services.AddSingleton<IDocumentSearchEngine,DocumentSearchEngine>();
builder.Services.AddScoped<IFileProcessingJob,FileProcessingJob>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


// hanfire client
builder.Services.AddHangfire(config => config
.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSQLiteStorage(HangfireConnectionString)
);


// hanfire service 
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
var app = builder.Build();

app.UseCors("AllowAll");
new LoginUserWithRefreshToken.Endpoint().MapEndpoint(app);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
    app.MapHangfireDashboard("/hangfire");
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var pw = scope.ServiceProvider.GetRequiredService<PasswordService>();

    await db.Database.MigrateAsync();

    await SeedData.SeedAsync(db, pw);
}
app.Run();

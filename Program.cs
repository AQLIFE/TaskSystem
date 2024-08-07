using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManangerSystem.Services.Auth;
using TaskManangerSystem.Services.Filter;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Tool;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddAuthorization(option =>
        option.AddPolicy("Admin", policy => policy.Requirements.Add(new CustomRequirement())))
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(option =>
        {
            BearerConfig obj = new();
            option.TokenValidationParameters = obj.tokenValidation;
            option.Events = obj.bearerEvents;
        });

builder
    .ConditionalCheck(e => e.Environment.IsDevelopment(), e => e.Services.AddSwaggerGen(), e => e.Services)
    .AddAutoMapper(Assembly.GetExecutingAssembly())
    .AddSingleton<IAuthorizationHandler, CustomHandler>()
    .AddSingleton<BearerInfo>()
    .AddSingleton<APILog>()
    .AddSingleton<DBLinkExcptionLog>()
    .AddDbContext<ManagementSystemContext>(p => p.UseMySQL(SystemInfo.DB_LINK))
    .AddControllers(options =>
    {
        options.Filters.Add<APIActionFilter>();
        options.Filters.Add<APIExceptionFilter>();
    });


var app = builder.Build();
// Configure the HTTP request pipeline.

app.ConditionalCheck(e => e.Environment.IsDevelopment(),
                     e => e.UseSwaggerUI().UseSwagger(), e => e)
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();
app.Run();

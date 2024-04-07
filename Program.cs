using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Logging.ClearProviders().AddConsole();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Configuration.Bind("Authentication", nameof(TokenOption));//绑定配置数据至对象
builder.Services
    .AddScoped<BearerInfo>()
    .AddControllers(options => options.Filters.Add<AppFilter>());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(option =>
        {
            BearerConfig obj = new();
            option.TokenValidationParameters = obj.tokenValidation;
            option.Events = obj.bearerEvents;
        });
builder.Services.AddAuthorization(option =>
    option.AddPolicy("Admin", policy => policy.Requirements.Add(new CustomRequirement()))
);
builder.Services.AddSingleton<IAuthorizationHandler, CustomHandler>();


if (!SystemInfo.DBLINK.IsNullOrEmpty())
    builder.Services.AddDbContext<ManagementSystemContext>(options => options.UseMySQL(SystemInfo.DBLINK));
//添加数据库链接上下文

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();
}
else throw new Exception("尚未达到发布标准");

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI()
    .UseSwagger();
}
app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();
app.Run();

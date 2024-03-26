using TaskManangerSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManangerSystem.IServices.SystemServices;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Logging.ClearProviders().AddConsole();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Configuration.Bind("Authentication", nameof(TokenOption));//绑定配置数据至对象
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




string? oj = Environment.GetEnvironmentVariable("DB_LINK");
if (!oj.IsNullOrEmpty() && oj is string str)
    builder.Services.AddDbContext<ManagementSystemContext>(options => options.UseMySQL(str));
//添加数据库链接上下文
else throw new Exception("Missing DB_LINK config");

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

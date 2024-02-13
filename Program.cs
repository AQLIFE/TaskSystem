using TaskManangerSystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TaskManangerSystem.IServices.SystemServices;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Services.Filters;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services
    .AddScoped<IBearer, BearerInfo>()
    .AddControllers(options => options.Filters.Add<AppFilter>());


if (!Environment.GetEnvironmentVariable("DB_LINK").IsNullOrEmpty())
{
    string str = Environment.GetEnvironmentVariable("DB_LINK")!;
    builder.Services.AddDbContext<ManagementSystemContext>(options => options.UseMySQL(str));
    //添加数据库链接上下文
}
else throw new Exception("Missing DB_LINK config");

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen();

    TokenOption tokenOption = new();
    builder.Configuration.Bind("Authentication", tokenOption);//绑定配置数据至对象

    BearerConfig obj = new();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(option =>{
        option.TokenValidationParameters = obj.tokenValidation;
        option.Events = obj.bearerEvents;});
}else throw new Exception("尚未达到发布标准");

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
        .UseSwaggerUI();
}
app.UseAuthentication()
    .UseAuthorization();

app.MapControllers();
app.Run();

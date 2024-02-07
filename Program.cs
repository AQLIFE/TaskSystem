using TaskManangerSystem.Services;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;

using TaskManangerSystem.Actions;
using TaskManangerSystem.IServices.SystemServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
    options.Filters.Add<AppFilter>());


if (builder.Environment.IsDevelopment())
{
    // string? issuer = builder.Configuration["Authentication:Schemes:Bearer:SigningKeys:0:Issuer"];
    string? str = builder.Configuration["TaskManangerSystem:ConnectionStrings:MysqlSource"];
    if (str != null)
        builder.Services.AddDbContext<ManagementSystemContext>(options => options.UseMySQL(str));//添加数据库链接上下文


    TokenOption tokenOption = new();
    builder.Configuration.Bind("Authentication", tokenOption);//绑定配置数据至对象

    builder.Services.AddScoped<ICustom, JsonWebTokenInfo>();


    var obj = new JsonWebTokenOption(builder.Configuration);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
    {
        option.TokenValidationParameters = obj.GetTokenValidation;
        
        option.Events = obj.GetBearerEvents;
    });

}
else throw new InvalidOperationException("尚未达到发布标准");




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();// 授权
app.UseAuthorization(); // 鉴权

app.MapControllers();

app.Run();

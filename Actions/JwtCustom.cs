// using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManangerSystem.IServer;
using TaskManangerSystem.Models;


namespace TaskManangerSystem.Actions
{
    public class JsonWebTokenInfo : ICustom
    {
        private readonly IConfiguration _configuration;
        private Claim[]? claims;//客户端信息

        private SecurityKey key;//加密密钥

        private SigningCredentials signing;//加密后密钥

        private JwtSecurityToken? token;

        private string issuer;

        private string audience;

        public JsonWebTokenInfo(IConfiguration configuration)
        {
            _configuration = configuration;

            string apiKey = _configuration["TaskManangerSystem:ServiceApiKey"] ?? throw new Exception("Program Error:Missing Key");
            byte[] secretByte = Encoding.UTF8.GetBytes(apiKey);
            this.issuer = _configuration["Authentication:Schemes:Bearer:SigningKeys:0:Issuer"] ?? throw new Exception("Program Error:Misssing Issuer");
            this.audience = _configuration["Authentication:Audience"] ?? throw new Exception("Program Error:Misssing Audience");


            this.key = new SymmetricSecurityKey(secretByte);
            signing = new SigningCredentials(this.key, SecurityAlgorithms.HmacSha256);
        }

        public string CreateToken(string name, string role = "default")
        {
            this.claims = [
                new Claim(ClaimTypes.NameIdentifier,audience),
                new Claim(ClaimTypes.Role,role),
                new  Claim(ClaimTypes.Name,name),
            ];
            this.token = new JwtSecurityToken(issuer: issuer, claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: signing);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }




    public class JsonWebTokenOption
    {
        private IConfiguration _configuration;
        private JwtBearerEvents bearerEvents = new JwtBearerEvents();
        private TokenValidationParameters tokenValidation;

        public JsonWebTokenOption(IConfiguration configuration)
        {
            _configuration = configuration;


            string apiKey = _configuration["TaskManangerSystem:ServiceApiKey"] ?? throw new Exception("Program Error:Missing Key");
            byte[] secretByte = Encoding.UTF8.GetBytes(apiKey);
            tokenValidation = new TokenValidationParameters
            {
                ValidateIssuer = true,//验证发布者
                ValidIssuer = _configuration["Authentication:Schemes:Bearer:SigningKeys:0:Issuer"] ?? throw new Exception("Program Error:Misssing Issuer"),
                ValidateAudience = true,
                ValidAudience = _configuration["Authentication:Audience"] ?? throw new Exception("Program Error:Misssing Audience"),
                ValidateLifetime = true,//验证失效时间
                ValidateIssuerSigningKey = true,//验证公钥
                IssuerSigningKey = new SymmetricSecurityKey(secretByte)
            };


            bearerEvents.OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsJsonAsync(
                    new ReturnInfo{status=false,data="无权访问"}
                    
                );
                return System.Threading.Tasks.Task.FromResult(0);

            };
        }

        public JwtBearerEvents GetBearerEvents => this.bearerEvents;
        public TokenValidationParameters GetTokenValidation => this.tokenValidation;
    }
}
// using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManangerSystem.IServices.BeanServices;
using TaskManangerSystem.IServices.SystemServices;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;


namespace TaskManangerSystem.Services
{
    public interface ICustom
    {
        public string CreateToken(IAuth auth);
    }

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

        public string CreateToken(EncryptAccount encrypt){
             this.claims = [
                // new Claim(ClaimTypes.Role,encrypt.AccountPermission.ToString()),
                new  Claim(ClaimTypes.Name,encrypt.EmployeeAlias),
                new Claim(ClaimTypes.Authentication,encrypt.EncryptionId)
            ];
            // Console.WriteLine($"Create => {audience}");
            this.token = new (issuer: issuer, audience:audience, claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: signing);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Obsolete("缺少合适的应用")]
        public string CreateToken(IAuth auth)
        {
            this.claims = [
                new Claim(ClaimTypes.Role,auth.AccountPermission.ToString()),
                new  Claim(ClaimTypes.Name,auth.EmployeeAlias),
                new Claim(ClaimTypes.Authentication,auth.EncryptionId)
            ];
            // Console.WriteLine($"Create => {audience}");
            this.token = new (issuer: issuer, audience:audience, claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: signing);
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
            // Console.WriteLine($"Vaildate => {_configuration["Authentication:Audience"]}");


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
                // context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsJsonAsync(
                    new Result<string>(){status=false,data="无权访问"}
                );
                return System.Threading.Tasks.Task.FromResult(0);

            };
        }

        public JwtBearerEvents GetBearerEvents => this.bearerEvents;
        public TokenValidationParameters GetTokenValidation => this.tokenValidation;
    }
}
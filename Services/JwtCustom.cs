using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.IServices.SystemServices;


namespace TaskManangerSystem.Services
{
    
    public class JsonWebTokenInfo : ComonJWT, ICustom
    {
        private Claim[]? claims;//客户端信息

        private JwtSecurityToken? token;

        public JsonWebTokenInfo(){}

        public string CreateToken(EncryptAccount encrypt)
        {
            this.claims = [
                new Claim(ClaimTypes.Name,encrypt.EmployeeAlias),
                new Claim(ClaimTypes.Authentication,encrypt.EncryptionId)
           ];
            this.token = new(issuer: Issuer, audience: Audience, claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: signing);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class JsonWebTokenOption : ComonJWT
    {
        public JwtBearerEvents bearerEvents = new JwtBearerEvents();
        public TokenValidationParameters tokenValidation;

        public JsonWebTokenOption()
        {
            tokenValidation = new TokenValidationParameters
            {
                ValidateIssuer = true,//验证发布者
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = true,//验证失效时间
                ValidateIssuerSigningKey = true,//验证公钥
                IssuerSigningKey = key
            };


            bearerEvents.OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.ContentType = "application/json";
                context.Response.WriteAsJsonAsync(GlobalResult.NotAccess);
                return Task.FromResult(0);
            };
        }
    }

    // public static class StaticJWTOption{
        
    // }
}
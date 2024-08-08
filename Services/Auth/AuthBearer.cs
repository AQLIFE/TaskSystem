using Jose;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Crypto;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Tool;

namespace TaskManangerSystem.Services.Auth
{
    public class BearerInfo
    {

        private readonly JwtSecurityToken jwt = new();

        public BearerInfo() { }
        public BearerInfo(Employee employeeAccount)
        {
            jwt = new(
                issuer: SystemInfo.ISSUER,
                audience: SystemInfo.AUDIENCE,
                claims:
                [
                    new(ClaimTypes.Authentication,employeeAccount.EmployeeId.ToString().ComputeSHA512Hash()),
                    new(ClaimTypes.Role,          employeeAccount.AccountPermission.ToString())
                ],
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: CryptoManager.SigningCredentials);
        }

        public string CreateJWT()
            => new JwtSecurityTokenHandler().WriteToken(jwt);// 创建JwtSecurityToken

        public string CreateToken()
            => JWT.Encode(payload: CreateJWT(), key: CryptoManager.rsaEncryptor.Rsa, alg: JweAlgorithm.RSA_OAEP_256, enc: JweEncryption.A256CBC_HS512);
        // 使用Jose库创建加密的JWE

    }

    public class BearerConfig
    {
        public readonly JwtBearerEvents bearerEvents = new();
        public readonly TokenValidationParameters tokenValidation;

        public BearerConfig()
        {
            tokenValidation = new()
            {
                ValidateIssuer = true,//验证发布者
                ValidIssuer = SystemInfo.ISSUER,
                ValidateAudience = true,//验证订阅者
                ValidAudience = SystemInfo.AUDIENCE,
                ValidateLifetime = true,//验证失效时间
                ValidateIssuerSigningKey = true,//验证公钥
                IssuerSigningKey = CryptoManager.SigningCredentials.Key

            };

            bearerEvents.OnTokenValidated = async context =>
            {
                int? accountPermission = (context.Principal?.Claims).GetClaim(ClaimTypes.Role)?.Value.ToInt32();
                const int roles = 1;// 设定一个固定的权限等级 Roles
                // 如果 AccountPermission Claim 不存在或其值小于设定的 Roles，则拒绝访问
                if (accountPermission.HasValue && accountPermission.Value < roles || accountPermission == 0)
                    context.Fail("全局规则:权限等级不足");
                context.HttpContext.Items.Add("IsAdmin", accountPermission >= SystemInfo.AdminRole);
                context.HttpContext.Items.Add("HashId", (context.Principal?.Claims).GetClaim(ClaimTypes.Authentication)?.Value);

                await Task.CompletedTask;
            };

            bearerEvents.OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(GlobalResult.NotAccess);
            };

            bearerEvents.OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(GlobalResult.Forbidden);
            };

            bearerEvents.OnMessageReceived = async context =>
            {
                var authorizationHeader = context.Request.Headers.Authorization;

                if ( !authorizationHeader.IsNullOrEmpty())
                {
                    context.Token = JWT.Decode(
                        authorizationHeader.ToString().Replace("Bearer ", ""),
                        key: CryptoManager.rsaDecryptor.Rsa,
                        alg: JweAlgorithm.RSA_OAEP_256,
                        enc: JweEncryption.A256CBC_HS512
                        );
                    context.Options.SaveToken = true;
                }
                await Task.CompletedTask;
            };
        }
    }
}
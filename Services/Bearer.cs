using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.IServices.SystemServices;
using TaskManangerSystem.Actions;
using Jose;

namespace TaskManangerSystem.Services
{

    // 注意：在实际应用中，你需要确保使用的是公钥进行加密，私钥进行解密。

    public static class StringExtensions
    {
        public static int? ToInt32(this string? value)
        {
            if (int.TryParse(value, out int result)) return result;
            else return null;
        }
    }
    public class BearerInfo
    {
        public BearerInfo() { }
        public BearerInfo(EmployeeAccount employeeAccount)
        {
            jwt = new(
                issuer: TokenOption.Issuer,
                audience: TokenOption.Audience,
                claims:
                [
                    new(ClaimTypes.Authentication,ShaHashExtensions.ComputeSHA512Hash(employeeAccount.EmployeeId.ToString())),
                    new(ClaimTypes.Role,          employeeAccount.AccountPermission.ToString())
                ],
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: KeyManager.SigningCredentials);
        }
        private JwtSecurityToken jwt;

        public string CreateJWT()
            => new JwtSecurityTokenHandler().WriteToken(jwt);// 创建JwtSecurityToken

        public string CreateToken()
            => JWT.Encode(payload: CreateJWT(), key: KeyManager.rsaEncryptor.Rsa, alg: JweAlgorithm.RSA_OAEP, enc: JweEncryption.A256CBC_HS512);
        // 使用Jose库创建加密的JWE

    }

    public class BearerConfig
    {
        public JwtBearerEvents bearerEvents = new();
        public TokenValidationParameters tokenValidation;

        public BearerConfig()
        {
            tokenValidation = new()
            {
                ValidateIssuer = true,//验证发布者
                ValidIssuer = TokenOption.Issuer,
                ValidateAudience = true,//验证订阅者
                ValidAudience = TokenOption.Audience,
                ValidateLifetime = true,//验证失效时间
                ValidateIssuerSigningKey = true,//验证公钥
                IssuerSigningKey = KeyManager.SigningCredentials.Key
            };

            bearerEvents.OnTokenValidated = async (context) =>
            {
                // Jose.JWE
                int? accountPermission = FilterAction.GetClaim(context.Principal?.Claims, ClaimTypes.Role)?.Value.ToInt32();

                // 设定一个固定的权限等级 Roles
                const int roles = 1;

                // 如果 AccountPermission Claim 不存在或其值小于设定的 Roles，则拒绝访问
                if (!accountPermission.HasValue || accountPermission.Value < roles)
                    context.Fail("全局规则:权限等级不足");

                await Task.CompletedTask;
            };

            bearerEvents.OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.WriteAsJsonAsync(GlobalResult.NotAccess);
                return Task.FromResult(0);
            };

            bearerEvents.OnForbidden = context =>
            {
                // context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.WriteAsJsonAsync(GlobalResult.Forbidden);
                return Task.FromResult(0);
            };
        }
    }
}
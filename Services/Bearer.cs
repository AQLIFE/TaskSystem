﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;
using TaskManangerSystem.Actions;
using Jose;

namespace TaskManangerSystem.Services
{

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

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public BearerInfo() { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public BearerInfo(EmployeeAccount employeeAccount)
        {
            jwt = new(
                issuer:   SystemInfo.ISSUER,
                audience: SystemInfo.AUDIENCE,
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
                ValidIssuer = SystemInfo.ISSUER,
                ValidateAudience = true,//验证订阅者
                ValidAudience = SystemInfo.AUDIENCE,
                ValidateLifetime = true,//验证失效时间
                ValidateIssuerSigningKey = true,//验证公钥
                IssuerSigningKey = KeyManager.SigningCredentials.Key

            };

            bearerEvents.OnTokenValidated = async (context) =>
            {
                int? accountPermission = FilterAction.GetClaim(context.Principal?.Claims, ClaimTypes.Role)?.Value.ToInt32();
                const int roles = 1;// 设定一个固定的权限等级 Roles
                // 如果 AccountPermission Claim 不存在或其值小于设定的 Roles，则拒绝访问
                if (!accountPermission.HasValue || accountPermission.Value < roles)
                    context.Fail("全局规则:权限等级不足");
                context.HttpContext.Items.Add("IsAdmin",accountPermission>=SystemInfo.adminRole);

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
            bearerEvents.OnMessageReceived = context =>
            {

                var authorizationHeader = context.Request.Headers["Authorization"];
                if (!authorizationHeader.IsNullOrEmpty())
                {

                    string deJwt = JWT.Decode(authorizationHeader.ToString().Replace("Bearer ", ""), KeyManager.rsaDecryptor.Rsa, JweAlgorithm.RSA_OAEP, JweEncryption.A256CBC_HS512);
                    // context.HttpContext.Items.Add("Bearer", deJwt);
                    // context.Token = context.HttpContext.Items.Where(e => e.Key.ToString() == "Bearer").First().Value!.ToString();
                    context.Token = deJwt;
                    return Task.FromResult(0);
                    
                }

                context.Fail("全局规则:token无效");
                return Task.CompletedTask;
            };
        }
    }
}
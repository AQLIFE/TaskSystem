using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TaskManangerSystem.Services.Tool
{
    public static class HttpAction
    {
        public static Claim? GetClaim(this IEnumerable<Claim>? claims, string source)
            => claims?.FirstOrDefault(e => e.Type == source);

        public static bool ExistsParmeter(this IDictionary<string, object?> pairs, string name)
            => !pairs.IsNullOrEmpty() && pairs.ContainsKey(name);

        public static bool IsAdmin(this HttpContext cx) => cx.Items.Where(e => e.Key.ToString() == "IsAdmin").FirstOrDefault().Value?.ToString() == true.ToString();

    }
}
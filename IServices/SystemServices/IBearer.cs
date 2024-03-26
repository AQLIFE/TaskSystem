using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Unicode;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.IServices.SystemServices
{
    public static class TokenOption
    {
        public static string Issuer => Environment.GetEnvironmentVariable("ISSUER") ?? throw new Exception("Program Error:Misssing Issuer");
        public static string Audience => Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new Exception("Program Error:Misssing Audience");
    }
    public static class KeyManager
    {
        private static string SecurityKey => Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("Program Error:Missing Key");

        private static X509Certificate2 cert = ReadCert();


        private static readonly Lazy<X509Certificate2> _lazyCertificate = new(cert);

        private static readonly SigningCredentials _signingCredentials = new(
                new X509SecurityKey(_lazyCertificate.Value),
                SecurityAlgorithms.RsaSha256);
        private static X509Certificate2 ReadCert()
        {
            string certPath = Environment.GetEnvironmentVariable("RSA_CERT_PATH") ?? throw new Exception("Miss certPath");// 假设私钥存储在环境变量中，需要根据实际情况调整
            return new X509Certificate2( certPath, ShaHashExtensions.ComputeSHA256Hash(SecurityKey), X509KeyStorageFlags.Exportable);// 从文件加载证书（如有必要提供密码）
        }

        public static RsaSecurityKey rsaEncryptor = new(cert.GetRSAPublicKey());// 在实际JWE中是使用公钥加密，此处仅为示例）
        public static RsaSecurityKey rsaDecryptor = new(cert.GetRSAPrivateKey());// 使用私钥解密


        public static SigningCredentials SigningCredentials => _signingCredentials;
    }

}
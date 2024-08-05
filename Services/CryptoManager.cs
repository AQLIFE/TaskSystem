using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using TaskManangerSystem.Actions;

namespace TaskManangerSystem.Services
{
    public static class CryptoManager
    {
        private static X509Certificate2 cert => ReadCert();


        private static readonly Lazy<X509Certificate2> _lazyCertificate = new(cert);

        private static readonly SigningCredentials _signingCredentials = new(
                new X509SecurityKey(_lazyCertificate.Value),
                SecurityAlgorithms.RsaSha512);
        private static X509Certificate2 ReadCert()
            => new(SystemInfo.CERTPATH, SystemInfo.SECURITYKEY.ComputeSHA512Hash(), X509KeyStorageFlags.Exportable);// 从文件加载证书（如有必要提供密码）

        //A2104EF28DDC230574C93CEB4B331DC30EBE32B838DEF947C4A1BFF2F7A91F4E

        public static RsaSecurityKey rsaEncryptor = new(cert.GetRSAPublicKey());
        public static RsaSecurityKey rsaDecryptor = new(cert.GetRSAPrivateKey());// 使用私钥解密


        public static SigningCredentials SigningCredentials => _signingCredentials;
    }

}
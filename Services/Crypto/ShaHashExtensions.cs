using System.Security.Cryptography;
using System.Text;

namespace TaskManangerSystem.Services.Crypto
{
    /// <summary>
    /// ShaEncrypted 的Static版本
    /// 功能实现一致
    /// </summary>
    public static class ShaHashExtensions
    {
        public static string ComputeSHA256Hash(this string plainText)
        => ComputeHash(plainText, data => SHA256.HashData(data));

        public static string ComputeSHA384Hash(this string plainText)
        => ComputeHash(plainText, data => SHA384.HashData(data));

        public static string ComputeSHA512Hash(this string plainText)
           => ComputeHash(plainText, data => SHA512.HashData(data));


        private static string ComputeHash(string input, Func<byte[], byte[]> hashAlgorithmFunc)
        {
            var bytesToHash = Encoding.UTF8.GetBytes(input);
            var hashedBytes = hashAlgorithmFunc(bytesToHash);

            return string.Join("", hashedBytes.Select(b => b.ToString("X2")));
        }
    }

    /// <summary>
    /// 提供SHA2 系列加密
    /// </summary>
    /// <param name="str"></param>
    public class ShaEncrypted(string str)
    {
        private readonly string _encryptedValue = str;
        private readonly Encoding _encoding = Encoding.UTF8;

        public static implicit operator ShaEncrypted(string plainText) => new ShaEncrypted(plainText);

        private string ComputeHash(Func<byte[], byte[]> hashAlgorithmFunc)
           => string.Join("", hashAlgorithmFunc(_encoding.GetBytes(_encryptedValue)).Select(b => b.ToString("x2")));

        public string ComputeSHA256Hash() => ComputeHash(data => SHA256.HashData(data));
        public string ComputeSHA384Hash() => ComputeHash(data => SHA384.HashData(data));
        public string ComputeSHA512Hash() => ComputeHash(data => SHA512.HashData(data));
    }
}

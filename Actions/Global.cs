using System.Security.Cryptography;
using System.Text;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Actions
{

    public static class SystemInfo
    {
        public const int adminRole = 90;
        public const int pageSize = 120;
        public static EmployeeAccount admin => new("admin", ShaHashExtensions.ComputeSHA512Hash("admin@123"), adminRole + 9);
        public static Category[] categories => [
            new("库存分类",100,"用于对产品进行分类"),
            new("客户分类",101,"用于对客户进行分类"),
            new("任务分类",102,"用于对任务进行分类"),
        ];
        // public static readonly Category category = new("本公司", 103, "管理员所属公司", 2, actions.GetCategoryBySerial(101)?.CategoryId);
        public static Customer customers = new("管理员", Guid.NewGuid(), 1, "13212345678", "本公司");


        public readonly static string DBLINK = Environment.GetEnvironmentVariable("DB_LINK")        ?? throw new Exception("Program Error:Miss DB_LINK");
        public readonly static string ISSUER = Environment.GetEnvironmentVariable("ISSUER")         ?? throw new Exception("Program Error:Misssing Issuer");
        public readonly static string AUDIENCE = Environment.GetEnvironmentVariable("AUDIENCE")     ?? throw new Exception("Program Error:Misssing Audience");
        public readonly static string SECURITYKEY = Environment.GetEnvironmentVariable("API_KEY")   ?? throw new Exception("Program Error:Missing Key");
        public readonly static string CERTPATH = Environment.GetEnvironmentVariable("RSA_CERT_PATH")?? throw new Exception("Program Error:Miss certPath");
        // 假设私钥存储在环境变量中，需要根据实际情况调整
    }

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

    public class SymmetricEncryption
    {
        public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.  
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an Aes object  
            // with the specified key and IV.  
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.  
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.  
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.  
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }

        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = string.Empty;

            // Create an Aes object  
            // with the specified key and IV.  
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.  
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.  
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream  
                            // and place them in a string.  
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }

}
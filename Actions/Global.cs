using System.Security.Cryptography;
using System.Text;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Actions
{

    public static class SystemInfo
    {

        public const int TRASH = 100;
        public const int EMPLOYEE = 101;
        public const int CATEGORY = 102;
        public const int CUSTOMER = 103;

        public const int DefaultRole = 1;//基础权限
        public const int AdminRole = 90;//完整权限级别
        public const int PageSize = 100;//单页最大数量

        private const string ValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateUniqueRandomName(int length)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                return new string(randomBytes.Select(b => ValidChars[b % (ValidChars.Length)]).ToArray());
            }
        }

        public static EmployeeAccount admin => new("admin", "admin@123".ComputeSHA512Hash(), AdminRole + 9);
        public static Category[] categories => [
            new("分类垃圾桶",TRASH,"用于存放标记为删除的分类信息"),
            new("库存分类",  CATEGORY,"用于对产品进行分类"),
            new("客户分类",  CUSTOMER,"用于对客户进行分类"),
            new("任务分类",  EMPLOYEE,"用于对任务进行分类")

        ];

        public static Category[] customer => [
            new("普通客户", CUSTOMER+2,level:2,parId:categories[2].CategoryId),
            new("特级客户", CUSTOMER+3,level:2,parId:categories[2].CategoryId),
            new("机密客户", CUSTOMER+4,level:2,parId:categories[2].CategoryId)
        ];
        // public static readonly Category category = new("本公司", 103, "管理员所属公司", 2, actions.GetCategoryBySerial(101)?.CategoryId);
        public static Customer customers = new("管理员", Guid.NewGuid(), DefaultRole, "10241024", "本公司");


        // public readonly static string DBLINK = Environment.GetEnvironmentVariable("DB_LINK") ?? throw new Exception("Program Error:Miss DB_LINK");
        private readonly static string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception(ErrorMessage.MISS_DBNAME);
        private readonly static string DB_HOST_NAME = Environment.GetEnvironmentVariable("DB_HOST_NAME") ?? throw new Exception(ErrorMessage.MISS_DBHOST);
        private readonly static string DB_HOST_PASS = Environment.GetEnvironmentVariable("DB_HOST_PASS") ?? throw new Exception(ErrorMessage.MISS_DBPASS);
        private readonly static string DB_PART_NAME = Environment.GetEnvironmentVariable("DB_PART_NAME") ?? throw new Exception(ErrorMessage.MISS_DBPART);

        public static string DB_LINK => $"server={SystemInfo.DB_HOST_NAME};port=3306;database={SystemInfo.DB_NAME};user={SystemInfo.DB_PART_NAME};password={DB_HOST_PASS};";

        public readonly static string ISSUER = Environment.GetEnvironmentVariable("ISSUER") ?? throw new Exception(ErrorMessage.MISS_JWT_ISSUER);
        public readonly static string AUDIENCE = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new Exception(ErrorMessage.MISS_JWT_AUDIENCE);
        public readonly static string SECURITYKEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception(ErrorMessage.MISS_JWT_KEY);
        public readonly static string CERTPATH = Environment.GetEnvironmentVariable("RSA_CERT_PATH") ?? throw new Exception(ErrorMessage.MISS_JWT_RSA_CERT);
        // 假设私钥存储在环境变量中，需要根据实际情况调整

    }



    public static class ConditionalCheckOperators
    {
        /// <summary>
        /// 方法A
        /// 支持Lambda 表达式的方式去检查对象<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <returns>符合条件则返回obj，反之返回default<T></returns>
        public static T? ConditionalCheck<T>(this T obj, Func<T, bool> condition) where T : class
            => obj is not null && condition(obj) ? obj : default;



        /// <summary>
        /// 方法B
        /// 支持Lambda 表达式的方式去检查对象<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <returns>符合条件则返回succeed(obj)，反之返回default</returns>
        public static TResult? ConditionalCheck<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> succeed) where T : class
            => obj is not null && condition(obj) ? succeed(obj) : default;


        /// <summary>
        /// 方法B 的异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj"></param>
        /// <param name="condition"></param>
        /// <param name="succeed"></param>
        /// <returns></returns>
        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, Func<T, Task<bool>> condition, Func<T, Task<TResult>> succeed) where T : class
            => obj is not null && await condition(obj) ? await succeed(obj) : default;

        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, Func<T, bool> condition, Func<T, Task<TResult>> succeed, TResult fail) where T : class
            => obj is not null && condition(obj) ? await succeed(obj) : fail;

        public static async Task<TResult?> ConditionalCheckAsync<T, TResult>(this T obj, bool condition, Func<T, Task<TResult>> succeed, TResult fail) where T : class
            => obj is not null && condition ? await succeed(obj) : fail;


        /// <summary>
        /// 方法C
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <param name="fail">失败时的操作</param>
        /// <returns>succeed(obj) Or fail(obj)</returns>
        /// <exception cref="Exception">若检查对象为null，则会产生报错</exception>
        public static TResult ConditionalCheck<T, TResult>(this T obj, Func<T, bool> condition, Func<T, TResult> succeed, Func<T, TResult> fail) where T : class
            => obj is not null ? (condition(obj) ? succeed(obj) : fail(obj)) : throw new Exception("obj is null,Cannot execute.");

        /// <summary>
        /// 方法C 的变体，在失败时返回一个自定义值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">自定义值</typeparam>
        /// <param name="obj">检查对象</param>
        /// <param name="condition">检查条件</param>
        /// <param name="succeed">成功时的操作</param>
        /// <param name="fail">失败的值</param>
        /// <returns>succeed(obj) Or fail</returns>
        public static TResult ConditionalCheck<T, TResult>(this T? obj, Func<T?, bool> condition, Func<T?, TResult> succeed, TResult fail) where T : class
        => condition(obj) ? succeed(obj) : fail;

        //不返回值,但可能会修改obj
        public static void ConditionalCheck<T>(this T obj, Func<T, bool> condition, Action<T> succeed) where T : class
        { if (obj is not null && condition(obj)) succeed(obj); }
        //不返回值，但可能会修改obj
        public static void ConditionalCheck<T>(this T obj, Func<T, bool> condition, Action<T> succeed, Action<T> fail) where T : class
        { if (obj is not null) { if (condition(obj)) succeed(obj); else fail(obj); } else throw new Exception("obj is null,Cannot execute."); }
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
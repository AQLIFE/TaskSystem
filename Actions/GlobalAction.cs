using System.Security.Cryptography;
using System.Text;

namespace TaskManangerSystem.Actions
{
    [Obsolete("Comon该加密可信度偏低")]
    public static class Comon
    {
        public static string GetMD5(string myString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(myString);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = "";
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }

    [Obsolete("尚未开发完成")]
    public class ActionTypeExtension : Exception
    {
        public string message { set; get; }

        public ActionTypeExtension(string str)
        {
            this.message = str;
        }
    }

    [Obsolete]
    partial class ExampleHandler(ILogger<ExampleHandler> logger)
    {
        public string HandleRequest()
        {
            LogHandleRequest(logger);
            return "Hello World";
        }

        [LoggerMessage(LogLevel.Information, "ExampleHandler.HandleRequest was called")]
        public static partial void LogHandleRequest(ILogger logger);
    }


    public  class GlobalActions
    {
        public static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert the input string to a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Compute the hash value
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
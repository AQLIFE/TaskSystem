using TaskManangerSystem.IServices;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Crypto;
using TaskManangerSystem.Services.Info;

namespace TaskManangerSystem.Services.Tool
{
    public static class StringExtensions
    {
        public static int? ToInt32(this string? value)
        {
            if (int.TryParse(value, out int result)) return result;
            else return null;
        }
    }

    public static class SystemInfo
    {

        public const int TRASH = 100;
        public const int EMPLOYEE = 101;
        public const int CATEGORY = 102;
        public const int CUSTOMER = 103;
        public const int TASKAFFAIR = 104;

        public const int DefaultRole = 1;//基础权限
        public const int AdminRole = 90;//完整权限级别
        public const int PageSize = 100;//单页最大数量


        public static Employee[] admin => [
            new("admin", "admin@123".ComputeSHA512Hash(), AdminRole + 9),
            new("tester","test1234".ComputeSHA512Hash(),DefaultRole)
            ];

        public static Category[] categories => [
            new("分类垃圾桶",TRASH,"用于存放标记为删除的分类信息"),
            new("账户分类",EMPLOYEE,"账户系统备用"),
            new("库存分类",  CATEGORY,"用于对产品进行分类"),
            new("客户分类",  CUSTOMER,"用于对客户进行分类"),
            new("任务分类",  TASKAFFAIR,"用于对任务进行分类")
        ];

        public static Guid customerId = categories.Where(e => e.SortSerial == CUSTOMER).First().CategoryId;


        public static Category[] customer => [
            new("特级客户", CUSTOMER+3,level:2,parId:customerId),
            new("普通客户", CUSTOMER+2,level:2,parId:customerId),
            new("机密客户", CUSTOMER+4,level:2,parId:customerId)
        ];
        
        public static Customer[] customers => [
            new("管理员", customerId, DefaultRole, "10241024", "King"),
            new("测试技员",customerId,DefaultRole,"5125121512","Product")
            ];


        // public readonly static string DBLINK = Environment.GetEnvironmentVariable("DB_LINK") ?? throw new Exception("Program Error:Miss DB_LINK");
        private readonly static string DB_NAME = Environment.GetEnvironmentVariable("DB_NAME") ?? throw new Exception(ErrorMessage.MISS_DBNAME);
        private readonly static string DB_HOST_NAME = Environment.GetEnvironmentVariable("DB_HOST_NAME") ?? throw new Exception(ErrorMessage.MISS_DBHOST);
        private readonly static string DB_HOST_PASS = Environment.GetEnvironmentVariable("DB_HOST_PASS") ?? throw new Exception(ErrorMessage.MISS_DBPASS);
        private readonly static string DB_PART_NAME = Environment.GetEnvironmentVariable("DB_PART_NAME") ?? throw new Exception(ErrorMessage.MISS_DBPART);

        public static string DB_LINK => $"server={DB_HOST_NAME};port=3306;database={DB_NAME};user={DB_PART_NAME};password={DB_HOST_PASS};";

        public readonly static string ISSUER = Environment.GetEnvironmentVariable("ISSUER") ?? throw new Exception(ErrorMessage.MISS_JWT_ISSUER);
        public readonly static string AUDIENCE = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new Exception(ErrorMessage.MISS_JWT_AUDIENCE);
        public readonly static string SECURITYKEY = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception(ErrorMessage.MISS_JWT_KEY);
        public readonly static string CERTPATH = Environment.GetEnvironmentVariable("RSA_CERT_PATH") ?? throw new Exception(ErrorMessage.MISS_JWT_RSA_CERT);
        // 假设私钥存储在环境变量中，需要根据实际情况调整

    }
}
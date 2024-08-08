namespace TaskManangerSystem.Services.Info
{
    public static class ErrorMessage
    {
        public static readonly string UnknownOrError = "登录校验不通过或者账号不存在";
        public static readonly string NOT_NULL = "输入不能为空或含有空格";

        public static readonly string NON_COMPLIACE = "不符内容规范";

        public static readonly string TEXT_OVERLENGTH = "超长文本";

        public static readonly string URDER_TEXT = "文本量不足";

        public static readonly string DATABASE_DISCONNECTION = "数据库失联，系统无法服务";

        public static readonly string UNKNOWN_ERROR = "未知错误，请稍后再试";


        #region Path_env Error message
        private static readonly string MISS_ENV = "Program Error:Misssing ";
        public static readonly string MISS_DBNAME = MISS_ENV + "DB_NAME";
        public static readonly string MISS_DBHOST = MISS_ENV + "DB_HOST_NAME";
        public static readonly string MISS_DBPASS = MISS_ENV + "DB_HOST_PASS";
        public static readonly string MISS_DBPART = MISS_ENV + "DB_PART_NAME";
        public static readonly string MISS_JWT_ISSUER = MISS_ENV + "ISSUER";
        public static readonly string MISS_JWT_AUDIENCE = MISS_ENV + "AUDIENCE";
        public static readonly string MISS_JWT_KEY = MISS_ENV + "KEY";
        public static readonly string MISS_JWT_RSA_CERT = MISS_ENV + "RSA_CERT";
        #endregion
    }
}

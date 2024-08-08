namespace TaskManangerSystem.Services.Info
{
    public static class ErrorMessage
    {
        public const string UnFindOrError = "登录校验不通过或者账号不存在";

        #region modelState
        public const string NotNull = "输入不能为空";
        public const string FailData = "数据验证失败";
        #endregion
        
        public const string UnknowError = "未知错误，请稍后再试";


        #region Path_env Error message
        private const string MISS_ENV = "Program Error:Misssing ";
        public const string MISS_DBNAME = MISS_ENV + "DB_NAME";
        public const string MISS_DBHOST = MISS_ENV + "DB_HOST_NAME";
        public const string MISS_DBPASS = MISS_ENV + "DB_HOST_PASS";
        public const string MISS_DBPART = MISS_ENV + "DB_PART_NAME";
        public const string MISS_JWT_ISSUER = MISS_ENV + "ISSUER";
        public const string MISS_JWT_AUDIENCE = MISS_ENV + "AUDIENCE";
        public const string MISS_JWT_KEY = MISS_ENV + "KEY";
        public const string MISS_JWT_RSA_CERT = MISS_ENV + "RSA_CERT";
        #endregion

        #region BaseException
        public const string FeedBack = "WEB API 错误，请稍后再试";
        public const string ConsoleExceptionInfo = "专用于控制台错误信息";
        #endregion

        #region db link
        public const string DBUnLink = "数据库未连接或连接失败";
        #endregion

        #region Employee add
        public const string IdMessage = "账户ID必须是普通字符[数字|小写字母]{5-16位}";

        public const string PWDMessage = "账户密码必须是普通字符[数字|字母]{8-128位}";

        #endregion

        #region Auth
        public const string NoPermission = "权限不足";
        #endregion
    }
}

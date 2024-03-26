namespace TaskManangerSystem.Services
{
    public class LogInfo<T>
    {
        public bool status { set; get; }

        public string? name { set; get; }

        public string funcName { set; get; } =string.Empty;

        public T? invalidParameterValue { set; get; }

        public LogInfo() { }
        public LogInfo(bool status, string? name, string funcName, T? ParameterValue)
        {
            this.status = status;
            this.name = name;
            this.funcName = funcName;
            this.invalidParameterValue = ParameterValue;
        }

        public LogInfo(bool status, string? name, string funcName)
        {
            this.status = status;
            this.name = name;
            this.funcName = funcName;
        }

        public string RequestInformation=>$"访问状态：{(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "未知用户"},请求方法:{funcName}";

        public string RespenseInfomation =>$"API Status: {(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "未知用户"},请求方法:{funcName},返回结果：{invalidParameterValue}";
    }


}
namespace TaskManangerSystem.Services
{
    public class LogInfo<T, U>
    {
        public bool status { set; get; }

        public string name { set; get; }

        public string funcName { set; get; }

        public T invalidParameterValue { set; get; }
        public U parameterValue { set; get; }

        public string RequestInformation
        {
            get
            {
                return $"访问状态：{(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "未知用户"},请求方法:{funcName},请求参数：{invalidParameterValue},还原参数：{parameterValue}";
            }
        }

        public string RespenseInfomation { get { return $"Api状态：{(status ? "succeed" : "fail")},时间：{DateTime.Now},请求用户:{name ?? "未知用户"},请求方法:{funcName},请求参数:{invalidParameterValue},返回结果：{parameterValue}";}}
    }


}
namespace TaskManangerSystem.IServices
{
    public interface IExceptionContext : IExceptionMessage
    {
        public string ExceptionSource { set; get; }//目标位置
        //public string ExceptionMessage  { set; get; }//错误信息
        public string ExceptionFeedback { set; get; }//友好反馈信息
        public DateTime TriggerTime { set; get; }//产生时间
    }

    public interface IExceptionMessage
    {
        public string ExceptionMessage { set; get; }//错误信息
    }
}

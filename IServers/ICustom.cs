namespace TaskManangerSystem.IServer
{

    public interface ICustom
    {
        public string CreateToken(string name,string role="default");
    }
}


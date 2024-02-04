namespace TaskManangerSystem.IServices.SystemServices
{

    public interface ICustom
    {
        public string CreateToken(string name,string role="default");
    }
}


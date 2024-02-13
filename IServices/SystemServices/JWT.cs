namespace TaskManangerSystem.IServices.SystemServices
{
    public class TokenOption
    {
        public string Audience { get;private  set; }

        public string Issuer { get;private  set; }

        public string SecurityKey { get;private  set; }

        public TokenOption()
        {
            this.SecurityKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("Program Error:Missing Key");
            this.Issuer = Environment.GetEnvironmentVariable("ISSUER") ?? throw new Exception("Program Error:Misssing Issuer");
            this.Audience = Environment.GetEnvironmentVariable("AUDIENCE") ?? throw new Exception("Program Error:Misssing Audience");
        }
    }
}

using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.IServices.SystemServices
{
    public interface IBearer
    {
        public string CreateToken(EncryptAccount encrypt);
    }

    public abstract class BearerBase : TokenOption
    {
        public byte[] secretByte
        {
            get
            {
                return Encoding.UTF8.GetBytes(SecurityKey);
            }
        }
        public SecurityKey key
        {
            get
            {
                return new SymmetricSecurityKey(secretByte);
            }
        }//加密密钥

        public SigningCredentials signing
        {
            get
            {
                return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
        }//加密后密钥
    }

}
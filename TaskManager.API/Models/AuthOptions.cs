using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.API.Models
{
    public class AuthOptions
    {
        // TODO: вынести ключи в secrets.json (Manage Users Secrets) 
        public const string ISSUER = "MyAuthServer";        // издатель токена
        public const string AUDIENCE = "MyAuthClient";      // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 10;                      // время жизни токена - 10 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}

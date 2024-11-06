using Infrastructure.Interfaces;
using bcrypt = BCrypt.Net.BCrypt;

namespace Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public bool Check(string hash, string password)
        {
            // Se desencripta password del usuario
            return bcrypt.Verify(password, hash);
        }

        public string Hash(string password)
        {
            // Se encripta password del usuario
            return bcrypt.HashPassword(password);
        }
    }
}

using UserServiceMS.Models;

namespace UserServiceMS.Helpers
{
    public interface IHelper
    {
        string GenerateToken(User user);
    }
}
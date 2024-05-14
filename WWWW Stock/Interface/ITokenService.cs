using WWWW_Stock.Models;

namespace WWWW_Stock.Interface
{
    public interface ITokenService
    {
        string  CreateToken(AppUser user);
    }
}

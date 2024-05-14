using Microsoft.AspNetCore.Identity;

namespace WWWW_Stock.Models
{
    public class AppUser:IdentityUser
    {
        public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();

    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace Users.Models
{
    public class AppUser : IdentityUser
    {
        //plus db update or migration
        public DateTime? PremiumExpiring { get; set; }
    }
}

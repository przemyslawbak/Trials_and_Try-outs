using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Infrastructure
{
    public class CustomUserValidator : UserValidator<AppUser>
    {
        public override async Task<IdentityResult> ValidateAsync(
   UserManager<AppUser> manager,
   AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() : result.Errors.ToList();
            // if (!user.Email.ToLower().EndsWith("@example.com"))
            // {
            // errors.Add(new IdentityError {
            // Code = "EmailDomainError",
            // Description = "Dozwolone jest użycie adresu e-mail tylko w domenie
            // example.com."
            // });
            //}
            return errors.Count == 0 ? IdentityResult.Success
            : IdentityResult.Failed(errors.ToArray());
        }
    }
}

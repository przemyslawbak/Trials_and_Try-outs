using Commerce.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _03_DI_Example_WebService
{
    public class AspNetUserContextAdapter : IUserContext
    {
        private static readonly HttpContextAccessor Accessor = new HttpContextAccessor();

        public bool IsInRole(Role role)
        {
            return Accessor.HttpContext.User.IsInRole(role.ToString());
        }
    }
}

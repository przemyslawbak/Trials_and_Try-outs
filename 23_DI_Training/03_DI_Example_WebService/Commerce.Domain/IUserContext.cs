using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Domain
{
    public interface IUserContext
    {
        bool IsInRole(Role role);
    }
}

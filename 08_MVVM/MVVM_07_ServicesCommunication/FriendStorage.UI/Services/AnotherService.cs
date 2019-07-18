using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.Services
{
    public interface IAnotherService
    {
        string ProcessMyString(string anotherString, string addMe);
    }
    public class AnotherService : IAnotherService
    {
        public string ProcessMyString(string anotherString, string addMe)
        {
            return addMe + anotherString;
        }
    }
}

using FriendStorage.UI.ViewModel;
using System.Windows;

namespace FriendStorage.UI.Services
{
    public interface ISomeService
    {
        string GetTheName(IPersonService personService);
    }
    public class SomeService : ISomeService
    {
        private IAnotherService _anotherService;
        public SomeService(IAnotherService anotherService)
        {
            _anotherService = anotherService;
        }
        public string GetTheName(IPersonService personService)
        {
            string dupa = _anotherService.ProcessMyString(personService.Name, "My name is: ");
            return dupa;
        }
    }
}

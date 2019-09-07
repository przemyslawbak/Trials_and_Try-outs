using Wpf_Models;

namespace Wpf_Services.Object
{
    public class SecondNameProvider : ISecondNameProvider
    {
        public string SecondName { get; set; }

        public SecondNameProvider()
        {
            SecondName = "Bak";
        }
    }
}

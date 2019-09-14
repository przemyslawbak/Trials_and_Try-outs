namespace Wpf_Services.Object
{
    public class ObjectModel : IObjectModel
    {
        public string FullName { get; set; }

        public ObjectModel(string firstName, ISomeService service)
        {
            FullName = firstName + ", " + service.SecondName;
        }
    }
}

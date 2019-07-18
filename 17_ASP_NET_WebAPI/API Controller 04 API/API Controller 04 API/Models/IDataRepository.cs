namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Interface for the controller
    /// </summary>
    public interface IDataRepository
    {
        void AddItem(RequestJSONModel request);
    }
}

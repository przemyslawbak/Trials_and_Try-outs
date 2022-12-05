using System.Collections.Generic;

namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Interface for the controller
    /// </summary>
    public interface ISaveFilesRepository
    {
        IEnumerable<RequestJSONModel> GetRequests { get; }
    }
}

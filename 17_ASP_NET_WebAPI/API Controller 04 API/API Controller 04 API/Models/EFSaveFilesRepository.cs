using System.Collections.Generic;
using System.Linq;

namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Repository for the controller
    /// </summary>
    public class EFSaveFilesRepository : ISaveFilesRepository
    {
        private ApplicationDbContext _context;
        public EFSaveFilesRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<RequestJSONModel> GetRequests => _context.Requests.ToList();
    }
}

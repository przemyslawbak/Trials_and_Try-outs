namespace API_Controller_04_API.Models
{
    /// <summary>
    /// Repository for the controller
    /// </summary>
    public class EFDataRepository : IDataRepository
    {
        private ApplicationDbContext _context;
        public EFDataRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public void AddItem(RequestJSONModel request)
        {
            RequestJSONModel dataItem = new RequestJSONModel
            {
                Index = request.Index,
                Name = request.Name,
                Visits = request.Visits,
                Date = request.Date
            };
            _context.Requests.Add(dataItem);
            _context.SaveChanges();
        }
    }
}

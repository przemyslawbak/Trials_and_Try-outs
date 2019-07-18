using Microsoft.EntityFrameworkCore;

namespace API_Controller_04_API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>
            options) : base(options) { }
        public DbSet<RequestJSONModel> Requests { get; set; }
    }
}

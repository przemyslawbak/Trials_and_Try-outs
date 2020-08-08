using Microsoft.EntityFrameworkCore;

namespace Core_Angular_SignalR.Presistance
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        public DbSet<SignalDataModel> Signals { get; set; }
    }
}

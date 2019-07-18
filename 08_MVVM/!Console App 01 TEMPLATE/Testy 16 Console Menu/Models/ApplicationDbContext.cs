using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Testy_16_Console_Menu.Models
{
    public class ApplicationDbContext : DbContext //DB context settings
    {
        public DbSet<DatabaseModel> DataModels { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=dbConsoleApp; Trusted_Connection=True; MultipleActiveResultSets=true");
        }
    }
}

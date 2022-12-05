using Financial.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial.DAL
{
    public class SampleDbContext : DbContext
    {

        public SampleDbContext(DbContextOptions<SampleDbContext>
            options) : base(options) { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Literature> Literatures { get; set; }
        public DbSet<LiteratureTechnology> LiteraturesTech { get; set; }
        public DbSet<TechnologyProject> TechProjects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LiteratureTechnology>()
                .HasKey(c => new { c.LiteratureID, c.TechnologyID });

            modelBuilder.Entity<TechnologyProject>()
                .HasKey(c => new { c.TechnologyID, c.ProjectID });
        }
    }
}

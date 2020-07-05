using Financial.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Financial.DAL
{
    public class EFSampleRepository : ISampleRepository
    {
        private readonly SampleDbContext _context;
        public EFSampleRepository(SampleDbContext context)
        {
            _context = context;
        }

        public List<PickedUp> GetViewModels()
        {
            return (from q in _context.Projects.Include(i => i.TechnologiesProjects).ThenInclude(techproj => techproj.Technology)
                    select new PickedUp
                    {
                        ProjectID = q.ProjectID,
                        Checked = false,
                        Name = q.Name,
                        Comment = q.Comments,
                        Techs = string.Join(", ", q.TechnologiesProjects.Select(sn => sn.Technology.Name).ToArray())
                    }).ToList();
        }

        public int GetPageCount(int itemsPerPage)
        {
            return (_context.Projects.Count() + itemsPerPage - 1) / itemsPerPage;
        }

        public ObservableCollection<PickedUp> GetResults(int itemsPerPage, int currentPage)
        {
            int skip = (currentPage - 1) * itemsPerPage;

            var query = from q in _context.Projects.Include(project => project.TechnologiesProjects).ThenInclude(techproj => techproj.Technology).Skip(skip).Take(itemsPerPage)
                        select new PickedUp
                        {
                            ProjectID = q.ProjectID,
                            Checked = false,
                            Name = q.Name,
                            Comment = q.Comments,
                        };

            return new ObservableCollection<PickedUp>(query);
        }
    }
}

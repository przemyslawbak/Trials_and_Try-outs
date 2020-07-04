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

        public List<PickedViewModel> GetViewModels()
        {
            return (from q in _context.Projects.Include(i => i.TechnologiesProjects)
                   select new PickedViewModel
                   {
                       ProjectID = q.ProjectID,
                       Checked = false,
                       Name = q.Name
                   }).ToList();
        }

        public int GetPageCount(int itemsPerPage)
        {
            return (_context.Projects.Count() + itemsPerPage - 1) / itemsPerPage;
        }

        public ObservableCollection<PickedViewModel> GetResults(int itemsPerPage, int currentPage)
        {
            int skip = (currentPage - 1) * itemsPerPage;

            var query = from q in _context.Projects.Include(i => i.TechnologiesProjects).Skip(skip).Take(itemsPerPage)
                        select new PickedViewModel
                        {
                            ProjectID = q.ProjectID,
                            Checked = false,
                            Name = q.Name
                        };

            return new ObservableCollection<PickedViewModel>(query);
        }
    }
}

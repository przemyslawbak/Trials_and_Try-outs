using Financial.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Financial.DAL
{
    public interface ISampleRepository
    {
        int GetPageCount(int itemsPerPage);
        ObservableCollection<PickedViewModel> GetResults(int itemsPerPage, int currentPage);
        List<PickedViewModel> GetViewModels();
    }
}
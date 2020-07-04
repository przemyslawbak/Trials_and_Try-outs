using Financial.Commands;
using Financial.DAL;
using Financial.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ISampleRepository _repo;
        private readonly int _itemsPerPage = 2;
        private List<PickedViewModel> _all;

        public MainViewModel(ISampleRepository repo)
        {
            //PageCount = _repo.GetPageCount(_itemsPerPage); //paging repo
            //Display = _repo.GetResults(_itemsPerPage,  CurrentPage);

            _repo = repo;
            CurrentPage = 1;
            _all = _repo.GetViewModels(); //?

            PageCount = GetPagesCount();
            Display = GetResults();

            NextClickCommand = new DelegateCommand(OnNextClick);
            PrevClickCommand = new DelegateCommand(OnPrevClick);
        }

        private int GetPagesCount()
        {
            return (_all.Count() + _itemsPerPage - 1) / _itemsPerPage;
        }

        private ObservableCollection<PickedViewModel> GetResults()
        {
            int skip = (CurrentPage - 1) * _itemsPerPage;

            return new ObservableCollection<PickedViewModel>(_all.Skip(skip).Take(_itemsPerPage));
        }

        public int PageCount { get; set; }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<PickedViewModel> _display;
        public ObservableCollection<PickedViewModel> Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        public ICommand NextClickCommand { get; private set; }
        public ICommand PrevClickCommand { get; private set; }

        private void OnNextClick(object obj)
        {
            if (CurrentPage < PageCount)
            {
                CurrentPage++;
                Display = GetResults();
            }
        }

        private void OnPrevClick(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                Display = GetResults();
            }
        }
    }

    public interface IPagingCollectionView
    {
        void MoveToNextPage(int currPage);
        void MoveToPreviousPage(int currPage);
    }
}

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
        private List<PickedUp> _all;
        private List<PickedUp> _displayCollection;

        public MainViewModel(ISampleRepository repo)
        {
            //PageCount = _repo.GetPageCount(_itemsPerPage); //paging repo
            //Display = _repo.GetResults(_itemsPerPage,  CurrentPage); //paging repo

            _repo = repo;
            CurrentPage = 1;
            _all = _repo.GetViewModels();
            _displayCollection = _all;
            PageCount = GetPagesCount();
            Display = GetResults(); //service

            NextClickCommand = new DelegateCommand(OnNextClick);
            PrevClickCommand = new DelegateCommand(OnPrevClick);
            FinishClickCommand = new DelegateCommand(OnFinishClick);
        }

        private string _filterTechPhrase;
        public string FilterTechPhrase
        {
            get => _filterTechPhrase;
            set
            {
                _filterTechPhrase = value;
                OnPropertyChanged();
                FilterDisplayCollection(); //service
            }
        }

        private int _pageCount;
        public int PageCount
        {
            get => _pageCount;
            set
            {
                _pageCount = value;
                OnPropertyChanged();
            }
        }

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

        private ObservableCollection<PickedUp> _display;
        public ObservableCollection<PickedUp> Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        private void FilterDisplayCollection()
        {
            CurrentPage = 1;
            if (!string.IsNullOrEmpty(FilterTechPhrase))
            {
                _displayCollection = _all.Where(a => a.Techs.ToLower().Contains(FilterTechPhrase.ToLower())).ToList();
            }
            else
            {
                _displayCollection = _all;
            }

            Display = GetResults(); //service
        }

        private int GetPagesCount()
        {
            return (_displayCollection.Count() + _itemsPerPage - 1) / _itemsPerPage;
        }

        private ObservableCollection<PickedUp> GetResults() //service
        {
            PageCount = GetPagesCount();
            int skip = (CurrentPage - 1) * _itemsPerPage;

            return new ObservableCollection<PickedUp>(_displayCollection.Skip(skip).Take(_itemsPerPage));
        }

        public ICommand NextClickCommand { get; private set; }
        public ICommand PrevClickCommand { get; private set; }
        public ICommand FinishClickCommand { get; private set; }

        private void OnNextClick(object obj)
        {
            if (CurrentPage < PageCount)
            {
                CurrentPage++;
                Display = GetResults(); //service
            }
        }

        private void OnPrevClick(object obj)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                Display = GetResults(); //service
            }
        }

        private void OnFinishClick(object obj)
        {
            List<Project> projects = _repo.GetCheckedProjects(_displayCollection.Where(item => item.Checked == true).ToList());
        }
    }
}

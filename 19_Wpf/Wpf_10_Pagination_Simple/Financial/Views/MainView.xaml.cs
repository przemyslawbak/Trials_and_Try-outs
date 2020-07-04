using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Financial.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private readonly PagingCollectionView _cview;

        public MainView()
        {
            InitializeComponent();
            _cview = new PagingCollectionView(
                new List<object>
                {
                    new { Animal = "One", Eats = "Tiger" },
                    new { Animal = "Two", Eats =  "Bear" },
                    new { Animal = "Three", Eats = "Oh my" },
                    new { Animal = "Four", Eats = "Oh my isn't an animal" },
                    new { Animal = "Five", Eats = "Who is counting anyway" },
                    new { Animal = "Six", Eats = "Oh my isn't an animal" },
                    new { Animal = "Seven", Eats = "Who is counting anyway" },
                    new { Animal = "Eight", Eats = "Oh my isn't an animal" },
                    new { Animal = "Nine", Eats = "Who is counting anyway" },
                    new { Animal = "Ten", Eats = "Oh my isn't an animal" },
                    new { Animal = "Eleven", Eats = "Who is counting anyway" },
                    new { Animal = "Twelve", Eats = "Who is counting anyway" },
                    new { Animal = "Thirteen", Eats = "For posting on stackoverflow" }
                },
                4
            );
            DataContext = _cview;
        }

        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            _cview.MoveToNextPage();
        }

        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            _cview.MoveToPreviousPage();
        }
    }

    public class PagingCollectionView : ListCollectionView
    {
        private readonly IList _innerList;
        private int _currentPage = 1;

        public PagingCollectionView(IList innerList, int itemsPerPage) : base(innerList)
        {
            _innerList = innerList;
            ItemsPerPage = itemsPerPage;
        }

        public override int Count
        {
            get
            {
                if (_innerList.Count == 0) return 0;
                if (_currentPage < PageCount) // page 1..n-1
                {
                    return ItemsPerPage;
                }
                else // page n
                {
                    var itemsLeft = _innerList.Count % ItemsPerPage;
                    if (0 == itemsLeft)
                    {
                        return ItemsPerPage; // exactly itemsPerPage left
                    }
                    else
                    {
                        // return the remaining items
                        return itemsLeft;
                    }
                }
            }
        }

        public int CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
            }
        }

        public int ItemsPerPage { get; }

        public int PageCount
        {
            get
            {
                return (_innerList.Count + ItemsPerPage - 1) / ItemsPerPage;
            }
        }

        private int EndIndex
        {
            get
            {
                var end = _currentPage * ItemsPerPage - 1;
                return (end > _innerList.Count) ? _innerList.Count : end;
            }
        }

        private int StartIndex
        {
            get { return (_currentPage - 1) * ItemsPerPage; }
        }

        public override object GetItemAt(int index)
        {
            var offset = index % (ItemsPerPage);
            return _innerList[StartIndex + offset];
        }

        public void MoveToNextPage()
        {
            if (_currentPage < PageCount)
            {
                CurrentPage += 1;
            }
            Refresh();
        }

        public void MoveToPreviousPage()
        {
            if (_currentPage > 1)
            {
                CurrentPage -= 1;
            }
            Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamGroups
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new ObservableCollection<GroupingObservableCollection<string, Character>>(
                Character.Characters
                    .OrderBy(c => c.Name)
                    .GroupBy(c => c.Name[0].ToString(), c => c)
                    .Select(g =>
                        new GroupingObservableCollection<string, Character>(g.Key, g)));
        }
    }
}

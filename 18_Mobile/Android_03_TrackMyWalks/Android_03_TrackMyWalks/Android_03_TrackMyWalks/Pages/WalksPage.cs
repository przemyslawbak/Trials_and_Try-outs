using Android_03_TrackMyWalks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Android_03_TrackMyWalks.Pages
{
    public class WalksPage : ContentPage
    {
        public WalksPage()
        {
            var newWalkItem = new ToolbarItem
            {
                Text = "Dodaj szlak"
            };
            newWalkItem.Clicked += (sender, e) =>
            {
                Navigation.PushAsync(new WalkEntryPage());
            };
            ToolbarItems.Add(newWalkItem);
            var walkItems = new List<WalkEntries>
            {
                new WalkEntries {
                Title = "10-milowy szlak wzdłuż strumienia, Margaret River",
                Notes = "10-milowy szlak wzdłuż strumienia zaczyna się w Rotary Park w pobliżu Old Kate, czyli starej lokomotywy stojącej w północnej części Margaret River.",
                Latitude = -33.9727604,
                Longitude = 115.0861599,
                Kilometers = 7.5,
                Distance = 0,
                Difficulty = "Średni",
                ImageUrl = "http://trailswa.com.au/media/cache/media/images/trails/_mid/FullSizeRender1_600_480_c1.jpg"
                },
                new WalkEntries {
                Title = "Szlak Ancient Empire, Dolina Gigantów",
                Notes = "Ancient Empire to 450-metrowy szlak pośród gigantycznych drzew, wśród których znajdują się popularne sękate olbrzymy zwane Grandma Tingle.",
                Latitude = -34.9749188,
                Longitude = 117.3560796,
                Kilometers = 450,
                Distance = 0,
                Difficulty = "Wysoki",
                ImageUrl = "http://trailswa.com.au/media/cache/media/images/trails/_mid/Ancient_Empire_534_480_c1.jpg"
                },
            };
            var itemTemplate = new DataTemplate(typeof(ImageCell));
            itemTemplate.SetBinding(TextCell.TextProperty, "Title");
            itemTemplate.SetBinding(TextCell.DetailProperty, "Notes");
            itemTemplate.SetBinding(ImageCell.ImageSourceProperty, "ImageUrl");
            var walksList = new ListView
            {
                HasUnevenRows = true,
                ItemTemplate = itemTemplate,
                ItemsSource = walkItems,
                SeparatorColor = Color.FromHex("#ddd"),
            };
            // konfiguracja procedury obsługi zdarzeń
            walksList.ItemTapped += (object sender, ItemTappedEventArgs e) =>
            {
                var item = (WalkEntries)e.Item;
                if (item == null) return;
                Navigation.PushAsync(new WalkTrailPage(item));
                item = null;
            };
            Content = walksList;
        }
    }
}
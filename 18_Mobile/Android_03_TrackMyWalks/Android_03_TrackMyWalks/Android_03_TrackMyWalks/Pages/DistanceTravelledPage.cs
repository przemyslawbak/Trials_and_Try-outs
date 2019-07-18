using Android_03_TrackMyWalks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Android_03_TrackMyWalks.Pages
{
    public class DistanceTravelledPage : ContentPage
    {
        public DistanceTravelledPage(WalkEntries walkItem)
        {
            Title = "Przebyty dystans";
            // utworzenie egzemplarza obiektu mapy
            var trailMap = new Map();
            // wbicie szpilki w mapę dla wybranego szlaku
            trailMap.Pins.Add(new Pin
            {
                Type = PinType.Place,
                Label = walkItem.Title,
                Position = new Position(walkItem.Latitude, walkItem.Longitude)
            });
            // wyśrodkowanie mapy na początku szlaku
            trailMap.MoveToRegion(MapSpan.FromCenterAndRadius(new
            Position(walkItem.Latitude, walkItem.Longitude),
            Distance.FromKilometers(1.0)));
            var trailNameLabel = new Label()
            {
                FontSize = 18,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                Text = walkItem.Title
            };
            var trailDistanceTravelledLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.Black,
                Text = "Przebyty dystans:",
                HorizontalTextAlignment = TextAlignment.Center
            };
            var totalDistanceTaken = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.Black,
                Text = $"{ walkItem.Distance } km",
                HorizontalTextAlignment = TextAlignment.Center
            };
            var totalTimeTakenLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.Black,
                Text = "Czas:",
                HorizontalTextAlignment = TextAlignment.Center
            };
            var totalTimeTaken = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.Black,
                Text = "0 h 0 m 0 s",
                HorizontalTextAlignment = TextAlignment.Center
                };
            var walksHomeButton = new Button
            {
                BackgroundColor = Color.FromHex("#008080"),
                TextColor = Color.White,
                Text = "Zakończ ten szlak"
            };
            // definicja procedury obsługi zdarzeń
            walksHomeButton.Clicked += (sender, e) =>
            {
                if (walkItem == null) return;
                Navigation.PopToRootAsync(true);
                walkItem = null;
            };
            this.Content = new ScrollView
            {
                Padding = 10,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        trailMap,
                        trailNameLabel,
                        trailDistanceTravelledLabel,
                        totalDistanceTaken,
                        totalTimeTakenLabel,
                        totalTimeTaken,
                        walksHomeButton
                    }

                }
            };
        }
    }
}
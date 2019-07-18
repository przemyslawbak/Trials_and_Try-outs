using Android_03_TrackMyWalks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Android_03_TrackMyWalks.Pages
{
    public class WalkTrailPage : ContentPage
    {
        public WalkTrailPage(WalkEntries walkItem)
        {
            Title = "Szlak";
            var beginTrailWalk = new Button
            {
                BackgroundColor = Color.FromHex("#008080"),
                TextColor = Color.White,
                Text = "Rozpocznij ten szlak"
            };
            // definicja procedury obsługi zdarzeń
            beginTrailWalk.Clicked += (sender, e) =>
            {
                if (walkItem == null) return;
                Navigation.PushAsync(new DistanceTravelledPage(walkItem));
                Navigation.RemovePage(this);
                walkItem = null;
            };
            var walkTrailImage = new Image()
            {
                Aspect = Aspect.AspectFill,
                Source = walkItem.ImageUrl
            };
            var trailNameLabel = new Label()
            {
                FontSize = 28,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Black,
                Text = walkItem.Title
            };
            var trailKilometersLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.Black,
                Text = $"Długość: { walkItem.Kilometers } km"
            };
            var trailDifficultyLabel = new Label()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.Black,
                Text = $"Poziom trudności: { walkItem.Difficulty } "
            };
            var trailFullDescription = new Label()
            {
                FontSize = 11,
                TextColor = Color.Black,
                Text = $"{ walkItem.Notes }",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            this.Content = new ScrollView
            {
                Padding = 10,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                    walkTrailImage,
                    trailNameLabel,
                    trailKilometersLabel,
                    trailDifficultyLabel,
                    trailFullDescription,
                    beginTrailWalk
                    }
                }
            };
        }
    }
}
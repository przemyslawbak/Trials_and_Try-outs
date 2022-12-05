using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CommonPlatform.CustomPages
{
    public class MapPage : ContentPage
    {
        private Map map;
        private StackLayout stackLayout;
        public MapPage()
        {
            MapSpan span = MapSpan.FromCenterAndRadius(
    new Position(50.289192, 18.659656),
    Distance.FromMiles(0.4));
            map = new Map(span)
            {
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            stackLayout = new StackLayout
            {
                Spacing = 0,
                Children = {
                        map
                        }
            };
            Content = stackLayout;
        }
    }
}

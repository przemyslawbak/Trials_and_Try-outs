using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Android_03_TrackMyWalks.Pages
{
	public class WalkEntryPage : ContentPage
	{
		public WalkEntryPage ()
		{
            // tytuł strony
            Title = "Nowy wpis";
            // pola do wypełnienia
            var walkTitle = new EntryCell
            {
                Label = "Tytuł:",
                Placeholder = "Nazwa szlaku"
            };
            var walkNotes = new EntryCell
            {
                Label = "Uwagi:",
                Placeholder = "Opis"
            };
            var walkLatitude = new EntryCell
            {
                Label = "Szerokość geograficzna:",
                Placeholder = "Szerokość",
                Keyboard = Keyboard.Numeric
            };
            var walkLongitude = new EntryCell
            {
                Label = "Długość geograficzna:",
                Placeholder = "Długość",
                Keyboard = Keyboard.Numeric
            };
            var walkKilometers = new EntryCell
            {
                Label = "Liczba kilometrów:",
                Placeholder = "Liczba kilometrów",
                Keyboard = Keyboard.Numeric
            };
            var walkDifficulty = new EntryCell
            {
                Label = "Poziom trudności:",
                Placeholder = "Poziom trudności szlaku"
            };
            var walkImageUrl = new EntryCell
            {
                Label = "URL obrazu:",
                Placeholder = "URL obrazu"
            };
            // definicja widoku TableView
            Content = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                new TableSection()
                    {
                    walkTitle,
                    walkNotes,
                    walkLatitude,
                    walkLongitude,
                    walkKilometers,
                    walkDifficulty,
                    walkImageUrl
                    }
                }
            };
            var saveWalkItem = new ToolbarItem
            {
                Text = "Zapisz"
            };
            saveWalkItem.Clicked += (sender, e) =>
            {
                Navigation.PopToRootAsync(true);
            };
            ToolbarItems.Add(saveWalkItem);
        }
	}
}
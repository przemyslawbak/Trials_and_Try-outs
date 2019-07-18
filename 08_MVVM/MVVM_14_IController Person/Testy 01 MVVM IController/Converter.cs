using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Testy_01_MVVM_IController
{
    public class HeightListToString : IValueConverter //do ANALIZY!!!!
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Model.HeightList heightList = (Model.HeightList)value;
            return Model.Person.NewHeight(heightList);
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string height = (value as string).ToLower();
            return Model.Person.ParseNewHeight(height);
        }
    }

    public class PersonConverter : IMultiValueConverter //do ANALIZY!!!!
    {

        HeightListToString pzts = new HeightListToString();
        public object Convert(object[] values, Type targetType, object parameter,
        CultureInfo culture)
        {
            int iloscRekordow = ViewModel.Persons.PersonsList.Count;
            int licznik;
            string id = (string)values[0];


            if (iloscRekordow != 0)
            {

                licznik = --iloscRekordow; //if any records
                int nowaLiczba = int.Parse(ViewModel.Persons.PersonsList[licznik].ID) + 1;
                id = nowaLiczba.ToString();
            }
            else
            {
                licznik = 0; //if no records
                id = licznik.ToString();
            }


            string firstName = (string)values[1];

            string secondName = (string)values[2];

            Model.HeightList heightList = (Model.HeightList)pzts.ConvertBack(
            values[3], typeof(Model.HeightList), null, CultureInfo.CurrentCulture);

            return new ViewModel.PersonsList(id, firstName, secondName, heightList);


        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

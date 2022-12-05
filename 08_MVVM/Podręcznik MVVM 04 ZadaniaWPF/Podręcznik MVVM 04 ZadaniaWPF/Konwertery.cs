using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Podręcznik_MVVM_04_ZadaniaWPF
{
    public class BoolToBrushConverter : IValueConverter
    {
        public Brush KolorDlaFałszu { get; set; } //własność koloru fałszu
        public Brush KolorDlaPrawdy { get; set; } //własność koloru prawdy


        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return !bvalue ? KolorDlaFałszu : KolorDlaPrawdy;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PriorytetZadaniaToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            Model.PriorytetZadania priorytetZadania = (Model.PriorytetZadania)value;
            return Model.Zadanie.OpisPriorytetu(priorytetZadania);
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            string opisPriorytetu = (value as string).ToLower();
            return Model.Zadanie.ParsujOpisPriorytetu(opisPriorytetu);
        }
    }
    public class PriorytetZadaniaToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            Model.PriorytetZadania priorytetZadania = (Model.PriorytetZadania)value;
            switch (priorytetZadania)
            {
                case Model.PriorytetZadania.MniejWażne:
                    return Brushes.Olive;
                case Model.PriorytetZadania.Ważne:
                    return Brushes.Orange;
                case Model.PriorytetZadania.Krytyczne:
                    return Brushes.OrangeRed;
                default:
                    throw new Exception("Nierozpoznany priorytet zadania");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class BoolToTextDecorationConverter : IValueConverter //text decoration
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return bvalue ? TextDecorations.Strikethrough : null; //skreślenie, jeśli wykonane zadanie
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ZadanieConverter : IMultiValueConverter
    {
        PriorytetZadaniaToString pzts = new PriorytetZadaniaToString();
        public object Convert(object[] values, Type targetType, object parameter,
        CultureInfo culture)
        {
            string opis = (string)values[0];
            DateTime terminUtworzenia = DateTime.Now;
            DateTime? planowanyTerminRealizacji = (DateTime?)values[1];
            Model.PriorytetZadania priorytet = (Model.PriorytetZadania)pzts.ConvertBack(
            values[2], typeof(Model.PriorytetZadania), null, CultureInfo.CurrentCulture);


            if (!string.IsNullOrWhiteSpace(opis) && planowanyTerminRealizacji.HasValue)
                return new ModelWidoku.ZadanieViewModel(opis, terminUtworzenia,
                planowanyTerminRealizacji.Value, priorytet, false);
            else return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using Testy_01_MVVM_IController.ViewModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Testy_01_MVVM_IController //do ANALIZY!!!
{
    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged //dziedziczenie
    {
        public event PropertyChangedEventHandler PropertyChanged; //event dla INotifyPropertyChanged

        protected void OnPropertyChanged(string propertyName) //interfejs dla INotifyPropertyChanged
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected Action<object> execute = null; //odpalanie przy bindowaniu z xaml, dla CustomContentDialogBox

        public string Caption { get; set; } //własność, nadaje oknu dialogowemu tytuł

        protected ICommand show;

        public virtual ICommand Show
        {
            get
            {
                if (show == null) show = new RelayCommand(execute); //korzystamy z klas RC, wykorzystujemy tylko akcję execute
                return show;
            }
        }
    }


    //klasa wykonuje komendę ExecuteCommand
    public abstract class CommandDialogBox : DialogBox //uruchamianie poleceń przed wyświetleniem lub po wyświetleniu okna dialogowego
    {  //jest to klasa abstrakcyjna, nie określa zatem tego, jak wyglądać będzie okno dialogowe, które ma być pokazane użytkownikowi.Dopiero w jej klasach potomnych będziemy definiować akcję
       //execute, która to dookreśli

        //utworzenie własności DependencyProperty dla CommandParameter z xaml
        public static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandDialogBox));

        //dla DependencyProperty powyżej
        public object CommandParameter //multibinded z xaml kilku własności Text, nie wrzucamy logiki
        {
            get
            {
                return GetValue(CommandParameterProperty); //nie wrzucamy logiki
            }
            set
            {
                SetValue(CommandParameterProperty, value); //nie wrzucamy logiki
            }
        }
        protected static void ExecuteCommand(ICommand command, object commandParameter)
        //metoda - wykonanie komendy CommandTrue="{Binding Path=NewPerson}"> oraz CommandParameter
        {
            if (command != null) //jeśli komenda różna od nic...
                if (command.CanExecute(commandParameter))
                {   //jeśli można wykonać zwraca true
                    command.Execute(commandParameter);

                }   //wykonanie komendy

        }


    }
    [ContentProperty("WindowContent")]
    //odwoluje się przycisk NEW w XAML
    public class CustomContentDialogBox : CommandDialogBox //końcowy produkt w linii 103
    {


        bool? LastResult;//bool? - wartość logiczna która też może być null
        public double WindowWidth { get; set; } //własność szerokości
        public double WindowHeight { get; set; } //własność wysokości
        public object WindowContent { get; set; } //własność zawartości
        private static Window window = null;
        public CustomContentDialogBox() //konstruktor
        {
            execute = //Action<object>
            o => //parametr "o" wyrażenia lambda
            {
                if (window == null)//jeśli nie ma jeszcze okna to...
                {
                    window = new Window();//instancja nowego okna

                    window.Width = WindowWidth;//pobiera szerokość okna z xaml
                    window.Height = WindowHeight;//pobiera wysokość okna z xaml
                    if (Caption != null) window.Title = Caption;//jeśli nie ma nazwy okna
                    window.Content = WindowContent;//pobiera zawartość okna z xaml

                    LastResult = window.ShowDialog();//nadanie wartości LastResult
                                                     //ShowDialog otwiera okno i zwraca tylko gdy nowootwarte okno jest zamykane
                                                     //ShowDialog dziedziczy z ContentControl, bierze ono z xaml ContentControl, czyli zawartość okna
                                                     //jeśli zamknięte w "odpowiedni" sposób, to przybiera wartość "true"




                    OnPropertyChanged("LastResult");
                    //końcowy produkt, dla wartości bool z DialogResultChanged:
                    switch (LastResult) //przy różnych wartościach LastResult...
                    {
                        case true:
                            //dla CommandTrue binduje w xaml komendę NewPerson, która jest wykonywana (line:222)
                            //CommandParameter jest bindowany z xaml CustomContentDialogBox.CommandParameter, przetworzone przez konwerter (line:52)
                            ExecuteCommand(CommandTrue, CommandParameter);

                            //dodaje do kolekcji, egzekucja komendy(CommandTrue) z parametrem(CommandParameter) (line:84)
                            break;
                        case false:
                            ExecuteCommand(CommandFalse, CommandParameter); //nie dodaje do kolekcji
                            break;
                        case null:
                            ExecuteCommand(CommandNull, CommandParameter);
                            break;
                    }
                    TraverseVisualTree(window); //czyszczenie toolboxów przed zamknięciem okna, po wysłaniu danych
                    window = null;//resetuje bool, dzieki temu można ponownie otworzyć okno dialogowe
                }
            };
        }//koniec konstruktora



        static public void TraverseVisualTree(Visual myMainWindow)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(myMainWindow);
            for (int i = 0; i < childrenCount; i++)
            {
                var visualChild = (Visual)VisualTreeHelper.GetChild(myMainWindow, i);
                if (visualChild is TextBox)
                {
                    TextBox tb = (TextBox)visualChild;
                    tb.Clear();
                }
                TraverseVisualTree(visualChild);
            }
        }



        public static bool? GetCustomContentDialogResult(DependencyObject d) //nic się nie dzieje
        {
            return (bool?)d.GetValue(DialogResultProperty);//wrapper dla DependencyProperty, nie dodajemy logiki
        }

        //przycisk Add w xaml automatycznie ustanawia dialog na "true", dzięki temu dodajemy do kolekcji
        public static void SetCustomContentDialogResult(DependencyObject d, bool? value) //
        {
            d.SetValue(DialogResultProperty, value);//wrapper dla DependencyProperty, nie dodajemy logiki
        }

        public static readonly DependencyProperty DialogResultProperty = //deklaracja DependencyProperty
        DependencyProperty.RegisterAttached( //register
        "DialogResult", //The name of the dependency property to register.
        typeof(bool?), //The type of the property.
        typeof(CustomContentDialogBox), //The owner type that is registering the dependency property.
        new PropertyMetadata(null, DialogResultChanged)); //A property metadata instance. This can contain a PropertyChangedCallback implementation reference.
        //w podsumowaniu, własność się nazywa DialogResult, true/false, klasa CustomContentDialogBox, instancja PM(default, zmiana)


        private static void DialogResultChanged(DependencyObject d, //wywołanie za każdym razem, gdy własność jest zmieniona
        DependencyPropertyChangedEventArgs e)
        {
            bool? dialogResult = (bool?)e.NewValue;
            if (d is Button) //jeśli DependencyObject jest przyciskiem, to...
            {

                Button button = d as Button; //zmienna button jest DependencyObject, logika Value Changed Callback
                button.Click += //dodajemy zdarzenie

                (object sender, RoutedEventArgs _e) => //obiekt, pewnie DependencyObject (przycisk), i wydarzenie inicjują...
                {
                    window.DialogResult = dialogResult;

                    //własność DependencyProperty = dialogResult, czyli true/false dla zm.własności
                };//jeśli się nie mylę, to xaml dostarcza na twardo własność "true" z przycisku "Add"
            }
        }


        //utworzenie własności DependencyProperty dla CommandTrue z xaml
        public static DependencyProperty CommandTrueProperty = DependencyProperty.Register("CommandTrue", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandFalseProperty = DependencyProperty.Register("CommandFalse", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandNullProperty = DependencyProperty.Register("CommandNull", typeof(ICommand), typeof(CustomContentDialogBox));

        //dla powyższych deklaracji Dependency Property:
        public ICommand CommandTrue //wrapping, bindowana z CustomContentDialogBox w xaml (line:84)
        {
            get
            {
                return (ICommand)GetValue(CommandTrueProperty); //.NET Property wrapper, nie wrzucamy logiki
            }
            set
            {
                SetValue(CommandTrueProperty, value); //.NET Property wrapper, nie wrzucamy logiki
            }
        }
        //dla powyższych deklaracji Dependency Property:
        public ICommand CommandFalse
        {
            get
            {
                return (ICommand)GetValue(CommandFalseProperty); //.NET Property wrapper, nie wrzucamy logiki
            }
            set
            {
                SetValue(CommandFalseProperty, value); //.NET Property wrapper, nie wrzucamy logiki
            }
        }
        //dla powyższych deklaracji Dependency Property:
        public ICommand CommandNull
        {
            get
            {
                return (ICommand)GetValue(CommandNullProperty); //.NET Property wrapper, nie wrzucamy logiki
            }
            set
            {
                SetValue(CommandNullProperty, value); //.NET Property wrapper, nie wrzucamy logiki
            }
        }



    }
}

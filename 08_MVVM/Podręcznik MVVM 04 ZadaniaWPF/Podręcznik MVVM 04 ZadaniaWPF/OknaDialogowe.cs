using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;
using Podręcznik_MVVM_04_ZadaniaWPF.ModelWidoku;
using System.Windows.Markup;
using System.Windows.Controls;

namespace Podręcznik_MVVM_04_ZadaniaWPF
{
    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged //FE dziedziczy z UIElement, tam się znajduje własność DataCOntext
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nazwaWłasności)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności)); //ZAMKNIĘCIE 1)
        }
        #endregion
        protected Action<object> execute = null;
        public string Caption { get; set; } //własność, nadaje oknu dialogowemu tytułu oraz własności Show; OKNO 7)
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

    public class SimpleMessageDialogBox : DialogBox //dziedziczenie z klasy wyzej
    {
        public SimpleMessageDialogBox()
        {
            execute =
            o => //parametr "o" wyrażenia lambda
            {
                MessageBox.Show((string)o, Caption);
            };
        }
    }

    public abstract class CommandDialogBox : DialogBox //uruchamianie poleceń przed wyświetleniem lub po wyświetleniu okna dialogowego
    {  //jest to klasa abstrakcyjna, nie określa zatem tego, jak wyglądać będzie okno dialogowe, które ma być pokazane użytkownikowi.Dopiero w jej klasach potomnych będziemy definiować akcję
       //execute, która to dookreśli
        public override ICommand Show //nadpisanie "Show"
        {
            get
            {
                if (show == null) show = new RelayCommand( //OKNO 2) pokazanie okna(?)
                o => //parametr "o" wyrażenia lambda
                {
                    ExecuteCommand(CommandBefore, CommandParameter); //stąd idziemy do ExecuteCommand metody, ale CommandBefore=null
                    execute(o);
                    ExecuteCommand(CommandAfter, CommandParameter);
                });
                return show;
            }
        }
        public static DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(object), typeof(CommandDialogBox));

        public object CommandParameter
        {
            get
            {
                return GetValue(CommandParameterProperty); //ZAMKNIĘCIE 3) 6)
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }
        protected static void ExecuteCommand(ICommand command, object commandParameter)
        {
            if (command != null) //OKNO 3) command=null; ZAMKNIĘCIE 4) 7)
                if (command.CanExecute(commandParameter))
                    command.Execute(commandParameter); //nie jest wykonane
        }
        public static DependencyProperty CommandBeforeProperty = DependencyProperty.Register("CommandBefore", typeof(ICommand), typeof(CommandDialogBox));
        public ICommand CommandBefore
        {
            get
            {
                return (ICommand)GetValue(CommandBeforeProperty); //OKNO 1) przed własnością
            }
            set
            {
                SetValue(CommandBeforeProperty, value);
            }
        }
        public static DependencyProperty CommandAfterProperty = DependencyProperty.Register("CommandAfter", typeof(ICommand), typeof(CommandDialogBox));
        public ICommand CommandAfter
        {
            get
            {
                return (ICommand)GetValue(CommandAfterProperty); //ZAMKNIĘCIE 5)
            }
            set
            {
                SetValue(CommandAfterProperty, value);
            }
        }
    }
    public class NotificationDialogBox : CommandDialogBox //inicjujemy akcję execute w taki sposób, aby wyświetlała okno dialogowe... 
                                                          //...MessageBox z opcją pokazywania ikony powiadomienia
    {
        public NotificationDialogBox()
        {
            execute =
            o => //parametr "o" wyrażenia lambda
            {
                MessageBox.Show((string)o, Caption, MessageBoxButton.OK, MessageBoxImage.Information);
            };
        }
    }

    public class MessageDialogBox : CommandDialogBox
    {
        public MessageBoxResult? LastResult { get; protected set; }
        public MessageBoxButton Buttons { get; set; }// = MessageBoxButton.OK;
        public MessageBoxImage Icon { get; set; }// = MessageBoxImage.None;

        public bool IsLastResultYes
        {
            get
            {
                if (!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.Yes;
            }
        }

        public bool IsLastResultNo
        {
            get
            {
                if (!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.No;
            }
        }

        public bool IsLastResultCancel
        {
            get
            {
                if (!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.Cancel;
            }
        }

        public bool IsLastResultOK
        {
            get
            {
                if (!LastResult.HasValue) return false;
                return LastResult.Value == MessageBoxResult.OK;
            }
        }

        public MessageDialogBox()
        {
            Buttons = MessageBoxButton.OK;
            Icon = MessageBoxImage.None;

            execute = o =>
            {
                LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
                OnPropertyChanged("LastResult");
                switch (LastResult)
                {
                    case MessageBoxResult.Yes:
                        OnPropertyChanged("IsLastResultYes");
                        ExecuteCommand(CommandYes, CommandParameter);
                        break;
                    case MessageBoxResult.No:
                        OnPropertyChanged("IsLastResultNo");
                        ExecuteCommand(CommandNo, CommandParameter);
                        break;
                    case MessageBoxResult.Cancel:
                        OnPropertyChanged("IsLastResultCancel");
                        ExecuteCommand(CommandCancel, CommandParameter);
                        break;
                    case MessageBoxResult.OK:
                        OnPropertyChanged("IsLastResultOK");
                        ExecuteCommand(CommandOK, CommandParameter);
                        break;
                }
            };
        }

        public static DependencyProperty CommandYesProperty = DependencyProperty.Register("CommandYes", typeof(ICommand), typeof(MessageDialogBox));
        public static DependencyProperty CommandNoProperty = DependencyProperty.Register("CommandNo", typeof(ICommand), typeof(MessageDialogBox));
        public static DependencyProperty CommandCancelProperty = DependencyProperty.Register("CommandCancel", typeof(ICommand), typeof(MessageDialogBox));
        public static DependencyProperty CommandOKProperty = DependencyProperty.Register("CommandOK", typeof(ICommand), typeof(MessageDialogBox));

        public ICommand CommandYes
        {
            get
            {
                return (ICommand)GetValue(CommandYesProperty);
            }
            set
            {
                SetValue(CommandYesProperty, value);
            }
        }

        public ICommand CommandNo
        {
            get
            {
                return (ICommand)GetValue(CommandNoProperty);
            }
            set
            {
                SetValue(CommandNoProperty, value);
            }
        }

        public ICommand CommandCancel
        {
            get
            {
                return (ICommand)GetValue(CommandCancelProperty);
            }
            set
            {
                SetValue(CommandCancelProperty, value);
            }
        }

        public ICommand CommandOK
        {
            get
            {
                return (ICommand)GetValue(CommandOKProperty);
            }
            set
            {
                SetValue(CommandOKProperty, value);
            }
        }
    }

    public class ConditionalMessageDialogBox : MessageDialogBox //Rozszerzenie klasy MessageBox o warunek wyświetlania okna dialogowego
    {
        public static DependencyProperty IsDialogBypassedProperty = DependencyProperty.Register("IsDialogBypassed",
                                                                                                typeof(bool),
                                                                                                typeof(ConditionalMessageDialogBox));
        public bool IsDialogBypassed //własność, czy jest bypassed czy nie? jeśli jest nieaktywny, to tak
        {
            get
            {
                return (bool)GetValue(IsDialogBypassedProperty);
            }
            set
            {
                SetValue(IsDialogBypassedProperty, value);
            }
        }
        public MessageBoxResult DialogBypassButton { get; set; } = MessageBoxResult.None;
        public ConditionalMessageDialogBox()
        {
            DialogBypassButton = MessageBoxResult.None;
            execute =
                o => //parametr "o" wyrażenia lambda
                {
                    MessageBoxResult result;
                    if (!IsDialogBypassed)
                    {
                        LastResult = MessageBox.Show((string)o, Caption, Buttons, Icon);
                        OnPropertyChanged("LastResult");
                        result = LastResult.Value;
                    }
                    else
                    {
                        result = DialogBypassButton;
                    }
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (!IsDialogBypassed) OnPropertyChanged("IsLastResultYes");
                            ExecuteCommand(CommandYes, CommandParameter);
                            break;
                        case MessageBoxResult.No:
                            if (!IsDialogBypassed) OnPropertyChanged("IsLastResultNo");
                            ExecuteCommand(CommandNo, CommandParameter);
                            break;
                        case MessageBoxResult.Cancel:
                            if (!IsDialogBypassed) OnPropertyChanged("IsLastResultCancel");
                            ExecuteCommand(CommandCancel, CommandParameter);
                            break;
                        case MessageBoxResult.OK:
                            if (!IsDialogBypassed) OnPropertyChanged("IsLastResultOK");
                            ExecuteCommand(CommandOK, CommandParameter);
                            break;
                    }
                };
        }
    }

    public abstract class FileDialogBox : CommandDialogBox
    {
        public bool? FileDialogResult { get; protected set; } //własność
        public string FilePath { get; set; } //własność
        public string Filter { get; set; } //własność
        public int FilterIndex { get; set; } //własność
        public string DefaultExt { get; set; } //własność
        protected Microsoft.Win32.FileDialog fileDialog = null; //wyzerowanie(?) ustawień dialogu
        protected FileDialogBox() //protected nie zezwala na dostęp z zewnątrz klasy
        {
            execute = //definiuje metodę wezwaną, gdy komenda jest przywołana
            o => //parametr "o" wyrażenia lambda
            {
                fileDialog.Title = Caption; //z własności klasy, tytuł okna dialogowego, przekazany z widoku
                fileDialog.Filter = Filter; //przekazany z widoku
                fileDialog.FilterIndex = FilterIndex; //przekazany z widoku
                fileDialog.DefaultExt = DefaultExt; //przekazany z widoku
                string ścieżkaPliku = ""; //pusta ścieżka
                if (FilePath != null) ścieżkaPliku = FilePath; else FilePath = ""; //jeśli pusta ścieżka
                if (o != null) ścieżkaPliku = (string)o; //jeśli "o" różne od 0, to ścieżka jest stringiem z parametru "o"
                if (!string.IsNullOrWhiteSpace(ścieżkaPliku)) //jeśli ścieżka pliku nie jest równa zero, to...
                {
                    fileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(ścieżkaPliku);
                    fileDialog.FileName = System.IO.Path.GetFileName(ścieżkaPliku);
                }
                FileDialogResult = fileDialog.ShowDialog();
                OnPropertyChanged("FileDialogResult");
                if (FileDialogResult.HasValue && FileDialogResult.Value)
                {
                    FilePath = fileDialog.FileName;
                    OnPropertyChanged("FilePath");
                    ExecuteCommand(CommandFileOk, FilePath);
                };
            };
        }
        public static DependencyProperty CommandFileOkProperty = DependencyProperty.Register("CommandFileOk",
        typeof(ICommand), typeof(FileDialogBox));
        public ICommand CommandFileOk
        {
            get
            {
                return (ICommand)GetValue(CommandFileOkProperty);
            }
            set
            {
                SetValue(CommandFileOkProperty, value);
            }
        }
    }
    public class OpenFileDialogBox : FileDialogBox
    {
        public OpenFileDialogBox()
        {
            fileDialog = new Microsoft.Win32.OpenFileDialog();
        }
    }

    //klasę wywołuje przycisk "Eksportuj..." z widoku, wraz z interfejsem EksportujZadaniaDoPlikuTekstowego w Zadania.cs
    public class SaveFileDialogBox : FileDialogBox //klasa dziedziczy z FileDialogBox
    {
        public SaveFileDialogBox()
        {
            fileDialog = new Microsoft.Win32.SaveFileDialog(); //utworzenie instancji klasy SaveFileDialog
        }
    }

    public class FileSavedNotificationDialogBox : CommandDialogBox
    {
        public FileSavedNotificationDialogBox()
        {
            execute =
                o =>
                {
                    MessageBox.Show("Plik " + (string)o + " został zapisany", Caption, MessageBoxButton.OK, MessageBoxImage.Information);
                };
        }
    }

    [ContentProperty("WindowContent")] //do sprawdzenia!!!!!!!!!!!!!!
    public class CustomContentDialogBox : CommandDialogBox
    {
        bool? LastResult;
        public double WindowWidth { get; set; }// = 640; //OKNO 5)
        public double WindowHeight { get; set; }// = 480; //OKNO 6)
        public object WindowContent { get; set; }// = null; //OKNO 8)

        private static Window window = null; //STATIC
        public CustomContentDialogBox() //konstruktor
        {
            execute =
            o => //parametr "o" wyrażenia lambda
            {
                if (window == null) //OKNO 4)
                {
                    window = new Window();
                    window.Width = WindowWidth;
                    window.Height = WindowHeight;
                    if (Caption != null) window.Title = Caption;
                    window.Content = WindowContent;
                    LastResult = window.ShowDialog(); //deklaracja LastResult "Opens a window and returns only when the newly opened window is closed."
                    OnPropertyChanged("LastResult"); //zmiana własności LastResult, przy zamknięciu
                    window = null;
                    switch (LastResult) //dla LastResult
                    {
                        case true:
                            ExecuteCommand(CommandTrue, CommandParameter);
                            break;
                        case false:
                            ExecuteCommand(CommandFalse, CommandParameter); //wykonuje przy zamknięciu
                            break;
                        case null:
                            ExecuteCommand(CommandNull, CommandParameter);
                            break;
                    }
                }
            };
        }
        public static bool? GetCustomContentDialogResult(DependencyObject d)
        {
            return (bool?)d.GetValue(DialogResultProperty);
        }
        public static void SetCustomContentDialogResult(DependencyObject d, bool? value)
        {
            d.SetValue(DialogResultProperty, value);
        }
        public static readonly DependencyProperty DialogResultProperty = //STATIC
        DependencyProperty.RegisterAttached(
        "DialogResult",
        typeof(bool?),
        typeof(CustomContentDialogBox),
        new PropertyMetadata(null, DialogResultChanged));
        private static void DialogResultChanged(DependencyObject d, //o przycisku mowa
        DependencyPropertyChangedEventArgs e)
        {
            bool? dialogResult = (bool?)e.NewValue;
            if (d is Button)
            {
                Button button = d as Button;
                button.Click +=
                (object sender, RoutedEventArgs _e) =>
                {
                    window.DialogResult = dialogResult;
                };
            }
        }
        //STATIC 3X
        public static DependencyProperty CommandTrueProperty = DependencyProperty.Register("CommandTrue", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandFalseProperty = DependencyProperty.Register("CommandFalse", typeof(ICommand), typeof(CustomContentDialogBox));
        public static DependencyProperty CommandNullProperty = DependencyProperty.Register("CommandNull", typeof(ICommand), typeof(CustomContentDialogBox));
        public ICommand CommandTrue
        {
            get
            {
                return (ICommand)GetValue(CommandTrueProperty);
            }
            set
            {
                SetValue(CommandTrueProperty, value);
            }
        }
        public ICommand CommandFalse
        {
            get
            {
                return (ICommand)GetValue(CommandFalseProperty); //ZAMKNIĘCIE 2)
            }
            set
            {
                SetValue(CommandFalseProperty, value);
            }
        }
        public ICommand CommandNull
        {
            get
            {
                return (ICommand)GetValue(CommandNullProperty);
            }
            set
            {
                SetValue(CommandNullProperty, value);
            }
        }
    }
}

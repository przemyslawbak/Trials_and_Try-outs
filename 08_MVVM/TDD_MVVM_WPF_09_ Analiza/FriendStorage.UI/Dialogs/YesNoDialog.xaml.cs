using System.Windows;

namespace FriendStorage.UI.Dialogs
{
  public partial class YesNoDialog : Window
  {
    public YesNoDialog(string title, string message )
    {
      InitializeComponent();
      Title = title;
      textBlock.Text = message;
    }

    private void ButtonYes_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }

    private void ButtonNo_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

  }
}

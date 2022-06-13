using System.Windows;

namespace App11.Databases.Views;

public partial class SlashWindow : Window
{
    public SlashWindow()
    {
        InitializeComponent();
    }

    private void ButtonSqlite_OnClick(object sender, RoutedEventArgs e)
    {
        SqliteWindow.GetInstance().ShowDialog();
    }

    private void ButtonMySql_OnClick(object sender, RoutedEventArgs e)
    {
        HotelWindow.GetInstance().ShowDialog();
    }
}
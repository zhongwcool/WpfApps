using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace App11.Databases.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var connection = new SQLiteConnection(App.DbPath);
            var datatable = new DataTable();
            var adp = new SQLiteDataAdapter("SELECT * FROM User", connection);
            adp.Fill(datatable);
            DataGrid.ItemsSource = datatable.DefaultView;
        }
    }
}
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace App11.Databases.Views;

public partial class SqliteWindow : Window
{
    private static SqliteWindow _instance;

    public static SqliteWindow GetInstance()
    {
        _instance ??= new SqliteWindow();
        return _instance;
    }

    public SqliteWindow()
    {
        InitializeComponent();

        var connection = new SQLiteConnection(App.DbPath);
        var datatable = new DataTable();
        var adp = new SQLiteDataAdapter("SELECT * FROM User", connection);
        adp.Fill(datatable);
        DataGrid.ItemsSource = datatable.DefaultView;
    }
}
using System.Windows;

namespace App11.Databases
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DbPath { get; set; } = @"DataSource=tt2.db";
    }
}
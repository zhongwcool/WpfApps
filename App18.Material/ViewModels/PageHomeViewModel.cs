using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using App18.Material.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using Mar.Console;

namespace App18.Material.ViewModels;

public class PageHomeViewModel : ObservableObject
{
    public PageHomeViewModel()
    {
        EmailList = new ObservableCollection<Mail>();
        _demoItemsView = CollectionViewSource.GetDefaultView(EmailList);
        _demoItemsView.Filter = CollectionFilter;

        Prepare();
    }

    private void Prepare()
    {
        var task = Task.Run(() =>
        {
            var model = JsonUtil.Load<MailMode>(JSON_FILE);
            return model?.Mails;
        });

        task.ContinueWith(_ =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (task.Result == null) return;
                foreach (var mail in task.Result) EmailList.Add(mail);

                SelectedItem = EmailList.First();
            });
        });
    }

    private const string JSON_FILE = "mails.json";

    public ObservableCollection<Mail> EmailList { get; set; }

    private Mail? _selectedItem;

    public Mail? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }

    private int _selectedIndex;

    public int SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    private readonly ICollectionView _demoItemsView;

    private string _searchKeyword;

    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (SetProperty(ref _searchKeyword, value)) _demoItemsView.Refresh();
        }
    }

    private bool CollectionFilter(object obj)
    {
        var item = (Mail)obj;
        if (string.IsNullOrWhiteSpace(_searchKeyword)) return true;

        return (!string.IsNullOrWhiteSpace(item.From) &&
                item.From.ToLower().Contains(_searchKeyword!.ToLower()))
               || (!string.IsNullOrWhiteSpace(item.Body) &&
                   item.Body.ToLower().Contains(_searchKeyword!.ToLower()))
               || (!string.IsNullOrWhiteSpace(item.Subject) &&
                   item.Subject.ToLower().Contains(_searchKeyword!.ToLower()));
    }

    public string MyAvatar { get; set; } =
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTh2iUZxg0UGw0mPw-sQLjTd64TR9lyif9Y7w&usqp=CAU";
}
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using App11.HIK.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App11.HIK.ViewModels;

public class ByPageViewModel : ObservableObject
{
    public ByPageViewModel()
    {
        DeleteCommand = new RelayCommand<JsNode>(DoDelete);

        // Create dummy data
        DoDummy();
    }

    private void DoDummy()
    {
        var task = JsNode.CreateDummy();
        task.ContinueWith(_ =>
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var robots = task.Result;
                foreach (var robot in robots)
                {
                    RobotList.Add(robot);
                }

                _demoItemsView = CollectionViewSource.GetDefaultView(RobotList);
                _demoItemsView.Filter = CollectionFilter;
            });
        });
    }

    private bool CollectionFilter(object obj)
    {
        if (string.IsNullOrWhiteSpace(_searchKeyword)) return true;

        return obj is JsNode item
               && (item.NodeName.ToLower().Contains(_searchKeyword!.ToLower())
                   || item.DevIp.ToLower().Contains(_searchKeyword!.ToLower())
                   || item.SerialNum.ToLower().Contains(_searchKeyword!.ToLower()));
    }

    private ICollectionView _demoItemsView;

    private string? _searchKeyword;

    public string? SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            if (SetProperty(ref _searchKeyword, value)) _demoItemsView.Refresh();
        }
    }

    public ObservableCollection<JsNode> RobotList { get; set; } = new();

    public IRelayCommand<JsNode> DeleteCommand { get; set; }

    private void DoDelete(JsNode selectedNode)
    {
        RobotList.Remove(selectedNode);
    }
}
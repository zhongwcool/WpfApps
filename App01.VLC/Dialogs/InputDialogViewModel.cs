using CommunityToolkit.Mvvm.ComponentModel;

namespace App01.VLC.Dialogs;

public class InputDialogViewModel : ObservableObject
{
    private string _site;

    public string Site
    {
        get => _site;
        set => SetProperty(ref _site, value);
    }
}
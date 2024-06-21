using CommunityToolkit.Mvvm.ComponentModel;

namespace App01.VLC.Dialogs;

public class NotifyDialogViewModel : ObservableObject
{
    public NotifyDialogViewModel(string content)
    {
        Body = content;
    }

    private string _body;

    public string Body
    {
        get => _body;
        set => SetProperty(ref _body, value);
    }
}
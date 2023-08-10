using System.Collections.ObjectModel;
using App18.Material.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App18.Material.ViewModels;

public class PageHomeViewModel : ObservableObject
{
    public PageHomeViewModel()
    {
        EmailList.Add(new Email
        {
            Name = "John Doe",
            Time = "Today",
            Subject = "豆花鱼",
            Content =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec aliquet, dolor sed vulputate euismod, arcu nunc ornare nisl, ut lacinia tellus nisl quis nunc. Sed sed nisl euismod, aliquet nisl sed, aliquam nisl. Sed euismod, nisl quis aliquam ultricies, nisl nisl aliquet nisl, quis aliquam nisl nisl quis nisl. Sed euismod, nisl quis aliquam ultricies, nisl nisl aliquet nisl, quis aliquam nisl nisl quis nisl."
        });

        EmailList.Add(new Email
        {
            Name = "John Doe",
            Time = "Today",
            Subject = "John Doe",
            Content =
                "Jetpack Compose offers an implementation of Material Design 3, the next evolution of Material Design. "
        });
    }

    public ObservableCollection<Email> EmailList { get; set; } = new();
}
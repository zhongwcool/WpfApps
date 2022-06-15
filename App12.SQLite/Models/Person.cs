using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace App12.SQLite.Models;

public class Person : ObservableObject
{
    private int _personId;

    public int PersonId
    {
        get => _personId;
        set => SetProperty(ref _personId, value);
    }

    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string _lastName;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    private DateTime? _birthdate;

    public DateTime? Birthdate
    {
        get => _birthdate;
        set => SetProperty(ref _birthdate, value);
    }
}
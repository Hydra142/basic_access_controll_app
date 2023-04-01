using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
[ObservableObject]
public partial class Role
{
    public int? Id;
    [ObservableProperty]
    public string _Name;

    public object ToObject()
    {
        return new
        {
            Id = Id,
            Name = Name
        };
    }
}


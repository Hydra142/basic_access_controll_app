using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
[ObservableObject]
public partial class ActionType
{
    [ObservableProperty]
    public int _Id;
    public bool IsReadAble;
    public bool IsWriteAble;
    public bool IsExecuteAble;
    [ObservableProperty]
    public string _Name;
}

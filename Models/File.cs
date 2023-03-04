using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
[ObservableObject]
public partial class File
{
    //db fields
    public int Id;
    [ObservableProperty]
    public string _Name;
    [ObservableProperty]
    public string _FilePath;
    public FileTypes FileType;
    public int MinimumClearanceId;

    //additional fields
    public string ClearanceName;
    public string ActionTypeName;
    public bool IsReadAble;
    public bool IsWriteAble;
    public bool IsExecuteAble;

    public object ToObject()
    {
        return new { Id = Id, Name = Name, FilePath = FilePath, FileType = (int)FileType, MinimumClearanceId = MinimumClearanceId };
    }
}
public enum FileTypes
{
    Txt,
    Img,
    Exe
}

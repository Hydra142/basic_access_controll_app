using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;

public class File
{
    public int Id;
    public string Name;
    public string FilePath;
    public string ClearanceName;
    public string ActionTypeName;
    public FileType FileType;
    public bool IsReadAble;
    public bool IsWriteAble;
    public bool IsExecuteAble;
}

public enum FileType
{
    Txt,
    Img,
    Exe
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;

public class DiscretionaryMatrixItem
{
    public int? Id;
    public int? UserId;
    public int? FileId;
    public int? ActionTypeId;
    public DateTime? AllowFrom;
    public DateTime? AllowTo;
    public bool IsActive;
}


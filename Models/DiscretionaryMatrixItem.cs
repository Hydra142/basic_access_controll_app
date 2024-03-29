﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
[ObservableObject]
public partial class DiscretionaryMatrixItem
{
    //db fields
    public int? Id;
    public int? UserId;
    public int? FileId;
    public int? ActionTypeId;
    [ObservableProperty]
    public DateTime? _AllowFrom = new();
    [ObservableProperty]
    public DateTime? _AllowTo = new();

    //additional fields
    public bool IsActive;
    public string FileName;
    public List<ActionType> ActionTypesOptions = new();

    public object ToObject()
    {
        return new {
            Id = Id,
            UserId = UserId,
            FileId = FileId,
            ActionTypeId = ActionTypeId,
            AllowFrom= AllowFrom != null ?  AllowFrom.Value.ToString("yyyy-MM-dd HH:mm:ss") :null,
            AllowTo = AllowTo != null ? AllowTo.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
        };
    }
}


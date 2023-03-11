﻿using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SafeMessenge.Models;

[ObservableObject]
public partial class User
{
    public long Id;
    [ObservableProperty]
    public string _Name;
    public string Password;
    public bool IsAdmin;
    public long PasswordTypeId;
    public string PasswordTypeName;
    public string PasswordValidationRegex;
    public string PasswordTypeDescription;
    public int ClearanceId;
    public int ActionTypeId;
    public AccessControlModelName AccessControlModelId;
    public string Avatar => $"https://dummyimage.com/400x400/000000/0011ff&text={(Password.IsNullOrEmpty() ? "Новий" : _Name)}";

    public object ToObject()
    {
        return new { Id = Id, UserName = _Name, Password = Password, IsAdmin = IsAdmin, PasswordTypeId = PasswordTypeId, ClearanceId = ClearanceId, ActionTypeId = ActionTypeId, AccessControlModelId = (int)AccessControlModelId };
    }
}

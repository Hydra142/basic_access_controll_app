using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Models;
[ObservableObject]
public partial class UserRoleItem
{
    //db fields
    public int? Id;
    public int? UserId;
    public int? RoleId;
    public string? RoleName;

    //additional fields
    public bool IsActive;
    

    public object ToObject()
    {
        return new {
            Id = Id,
            RoleId = RoleId,
            UserId = UserId,
        };
    }
}


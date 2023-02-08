using SafeMessenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Services;

public class AppDataService
{
    public List<User> Users { get; set; } = new();
    public User? CurrentUser { get; set; }

    public AppDataService()
    {

    }

    public List<User> SetUsersFromApi()
    {
        Users = new List<User>() { new User() { Name = "ddd", Id = 1 }, new User() { Name = "kkk", Id = 2 }, new User() { Name = "lll", Id = 3 } };
        return Users;
    }
}

using SafeMessenge.Contracts.Services;
using SafeMessenge.Models;
using SafeMessenge.Properties;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System;

namespace SafeMessenge.Services;

public class AppDataService
{
    public List<User> Users { get; set; } = new();
    public User? CurrentUser { get; set; }

    private ISqliteConnector _sqliteConnector;

    public AppDataService(ISqliteConnector conn)
    {
        _sqliteConnector = conn;
    }

    public async Task<List<User>> SetUsers()
    {
        Users = await GetUsers();
        return Users;
    }

    public async Task<List<User>> GetUsers()
    {
        var result = new List<User>();
        result.AddRange(await _sqliteConnector.Read<User>(Resources.GetAllUsers, new { }));
        return result;
    }

    public async Task<User?> UpdateUserData(User user)
    {
        var res = await _sqliteConnector.Write(Resources.UpdateUser, user.ToObject());
        var userData = await _sqliteConnector.Read<User>(Resources.GetUserById, user.ToObject());
        if (userData != null && userData.Count > 0)
        {
            return userData.First();
        }
        return null;

    }
}

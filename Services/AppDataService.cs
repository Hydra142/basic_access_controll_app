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
    public List<Role> Roles { get; set; } = new();
    public List<PasswordType> PasswordTypes { get; set; } = new();
    public List<SecurityClearance> SecurityClearances { get; set; } = new();
    public List<ActionType> ActionTypes { get; set; } = new();
    public User? CurrentUser { get; set; }
    public List<File> Files { get; set; } = new();

    private ISqliteConnector _sqliteConnector;

    public AppDataService(ISqliteConnector conn)
    {
        _sqliteConnector = conn;
    }
    public async Task InitAsync()
    {
        await LoadPasswordTypes();
        await LoadSecurityClearances();
        await LoadActionTypes();
        await LoadFiles();
        await LoadRoles();
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
    public async Task<List<string>> GetLastUserPasswords(long userId)
    {
        var result = new List<string>();
        result.AddRange(await _sqliteConnector.Read<string>(Resources.GetLastUserPasswords, new { UserId = userId }));
        return result;
    }
    public async Task<User?> UpdateUserData(User user)
    {
        //запит на зміну данних корстувача
        var res = await _sqliteConnector.Write(Resources.UpdateUser, user.ToObject());
        var userData = await _sqliteConnector.Read<User>(Resources.GetUserById, user.ToObject());
        if (userData != null && userData.Count > 0)
        {
            return userData.First();
        }
        return null;

    }
    public async Task<User?> GetUserData(User user)
    {
        var userData = await _sqliteConnector.Read<User>(Resources.GetUserById, user.ToObject());
        if (userData != null && userData.Count > 0)
        {
            return userData.First();
        }
        return null;

    }
    public async Task<User?> UpdateUserPassword(User user)
    {
        //запит на зміну данних корстувача
        var res = await _sqliteConnector.Write(Resources.UpdateUserPassword, user.ToObject());
        var userData = await _sqliteConnector.Read<User>(Resources.GetUserById, user.ToObject());
        if (userData != null && userData.Count > 0)
        {
            return userData.First();
        }
        return null;

    }
    public async Task LoadPasswordTypes()
    {
        PasswordTypes = (await _sqliteConnector.Read<PasswordType>(Resources.GetPasswordTypes, new { })).ToList();
    }
    public async Task LoadSecurityClearances()
    {
        SecurityClearances = (await _sqliteConnector.Read<SecurityClearance>(Resources.GetSecurityClearances, new { })).ToList();
    }
    public async Task LoadActionTypes()
    {
        ActionTypes = (await _sqliteConnector.Read<ActionType>(Resources.GetActionTypes, new { })).ToList();
    }
    public async Task<User?> CreateUser(User user)
    {
        //створення новго користувача
        var insert = await _sqliteConnector.Read<User>(Resources.InsertUser, user.ToObject());
        if (insert != null && insert.Count > 0)
        {
            Users.Add(insert.First());
            return insert.First();
        }
        return null;
    }
    public async Task<List<File>> GetUserFiles(User? user)
    {
        List<File> result = new();
        if (user == null)
        {
            return result;
        }
        var sql = user.AccessControlModelId switch
        {
            //при мандатному розмежуванні
            AccessControlModel.MandatoryAccessControl => 
                Resources.GetUserFilesByUserId,
            //при дискреційному
            AccessControlModel.DiscretionaryAccessControl => 
                Resources.GetDiscretionaryAccessModelUserAvailableFilesById,
            //при рольовому
            AccessControlModel.RoleAccessControl =>
                Resources.GetAvailableUserFiles,
            //значення за замовчуванням (мандат)
            _ => Resources.GetUserFilesByUserId,
        };
        return (await _sqliteConnector.Read<File>(sql, new { UserId = user.Id })).ToList();
    }
    public async Task<List<File>> LoadFiles()
    {
        Files = (await _sqliteConnector.Read<File>(Resources.GetAllFiles, new { })).ToList();
        return Files;
    }
    public async Task<File?> InsertOrUpdateFile(File file)
    {
        var resp = await _sqliteConnector.Read<File>(Resources.InsertOrUpdateFile, file.ToObject());
        if (resp != null && resp.Count > 0)
        {
            return resp.First();
        }
        return null;
    }

    public async Task<List<Role>> LoadRoles()
    {
        Roles = (await _sqliteConnector.Read<Role>(Resources.GetAllRoles, new { })).ToList();
        return Roles;
    }
    public async Task<Role?> InsertOrUpdateRole(Role role)
    {
        var resp = await _sqliteConnector.Read<Role>(Resources.InsertOrUpdateRole, role.ToObject());
        if (resp != null && resp.Count > 0)
        {
            return resp.First();
        }
        return null;
    }
    public async Task<List<DiscretionaryMatrixItem>> GetUserDiscretionaryAccessMatrixById(long id)
    {
        return (await _sqliteConnector.Read<DiscretionaryMatrixItem>(Resources.GetUserDiscretionaryAccessMatrixById, new { UserId = id })).ToList();
    }

    public async Task InsertOrUpdateDiscretionaryAccessMatrixItems(List<DiscretionaryMatrixItem> items)
    {
        foreach (var item in items)
        {
            await _sqliteConnector.Write(Resources.InsertOrUpdateDiscretionaryAccessMatrixItem, item.ToObject());
        }
    }

    public async Task DeleteDiscretionaryAccessMatrixItems(IEnumerable<int> ids)
    {
        if (!ids.Any()) return;
        await _sqliteConnector.Write(Resources.DeleteDiscretionaryAccessMatrixItems, new { Ids = ids.ToArray() });
    }

    public async Task<List<RoleFileItem>> GetRoleFilesById(int id)
    {
        return (await _sqliteConnector.Read<RoleFileItem>(Resources.GetRoleFiles, new { RoleId = id })).ToList();
    }

    public async Task InsertOrUpdateRoleFile(List<RoleFileItem> items)
    {
        foreach (var item in items)
        {
            await _sqliteConnector.Write(Resources.InsertOrUpdateRoleFile, item.ToObject());
        }
    }

    public async Task DeleteRoleFiles(IEnumerable<int> ids)
    {
        if (!ids.Any()) return;
        await _sqliteConnector.Write(Resources.DeleteRoleFiles, new { Ids = ids.ToArray() });
    }

    public async Task<List<UserRoleItem>> GetUserRolesById(long id)
    {
        return (await _sqliteConnector.Read<UserRoleItem>(Resources.GetUserRoles, new { UserId = id })).ToList();
    }

    public async Task InsertOrUpdateUserRoles(List<UserRoleItem> items)
    {
        foreach (var item in items)
        {
            await _sqliteConnector.Write(Resources.InsertOrUpdateUserRole, item.ToObject());
        }
    }

    public async Task DeleteUserRoles(IEnumerable<int> ids)
    {
        if (!ids.Any()) return;
        await _sqliteConnector.Write(Resources.DeleteUserRoles, new { Ids = ids.ToArray() });
    }
}

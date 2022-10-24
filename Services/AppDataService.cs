using SafeMessenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeMessenge.Services;

public class AppDataService
{
    public ApiService ApiService;
    public List<User> Users { get; set; } = new();
    public User? CurrentUser { get; set; }
    public List<Chat> Chats { get; set; } = new();

    public AppDataService(ApiService apiService)
    {
        ApiService = apiService;

    }

    public List<User> SetUsersFromApi()
    {
        Users = ApiService.ReadAll<User>("users").ToList();
        return Users;
    }

    public List<Chat> SetUserChatsFromApi(long userId)
    {
        Chats = ApiService.ReadAll<Chat>($"chats?users_id_array={userId}").ToList();
        return Chats;
    }
}

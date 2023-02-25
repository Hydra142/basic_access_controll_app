using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
namespace SafeMessenge.Models;

[JsonObject]
public class User
{
    public long Id;
    public string Name;
    public string Password;
    public bool IsAdmin;
    public long PasswordTypeId;
    public string PasswordTypeName;
    public string PasswordValidationRegex;
    public string PasswordTypeDescription;
    public int ClearanceId;
    public string Avatar => $"https://dummyimage.com/400x400/000000/0011ff&text={(Password.IsNullOrEmpty() ? "Новий" : Name)}";

    public object ToObject()
    {
        return new { Id = Id, UserName = Name, Password = Password, IsAdmin = IsAdmin, PasswordTypeId = PasswordTypeId, ClearanceId = ClearanceId };
    }
}

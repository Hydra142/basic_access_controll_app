using Newtonsoft.Json;

namespace SafeMessenge.Models;

[JsonObject]
public class User
{
    [JsonProperty("id")]
    public long Id;
    [JsonProperty("name")]
    public string Name;
    [JsonProperty("password")]
    public string Password;
    public string Avatar => $"https://dummyimage.com/400x400/000000/0011ff&text=user{Id}";
}

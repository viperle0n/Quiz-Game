using System.Net.Http.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("http://localhost:5165/");
    }

    //REGISTER
    public async Task<bool> RegisterAsync(string username, string password)
    {
        var registerData = new { username, password };
        var response = await _httpClient.PostAsJsonAsync("api/users/register", registerData);
        return response.IsSuccessStatusCode;
    }

    //LOGIN
    public async Task<bool> LoginAsync(string username, string password)
    {
        var loginData = new { username, password };
        var response = await _httpClient.PostAsJsonAsync("api/users/login", loginData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResult>();
            // Optionally save user id or token
            return true;
        }

        return false;
    }

    //CATEGORY HIGHSCORE
    public async Task<bool> UpdateCategoryHighscoreAsync(string username, string category, int score)
    {
        var request = new
        {
            Username = username,
            Category = category,
            Score = score
        };

        var response = await _httpClient.PostAsJsonAsync("api/users/update-category-highscore", request);
        return response.IsSuccessStatusCode;
    }

    //LEADERBOARD
    public async Task<List<LeaderboardEntry>> GetLeaderboardAsync(string category)
    {
        var response = await _httpClient.GetAsync($"api/users/leaderboard/{category}");
        if (!response.IsSuccessStatusCode) return new List<LeaderboardEntry>();

        var result = await response.Content.ReadFromJsonAsync<List<LeaderboardEntry>>();
        return result ?? new List<LeaderboardEntry>();
    }
}


public class LoginResult
{
    public int id { get; set; }
    public string username { get; set; }
}


public class LeaderboardEntry
{
    public int Rank { get; set; }
    public string Username { get; set; }
    public int Highscore { get; set; }
}
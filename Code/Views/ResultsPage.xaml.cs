namespace QuizGame.Views;

public partial class ResultsPage : ContentPage
{
    private string _category;
    private int _score;
    private int _total;

    public ResultsPage(string category, int score, int total)
    {
        InitializeComponent();

        _category = category;
        _score = score;
        _total = total;

        ScoreLabel.Text = $"Score: {_score}/{_total}";
        PercentageLabel.Text = $"Accuracy: {(int)((double)_score / _total * 100)}%";

        _ = UpdateHighscoreIfNeededAsync();
        _ = LoadLeaderboardAsync();
    }

    // HIHSCORE UPDATE
    private async Task UpdateHighscoreIfNeededAsync()
    {
        try
        {
            var username = await SecureStorage.GetAsync("username");
            if (!string.IsNullOrEmpty(username))
            {
                var api = new ApiService();
                await api.UpdateCategoryHighscoreAsync(username, _category, _score);
            }
            else
            {
                Console.WriteLine("Username not found in secure storage.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating highscore: {ex.Message}");
        }
    }

    // LEADERBOARD
    private async Task LoadLeaderboardAsync()
    {
        try
        {
            var api = new ApiService();
            var leaderboard = await api.GetLeaderboardAsync(_category);

            // Rank
            var rankedLeaderboard = leaderboard
                .Select((entry, index) => new LeaderboardEntry
                {
                    Rank = index + 1,
                    Username = entry.Username,
                    Highscore = entry.Highscore
                })
                .ToList();

            LeaderboardView.ItemsSource = rankedLeaderboard;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading leaderboard: {ex.Message}");
        }
    }


    // REPLAY QUIZ
    private async void OnRetryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new QuizPage(_category));
    }

    // BACK TO MAIN MENU
    private async void OnMainMenuClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
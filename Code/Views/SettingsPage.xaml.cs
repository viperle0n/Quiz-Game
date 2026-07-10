using QuizGame.Helpers;
namespace QuizGame.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        VolumeSlider.Value = 50;
    }

    // VOLUME
    private void OnVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        int volume = (int)e.NewValue;
        VolumeLabel.Text = $"Volume: {volume}%";
    }

    // DIFFICULTY
    private void OnDifficultyChecked(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        var selected = (RadioButton)sender;
        string difficulty = selected.Content.ToString();
        Console.WriteLine($"Difficulty selected: {difficulty}");

        switch (difficulty)
        {
            case "Easy":
                GameSettings.TimePerQuestionInSeconds = 30;
                break;
            case "Medium":
                GameSettings.TimePerQuestionInSeconds = 20;
                break;
            case "Hard":
                GameSettings.TimePerQuestionInSeconds = 10;
                break;
        }

        // SAVE PREFERENCE
        Preferences.Set("Difficulty", difficulty);
    }
}
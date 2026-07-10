namespace QuizGame.Views;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
        UpdateLoginStatus();
    }

    // PLAY QUIZ
    private async void OnPlayClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CategoryPage());
    }

    //UPDATE ACCOUNT STATUS
    private async void UpdateLoginStatus()
    {
        var username = await SecureStorage.GetAsync("username");

        if (!string.IsNullOrEmpty(username))
        {
            AvatarImage.Source = "avatarlogin.png";
            WelcomeLabel.Text = $"Logged in as: {username}";
            LogoutButton.IsVisible = true;
        }
        else
        {
            AvatarImage.Source = "avatarlogout.png";
            WelcomeLabel.Text = "Not logged in.";
            LogoutButton.IsVisible = false;
        }
    }

    // LOGOUT
    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("username");
        SecureStorage.Remove("password");

        UpdateLoginStatus();

        await Task.Delay(1000);
        await Navigation.PushAsync(new LoginPage());
    }

    // CONNECTION
    private async void OnLoginSignupClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }

    // SETTINGS
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    // EXIT
    private void OnExitClicked(object sender, EventArgs e)
    {
#if ANDROID
        Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
#elif WINDOWS
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
}
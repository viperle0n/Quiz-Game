namespace QuizGame.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // SIGNUP
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageLabel.Text = "Please enter a username and password.";
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.IsVisible = true;
                return;
            }

            var apiService = new ApiService();
            var success = await apiService.RegisterAsync(username, password);

            if (success)
            {
                MessageLabel.TextColor = Colors.Green;
                MessageLabel.Text = "Account created successfully!";
                MessageLabel.IsVisible = true;
                await Task.Delay(1500);
                await Navigation.PopAsync();
            }
            else
            {
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Registration failed. Username may be taken.";
                MessageLabel.IsVisible = true;
            }
        }

        // LOGIN
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var apiService = new ApiService();
            bool loggedIn = await apiService.LoginAsync(UsernameEntry.Text, PasswordEntry.Text);

            if (loggedIn)
            {
                await DisplayAlert("Success", "You are logged in!", "OK");
                MessageLabel.TextColor = Colors.Green;
                MessageLabel.Text = "Login successful!";
                MessageLabel.IsVisible = true;

                await Task.Delay(1000);
                await Navigation.PushAsync(new MainMenuPage());
                await SecureStorage.SetAsync("username", UsernameEntry.Text);
            }
            else
            {
                await DisplayAlert("Error", "Login failed!", "Try again");
                MessageLabel.TextColor = Colors.Red;
                MessageLabel.Text = "Invalid username or password.";
                MessageLabel.IsVisible = true;
            }
        }
    }
}
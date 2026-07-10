using QuizGame.Views;
using Plugin.Maui.Audio;
using QuizGame.Services;

namespace QuizGame
{
    
    public partial class App : Application
    {
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _player;
        public App(BackgroundAudioService audioService)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainMenuPage());

            _ = audioService.StartBackgroundMusicAsync();
        }

    }
}
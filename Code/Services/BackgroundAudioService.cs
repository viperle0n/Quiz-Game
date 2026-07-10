using Plugin.Maui.Audio;

namespace QuizGame.Services
{
    public class BackgroundAudioService
    {
        private readonly IAudioManager _audioManager;
        private IAudioPlayer _player;

        public BackgroundAudioService(IAudioManager audioManager)
        {
            _audioManager = audioManager;
        }

        public async Task StartBackgroundMusicAsync()
        {
            if (_player != null && _player.IsPlaying)
                return;

            var stream = await FileSystem.OpenAppPackageFileAsync("theme.mp3");
            _player = _audioManager.CreatePlayer(stream);
            _player.Loop = true;
            _player.Play();
        }

        public void StopMusic()
        {
            _player?.Stop();
        }
    }
}
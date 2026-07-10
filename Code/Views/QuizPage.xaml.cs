using System.Timers;
using System.Text.Json;
using QuizGame.Models;
using QuizGame.Helpers;
using Microsoft.Maui.Graphics.Text;
using Plugin.Maui.Audio;

namespace QuizGame.Views;

public partial class QuizPage : ContentPage
{
    private string _category;
    private List<Question> _questions;
    private int _currentQuestionIndex = 0;
    private int _score = 0;
    private int _timeLeft = GameSettings.TimePerQuestionInSeconds;
    private System.Timers.Timer _timer;
    private readonly IAudioManager _audioManager = AudioManager.Current;
    private IAudioPlayer _correctSoundPlayer;
    private IAudioPlayer _wrongSoundPlayer;

    public QuizPage(string category)
    {
        InitializeComponent();
        _category = category;
        LoadCategoryQuestions();
        LoadSoundsAsync();
    }

    // LOAD CATEGORY QUESTIONS
    private async void LoadCategoryQuestions()
    {
        _questions = await LoadQuestionsByCategoryAsync(_category);
        ShowNextQuestion();
    }

    // SOUND FOR ANSWERS
    private async Task LoadSoundsAsync()
    {
        var correctStream = await FileSystem.OpenAppPackageFileAsync("correct.mp3");
        _correctSoundPlayer = _audioManager.CreatePlayer(correctStream);

        var wrongStream = await FileSystem.OpenAppPackageFileAsync("wrong.mp3");
        _wrongSoundPlayer = _audioManager.CreatePlayer(wrongStream);
    }

    // NEXT QUESTION
    private async void ShowNextQuestion()
    {
        if (_currentQuestionIndex >= _questions.Count)
        {
            await Navigation.PushAsync(new ResultsPage(_category, _score, _questions.Count));
            return;
        }

        var question = _questions[_currentQuestionIndex];
        QuestionLabel.Text = question.Text;
        OptionsContainer.Children.Clear();

        for (int i = 0; i < question.Options.Count; i++)
        {
            var button = new Button
            {
                Text = question.Options[i],
                CommandParameter = i,
                BackgroundColor = Colors.Black,
                TextColor = Colors.White
            };
            button.Clicked += OnOptionClicked;
            OptionsContainer.Children.Add(button);
        }

        ScoreLabel.Text = $"Score: {_score}";
        StartTimer();
    }

    // ANSWER
    private async void OnOptionClicked(object sender, EventArgs e)
    {
        StopTimer();

        var button = (Button)sender;
        int selected = (int)button.CommandParameter;
        int correct = _questions[_currentQuestionIndex].AnswerIndex;

        // BUTTON COLOR
        if (selected == correct)
        {
            _correctSoundPlayer?.Play();
            await FlashButton(button, Colors.Green, Colors.Black, 2); //ANIMATION
            _score++;
        }
        else
        {
            
            button.BackgroundColor = Colors.Red; // WRONG ANSWER
            button.TextColor = Colors.White;
            _wrongSoundPlayer?.Play();

            // SHOW CORRECT ANSWER
            foreach (var child in OptionsContainer.Children)
            {
                if (child is Button b && (int)b.CommandParameter == correct)
                {
                    b.BackgroundColor = Colors.Green;
                    b.TextColor = Colors.White;
                    break;
                }
            }
        }
        await Task.Delay(1000); // DELAY COLOR DIPLAY
        _currentQuestionIndex++;
        ShowNextQuestion();
    }

    // GRAPHICS
    private async Task FlashButton(Button button, Color color1, Color color2, int flashes)
    {
        for (int i = 0; i < flashes; i++)
        {
            button.BackgroundColor = color1;
            await Task.Delay(200);
            button.BackgroundColor = color2;
            await Task.Delay(200);
        }

        // End on the original color
        button.BackgroundColor = color1;
    }

    // TIMER START
    private void StartTimer()
    {
        _timeLeft = GameSettings.TimePerQuestionInSeconds;
        TimerLabel.Text = $"Time: {_timeLeft}";

        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += OnTimerTick;
        _timer.Start();
    }

    // TIMER TICK
    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        _timeLeft--;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            TimerLabel.Text = $"Time: {_timeLeft}";
        });

        if (_timeLeft <= 0)
        {
            _timer.Stop();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                _currentQuestionIndex++;
                ShowNextQuestion();
            });
        }
    }

    // TIMER STOP
    private void StopTimer()
    {
        if (_timer != null)
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }

    // LOAD QUESTIONS FROM JSON FILES
    private async Task<List<Question>> LoadQuestionsByCategoryAsync(string category)
    {
        string filename = $"Data/{category.ToLower()}.json";
        using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        var questions = JsonSerializer.Deserialize<List<Question>>(json);
        return questions;
    }
}
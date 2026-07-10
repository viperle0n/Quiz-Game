using System.Text.Json;
using QuizGame.Models;

namespace QuizGame.ViewModels
{
    public class QuizViewModel
    {
        //LOAD QUESTIONS
        public async Task<List<Question>> LoadQuestionsByCategoryAsync(string category)
        {
            string filename = $"Data/{category.ToLower()}.json";

            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();

            var questions = JsonSerializer.Deserialize<List<Question>>(json);
            return questions ?? new List<Question>();
        }
    }
}
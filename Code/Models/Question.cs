using System.Text.Json.Serialization;

namespace QuizGame.Models;

public class Question
{
    [JsonPropertyName("questionText")]
    public string Text { get; set; }

    [JsonPropertyName("options")]
    public List<string> Options { get; set; } = new List<string>();

    [JsonPropertyName("correctIndex")]
    public int AnswerIndex { get; set; }
}

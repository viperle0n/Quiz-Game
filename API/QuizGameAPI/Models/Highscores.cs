using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGameAPI.Models
{
    [Table("highscores")]
    public class Highscores
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Category { get; set; }

        public int Highscore { get; set; }

        public User User { get; set; }
    }
}

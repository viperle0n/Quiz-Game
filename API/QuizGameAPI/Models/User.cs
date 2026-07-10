using System.ComponentModel.DataAnnotations.Schema;

namespace QuizGameAPI.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int id { get; set; }

        [Column("username")]
        public string username { get; set; }

        [Column("password_hash")]
        public string password_hash { get; set; }

        public ICollection<Highscores> Highscores { get; set; }
    }
}

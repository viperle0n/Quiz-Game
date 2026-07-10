using QuizGame.Views;

namespace QuizGame
{

    public partial class CategoryPage : ContentPage
    {
        public CategoryPage ()
        {
            InitializeComponent();
        }

        // CATEGORY
        private async void OnCategoryClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var category = button.CommandParameter.ToString();
 
            await Navigation.PushAsync(new QuizPage(category));
        }
    }
}
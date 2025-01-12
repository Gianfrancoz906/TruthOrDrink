namespace TruthOrDrink;


    public partial class QuestionPage : ContentPage
{
    private readonly DatabaseService _databaseService;

    public QuestionPage()
    {
        InitializeComponent();

        // Initialize the database service
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TruthOrDrink.db");
        _databaseService = new DatabaseService(dbPath);

        LoadQuestions();
    }

    private async void LoadQuestions()
    {
        var questions = await _databaseService.GetQuestionsAsync();
        QuestionsListView.ItemsSource = questions; // Bind to the CollectionView
    }

    private async void AddQuestionClicked(object sender, EventArgs e)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(QuestionEntry.Text))
        {
            await DisplayAlert("Error", "Question cannot be empty.", "OK");
            return;
        }

        // Create a new question
        var newQuestion = new Question
        {
            Content = QuestionEntry.Text.Trim(),
            Category = string.IsNullOrWhiteSpace(CategoryEntry.Text) ? "Uncategorized" : CategoryEntry.Text.Trim()
        };

        // Add to the database
        await _databaseService.AddQuestionAsync(newQuestion);

        // Clear input fields
        QuestionEntry.Text = string.Empty;
        CategoryEntry.Text = string.Empty;

        // Reload the list
        LoadQuestions();
    }

    private async void DeleteQuestionClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var question = button?.BindingContext as Question;
        if (question != null)
        {
            await _databaseService.DeleteQuestionAsync(question);
            LoadQuestions();
        }
    }
}


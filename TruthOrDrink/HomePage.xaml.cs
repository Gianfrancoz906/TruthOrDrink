namespace TruthOrDrink;

public partial class HomePage : ContentPage
{
    private readonly DatabaseService _databaseService;
    private List<Question> _questions;
    private int _currentQuestionIndex;
    private int _currentPlayerIndex;
    private int _playerCount;
    

    public HomePage()
    {
        InitializeComponent();

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TruthOrDrink.db");
        _databaseService = new DatabaseService(dbPath);

        _questions = new List<Question>();
        _currentQuestionIndex = -1;
        _currentPlayerIndex = 0;
        _playerCount = 1;
        
    }

    private async void OnNextQuestionTapped(object sender, EventArgs e)
    {
        if (_questions.Count == 0)
        {
            _questions = await _databaseService.GetQuestionsAsync();

            if (_questions.Count == 0)
            {
                await DisplayAlert("No Questions", "The database is empty. Add some questions first!", "OK");
                return;
            }

            _questions = _questions.OrderBy(q => Guid.NewGuid()).ToList();
        }

        _currentQuestionIndex++;

        if (_currentQuestionIndex < _questions.Count)
        {
            var currentQuestion = _questions[_currentQuestionIndex];
            QuestionLabel.Text = currentQuestion.Content;

            
            RotateQuestionLabel();
        }
        else
        {
            
            await DisplayAlert("Game Over", "You've answered all the questions! Tap anywhere to restart.", "OK");

            
            _questions.Clear();
            _currentQuestionIndex = -1;
            _currentPlayerIndex = 0;
        }
    }

    private void OnPlayerCountChanged(object sender, ValueChangedEventArgs e)
    {
        _playerCount = (int)e.NewValue;
        PlayerCountLabel.Text = $"Players: {_playerCount}";
        _currentPlayerIndex = 0; 
    }

    private void RotateQuestionLabel()
    {
        
        QuestionLabel.Rotation = _currentPlayerIndex * (360 / _playerCount);

        
        _currentPlayerIndex = (_currentPlayerIndex + 1) % _playerCount;
    }
}

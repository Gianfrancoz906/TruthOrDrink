namespace TruthOrDrink;

public partial class SettingsPage : ContentPage
{
    public int MaxQuestions { get; set; }

    public SettingsPage()
    {
        InitializeComponent();

        
        MaxQuestions = Preferences.Get("MaxQuestions", 10); 
        MaxQuestionsEntry.Text = MaxQuestions.ToString();
    }

    private void OnSaveClicked(object sender, EventArgs e)
    {
        
        if (int.TryParse(MaxQuestionsEntry.Text, out int maxQuestions))
        {
            Preferences.Set("MaxQuestions", maxQuestions);
            DisplayAlert("Settings", "Maximum number of questions saved.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please enter a valid number.", "OK");
        }
    }
}

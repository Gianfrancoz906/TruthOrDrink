namespace TruthOrDrink
{
    public partial class MainPage : ContentPage
    {
        int errorCount = 0;

        public MainPage()
        {
            InitializeComponent();
        }
       

       
        private async void logInButton_Clicked(object sender, EventArgs e)
        {
            if (usernameEntry.Text == "a" && passwordEntry.Text == "a")
            {
                errorLabel.Text = "";
                helpLabel.Text = "";

                await Shell.Current.GoToAsync("//HomePage");
            }

            if (usernameEntry.Text == "" || passwordEntry.Text == "")
            {
                errorLabel.Text = "Please fill in both fields";
                errorCount++;

            }
            if (usernameEntry.Text !=  "a" && passwordEntry.Text != "a")
            {
                errorLabel.Text = "Login failed, please check username and password";
                errorCount++;

            }
            if (errorCount > 3)
            {
                helpLabel.Text = "Message +31639586077 for help";
            }
        }
        
    }

}

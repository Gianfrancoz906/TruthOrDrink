namespace TruthOrDrink;

public partial class FullScreenImagePage : ContentPage
{
	public FullScreenImagePage(string imagePath)
	{
		InitializeComponent();
        Title = "";
        BindingContext = new { ImagePath = imagePath };
    }
}
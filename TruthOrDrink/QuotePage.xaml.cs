namespace TruthOrDrink;

public partial class QuotePage : ContentPage
{
    private readonly QuoteService _quoteService;
    public QuotePage()
	{
		InitializeComponent();
        _quoteService = new QuoteService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var quote = await _quoteService.GetRandomQuoteAsync();
        if (quote != null)
        {
            QuoteLabel.Text = $"\"{quote.q}\" - {quote.a}";
        }
    }
}
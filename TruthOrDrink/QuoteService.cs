using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TruthOrDrink
{
    public class QuoteService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<Quote> GetRandomQuoteAsync()
        {
            var response = await client.GetStringAsync("https://zenquotes.io/api/random");
            var quotes = JsonSerializer.Deserialize<List<Quote>>(response);
            return quotes?.FirstOrDefault();
        }
    }
}

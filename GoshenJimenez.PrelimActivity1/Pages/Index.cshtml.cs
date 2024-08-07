using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GoshenJimenez.PrelimActivity1.Pages
{
    public class Index : PageModel
    {
        private readonly ILogger<Index> _logger;

        public Index(ILogger<Index> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public Result? DataResult { get; set; }


        public async Task<IActionResult> OnGet()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("https://api.nationalize.io/?name=nathaniel");

            if (response.IsSuccessStatusCode)
            {
                string? content = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(content))
                {
                    Result? result = JsonConvert.DeserializeObject<Result>(content);

                    if (result != null)
                    {
                        this.DataResult = result;                        
                    }
                }
            }

            return Page();
        }

        public class Result
        {
            [JsonPropertyName("count")]
            public int? Count { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonProperty("country")]
            public List<ResultCountry>? Countries { get; set; }
        }

        public class ResultCountry
        {
            [JsonProperty("country_id")]
            public string? CountryId { get; set; }

            [JsonPropertyName("probability")]
            public decimal? Probability { get; set; }
        }
    }
}

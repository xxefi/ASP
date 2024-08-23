using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorCV.Pages
{
    public class HomeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public HomeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}

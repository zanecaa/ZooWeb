using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ZooWeb.Pages.Home
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            if (Username == "your_username" && Password == "your_password")
            {
                return RedirectToPage("/SuccessPage");
            }
            else
            {
                return Page();
            }
        }
    }
}
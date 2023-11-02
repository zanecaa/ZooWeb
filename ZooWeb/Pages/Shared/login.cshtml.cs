using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ZooWeb.Pages.Shared
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public void OnPost()
        {
            // Validate username and password and perform login action
            // Redirect to the main page if login is successful
            // Show an error message if login fails
        }
    }
}

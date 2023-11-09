using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;


namespace ZooWeb.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            
            // Validate username and password
            if (IsValidUser(Username, Password))
            {

                //indicate the user is authenticated.
                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, "admin"),
                    new Claim("user", "admin"),
                    // Add other claims as needed
                };

                var identity = new ClaimsIdentity(claims, "admin");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("admin", claimsPrincipal);

                // Redirect to the main page if login is successful
                return RedirectToPage("/Home");
            }
            else
            {
                //if login fails
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            //validate the user here.
            if (username == "zooadmin" && password == "password")
            {
                return true;
            }

            return false;
        }
    }
}

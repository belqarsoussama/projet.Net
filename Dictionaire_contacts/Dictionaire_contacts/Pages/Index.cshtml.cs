using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dictionaire_contacts.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnPost()
        {
            // Retrieve the username and password from the form submission
            var username = Request.Form["username"];
            var password = Request.Form["password"];

            // Check if the username and password match the static values
            if (username == "admin" && password == "123")
            {
                // Redirect to the contact page upon successful login
                return RedirectToPage("/Clients/Index");
            }

            // If login fails, reload the page or display an error message
            // For simplicity, we're just reloading the page here
            return Page();
        }
    }
}

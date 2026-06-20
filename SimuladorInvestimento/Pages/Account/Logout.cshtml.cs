using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorInvestimento.Models;

namespace SimuladorInvestimento.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LogoutModel(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Account/Login");
        }
    }
}

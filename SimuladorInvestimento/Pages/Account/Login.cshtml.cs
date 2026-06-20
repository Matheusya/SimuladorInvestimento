using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorInvestimento.Models;
using System.ComponentModel.DataAnnotations;

namespace SimuladorInvestimento.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;

        public LoginModel(SignInManager<Usuario> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email é obrigatório")]
            [EmailAddress(ErrorMessage = "Email inválido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Senha é obrigatória")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lembrar-me")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Email ou senha inválidos");
            return Page();
        }

        public void OnGet(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
        }
    }
}

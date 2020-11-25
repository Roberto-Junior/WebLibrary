using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BiblioTechA.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BiblioTechA.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [EmailAddress(ErrorMessage = "Email inválido.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Campo obrigatório.")]
            [StringLength(100, ErrorMessage = "A {0} tem que ter entre {2} a {1} digitos.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Senha")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("Password", ErrorMessage = "As senhas não correspondem.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        //public IActionResult OnGet(string code = null)
        public void Get(string code = null)
        {
            //if (code == null)
            //{
            //    return BadRequest("A code must be supplied for password reset.");
            //}
            //else
            //{
            //    Input = new InputModel
            //    {
            //        Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
            //    };
            //    return Page();
            //}
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Input.Email = TempData["UserEmail"].ToString();
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                TempData["msg"] = "<script>alert('Email Incorreto!');</script>";
                return RedirectToPage("./ForgotPassword");
            }          

            Input.Code = await _userManager.GeneratePasswordResetTokenAsync(user); //codigo add
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                TempData["msg"] = "<script>alert('Senha Resetada com Sucesso!');</script>";
                //return RedirectToPage("./ResetPasswordConfirmation");
                return RedirectToPage("/Index"); //código adicionado 
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}

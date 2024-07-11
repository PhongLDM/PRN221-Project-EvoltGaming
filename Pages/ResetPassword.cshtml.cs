using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EvoltingStore.Entity;
using System.Linq;
using System;

namespace EvoltingStore.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string Message { get; set; }

        public string MessageType { get; set; }

        public void OnGet(string token)
        {
            Token = token;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(NewPassword))
            {
                Message = "Password is required.";
                MessageType = "danger";
                return Page();
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                Message = "Confirm Password does not match.";
                MessageType = "danger";
                return Page();
            }

            var user = context.Users.FirstOrDefault(u => u.ResetToken.Equals(Token) && u.ResetTokenExpiry > DateTime.Now);
            if (user == null)
            {
                Message = "Invalid or expired token.";
                MessageType = "danger";
                return Page();
            }

            // Update password and clear the token
            user.Password = NewPassword;
            user.ResetToken = null;
            user.ResetTokenExpiry = null;
            context.SaveChanges();

            Message = "Your password has been reset. You can now log in with your new password.";
            MessageType = "success";
            return Redirect("/Login");
        }
    }
}
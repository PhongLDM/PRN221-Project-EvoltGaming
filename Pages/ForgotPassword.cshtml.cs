using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EvoltingStore.Entity;
using EvoltingStore.EmailService;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System;

namespace EvoltingStore.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly EvoltingStoreContext context;
        private readonly IEmailSender emailSender;

        public ForgotPasswordModel(EvoltingStoreContext context, IEmailSender emailSender)
        {
            this.context = context;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public string Email { get; set; }

        public string Message { get; set; }

        public string MessageType { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email))
            {
                Message = "Email is required.";
                MessageType = "danger";
                return Page();
            }

            var userDetail = context.UserDetails.FirstOrDefault(ud => ud.Email.Equals(Email));
            if (userDetail == null)
            {
                Message = "Email not found.";
                MessageType = "danger";
                return Page();
            }

            var user = context.Users.FirstOrDefault(u => u.UserId == userDetail.UserId);
            if (user == null)
            {
                Message = "User not found.";
                MessageType = "danger";
                return Page();
            }

            // Generate reset token
            string token = GenerateToken();
            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);
            context.SaveChanges();

            // Send email with the reset link
            string resetLink = Url.Page("/ResetPassword", null, new { token = token }, Request.Scheme);
            emailSender.SendEmailAsyns(Email, "Password Reset", $"Reset your password using this link: {resetLink}");

            Message = "A reset link has been sent to your email.";
            MessageType = "success";
            return Page();
        }

        private string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[32];
                rng.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }
    }
}

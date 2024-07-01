using EvoltingStore.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EvoltingStore.Pages
{
    public class ProfileModel : PageModel
    {
        private EvoltingStoreContext context = new EvoltingStoreContext();

        public void OnGet()
        {
        }

        public void OnPostPassword(String username, String password)
        {
            User user = context.Users.First(u => u.Username == username);
            if (user != null)
            {
                user.Password = password;

                context.SaveChanges();
            }

            String jsonUser = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            HttpContext.Session.SetString("user", jsonUser);
        }

        public void OnPostDetail(int userId, String firstName, String lastName, String email)
        {
            UserDetail userDetail = context.UserDetails.First(u => u.UserId == userId);
            if (userDetail != null)
            {
                userDetail.FirstName = firstName;
                userDetail.LastName = lastName;
                userDetail.Email = email;

                context.SaveChanges();

                String jsonUserDetail = Newtonsoft.Json.JsonConvert.SerializeObject(userDetail);
                HttpContext.Session.SetString("userDetail", jsonUserDetail);
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PhoneService.DatabaseContext;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PhoneService.Areas.HashingPassword;
using PhoneService.Dtos.AdminDtos;
using System.Net.Mail;
using System.Net;
using PhoneService.MailService;
using PhoneService.Models;


namespace PhoneService.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AdminController : Controller
    {

        private DataContext context;

        public AdminController(DataContext context)
        {
            this.context = context;
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(PhoneService.Entities.Admin data)
        {
            if (data != null)
            {
                string hashedPassword = PasswordHasher.HashPassword(data.Password);
                data.Password = hashedPassword;
                context.Admins.Add(data);
                data.Approved = false;
                context.SaveChanges();

                return RedirectToAction("Login");
            }

            ViewBag.error = "Bir hata oluştu";
            return View();
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(PhoneService.Entities.Admin data)
        {

            var passwrodfind = context.Admins.Where(x => x.Email == data.Email).Select(x => x.Password).FirstOrDefault();

            var userapproved = context.Admins.Where(x => x.Email == data.Email).Select(x => x.Approved).FirstOrDefault();

            bool isPasswordValid = PasswordHasher.VerifyPassword(passwrodfind, data.Password);

            if (isPasswordValid == true)
            {


                if (data.Email != null && data.Password != null)
                {

                    if (userapproved == true)
                    {

                        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, data.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {

                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);


                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ViewBag.error = "Hesabınız henüz onaylanmadı";
                        return View();
                    }
                }



            }
            else
            {
                ViewBag.error = "Eposta ya da şifreniz hatalı.";
                return View();
            }


            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login");
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(AdminPasswordChangeDto adminPasswordChangeDto)
        {
          
            var oldPasswordHash = context.Admins
                .Where(a => a.Email == User.Identity.Name)
                .Select(a => a.Password)
                .FirstOrDefault();
             

            if (PasswordHasher.VerifyPassword(oldPasswordHash, adminPasswordChangeDto.OldPassword))
            {
               
                string newHashedPassword = PasswordHasher.HashPassword(adminPasswordChangeDto.NewPassword);

              
                var admin = context.Admins.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                admin.Password = newHashedPassword;
                context.SaveChanges();

                ViewBag.SuccessMessage = "Şifre başarıyla değiştirildi.";
                return View();
            }
            else
            {
               
                ModelState.AddModelError("OldPassword", "Eski şifre hatalı.");
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var user = context.Admins.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "No account associated with this email address was found.";
                return View();
            }


            var resetToken = Guid.NewGuid().ToString();
            user.ResetPasswordToken = resetToken;
            user.ResetPasswordExpiration = DateTime.UtcNow.AddHours(1); 
            context.SaveChanges();

            
            var callbackUrl = Url.Action("ResetPassword", "Admin", new { token = resetToken, Area="Admin"}, protocol: HttpContext.Request.Scheme);

            
            MailSender.SendMail("Password Reset", $"Please click the following link to reset your password: {callbackUrl}", email);
           
            ViewBag.SuccessMessage = "Password reset link sent to your email address.";
            return View();
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token)
        {
            ViewBag.token = token;
            return View(new ResetPasswordViewModel { Token = token });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = context.Admins.SingleOrDefault(u => u.ResetPasswordToken == model.Token && u.ResetPasswordExpiration > DateTime.UtcNow);

            if (user == null)
            {
                ViewBag.ErrorMessage = "An invalid or expired password reset link.";
                return View();
            }

      
            string newHashedPassword = PasswordHasher.HashPassword(model.NewPassword);
            user.Password = newHashedPassword;

            user.ResetPasswordToken = null;
            user.ResetPasswordExpiration = null;

            context.SaveChanges();

            ViewBag.SuccessMessage = "Your password has been successfully reset.";
            return View(model);
        }
    }
}


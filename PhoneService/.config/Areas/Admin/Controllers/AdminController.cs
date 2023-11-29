using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PhoneService.DatabaseContext;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PhoneService.Areas.HashingPassword;
using PhoneService.Dtos.AdminDtos;

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
            // Kullanıcının eski şifresini veritabanından al
            var oldPasswordHash = context.Admins
                .Where(a => a.Email == User.Identity.Name)
                .Select(a => a.Password)
                .FirstOrDefault();
             
            // Eski şifreyi doğrula
            if (PasswordHasher.VerifyPassword(oldPasswordHash, adminPasswordChangeDto.OldPassword))
            {
                // Yeni şifreyi hashle
                string newHashedPassword = PasswordHasher.HashPassword(adminPasswordChangeDto.NewPassword);

                // Yeni şifreyi güncelle
                var admin = context.Admins.Where(a => a.Email == User.Identity.Name).FirstOrDefault();
                admin.Password = newHashedPassword;
                context.SaveChanges();

                // Şifre değiştirme başarılı mesajı veya yönlendirme ekleyin
                ViewBag.SuccessMessage = "Şifre başarıyla değiştirildi.";
                return View();
            }
            else
            {
                // Eski şifre yanlış ise hata mesajı gösterin
                ModelState.AddModelError("OldPassword", "Eski şifre hatalı.");
                return View();
            }
        }
    }
}

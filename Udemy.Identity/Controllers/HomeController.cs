using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Udemy.Identity.Entity;
using Udemy.Identity.Models;

namespace Udemy.Identity.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;




        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            
        }

        public IActionResult Index()
        {
           
            
            return View();
        }

        public IActionResult Create()
        {
            return View(new UserCreateModel());  
        }

       [HttpPost]
        public async Task <IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                { 
                    UserName = model.Username,
                    Email = model.Email,
                    Gender = model.Gender,

                
                
                };

              

          var IdentityResult =   await _userManager.CreateAsync(user, model.Password);

               
                if (IdentityResult.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");

                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new() { Name = "Member", CreatedTime = DateTime.Now });

                    }


                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");
                }

                foreach (var error in IdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
           
                return View(model);
            
        }


        public IActionResult SignIn(string returnUrl)
        {
          
            return View(new UserSignInModel() {  ReturnUrl = returnUrl});
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
            var signInResult =  await  _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            

                if (signInResult.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("Panel");
                    }
                }
                
                
                    ModelState.AddModelError("", "Kullanıcı adı ve şifre hatalı");
                
              /*  else if (signInResult.IsLockedOut)
                {
                    //hesap kilitli mi
                }
                else if (signInResult.IsNotAllowed)
                {
                    // email, phone number dogrulanmış mı
                }  */

            }
            return View(model);
        }


        [Authorize]
        public IActionResult GetUserInfo()
        {
           var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            User.IsInRole("Member");


            return View();  
        }


        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles ="Member")]
        public IActionResult Panel()
        {
            return View();
        }


        [Authorize(Roles = "Member")]
        public IActionResult MemberPage()
        {
            return View();
        }

        public async Task< IActionResult> SignOut()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


    }
}

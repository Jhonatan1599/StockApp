using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using StockMarketSolution.Controllers;
using Stocks.Core.Domain.IdentityEntities;
using Stocks.Core.Enums;

namespace Stocks.Web.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).
                                    Select(temp => temp.ErrorMessage);
                return View(registerDTO);
            }

            //Create an object of ApplicationUser
            ApplicationUser user = new ApplicationUser()
            {
                /*The id will be generated automatically and remains properties will be initialized automatically */
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                //UserName will be used for login purpose
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };

            //Create a user
            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (registerDTO.UserType == UserTypeOptions.Admin)
                {
                    //Step 1, Create the role in AspNetRoles table
                    //Create "Admin" role
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserTypeOptions.Admin.ToString()
                        };
                        await _roleManager.CreateAsync(applicationRole);
                    }
                    //Step2, Create the row into AspNetUserRoles table
                    //Add the new user into "Admin" role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    //Add the new user into "User" role
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                //Sign in
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(StocksController.Explore), "Stocks");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
            }

            return View(registerDTO);



        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(ReturnUrl);
            }

            ModelState.AddModelError("Login", "Invalid email or password");

            return View(loginDTO);
        }


        public async Task<IActionResult> Logout()
        {
            //Remove the identity cookie
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(StocksController.Explore), "Stocks");
        }


        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true); //valid
            }
            else
            {
                return Json(false); //invalid
            }
        }
    }
}

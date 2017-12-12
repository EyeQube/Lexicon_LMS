﻿using Lexicon_LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace Lexicon_LMS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private ApplicationDbContext _context;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;




        public AccountController()
        {
            _context = new ApplicationDbContext();
        }




        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _context = new ApplicationDbContext();
            UserManager = userManager;
            SignInManager = signInManager;
        }





        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }




        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }




        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }




        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }





        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }





        [Authorize(Roles = Role.Teacher)]
        public ActionResult Register(string role, int? courseid, string message)
        {
            var viewModel = new RegisterViewModel
            {
                Role = role ?? Role.Student,
                Courses = _context.Courses,
                CourseId = courseid
            };
            if (message != null)
                ViewBag.Message = message;
            return View("Register", viewModel);
        }




        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel viewModel, string button)
        {

            if (ModelState.IsValid)
            {
                //TODO: Validate courseId and Role

                var user = new ApplicationUser { UserName = viewModel.Email, Email = viewModel.Email, FirstName = viewModel.FirstName, LastName = viewModel.LastName, CourseId = viewModel.CourseId };
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, viewModel.Role);

                    if (button == "SaveNew")
                    {
                        viewModel.Courses = _context.Courses;
                        var message = $"Successfully added user \"{user.Email}\"";
                        return RedirectToAction("Register", "Account", new { role = viewModel.Role, courseid = viewModel.CourseId, message = message });

                    }

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);

            }

            viewModel.Courses = _context.Courses;
            return View(viewModel);
        }

        // Note: reusing parts of RegisterViewModel
        //[Authorize(Roles = Role.Teacher)]
        //public ActionResult EditUser(string userName)
        //{
        //    var User = _context.Users.FirstOrDefault(u => u.UserName == userName);
        //    var viewModel = new RegisterViewModel
        //    {
        //        Id = User.Id,
        //        Role = User.Roles.ToString(),
        //        FirstName = User.FirstName,
        //        LastName = User.LastName,
        //        Email = User.Email,
        //        Password = User.PasswordHash
        //    };

        //    return View("EditUser", viewModel);
        //}




        [Authorize(Roles = Role.Teacher)]
        public ActionResult EditUser(string userId)
        {
            var User = _context.Users.FirstOrDefault(u => u.Id == userId);
            var viewModel = new RegisterViewModel
            {
                Id = User.Id,
                Role = _context.Roles.Find(User.Roles.First().RoleId).Name,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Email = User.Email,
                Password = User.PasswordHash,
                CourseId = User.CourseId,
                Courses = _context.Courses
            };

            return View("EditUser", viewModel);
        }




        // POST: /Account/Register
        //[HttpPost]
        //[Authorize(Roles = Role.Teacher)]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditUser(RegisterViewModel viewModel)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var User = _context.Users.FirstOrDefault(u => u.UserName == viewModel.Email);
        //        User.FirstName = viewModel.FirstName;
        //        User.LastName = viewModel.LastName;
        //        User.Email = viewModel.Email;
        //        var result = _context.SaveChanges();
        //        return RedirectToAction("ListUsers");
        //    }
        //    return RedirectToAction("Home");
        //}



        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(RegisterViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                var User = _context.Users.FirstOrDefault(u => u.Id == viewModel.Id);
                User.FirstName = viewModel.FirstName;
                User.LastName = viewModel.LastName;
                User.Email = viewModel.Email;
                User.CourseId = viewModel.CourseId;
                var result = _context.SaveChanges();
                return RedirectToAction("ListUsers");
            }
            return RedirectToAction("EditUser", "AccountController");
        }




        public ActionResult ListUsers(string searchString)
        {
           
            var rolesLookup = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

            var Users = _context.Users.ToList();

            var users = from s in _context.Users select s;
            if (!String.IsNullOrEmpty(searchString))
            {

                users = users.Where(s => s.LastName.Contains(searchString)
                                   || s.FirstName.Contains(searchString));

                ViewBag.Bool = true;
                ViewBag.SearchString = searchString;
                
                ViewBag.RolesLookup = rolesLookup;
                return View(users.ToList());
            }

            ViewBag.Bool = true;
            // Generate lookup table for roles to use in list view
            //var rolesLookup = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            ViewBag.RolesLookup = rolesLookup;

            return View(Users);
        }
            




        public ActionResult SortUsers(string FirstSortOrder, string LastSortOrder, string searchString, bool boll)
        {
            var rolesLookup = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
            var users = from s in _context.Users select s;

            if (!String.IsNullOrEmpty(searchString))
            {

                users = users.Where(s => s.LastName.Contains(searchString)
                                   || s.FirstName.Contains(searchString));

                ViewBag.Bool = boll;


                ViewBag.RolesLookup = rolesLookup;
                return View("ListUsers", users.ToList());
            }


            if (boll == true && FirstSortOrder != null)
            {
                users = users.OrderByDescending(s => s.FirstName);
            }
            else if(boll == false && FirstSortOrder != null)
            {
                users = users.OrderBy(s => s.FirstName);
            }
            else if(boll == true && LastSortOrder != null)
            {
                users = users.OrderByDescending(s => s.LastName);
            }
            else if(boll == false && LastSortOrder != null)
            {
                users = users.OrderBy(s => s.LastName);
            }
            else
            {
                users = users.OrderBy(s => s.LastName);
            }
                

            ViewBag.Bool = boll == true ? false : true;
            
            ViewBag.RolesLookup = rolesLookup;


            return View("ListUsers", users.ToList());
        }





        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }





        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }






        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }





        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }





        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }





        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }




        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }




        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }




        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }




        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }




        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }




        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }




        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }




        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
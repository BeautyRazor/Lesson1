using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using WebApplication1.App_Start;
using WebApplication1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return System.Web.HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (model.Email.ToLower() == "beautyrazor")
            {
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, model.Email));
                claims.Add(new Claim(ClaimTypes.Email, model.Email));
                var id = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;
                authenticationManager.SignIn(id);
            }


            //this.SignInManager.SignIn(new ApplicationUser()
            //{
            //    Id = model.Email,
            //    Email = "ramm@gmail.com",
            //    EmailConfirmed = true,

            //}, model.RememberMe, false);

            return View();
        }
    }
}
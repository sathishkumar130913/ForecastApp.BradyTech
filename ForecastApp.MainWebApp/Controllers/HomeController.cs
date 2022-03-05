using ForecastApp.MainWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis;
using Microsoft.AspNetCore.Authentication.Google;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;


namespace ForecastApp.MainWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task Login()
        {
            try
            {

                await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("GoogleResponse")
                }); ;
            }
            catch(Exception ex)
            {

                throw ex;
            }

        }

        public async Task<ActionResult> GoogleResponse()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var cliams = result.Principal.Identities.FirstOrDefault().Claims.Select(cliam => new
                {
                    cliam.Issuer,
                    cliam.OriginalIssuer,
                    cliam.Type,
                    cliam.Value
                });

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }
}

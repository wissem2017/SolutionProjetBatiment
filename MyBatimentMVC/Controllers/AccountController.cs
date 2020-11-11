using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyBatimentMVC.Models;
using MyBatimentMVC.ViewModels;

using Newtonsoft.Json;

namespace MyBatimentMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _Config;
        private string URLBase
        {
            get
            {
                return _Config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }

        public AccountController(IConfiguration Config)
        {
            _Config = Config;
        }
        public IActionResult Login()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    var user = new User();
                    user.Username = loginViewModel.Username;
                    user.Password = loginViewModel.Password;
                    string stringData = JsonConvert.SerializeObject(user);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(URLBase + "User/authenticate", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        string stringJWT = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var jwtString = jwt["token"].ToString();
                        var userId = jwt["id"].ToString();
                        HttpContext.Session.SetString("token", jwtString);//username

                        HttpContext.Session.SetString("username", jwt["username"].ToString());

                        HttpContext.Session.SetString("Id", userId);

                        ViewBag.Message = "User logged in successfully!" + jwt["username"].ToString();
                        return RedirectToAction("Index", "Home");
                    }

                }
            }

            return View();
        }
        public IActionResult Index()
        {
            return View();

        }
        public IActionResult Register()
        {
            var register = new RegisterViewModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    string stringData = JsonConvert.SerializeObject(register);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(URLBase + "User/register", contentData);
                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        string stringJWT = response.Content.ReadAsStringAsync().Result;
                        var jwt = JsonConvert.DeserializeObject<System.IdentityModel.Tokens.Jwt.JwtPayload>(stringJWT);
                        var jwtString = jwt["token"].ToString();
                        HttpContext.Session.SetString("token", jwtString);//username

                        HttpContext.Session.SetString("username", jwt["username"].ToString());

                        ViewBag.Message = "User logged in successfully!" + jwt["username"].ToString();
                        return RedirectToAction("Index", "Home");

                    }
                }
            }
            return View();
        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = new RegisterViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "user/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    user = JsonConvert.DeserializeObject<RegisterViewModel>(apiResponse);
                }
            }
            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    //--> Récupérer Token de session
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "Vous devez être authentifié";
                        return View(register);
                    }

                    string stringData = JsonConvert.SerializeObject(register);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PutAsync(URLBase + "user/updateUser", contentData);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(register);
                }
            }
            return View();
        }

        public IActionResult LogOff()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

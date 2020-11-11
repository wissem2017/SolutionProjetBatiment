using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MyBatimentMVC.Models;
using MyBatimentMVC.ViewModels;

using Newtonsoft.Json;
using System.Net.Mail;

namespace MyBatimentMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var listBatiment = new ListBatimentViewModel();
            var listProject = new List<ProjectItem>();
            var listService = new List<ServiceItem>();
            var listOwner = new List<Owner>();

            using(var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "projectitem"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    listProject = JsonConvert.DeserializeObject<List<ProjectItem>>(apiResponse);
                }

                using (var respense = await httpClient.GetAsync(URLBase + "ServiceItem"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();
                    listService = JsonConvert.DeserializeObject<List<ServiceItem>>(apiResponse);
                }

                using (var respense = await httpClient.GetAsync(URLBase + "Owner"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();
                    listOwner = JsonConvert.DeserializeObject<List<Owner>>(apiResponse);
                }
            }

            listBatiment.Owner = listOwner.First();
            listBatiment.ListeService = listService;
            listBatiment.ListProject = listProject;



            return View(listBatiment);
        }

        [HttpPost]
        public IActionResult Index(ListBatimentViewModel emailViewModel)
        {
            string to = "wissemwar@gmail.com";
            string subject = emailViewModel.Subject;
            string body ="De : "+ emailViewModel.To +"\n"+ emailViewModel.Body;

            MailMessage email = new MailMessage();
            email.To.Add(to);
            email.Subject = subject;
            email.Body = body;
            email.From = new MailAddress("wissemwar@gmail.com");
            email.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("wissemwar@gmail.com", "AliAhmedZayneb*30061979");
           
            smtp.Send(email);

            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }
    }
}

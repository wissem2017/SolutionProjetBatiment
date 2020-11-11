using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyBatimentMVC.Models;
using MyBatimentMVC.ViewModels;
using Newtonsoft.Json;

namespace MyBatimentMVC.Controllers
{
    public class ServiceItemController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IHostingEnvironment _hosting;
        private string URLBase
        {
            get
            {
                return _config.GetSection("BaseURL").GetSection("URL").Value;
            }
        }
        public ServiceItemController(IConfiguration Config, IHostingEnvironment hosting)
        {
            _config = Config;
            _hosting = hosting;
        }

        public async Task<IActionResult> Index()
        {
            var listSerivce = new List<ServiceItem>();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "Serviceitem"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    listSerivce = JsonConvert.DeserializeObject<List<ServiceItem>>(apiResponse);
                }
            }
            return View(listSerivce);
        }

        public async Task<IActionResult> AddService()
        {
            var serviceItemViwModel = new ServiceItemViewModel();

            return View(serviceItemViwModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceItemViewModel serviceItemModelView)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    if (serviceItemModelView.File != null)
                    {
                        //WebRootPath retourne chemain de wwwroot
                        string uploads = Path.Combine(_hosting.WebRootPath, @"img\services");
                        //Ajout le chemain de nouveau fichier
                        string fullPath = Path.Combine(uploads, serviceItemModelView.File.FileName);
                        //Fait copier fichier dans ce chemain
                        //model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            serviceItemModelView.File.CopyTo(stream);
                        }

                    }

                    var serviceItem = new ServiceItem()
                    {

                        //Id = Guid.Parse(ServiceItemModelView.Id), 
                        ServiceName = serviceItemModelView.ServiceName,
                        Description = serviceItemModelView.Description,
                        Image = serviceItemModelView.File.FileName
                    };

                    //--> Récupérer Token de session
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "Vous devez être authentifié";
                        return View(serviceItemModelView);
                    }

                    string stringData = JsonConvert.SerializeObject(serviceItem);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PostAsync(URLBase + "ServiceItem", contentData);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "ServiceItem");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(serviceItemModelView);

                }

            }
            return View(serviceItemModelView);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = new ServiceItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "serviceitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                   service = JsonConvert.DeserializeObject<ServiceItemViewModel>(apiResponse);
                }
            }
            return View(service);
        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = new ServiceItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "serviceitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    service = JsonConvert.DeserializeObject<ServiceItemViewModel>(apiResponse);
                }
            }
            return View(service);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceItemViewModel serviceItemModelView)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {

                    var serviceOld = new ServiceItemViewModel();
                    using (var respense = await client.GetAsync(URLBase + "serviceitem/" + serviceItemModelView.Id))
                    {
                        string apiResponse = await respense.Content.ReadAsStringAsync();
                        serviceOld = JsonConvert.DeserializeObject<ServiceItemViewModel>(apiResponse);
                    }
                    var image = serviceOld.Image;

                    if (serviceItemModelView.File != null)
                    {
                        //WebRootPath retourne chemain de wwwroot
                        string Olduploads = Path.Combine(_hosting.WebRootPath, @"img\services");
                        //--> Supprimer ancien Image
                        //--> Retourner l'ancien nom de image
                        string OldNameImage = serviceOld.Image;
                        //Ajout le chemain de ancien fichier
                        string oldPath = Path.Combine(Olduploads, OldNameImage);
                        //--> Sypprimer l'ancien image
                        System.IO.File.Delete(oldPath);

                        //WebRootPath retourne chemain de wwwroot
                        string uploads = Path.Combine(_hosting.WebRootPath, @"img\services");
                        //Ajout le chemain de nouveau fichier
                        string fullPath = Path.Combine(uploads, serviceItemModelView.File.FileName);
                        //Fait copier fichier dans ce chemain
                        //model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            serviceItemModelView.File.CopyTo(stream);
                        }
                        image = serviceItemModelView.File.FileName;
                    }

                    var serviceItem = new ServiceItem()
                    {
                        Id = Guid.Parse(serviceItemModelView.Id),
                        ServiceName = serviceItemModelView.ServiceName,
                        Description = serviceItemModelView.Description,
                        Image = image
                    };

                    //--> Récupérer Token de session
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "Vous devez être authentifié";
                        return View(serviceItemModelView);
                    }

                    string stringData = JsonConvert.SerializeObject(serviceItem);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PutAsync(URLBase + "ServiceItem/" + serviceItemModelView.Id, contentData);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "ServiceItem");
                    }

                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(serviceItemModelView);
                }

            }
            return View(serviceItemModelView);
        }

        // GET: ServiceItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = new ServiceItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "serviceitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    service = JsonConvert.DeserializeObject<ServiceItemViewModel>(apiResponse);
                }
            }
            return View(service);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            using (var client = new HttpClient())
            {
                var serviceOld = new ServiceItemViewModel();
                using (var respense = await client.GetAsync(URLBase + "serviceitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();
                    serviceOld = JsonConvert.DeserializeObject<ServiceItemViewModel>(apiResponse);
                }

                //WebRootPath retourne chemain de wwwroot
                string Olduploads = Path.Combine(_hosting.WebRootPath, @"img\services");
                //--> Supprimer ancien Image
                //--> Retourner l'ancien nom de image
                string OldNameImage = serviceOld.Image;
                //Ajout le chemain de ancien fichier
                string oldPath = Path.Combine(Olduploads, OldNameImage);
                //--> Sypprimer l'ancien image
                System.IO.File.Delete(oldPath);

                //--> Récupérer Token de session
                var JWToken = HttpContext.Session.GetString("token");
                if (string.IsNullOrEmpty(JWToken))
                {
                    ViewBag.MessageError = "Vous devez être authentifié";
                    return View();
                }

                //--> Ajouter Token dans Header Request pour avoir l'autorisation
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                var response = await client.DeleteAsync(URLBase + "serviceItem/" + id);

                var result = response.IsSuccessStatusCode;
                if (result)
                {
                    return RedirectToAction("Index", "ServiceItem");
                }

                ViewBag.MessageError = response.ReasonPhrase;
                return View();
            }
        }
    }
}

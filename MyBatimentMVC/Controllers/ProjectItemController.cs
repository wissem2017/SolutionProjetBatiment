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
    public class ProjectItemController : Controller
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
        public ProjectItemController(IConfiguration Config, IHostingEnvironment hosting)
        {
            _config = Config;
            _hosting = hosting;
        }

        public async Task<IActionResult> Index()
        {
            var listProject = new List<ProjectItem>();
          
            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "projectitem"))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    listProject = JsonConvert.DeserializeObject<List<ProjectItem>>(apiResponse);
                }
            }
            return View(listProject);
        }

        public async Task<IActionResult> AddProject()
        {
            var projectItemViwModel = new ProjectItemViewModel();
                    
            return View(projectItemViwModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectItemViewModel projectItemModelView)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    if (projectItemModelView.File != null)
                    {
                        //WebRootPath retourne chemain de wwwroot
                        string uploads = Path.Combine(_hosting.WebRootPath, @"img\projects");
                        //Ajout le chemain de nouveau fichier
                        string fullPath = Path.Combine(uploads, projectItemModelView.File.FileName);
                        //Fait copier fichier dans ce chemain
                        //model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            projectItemModelView.File.CopyTo(stream);
                        }

                    }

                    var projectItem = new ProjectItem() { 
                        
                        //Id = Guid.Parse(projectItemModelView.Id), 
                        ProjectName = projectItemModelView.ProjectName,
                        Description = projectItemModelView.Description,
                        Image = projectItemModelView.File.FileName
                    };

                    //--> Récupérer Token de session
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "Vous devez être authentifié";
                        return View(projectItemModelView);
                    }

                    string stringData = JsonConvert.SerializeObject(projectItem);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PostAsync(URLBase + "ProjectItem", contentData);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "ProjectItem");
                    }
                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(projectItemModelView);

                }

            }
            return View(projectItemModelView);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = new ProjectItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "projectitem/"+id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    project = JsonConvert.DeserializeObject<ProjectItemViewModel>(apiResponse);
                }
            }
            return View(project);
        }

        // GET: ServiceItems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = new ProjectItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "projectitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    project = JsonConvert.DeserializeObject<ProjectItemViewModel>(apiResponse);
                }
            }
            return View(project);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProjectItemViewModel projectItemModelView)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    
                    var projectOld = new ProjectItemViewModel();
                    using (var respense = await client.GetAsync(URLBase + "projectitem/" + projectItemModelView.Id))
                    {
                        string apiResponse = await respense.Content.ReadAsStringAsync();
                        projectOld = JsonConvert.DeserializeObject<ProjectItemViewModel>(apiResponse);
                    }
                    var image = projectOld.Image;

                    if (projectItemModelView.File != null)
                    {
                        //WebRootPath retourne chemain de wwwroot
                        string Olduploads = Path.Combine(_hosting.WebRootPath, @"img\projects");
                        //--> Supprimer ancien Image
                        //--> Retourner l'ancien nom de image
                        string OldNameImage = projectOld.Image;
                        //Ajout le chemain de ancien fichier
                        string oldPath = Path.Combine(Olduploads, OldNameImage);
                        //--> Sypprimer l'ancien image
                        System.IO.File.Delete(oldPath);

                        //WebRootPath retourne chemain de wwwroot
                        string uploads = Path.Combine(_hosting.WebRootPath, @"img\projects");
                        //Ajout le chemain de nouveau fichier
                        string fullPath = Path.Combine(uploads, projectItemModelView.File.FileName);
                        //Fait copier fichier dans ce chemain
                        //model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            projectItemModelView.File.CopyTo(stream);
                        }
                        image = projectItemModelView.File.FileName;
                    }

                    var projectItem = new ProjectItem()
                    {
                        Id = Guid.Parse(projectItemModelView.Id), 
                        ProjectName = projectItemModelView.ProjectName,
                        Description = projectItemModelView.Description,
                        Image = image
                    };

                    //--> Récupérer Token de session
                    var JWToken = HttpContext.Session.GetString("token");
                    if (string.IsNullOrEmpty(JWToken))
                    {
                        ViewBag.MessageError = "Vous devez être authentifié";
                        return View(projectItemModelView);
                    }

                    string stringData = JsonConvert.SerializeObject(projectItem);
                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.PutAsync(URLBase + "ProjectItem/" + projectItemModelView.Id, contentData);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "ProjectItem");
                    }

                    ViewBag.MessageError = response.ReasonPhrase;
                    return View(projectItemModelView);
                }

            }
            return View(projectItemModelView);
        }

        // GET: ServiceItems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = new ProjectItemViewModel();

            using (var httpClient = new HttpClient())
            {
                using (var respense = await httpClient.GetAsync(URLBase + "projectitem/" + id))
                {
                    string apiResponse = await respense.Content.ReadAsStringAsync();

                    project = JsonConvert.DeserializeObject<ProjectItemViewModel>(apiResponse);
                }
            }
            return View(project);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
                using (var client = new HttpClient())
                {
                        var projectOld = new ProjectItemViewModel();
                        using (var respense = await client.GetAsync(URLBase + "projectitem/" + id))
                        {
                            string apiResponse = await respense.Content.ReadAsStringAsync();
                            projectOld = JsonConvert.DeserializeObject<ProjectItemViewModel>(apiResponse);
                        }

                        //WebRootPath retourne chemain de wwwroot
                        string Olduploads = Path.Combine(_hosting.WebRootPath, @"img\projects");
                        //--> Supprimer ancien Image
                        //--> Retourner l'ancien nom de image
                        string OldNameImage = projectOld.Image;
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

                    //string stringData = JsonConvert.SerializeObject(projectOld);
                    //var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    //--> Ajouter Token dans Header Request pour avoir l'autorisation
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", JWToken);
                    var response = await client.DeleteAsync(URLBase + "ProjectItem/" + id);

                    var result = response.IsSuccessStatusCode;
                    if (result)
                    {
                        return RedirectToAction("Index", "ProjectItem");
                    }

                    ViewBag.MessageError = response.ReasonPhrase;
                    return View();
                }
        }
    }
}

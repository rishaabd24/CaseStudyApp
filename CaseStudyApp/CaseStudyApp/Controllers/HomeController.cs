using Microsoft.AspNetCore.Mvc;
using CaseStudyApp.Models;
using CaseStudyApp.Model;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using CaseStudyApp.Data;
using Microsoft.AspNetCore.Identity;

namespace CaseStudyApp.Controllers
{
    public class HomeController : Controller


    {
        private readonly AuthDbContext context;
        private readonly UserManager<AppUser> _userManager;


        public bool flag = true;
        public string ErrorMessage { get; set; }
        public HomeController(AuthDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.userid = _userManager.GetUserName(HttpContext.User);
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();
            viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }
    }
}

using CaseStudyApp.Data;
using CaseStudyApp.Model;
using CaseStudyApp.Models;
using CaseStudyApp.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CaseStudyApp.Controllers
{
    public class FileController : Controller
    {
        private readonly AuthDbContext context;
        private readonly UserManager<AppUser> _userManager;
        public bool flag = true;
        public string ErrorMessage { get; set; }
        public FileController(AuthDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> File()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.userid = _userManager.GetUserName(HttpContext.User);
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                bool basePathExists = System.IO.Directory.Exists(basePath);
                if (!basePathExists) Directory.CreateDirectory(basePath);
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var filePath = Path.Combine(basePath, file.FileName);
                var extension = Path.GetExtension(file.FileName);
                string supportedTypes = ".pbix";
                if (!supportedTypes.Equals(extension))
                {
                    flag = false;
                    break;
                }
                if (!System.IO.File.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                            await file.CopyToAsync(stream);
                    }
                    var fileModel = new FileOnFileSystemModel
                    {
                        CreatedOn = DateTime.Now,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        FilePath = filePath,
                        UploadedBy = _userManager.GetUserName(HttpContext.User),
                    };
                    context.FilesOnFileSystem.Add(fileModel);
                    context.SaveChanges();
                }
            }
            if (flag == true)
            {
                TempData["Message"] = "File successfully uploaded";
            }
            else
            {
                TempData["Message"] = "File Extension Is InValid - Only Upload .pbix File";
            }
            return RedirectToAction("File");
        }
        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {

            var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, file.FileType, file.Name + file.Extension);
        }
        public async Task<IActionResult> DeleteFileFromFileSystem(int id)
        {

            var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            if (System.IO.File.Exists(file.FilePath))
            {
                System.IO.File.Delete(file.FilePath);
            }
            context.FilesOnFileSystem.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
            return RedirectToAction("File");
        }        
    }
}

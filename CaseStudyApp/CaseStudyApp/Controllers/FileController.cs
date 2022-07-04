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
        public async Task<IActionResult> I()
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
                TempData["Message"] = "File successfully uploaded to Database";
            }
            else
            {
                TempData["Message"] = "File Extension Is InValid - Only Upload .pbix File";
            }
            return RedirectToAction("I");
        }
        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                string supportedTypes = ".pbix";
                if (!supportedTypes.Equals(extension))
                {
                    flag = false;
                    break;
                }
                var fileModel = new FileOnDatabaseModel
                { 
                        CreatedOn = DateTime.Now,
                        FileType = file.ContentType,
                        Extension = extension,
                        Name = fileName,
                        Description = description,
                        UploadedBy = _userManager.GetUserName(HttpContext.User)
                };
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    fileModel.Data = dataStream.ToArray();
                }
                context.FilesOnDatabase.Add(fileModel);
                context.SaveChanges();
            }
            if (flag == true)
            {
                TempData["Message"] = "File successfully uploaded to Database";
            }
            else
            {
                TempData["Message"] = "File Extension Is InValid - Only Upload .pbix File";
            }
            return RedirectToAction("I");
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();
            viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Name + file.Extension);
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
            return RedirectToAction("I");
        }
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.FilesOnDatabase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
            return RedirectToAction("I");
        }

        
        
    }
}

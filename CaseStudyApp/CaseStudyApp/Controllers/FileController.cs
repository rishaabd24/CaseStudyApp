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
    //Controller for File related operations
    public class FileController : Controller
    {
        private readonly AuthDbContext context;
        private readonly UserManager<AppUser> _userManager;
        public bool flag = true;
        public string ErrorMessage { get; set; }

        // Injecting context 
        public FileController(AuthDbContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }

        // Loading all the available files and passing the view model to the view.
        public async Task<IActionResult> File()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.userid = _userManager.GetUserName(HttpContext.User);
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }

        // Action method that inputs list of files of type IFormFIle and description of type string
        [HttpPost]
        public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        {
            // Extracting file path, file name, and file extension for all files to be uploaded
            foreach (var file in files)
            {
                
                var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                // Checking if the base path directory exists, else creating it 
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

                    // Creating a new FileOnFileSystemModel and inserting into DB via context
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
                TempData["Message"] = "True";
            }
            else
            {
                TempData["Message"] = "False";
            }
            return RedirectToAction("File");
        }
        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
            return viewModel;
        }

        // Gets records from DB table and copies file data into memory object, returning for download
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

        // Deletes file from file system and removes record from DB table
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
            TempData["Message"] = "Removed";
            return RedirectToAction("File");
        }        
    }
}

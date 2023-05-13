using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {

        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }
        public async Task<IActionResult> Index(string containerName)
        {
            var allBlobs = await _blobService.GetAllBlobs(containerName);
            return View(allBlobs);
        }

        public async Task<IActionResult> DeleteBlob(string containerName, string bolbName)
        {
            await _blobService.DeleteBlob(bolbName, containerName);
            return RedirectToAction(nameof(Index), "Blob", new { @containerName = containerName });
        }

        public async Task<IActionResult> ViewFile(string containerName, string bolbName)
        {
           return Redirect( await _blobService.GetBlob(bolbName, containerName));
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName,Blob blob, IFormFile file)
        {
            if (file == null || file.Length < 1) return View();

            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);
            var result = await _blobService.UploadBlob(fileName, file, containerName, blob);

            if (result)
                return RedirectToAction("Index", "Container");

            return View();
        }

    }
}

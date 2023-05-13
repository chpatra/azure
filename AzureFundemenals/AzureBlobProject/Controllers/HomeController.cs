using Azure.Storage.Blobs;
using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;

        public HomeController(ILogger<HomeController> logger, IContainerService containerService, IBlobService blobService)
        {
            _logger = logger;
            _containerService = containerService;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            var allContainerAndBlobs = await _containerService.GetAllContainerAndBlobs();
            return View(allContainerAndBlobs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Images()
        {
            List<Blob> blobLis = await _blobService.GetAllBlobsWithUri("private-container");
            return View(blobLis);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

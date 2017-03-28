using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobGallery.Services;

namespace BlobGallery.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Gallery()
        {
            ViewData["Message"] = "Gallery of blobs";

            var blobSvc = new BlobService();
            ViewData["Blobs"] = blobSvc.GetBlobList();

            return View();
        }


        public IActionResult Upload()
        {
            ViewData["Message"] = "Upload a new blob";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

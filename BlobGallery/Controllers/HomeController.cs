using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlobGallery.Services;
using Microsoft.WindowsAzure.Storage.Blob;
using BlobGallery.Models;

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
            var blobs = blobSvc.GetBlobList();
            var sasPolicy = blobSvc.GetBlobSasPolicy();

            ViewData["Blobs"] = blobs.Select(x => new BlobViewModel {
                Url = x.Uri + ((CloudBlockBlob)x).GetSharedAccessSignature(sasPolicy),
                File = ((CloudBlockBlob)x).Name.Substring(((CloudBlockBlob)x).Parent.Prefix.Length, ((CloudBlockBlob)x).Name.Length - ((CloudBlockBlob)x).Parent.Prefix.Length),
                Type = ((CloudBlockBlob)x).Properties.ContentType.ToLower()
            }).ToList();

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

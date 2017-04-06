using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using BlobGallery.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlobGallery.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            var blobSvc = new BlobService();
            

            var container = blobSvc.GetBlobContainer();
            long size = files.Sum(f => f.Length);

            foreach (var file in files)
            {
                var blob = container.GetBlockBlobReference(file.FileName);
                if (!blob.ExistsAsync().Result)
                {

                    if (file.Length > 0)
                    {
                        using (var fileStream = file.OpenReadStream())
                        using (var ms = new MemoryStream())
                        {
                            fileStream.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            string s = Convert.ToBase64String(fileBytes);
                            // act on the Base64 data
                            await blob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
                            blob.Properties.ContentType = file.ContentType;
                            await blob.SetPropertiesAsync();
                        }
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            Redirect("/Home/Gallery");
            
            return Ok(new { count = files.Count, size});

            //Redirect here?
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

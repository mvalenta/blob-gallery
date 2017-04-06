using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobGallery.Services
{
    public class BlobService
    {
        private string conn = "UseDevelopmentStorage=true;";

        public BlobService()
        {

        }

        public List<IListBlobItem> GetBlobList()
        {
            var container = GetBlobContainer();

            //CloudBlockBlob samplePic = container.GetBlockBlobReference("2016-09-09.jpg");
            //var sas = samplePic.GetSharedAccessSignature(policy);

            var list = container.ListBlobsSegmentedAsync(null).Result.Results;

            var blobs = new List<IListBlobItem>();

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in list)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    
                    blobs.Add(blob);
                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;
                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                }
            }

            return blobs;
        }


        public CloudBlobContainer GetBlobContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conn);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("myfirstcontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExistsAsync();

            return container;
        }


        public List<string> GetBlobUrls(List<IListBlobItem> list)
        {
            var policy = GetBlobSasPolicy();   
            var urls = new List<string>();
            foreach (var item in list)
            {
                var sas = ((CloudBlockBlob)item).GetSharedAccessSignature(policy);

                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    urls.Add(blob.Uri.ToString() + sas);
                }
            }

            return urls;
        }
        public SharedAccessBlobPolicy GetBlobSasPolicy()
        {
            return new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Add |
                                  SharedAccessBlobPermissions.Create |
                                  SharedAccessBlobPermissions.Delete |
                                  SharedAccessBlobPermissions.List |
                                  SharedAccessBlobPermissions.Read |
                                  SharedAccessBlobPermissions.Write,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(2)
            };
        }
    }
}  

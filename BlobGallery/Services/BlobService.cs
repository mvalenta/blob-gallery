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

        public List<string> GetBlobList()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conn);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("myfirstcontainer");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExistsAsync();



            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Add |
                              SharedAccessBlobPermissions.Create |
                              SharedAccessBlobPermissions.Delete |
                              SharedAccessBlobPermissions.List |
                              SharedAccessBlobPermissions.Read |
                              SharedAccessBlobPermissions.Write,
                SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddDays(2)
            };

            //CloudBlockBlob samplePic = container.GetBlockBlobReference("2016-09-09.jpg");
            //var sas = samplePic.GetSharedAccessSignature(policy);

            var list = container.ListBlobsSegmentedAsync(null).Result.Results;

            var blobUrls = new List<string>();

            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in list)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    var sas = blob.GetSharedAccessSignature(policy);
                    blobUrls.Add(blob.Uri.ToString() + sas);
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

            return blobUrls;
        }
        // Create the blob client.
    }
}

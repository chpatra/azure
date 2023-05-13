using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobProject.Services
{
    public class ContainerServicec : IContainerService
    {
        private readonly BlobServiceClient _blobClient;
        public ContainerServicec(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainer()
        {
            List<string> containerName = new List<string>();
          await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobContainerItem.Name);
            }

            return  containerName;
        }

        public async Task<List<string>> GetAllContainerAndBlobs()
        {
            List<string> allContainerAndBlobs = new List<string>();

            allContainerAndBlobs.Add("Account Name: " + _blobClient.AccountName);
            allContainerAndBlobs.Add("---------------------------------------------------------------");

            await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                allContainerAndBlobs.Add("Container Name: " + blobContainerItem.Name);               
                BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(blobContainerItem.Name);

                await foreach(BlobItem item in blobContainerClient.GetBlobsAsync())
                {
                    BlobClient blobClient = blobContainerClient.GetBlobClient(item.Name);
                    BlobProperties blobproperties = await blobClient.GetPropertiesAsync();
                    string blobToAdd = item.Name;
                    if (blobproperties.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobproperties.Metadata["title"] + ")";
                    }
                    allContainerAndBlobs.Add(blobToAdd);
                }

                allContainerAndBlobs.Add("---------------------------------------------------------------");
            }

            return allContainerAndBlobs;

        }
    }
}

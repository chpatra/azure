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

        public Task<List<string>> GetAllContainerAndBlobs()
        {
            throw new NotImplementedException();
        }
    }
}

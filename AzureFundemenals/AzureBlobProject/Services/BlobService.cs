using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobProject.Services
{
    public class BlobService : IBlobService
    {

        private readonly BlobServiceClient _blobSericeClient;
        public BlobService(BlobServiceClient blobSericeClient)
        {
            _blobSericeClient = blobSericeClient;
        }
        public Task<bool> DeleteBlob(string name, string containerName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            List<string> allBlobs = new List<string>();
            BlobContainerClient conainerClient = _blobSericeClient.GetBlobContainerClient(containerName);        
            
            await foreach(var item in conainerClient.GetBlobsAsync())
            {
                allBlobs.Add(item.Name);
            }

            return allBlobs;
        }

        public Task<List<string>> GetAllBlobsWithUri(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetBlob(string name, string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            throw new NotImplementedException();
        }
    }
}

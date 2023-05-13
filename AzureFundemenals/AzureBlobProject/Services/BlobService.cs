using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using AzureBlobProject.Models;
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
        public async Task<bool> DeleteBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobSericeClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
           return await blobClient.DeleteIfExistsAsync();
          
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

        public async Task<List<Blob>> GetAllBlobsWithUri(string containerName)
        {
            List<Blob> blobList = new List<Blob>();
            BlobContainerClient conainerClient = _blobSericeClient.GetBlobContainerClient(containerName);
            string sasCnainerSignaure = string.Empty;

            if (conainerClient.CanGenerateSasUri)
            {
                BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = conainerClient.Name,                   
                    Resource = "b",
                    ExpiresOn = DateTimeOffset.Now.AddMinutes(5)
                };

                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
                sasCnainerSignaure = conainerClient.GenerateSasUri(blobSasBuilder).AbsoluteUri.Split("?")[1].ToString();
            }
        
            await foreach (var item in conainerClient.GetBlobsAsync())
            {
                BlobClient blobClient = conainerClient.GetBlobClient(item.Name);

                Blob blobObj = new Blob()
                {
                    Uri = blobClient.Uri.AbsoluteUri +"?" + sasCnainerSignaure
                };

                //if (blobClient.CanGenerateSasUri)
                //{
                //    BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                //    {
                //        BlobContainerName = containerName,
                //        BlobName = blobClient.Name,
                //        Resource = "b",
                //        ExpiresOn = DateTimeOffset.Now.AddMinutes(5)
                //    };

                //    blobSasBuilder.SetPermissions(BlobSasPermissions.Read);
                //    blobObj.Uri = blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
                //}


                BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
                if (blobProperties.Metadata.ContainsKey("title"))
                {
                    blobObj.Title = blobProperties.Metadata["title"].ToString();
                }

                if (blobProperties.Metadata.ContainsKey("comment"))
                {
                    blobObj.Comment = blobProperties.Metadata["comment"].ToString();
                }

                blobList.Add(blobObj);
            }

            return blobList;
        }

        public async Task<string> GetBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobSericeClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            if (blobClient.CanGenerateSasUri)
            {
                BlobSasBuilder blobSasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobContainerClient.Name,
                    BlobName = blobClient.Name,
                    Resource = "B",
                    ExpiresOn = DateTimeOffset.Now.AddMinutes(5)
                };
                blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                return blobClient.GenerateSasUri(blobSasBuilder).AbsoluteUri;
            }
            return  blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string blobName, IFormFile file, string containerName, Blob blob)
        {
            BlobContainerClient blobContainerClient = _blobSericeClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            IDictionary<string, string> meadata = new Dictionary<string, string>();
            if(blob != null)
            {
                if (!String.IsNullOrEmpty(blob.Title))
                {
                    meadata.Add("title", blob.Title);
                }

                if (!String.IsNullOrEmpty(blob.Comment))
                {
                    meadata.Add("comment", blob.Comment);
                }

            }            

            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpHeaders, meadata);

            return (result != null);            

        }
    }
}

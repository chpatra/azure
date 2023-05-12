﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureBlobProject.Services
{
    public interface IContainerService
    {
        Task<List<string>> GetAllContainerAndBlobs();
        Task<List<string>> GetAllContainer();
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}

using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RYoshiga.Demo.Domain.Adapters;

namespace RYoshiga.Demo.Infrastructure
{
    public class StorageAccountFileManager : IFileSaver
    {
        private const string BlobContainerName = "files";
        private readonly StorageAccountConfiguration _storageAccountConfiguration;
        private readonly BlobServiceClient _blobServiceClient;

        public StorageAccountFileManager(StorageAccountConfiguration storageAccountConfiguration)
        {
            _storageAccountConfiguration = storageAccountConfiguration;
            _blobServiceClient = new BlobServiceClient(_storageAccountConfiguration.ConnectionString);
        }

        private async Task<BlobContainerClient> CreateContainerIfNotExists()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(BlobContainerName);
            if (blobContainerClient.Exists())
                return blobContainerClient;

            return await _blobServiceClient.CreateBlobContainerAsync(BlobContainerName);
        }

        public async Task Save(string fileName, Stream stream)
        {
            var container = await CreateContainerIfNotExists();
            var blobClient = container.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream);
        }

        public async Task<Stream> Read(string fileName)
        {
            var container = await CreateContainerIfNotExists();
            var blobClient = container.GetBlobClient(fileName);

            BlobDownloadInfo download = await blobClient.DownloadAsync();
            return download.Content;
        }

        public async Task Delete(string fileName)
        {
            var container = await CreateContainerIfNotExists();
            var blobClient = container.GetBlobClient(fileName);

            await blobClient.DeleteAsync();
        }
    }
}

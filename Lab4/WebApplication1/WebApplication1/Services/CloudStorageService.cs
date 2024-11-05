using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WebApplication1.Services
{
    public class CloudStorageService
    {
        private readonly StorageClient _storageClient;
        private const string BucketName = "datc_lab4_bucket";

        private readonly List<FileInfo> _files;

        public CloudStorageService()
        {
            var credentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lab4cloudstorage-133635ceecde.json");
            _storageClient = StorageClient.Create(GoogleCredential.FromFile(credentialsPath));
            _files = new List<FileInfo>(); // Initialize the list
        }

        public async Task<List<FileInfo>> ListFilesAsync()
        {
            var files = new List<FileInfo>();

            // Fetch files from your cloud storage API
            var cloudFiles = _storageClient.ListObjects(BucketName); // Use _storageClient here

            foreach (var file in cloudFiles)
            {
                files.Add(new FileInfo
                {
                    Name = file.Name,
                    LastUpdated = file.UpdatedDateTimeOffset?.UtcDateTime // Use UpdatedDateTimeOffset directly
                });
            }

            return files;
        }

        public async Task UploadFileAsync(string cloudFileName, Stream fileStream)
        {
            // Upload the file to cloud storage
            await _storageClient.UploadObjectAsync(BucketName, cloudFileName, null, fileStream);

            // Store metadata in your own structure
            var fileInfo = new FileInfo
            {
                Name = cloudFileName,
                LastUpdated = DateTime.UtcNow // Set to the current time
            };

            // Add this fileInfo to the list
            _files.Add(fileInfo); // Use the _files list
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(BucketName, fileName, memoryStream);
            memoryStream.Position = 0; // Reset stream position for reading
            return memoryStream;
        }

        public async Task UpdateFileAsync(string fileName, Stream fileStream)
        {
            await _storageClient.UploadObjectAsync(BucketName, fileName, null, fileStream);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            await _storageClient.DeleteObjectAsync(BucketName, fileName);
        }
    }

}

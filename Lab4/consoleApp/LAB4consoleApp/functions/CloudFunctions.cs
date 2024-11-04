using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LAB4consoleApp.functions;

namespace LAB4consoleApp.functions
{
    public class CloudFunctions
    {

        private const string BucketName = "datc_lab4_bucket";
        private static readonly string CredentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lab4cloudstorage-133635ceecde.json");
        private static readonly StorageClient storageClient = StorageClient.Create(GoogleCredential.FromFile(CredentialsPath));

        private static readonly string DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");

        public static void DisplayOptionsCloud()
        {
            Console.WriteLine("0 = Exit");
            Console.WriteLine("1 = Upload file");
            Console.WriteLine("2 = Download file");
            Console.WriteLine("3 = Update file");
            Console.WriteLine("4 = Delete file");
            Console.WriteLine("5 = Switch to LOCAL mode");
        }



        public static void DisplayCloudFiles()
        {
            Console.WriteLine("CLOUD Storage:");
            Console.WriteLine("===================================");

            try
            {
                var objects = storageClient.ListObjects(BucketName, "");
                bool filesExist = false;

                foreach (var obj in objects)
                {
                    Console.WriteLine(obj.Name);
                    filesExist = true;
                }

                if (!filesExist)
                {
                    Console.WriteLine("No files found in the cloud storage bucket.");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while accessing the cloud storage: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }

            Console.WriteLine("===================================");
        }


        public static void UploadFile()
        {
            // Display the list of files available locally
            LocalFunctions.DisplayFiles("Your LOCAL files, choose one");

            Console.Write("\nEnter the name of the file you want to upload (including extension): ");
            string selectedFileName = Console.ReadLine();

            string selectedFilePath = Path.Combine(DirectoryPath, selectedFileName);

            if (!File.Exists(selectedFilePath))
            {
                Console.WriteLine($"File not found: {selectedFilePath}");
                return;
            }

            // Prompt for the cloud storage name
            Console.Write("Enter the name for the file in the cloud (including extension): ");
            string cloudFileName = Console.ReadLine();

            try
            {
                using var fileStream = File.OpenRead(selectedFilePath);
                storageClient.UploadObject(BucketName, cloudFileName, null, fileStream);
                string message = $"Uploaded '{selectedFileName}' as '{cloudFileName}' to {BucketName}.";
                Console.WriteLine(message);
                OtherFunctions.Log(message);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred during upload: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void UpdateCloudFile()
        {
            // Display the list of files available locally

            Console.Write("\n1. Enter the name of the file currently stored in the cloud that you want to update (including extension): ");
            string selectedCloudFileName = Console.ReadLine();

            LocalFunctions.DisplayFiles("2. Here are your LOCAL files. Please choose the one that will replace the cloud file:");

            Console.Write("\n3. Enter the name of the LOCAL file that will be uploaded as the new version in the cloud (including extension): ");
            string selectedFileName = Console.ReadLine();

            string selectedFilePath = Path.Combine(DirectoryPath, selectedFileName);

            // Check if the local file exists
            if (!File.Exists(selectedFilePath))
            {
                Console.WriteLine($"File not found locally: {selectedFilePath}");
                return;
            }

            // Check if the file exists in Google Cloud Storage
            try
            {
                var cloudObject = storageClient.GetObject(BucketName, selectedCloudFileName);
                if (cloudObject == null)
                {
                    Console.WriteLine($"File not found in cloud storage: {selectedCloudFileName}");
                    return;
                }
            }
            catch (Google.GoogleApiException e) when (e.Error.Code == 404)
            {
                Console.WriteLine($"File not found in cloud storage: {selectedCloudFileName}");
                return;
            }

            // Replace the file in cloud storage with the local file
            try
            {
                using var fileStream = File.OpenRead(selectedFilePath);
                storageClient.UploadObject(BucketName, selectedCloudFileName, null, fileStream);
                string message = $"Updated '{selectedCloudFileName}' in {BucketName} with local file '{selectedFileName}'.";
                Console.WriteLine(message);
                OtherFunctions.Log(message);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred during deletion: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void DownloadCloudFile()
        {

            Console.Write("\nEnter the name of the file to download: ");
            string objectName = Console.ReadLine();


            string destinationPath = Path.Combine(DirectoryPath, objectName);

            try
            {
                using var fileStream = File.OpenWrite(destinationPath);
                storageClient.DownloadObject(BucketName, objectName, fileStream);
                string message = $"Downloaded '{objectName}' from {BucketName} to '{destinationPath}'.";
                Console.WriteLine(message);
                OtherFunctions.Log(message);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred during downlaod: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void DeleteCloudFile()
        {

            Console.Write("\nEnter the name of the file to delete: ");
            string objectName = Console.ReadLine();

            Console.WriteLine($"Are you sure you want to delete '{objectName}'? Type 'yes' to confirm or 'no' to cancel.");
            string confirmation = Console.ReadLine();

            if (confirmation?.ToLower() == "yes")
            {
                try
                {
                    storageClient.DeleteObject(BucketName, objectName);
                    string message = $"Deleted '{objectName}' from {BucketName}.";
                    Console.WriteLine(message);
                    OtherFunctions.Log(message);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"An error occurred during deletion: {ex.Message}";
                    Console.WriteLine(errorMessage);
                    OtherFunctions.Log(errorMessage);
                }
            }
            else
            {
                string errorMessage = "File deletion canceled.";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }
    }
}

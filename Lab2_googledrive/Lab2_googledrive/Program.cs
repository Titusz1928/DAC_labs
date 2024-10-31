using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;

namespace GoogleDriveAPIExample
{
    class Program
    {
        static string[] Scopes = { DriveService.Scope.Drive }; // This scope allows for file upload
        static string ApplicationName = "Google Drive API Demo";

        static void Main(string[] args)
        {
            try
            {
                UserCredential credential;

                // Load client secrets from the credentials.json file
                using (var stream =
                    new FileStream("borostitusz_lab2_secrets.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token.json";
                    // Reauthorize if token.json exists
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                ListFiles(service);
                UploadFile(service); // Pass service to the UploadFile method
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ListFiles(DriveService service)
        {
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            var request = listRequest.Execute();
            Console.WriteLine("Files:");
            if (request.Files != null && request.Files.Count > 0)
            {
                foreach (var file in request.Files)
                {
                    Console.WriteLine($"{file.Name} ({file.Id})");
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
        }

        static void UploadFile(DriveService service)
        {
            try
            {
                string filePath = "testfile.txt"; // Your test file

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = "UploadedTestFile.txt" // Name on Google Drive
                };

                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var request = service.Files.Create(
                        fileMetadata, stream, "text/plain");
                    request.Fields = "id";

                    var file = request.Upload();
                    if (file.Status == Google.Apis.Upload.UploadStatus.Completed)
                    {
                        Console.WriteLine("File uploaded successfully!");
                        Console.WriteLine("File ID: " + request.ResponseBody.Id);
                    }
                    else
                    {
                        Console.WriteLine("File upload failed: " + file.Exception.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during file upload: " + ex.Message);
            }
        }
    }
}

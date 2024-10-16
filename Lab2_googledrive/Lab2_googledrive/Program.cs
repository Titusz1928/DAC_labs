using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GoogleDriveAPIExample
{
    class Program
    {
        // Scopes define the level of access to the Google Drive
        static string[] Scopes = { DriveService.Scope.DriveReadonly };
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
                    // Path to save token response
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;

                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Drive API service
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });

                // List files from the user's Google Drive
                ListFiles(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void ListFiles(DriveService service)
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
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
    }
}
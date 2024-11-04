using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;


using LAB4consoleApp.functions;


class Program
{
/*    private const string BucketName = "datc_lab4_bucket";
    private static readonly string DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");
    private static readonly string CredentialsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lab4cloudstorage-133635ceecde.json");
    private static readonly StorageClient storageClient = StorageClient.Create(GoogleCredential.FromFile(CredentialsPath));*/

    static void Main(string[] args)
    {

        int SelectedItem = -1;
        bool CloudMode=false;

        Console.WriteLine("Welcome!");
        Console.ReadLine();

        do
        {
            Console.Clear();
            if (!CloudMode)
            {
                LocalFunctions.DisplayFiles("LOCAL files: ");
                LocalFunctions.DisplayOptionsLocal();
            }
            else
            {
                CloudFunctions.DisplayCloudFiles();
                CloudFunctions.DisplayOptionsCloud();
            }

            Console.Write("Please select an option: ");
            string input = Console.ReadLine();

            // Try parsing the input to an integer
            if (int.TryParse(input, out SelectedItem))
            {
                // Optionally, handle the selection
                switch (SelectedItem)
                {
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;
                    case 1:
                        if (!CloudMode)
                            LocalFunctions.OpenFile();
                        else
                            CloudFunctions.UploadFile();
                        break;
                    case 2:
                        if (!CloudMode)
                            LocalFunctions.CreateAndWriteFile();
                        else
                            CloudFunctions.DownloadCloudFile();
                        break;
                    case 3:
                        if (!CloudMode)
                            LocalFunctions.UpdateFile();
                        else
                            CloudFunctions.UpdateCloudFile();
                        break;
                    case 4:
                        if (!CloudMode)
                            LocalFunctions.DeleteFile();
                        else
                        {
                            CloudFunctions.DeleteCloudFile();
                        }
                        break;
                    case 5:
                        CloudMode=!CloudMode;
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
            }
            else
            {
                SelectedItem = -1;
                Console.WriteLine("Invalid input. Please enter a number.");
            }

            // Pause before the next iteration
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();

        } while (SelectedItem != 0);

    }

}

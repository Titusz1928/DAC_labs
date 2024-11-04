using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB4consoleApp.functions
{
    public class LocalFunctions
    {
        private static readonly string DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");

        public static void DisplayOptionsLocal()
        {
            Console.WriteLine("0 = Exit");
            Console.WriteLine("1 = Open file");
            Console.WriteLine("2 = Create file");
            Console.WriteLine("3 = Update file");
            Console.WriteLine("4 = Delete file");
            Console.WriteLine("5 = Switch to CLOUD mode");
        }

        public static void DisplayFiles(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("===================================");

            try
            {
                if (Directory.Exists(DirectoryPath))
                {
                    string[] files = Directory.GetFiles(DirectoryPath);

                    if (files.Length == 0)
                    {
                        Console.WriteLine("No files found in the directory.");
                    }
                    else
                    {
                        foreach (string file in files)
                        {
                                Console.WriteLine(Path.GetFileName(file));
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Directory not found: {DirectoryPath}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
            Console.WriteLine("===================================");
        }

        public static void OpenFile()
        {
            Console.WriteLine("Enter the name of the file to read (including extension): ");
            string fileName = Console.ReadLine();

            string filePath = Path.Combine(DirectoryPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    OtherFunctions.Log("Opened file: " + filePath);
                    string fileContents = File.ReadAllText(filePath);
                    Console.WriteLine("\nFile Contents:");
                    Console.WriteLine(fileContents);
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while reading the file: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void CreateAndWriteFile()
        {
            Console.WriteLine("Enter the name of the new file to create (including extension): ");
            string fileName = Console.ReadLine();

            string filePath = Path.Combine(DirectoryPath, fileName);

            Console.WriteLine("Enter the content you want to write to the file. Type /done to finish:");

            string fileContent = "";
            string line;

            while (true)
            {
                line = Console.ReadLine();

                // Check if the line is /done to break the loop
                if (line != null && line.ToLower() == "/done")
                    break; // Exit the loop if user types /done

                // Append the line to the file content, including new line character
                fileContent += line + Environment.NewLine;
            }

            try
            {
                // Write the content to the new file
                File.WriteAllText(filePath, fileContent);
                string message = $"File '{fileName}' created successfully at: {filePath}";
                Console.WriteLine(message);
                OtherFunctions.Log(message);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error while creating the file: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void UpdateFile()
        {
            Console.WriteLine("Enter the name of the file to update (including extension): ");
            string fileName = Console.ReadLine();

            string filePath = Path.Combine(DirectoryPath, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    // Read all lines from the file
                    string[] lines = File.ReadAllLines(filePath);
                    Console.WriteLine("\nCurrent File Contents:");
                    for (int i = 0; i < lines.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}: {lines[i]}");
                    }

                    Console.WriteLine("\nChoose an option:");
                    Console.WriteLine("1: Overwrite the entire file");
                    Console.WriteLine("2: Update a specific line");

                    string option = Console.ReadLine();

                    if (option == "1")
                    {
                        Console.WriteLine("Enter the new content for the file. Type /done to finish:");

                        string fileContent = "";
                        string line;

                        while (true)
                        {
                            line = Console.ReadLine();
                            if (line != null && line.ToLower() == "/done")
                                break; // Exit the loop if user types /done

                            // Append the line to the file content, including new line character
                            fileContent += line + Environment.NewLine;
                        }

                        // Write the accumulated content to the file
                        File.WriteAllText(filePath, fileContent);
                        string message = $"File '{fileName}' updated successfully.";
                        Console.WriteLine(message);
                        OtherFunctions.Log(message);
                    }
                    else if (option == "2")
                    {
                        Console.WriteLine("Enter the line number to update:");
                        if (int.TryParse(Console.ReadLine(), out int lineNumber) && lineNumber > 0 && lineNumber <= lines.Length)
                        {
                            Console.WriteLine("Enter the new content for the selected line:");
                            string newLineContent = Console.ReadLine();
                            lines[lineNumber - 1] = newLineContent; // Update the specific line

                            // Write the updated content back to the file
                            File.WriteAllLines(filePath, lines);
                            string message = $"Line {lineNumber} updated successfully in file {fileName}";
                            Console.WriteLine(message);
                            OtherFunctions.Log(message);
                        }
                        else
                        {
                            Console.WriteLine("Invalid line number.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid option selected.");
                    }
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while updating the file: {ex.Message}";
                Console.WriteLine(errorMessage);
                OtherFunctions.Log(errorMessage);
            }
        }

        public static void DeleteFile()
        {
            Console.WriteLine("Enter the name of the file to delete (including extension): ");
            string fileName = Console.ReadLine();

            string filePath = Path.Combine(DirectoryPath, fileName);

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            Console.WriteLine($"\nAre you sure you want to delete '{fileName}'? Type 'yes' to confirm or 'no' to cancel.");
            string confirmation = Console.ReadLine();

            if (confirmation?.ToLower() == "yes")
            {
                try
                {
                    File.Delete(filePath);
                    string message = $"File '{fileName}' deleted successfully.";
                    Console.WriteLine(message);
                    OtherFunctions.Log(message);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"An error occurred while deleting the file : {ex.Message}";
                    Console.WriteLine(errorMessage);
                    OtherFunctions.Log(errorMessage);
                }
            }
            else
            {
                Console.WriteLine("File deletion canceled.");
            }
        }

    }
}

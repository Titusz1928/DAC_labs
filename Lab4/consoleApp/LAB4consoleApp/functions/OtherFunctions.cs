using System;
using System.IO;

namespace LAB4consoleApp.functions
{
    public class OtherFunctions
    {
        private static readonly string DirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files");
        private static readonly string LogFilePath = Path.Combine(DirectoryPath, "log.txt");

        public static void Log(string message)
        {
            try
            {
                // Ensure the directory exists
                Directory.CreateDirectory(DirectoryPath);

                // Append the message to the log file
                using (StreamWriter writer = new StreamWriter(LogFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to the log: {ex.Message}");
            }
        }
    }
}


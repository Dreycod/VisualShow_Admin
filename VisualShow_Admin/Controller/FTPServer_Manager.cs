using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentFTP;


namespace VisualShow_Admin.Controller
{
    public class FTPServer_Manager
    {
        public FTPServer_Manager()
        {

        }

        public async Task<List<string>> ListFilesFromFTP()
        {
            string ftpDirectory = "ftp://ftp-borne-arcade.alwaysdata.net/Images/KM103/";
            List<string> fileContents = new List<string>();


            using (var client = new AsyncFtpClient("ftp-borne-arcade.alwaysdata.net", "borne-arcade", "borne-testing"))
            {
                await client.Connect();

                // Get the list of files in the directory
                var items = await client.GetListing(ftpDirectory);

                Console.WriteLine("Files in directory:");

                foreach (var item in items)
                {
                    // Check if the item is a file
                    if (item.Type == FtpObjectType.File)
                    {
                        string filename = item.Name;
                        Console.WriteLine(filename);

                        // Example: You can process the file here without downloading
                        // Read the file content directly from the server as a stream
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            bool status = await client.DownloadStream(memoryStream, ftpDirectory + filename);

                            if (status == true)
                            {
                                // Reset the stream position to read it
                                memoryStream.Position = 0;

                                // Handle the file content (e.g., if it's a text file, you can read it as a string)
                                byte[] fileData = memoryStream.ToArray();
                                string fileContent = System.Text.Encoding.UTF8.GetString(fileData);  // Assuming it's a text file

                                fileContents.Add(fileContent);
                            }
                            else
                            {
                                Console.WriteLine($"Failed to read file: {filename}");
                            }
                        }
                    }
                }
                await client.Disconnect();
            }
            return fileContents;
        }

        public void UploadToFTPServer(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine("Local file does not exist.");
                return;
            }       

            try
            {
                string ftpPath = "ftp://ftp-borne-arcade.alwaysdata.net/Images/KM103/" + Path.GetFileName(filename);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Credentials = new NetworkCredential("borne-arcade", "borne-testing");
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UsePassive = true;

                using (FileStream fileStream = File.OpenRead(filename))
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        fileStream.CopyTo(requestStream);
                    }
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                Console.WriteLine($"Error: {response.StatusDescription}");
            }
        }



    }
}

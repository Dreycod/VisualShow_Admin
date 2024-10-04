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

        public async Task DownloadFromFTP(string filename)
        {
            string ftpPath = "ftp://ftp-borne-arcade.alwaysdata.net/Images/KM103/";
            var client = new AsyncFtpClient(ftpPath, "borne-arcade", "borne-testing");
            client.Connect();
            
           // await client.DownloadDirectory(@"")
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

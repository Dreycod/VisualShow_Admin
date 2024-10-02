using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VisualShow_Admin.Controller
{
    public class FTPServer_Manager
    {
        public FTPServer_Manager()
        {
        
        }

        public void UploadToFTPServer(string filename)
        {
            FtpWebRequest request =
            (FtpWebRequest)WebRequest.Create("ftp://ftp-borne-arcade.alwaysdata.net/Images/KM103/"+filename);
            request.Credentials = new NetworkCredential("borne-arcade", "borne-testing");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using(Stream fileStream = File.OpenRead(filename))
            using (Stream ftpStream = request.GetRequestStream())
            {
                fileStream.CopyTo(ftpStream);
            }
        }



    }
}

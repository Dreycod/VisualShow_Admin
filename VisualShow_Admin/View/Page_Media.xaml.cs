using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using VisualShow_Admin.Controller;
using System.IO;
using System.Collections.ObjectModel;

using System.Net;


namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Media.xaml
    /// </summary>
    public partial class Page_Media : Page
    {
        public ObservableCollection<ImageItem> Images { get; set; } = new ObservableCollection<ImageItem>();
        DAO_Ecrans daoEcrans;
        string filename = null;
        public Page_Media()
        {
            InitializeComponent();
            daoEcrans = new DAO_Ecrans();
            ImagesControl.ItemsSource = Images; // Bind ImagesControl to the ObservableCollection
            LoadComboBox();
        }

        public class MediaItem : INotifyPropertyChanged
        {
            private BitmapImage _imageSource;
            public string FileUrl { get; set; }
            public string NomSalle { get; set; }
            private AsyncFtpClient _ftpClient;  // Added FTP client to download images asynchronously

            public BitmapImage ImageSource
            {
                get { return _imageSource; }
                set
                {
                    _imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }

            public MediaItem(string fileUrl, string nomSalle, AsyncFtpClient ftpClient)
            {
                FileUrl = fileUrl;
                NomSalle = nomSalle;
                _ftpClient = ftpClient;

                // Detect media type and load image if it's an image file
                string extension = Path.GetExtension(fileUrl).ToLower();
                if (new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" }.Contains(extension))
                {
                    LoadImageAsync();  // Load the image asynchronously
                }
            }

            private async Task LoadImageAsync()
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Download the image from the FTP server into a memory stream
                        bool status = await _ftpClient.DownloadStream(memoryStream, FileUrl);

                        if (status)
                        {
                            memoryStream.Position = 0;
                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.StreamSource = memoryStream;
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.EndInit();

                            // Assign the image to the ImageSource property
                            ImageSource = image;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement de l'image: {ex.Message}");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public async void LoadComboBox()
        {
            var ecrans = await daoEcrans.GetEcrans();
            if (ecrans != null)
            {
                int count = ecrans.Count;

                for (int i = 0; i < count; i++)
                {
                    ScreenComboBox.Items.Add(ecrans[i].name);
                }
            }
        }

        private void UploadToFTPServer(string filePath)
        {
            string ftpUrl = "ftp://ftp-borne-arcade.alwaysdata.net/Images/" + ScreenComboBox.SelectedItem.ToString() + "/";
            string username = "borne-arcade";
            string password = "borne-testing";

            string fileName = System.IO.Path.GetFileName(filePath);
            string ftpFullUrl = ftpUrl + fileName;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullUrl);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(username, password);
            request.UsePassive = true; // Enable passive mode

            try
            {
                byte[] fileContents = File.ReadAllBytes(filePath);
                if (fileContents.Length == 0)
                {
                    MessageBox.Show("Error: File is empty or unreadable.");
                    return;
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    // Check the status code
                    if (response.StatusCode == FtpStatusCode.ClosingData)
                    {
                        MessageBox.Show("Upload complete!");
                    }
                    else
                    {
                        MessageBox.Show($"Upload failed: {response.StatusDescription}");
                    }
                }
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                MessageBox.Show($"Error uploading file: {response.StatusDescription}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}");
            }
        }


        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            if (filename != null)
            {
                SyncProgressBar.Visibility = Visibility.Visible; // Afficher la barre de progression
                // Appel de la méthode pour uploader le fichier vers le serveur FTP
                UploadToFTPServer(FilePathTextBox.Text);
                SyncProgressBar.Visibility = Visibility.Hidden; // Cacher la barre de progression après l'upload
            }
            else
            {
                MessageBox.Show("Please select a file to upload."); // Alerte si aucun fichier n'est sélectionné
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg"; // Extension par défaut
            dlg.Filter = "Image Files (*.jpg;*.png)|*.jpg;*.png"; // Types de fichiers autorisés

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName; // Stocker le chemin du fichier sélectionné
                FilePathTextBox.Text = filename; // Afficher le chemin dans une TextBox
                MessageBox.Show($"Selected file: {filename}"); // Afficher un message de confirmation
            }
        }
        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            // Find the selected image to delete
            var button = sender as Button;
            var imageItem = button.DataContext as ImageItem;

            if (imageItem != null)
            {
                string ftpUrl = "ftp://ftp-borne-arcade.alwaysdata.net/Images/" + ScreenComboBox.SelectedItem.ToString() + "/" + imageItem.FileName;
                string username = "borne-arcade";
                string password = "borne-testing";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(username, password);

                try
                {
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == FtpStatusCode.FileActionOK)
                        {
                            // Remove the image from the ObservableCollection
                            Images.Remove(imageItem);
                            MessageBox.Show("Image deleted successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting image: {ex.Message}");
                }
            }
        }

        private void OnImage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OnVideo_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Images.Clear();
            string selectedScreen = ScreenComboBox.SelectedItem.ToString();
            LoadFtpImages();
        }

        // List and download images from the FTP server
        private void LoadFtpImages()
        {
            string ftpUrl = "ftp://ftp-borne-arcade.alwaysdata.net/Images/"+ ScreenComboBox.SelectedItem.ToString()+"/";
            string username = "borne-arcade";
            string password = "borne-testing";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(username, password);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string fileName;
                    while ((fileName = reader.ReadLine()) != null)
                    {
                        string fileUrl = ftpUrl + fileName;
                        BitmapImage image = DownloadImage(fileUrl, username, password);

                        if (image != null)
                        {
                            Images.Add(new ImageItem { FileName = fileName, ImageSource = image });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Download an image from FTP
        private BitmapImage DownloadImage(string ftpUrl, string username, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(username, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    ms.Position = 0;

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    return image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading image: {ex.Message}");
                return null;
            }
        }
    }

    // Class to hold the image source
    public class ImageItem
    {
        public string FileName { get; set; } // Propriété pour stocker le nom du fichier
        public BitmapImage ImageSource { get; set; }
    }

}
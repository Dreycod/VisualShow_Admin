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
namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Media.xaml
    /// </summary>
    public partial class Page_Media : Page
    {
        FTPServer_Manager ftpServer_Manager;
        string filename = "aLegendaryNinja.png";
        DAO_Ecrans daoEcrans;
        public Page_Media()
        {
            InitializeComponent();
            ftpServer_Manager = new FTPServer_Manager();
            daoEcrans = new DAO_Ecrans();
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

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            // communicate with FTP server using settings from the settings view page
            SyncProgressBar.Visibility = Visibility.Visible;
            if (filename != null)
            {
                ftpServer_Manager.UploadToFTPServer(filename);
                SyncProgressBar.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Please select a file to upload");
            }
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg, .png";
            dlg.Filter = "Image Files (*.jpg, *.png)|*.jpg;*.png";
            Nullable<bool> result = dlg.ShowDialog();

            filename = dlg.FileName;
            FilePathTextBox.Text = filename;
            MessageBox.Show(filename);
        }
        
        public async Task<List<string>> GetFtpFiles()
        {
            List<string> fileContents = await ftpServer_Manager.ListFilesFromFTP();
            return fileContents;
        }

        private async void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedScreen = ScreenComboBox.SelectedItem.ToString();
            LoadMediaForClasse(selectedScreen);
        }

        private async Task LoadMediaForClasse(string nomSalle)
        {
            string ftpBaseUrl = "/Images/"; // Relative FTP path for the media directory
            string salleUrl = $"{ftpBaseUrl}{nomSalle}/";

            List<string> availableFiles = new List<string>();

            try
            {
                // Connect to the FTP server and list available files
                using (var client = new AsyncFtpClient("ftp-borne-arcade.alwaysdata.net", "borne-arcade", "borne-testing"))
                {
                    await client.Connect();
                    var items = await client.GetListing(salleUrl);

                    // Filter out directories and invalid file names
                    availableFiles = items
                        .Where(item => item.Type == FtpObjectType.File &&
                                       !string.IsNullOrWhiteSpace(item.Name) &&
                                       item.Name != "." && item.Name != "..")
                        .Select(item => item.Name)
                        .ToList();

                    if (availableFiles.Count == 0)
                    {
                        MessageBox.Show("Aucun fichier disponible pour la salle sélectionnée.");
                        ImagesControl.ItemsSource = null;
                        VideosControl.ItemsSource = null;
                        return;
                    }

                    // Filter and categorize media files (images and videos)
                    List<MediaItem> mediaItems = availableFiles
                        .Where(fileName => new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".mp4", ".avi", ".mov", ".wmv" }
                        .Contains(Path.GetExtension(fileName).ToLower()))
                        .Select(fileName =>
                        {
                            string fileUrl = $"{salleUrl}{fileName}";  // Construct FTP file path
                            return new MediaItem(fileUrl, nomSalle, client);
                        })
                        .ToList();

                    if (mediaItems.Count == 0)
                    {
                        MessageBox.Show("Aucun fichier valide disponible.");
                        ImagesControl.ItemsSource = null;
                        VideosControl.ItemsSource = null;
                        return;
                    }

                    // Separate images and videos and set them to the controls
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var images = mediaItems.Where(m => new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" }.Contains(Path.GetExtension(m.FileUrl).ToLower())).ToList();
                        var videos = mediaItems.Where(m => new[] { ".mp4", ".avi", ".mov", ".wmv" }.Contains(Path.GetExtension(m.FileUrl).ToLower())).ToList();

                       
                        ImagesControl.ItemsSource = images;
                        VideosControl.ItemsSource = videos;
                    });

                    await client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des fichiers : {ex.Message}");
            }
        }


    }
}

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using VisualShow_Admin.Controller;

namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Media.xaml
    /// </summary>
    public partial class Page_Media : Page
    {
        FTPServer_Manager ftpServer_Manager;
        string filename = "";
        public Page_Media()
        {
            InitializeComponent();
            ftpServer_Manager = new FTPServer_Manager();

        }

        private void UploadFile_Click(object sender, RoutedEventArgs e)
        {
            // communicate with FTP server using settings from the settings view page
            if (filename != null)
            {
                ftpServer_Manager.UploadToFTPServer(filename);
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
    }
}

using System.Text;
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
using VisualShow_Admin.Model;

namespace VisualShow_Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DAO_Etages dao_etages;
        public MainWindow()
        {
            InitializeComponent();
            dao_etages = new DAO_Etages();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ComboBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var reponse = await dao_etages.GetEtages();
            MessageBox.Show(reponse[0].name);
        }
    }
}
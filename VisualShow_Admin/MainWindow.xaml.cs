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
        DAO_Ecrans dao_ecrans;
        DAO_Events dao_events;
        DAO_Air dao_air;
        DAO_Salles dao_salles;
        DAO_Users dao_users;
        DAO_Son DAO_Son;
        DAO_TempHum DAO_TempHum;

        public MainWindow()
        {
            InitializeComponent();
            dao_etages = new DAO_Etages();
            dao_ecrans = new DAO_Ecrans();
            dao_events = new DAO_Events();
            dao_air = new DAO_Air();
            dao_salles = new DAO_Salles();
            dao_users = new DAO_Users();
            DAO_Son = new DAO_Son();
            DAO_TempHum = new DAO_TempHum();

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void ComboBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

            var response = await dao_etages.UpdateEtage("12", "Etage updated");
        }
    }
}
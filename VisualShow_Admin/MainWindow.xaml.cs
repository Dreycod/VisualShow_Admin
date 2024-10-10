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
    public partial class MainWindow
    {
        DAO_mqtt dao_mqtt;
        DAO_Etages dao_etages;
        DAO_Ecrans dao_ecrans;
        DAO_Events dao_events;
        DAO_Air dao_air;
        DAO_Salles dao_salles;
        DAO_Users dao_users;
        DAO_Son dao_son;
        DAO_TempHum dao_temphum;

        public MainWindow()
        {
            InitializeComponent();
            dao_mqtt = new DAO_mqtt();
            dao_etages = new DAO_Etages();
            dao_ecrans = new DAO_Ecrans();
            dao_events = new DAO_Events();
            dao_air = new DAO_Air();
            dao_salles = new DAO_Salles();
            dao_users = new DAO_Users();
            dao_son = new DAO_Son();
            dao_temphum = new DAO_TempHum();
            dao_mqtt.ConnexionBroker();
        }
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void NavigationViewItem_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
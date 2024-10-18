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
    /// Logique d'interaction pour Page_News.xaml
    /// </summary>
    public partial class Page_News : Page
    {
        DAO_mqtt prout = new DAO_mqtt();
        DAO_Ecrans daoEcrans = new DAO_Ecrans();
        public Page_News()
        {
            InitializeComponent();
            prout.ConnexionBroker();
            LoadComboBox();
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

        public void BroadcastMessage_Click(object sender, RoutedEventArgs e)
        {
            int num = MessagesListBox.Items.Count;
            num += 1;
            string cheminTopic = ScreenComboBox.SelectedItem.ToString()+"/emergency";
            string message = MessageInput.Text;
            prout.PublishTopicMessage(cheminTopic, message);
            ListBoxItem machin = new ListBoxItem();
            machin.Content = "Message " + num + ": " + message;
            MessagesListBox.Items.Add(machin);
        }

        private async void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedScreen = ScreenComboBox.SelectedItem.ToString();
        }
    }
}

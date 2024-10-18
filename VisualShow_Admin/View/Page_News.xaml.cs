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

namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_News.xaml
    /// </summary>
    public partial class Page_News : Page
    {
        DAO_mqtt prout = new DAO_mqtt();

        public Page_News()
        {
            InitializeComponent();
            prout.ConnexionBroker();
        }

        public void BroadcastMessage_Click(object sender, RoutedEventArgs e)
        {
            int num = MessagesListBox.Items.Count;
            num += 1;
            string cheminTopic = "KM103/emergency";
            string message = MessageInput.Text;
            prout.PublishTopicMessage(cheminTopic, message);
            ListBoxItem machin = new ListBoxItem();
            machin.Content = "Message " + num + ": " + message;
            MessagesListBox.Items.Add(machin);
        }
}
}

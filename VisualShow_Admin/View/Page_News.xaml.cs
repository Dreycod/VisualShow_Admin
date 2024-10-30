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
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;
using VisualShow_Admin.Controller;

namespace VisualShow_Admin.View
{
    public partial class Page_News : Page
    {
        DAO_mqtt prout = new DAO_mqtt();

        public Page_News()
        {
            InitializeComponent();
            prout.ConnectionStatusChanged += UpdateConnectionStatus;
            prout.ConnexionBroker();
        }

        public void BroadcastMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    int num = MessagesListBox.Items.Count;
                    num += 1;
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = MessageInput.Text;
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "Message " + num + ": " + message;
                    MessagesListBox.Items.Add(machin);
                }
                else
                {
                    MessageBox.Show("Please select a screen to sent the message to", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
            }
        }

        private async void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedScreen = ScreenComboBox.SelectedItem.ToString();
        }

        //publication préfaite d'un message d'urgence (incendie) sur le topic "emergency" du screen selectionné
        private void FireAlert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = "FireAlert";
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "Fire Alert issued";
                    MessagesListBox.Items.Add(machin);  //jhvsdlhfs
                }
                else
                {
                    MessageBox.Show("Please select a screen to sent the message to", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
            }
        }

        //publication préfaite d'un message d'urgence (alerte) sur le topic "emergency" du screen selectionné
        private void GeneralEmergency_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = "GeneralEmergency";
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "General Emergency issued";
                    MessagesListBox.Items.Add(machin);
                }
                else
                {
                    MessageBox.Show("Please select a screen to sent the message to", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
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

        //broadcast d'alerte incendie
        private void FireAlert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = "FireAlert";
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "Fire Alert issued";
                    MessagesListBox.Items.Add(machin);
                }
                else
                {
                    MessageBox.Show("Please select a screen to sent the message to", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
            }
        }


        //alerte d'intrusion
        private void IntruderAlert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = "IntruderAlert";
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "Intruder Alert issued";
                    MessagesListBox.Items.Add(machin);
                }
                else
                {
                    MessageBox.Show("Please select a screen", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
            }
        }

        //alerte générale
        private void GeneralEmergency_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ScreenComboBox.SelectedItem != null)
                {
                    string cheminTopic = ScreenComboBox.SelectedItem.ToString() + "/emergency";
                    string message = "GeneralEmergency";
                    prout.PublishTopicMessage(cheminTopic, message);
                    ListBoxItem machin = new ListBoxItem();
                    machin.Content = "General Emergency issued" ;
                    MessagesListBox.Items.Add(machin);
                }
                else
                {
                    MessageBox.Show("Please select a screen to sent the message to", "Error, no screen selected", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred", "Error", MessageBoxButton.OK);
            }
        }

        //fonction pour la couleur de l'indicateur de connexion (uniquement faite pour etre appelée)
        private void UpdateConnectionStatus(bool isConnected)
        {
            Dispatcher.Invoke(() =>
            {
                ConnectionStatusIndicator.Fill = new SolidColorBrush(isConnected ? Colors.Green : Colors.Red);
            });
        }

    }
}

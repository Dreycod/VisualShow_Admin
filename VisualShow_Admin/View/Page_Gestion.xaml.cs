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
using VisualShow_Admin.Model;

namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Gestion.xaml
    /// </summary>
    public partial class Page_Gestion : Page
    {
        DAO_Ecrans daoEcrans;

        public Page_Gestion()
        {
            InitializeComponent();
            daoEcrans = new DAO_Ecrans(); // Initialize the variable
            InitializeScreens(); // Call the method
        }

        private async void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScreenComboBox.SelectedValue != null)
            {
                string name = ScreenComboBox.SelectedIndex.ToString();
                MessageBox.Show(name);
                var ecran = await daoEcrans.GetEcranByName(name);

                // display the screen information on the listview LV_Ecran

                // Clear the listview
                LV_Ecrans.Items.Clear();

                // Add ecran to ListView
                LV_Ecrans.Items.Add(ecran.name);


            }
            else
            {
                MessageBox.Show("Please select a valid screen.");
            }
        }

        private async void InitializeScreens()
        {
            var ecrans = await daoEcrans.GetEcrans();
            foreach (var ecran in ecrans)
            {
                ScreenComboBox.Items.Add(ecran.name);

            }

        }
    }
}

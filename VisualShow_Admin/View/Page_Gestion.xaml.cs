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
    /// Logique d'interaction pour Page_Gestion.xaml
    /// </summary>
    public partial class Page_Gestion : Page
    {
        DAO_Ecrans daoEcrans;
        public Page_Gestion()
        {
            InitializeComponent();
             daoEcrans = new DAO_Ecrans();
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
        private void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
    }
}

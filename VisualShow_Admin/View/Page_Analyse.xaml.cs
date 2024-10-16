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
    /// Logique d'interaction pour Page_Analyse.xaml
    /// </summary>
    
    public partial class Page_Analyse : Page
    {
        DAO_Ecrans daoEcrans;
        DAO_Air daoAir;
        DAO_TempHum daoTempHum;

        public Page_Analyse()
        {
            InitializeComponent();
            daoEcrans = new DAO_Ecrans();
            daoAir = new DAO_Air();
            daoTempHum = new DAO_TempHum();
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
                    RoomComboBox.Items.Add(ecrans[i].name);
                }

            }
        }

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var TempHum = daoTempHum.GetTemp_Hum(RoomComboBox.SelectedItem.ToString());

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        DAO_Salles daoSalles;
        DAO_Etages daoEtages;
        DAO_Air daoAir;
        DAO_TempHum daoTempHum;

        public Page_Analyse()
        {
            InitializeComponent();
            daoEcrans = new DAO_Ecrans();
            daoAir = new DAO_Air();
            daoTempHum = new DAO_TempHum();
            daoSalles = new DAO_Salles();
            daoEtages = new DAO_Etages();
            daoAir = new DAO_Air();
            LoadComboBox();
        }
        public async void LoadComboBox()
        {
            var salles = await daoSalles.GetSalles();
            var etages = await daoEtages.GetEtages();

            if (salles != null)
            {
                int count = salles.Count;

                for (int i = 0; i < count; i++)
                {
                    RoomComboBox.Items.Add(salles[i].name);
                }

            }

            if (etages != null)
            {
                int count = etages.Count;

                for (int i = 0; i < count; i++)
                {
                    EtageComboBox.Items.Add(etages[i].name);
                }

            }
        }

        private async void LoadTempHumValues()
        {
            var Salle = daoSalles.GetSalleByName(RoomComboBox.SelectedItem.ToString());
            var TempHumList = await daoTempHum.GetTemp_Hum(Salle.Id.ToString());

            var Temps = new List<double>();
            var Hums = new List<double>();

            // Loop through each TempHum object in the list and extract temperature and humidity
            foreach (var tempHum in TempHumList)
            {
                Temps.Add(Double.Parse(tempHum.temperature));
                Hums.Add(Double.Parse(tempHum.humidite));
            }

        }

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTempHumValues();

        }

        private void EtageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

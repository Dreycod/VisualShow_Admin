using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
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
using VisualShow_Admin.Model;

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
        DAO_Son daoSon;
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
            daoSon = new DAO_Son(); 
            LoadComboBox();
        }
        public async void LoadComboBox()
        {
            var salles = await daoSalles.GetSalles();
            var etages = await daoEtages.GetEtages();
            var ecrans = await daoEcrans.GetEcrans();

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

            if (ecrans != null)
            {
                int count = ecrans.Count;

                for (int i = 0; i < count; i++)
                {
                    ScreenComboBox.Items.Add(ecrans[i].name);
                }
            }
        }

        private async void LoadGraphics(string name)
        {

            var Ecran = await daoEcrans.GetEcranByName(name);

            if (Ecran != null)
            {
                var TempHumList = await daoTempHum.GetTemp_Hum(Ecran[0].idecran.ToString());

                var temperatureViewModel = new TemperatureViewModel(TempHumList, true); // Use 'false' for Min Temperature

                TemperaturePlot.Visibility = Visibility.Visible;
                TemperaturePlot.Model = temperatureViewModel.TemperaturePlot;

                var SoundList = await daoSon.GetSon(Ecran[0].idecran.ToString());   

                var soundViewModel = new SonViewModel(SoundList); 
                SoundPlot.Visibility = Visibility.Visible;
                SoundPlot.Model = soundViewModel.SonPlot;

                var humiditeViewModel = new HumidityViewModel(TempHumList);
                HumidityPlot.Visibility = Visibility.Visible;
                HumidityPlot.Model = humiditeViewModel.HumidityPlot;

                var AirList = await daoAir.GetAir(Ecran[0].idecran.ToString()); 
                AirPlot.Visibility = Visibility.Visible;
                AirPlot.Model = new AirViewModel(AirList).AirPlot;
            }
        }

        private void RoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

        }

        private void EtageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ScreenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ecran_name = ScreenComboBox.SelectedItem.ToString();

            LoadGraphics(ecran_name);
        }
    }
}

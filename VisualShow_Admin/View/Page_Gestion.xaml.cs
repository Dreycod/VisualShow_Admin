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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualShow_Admin.Controller;
using VisualShow_Admin.View.ClientView;
namespace VisualShow_Admin.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Gestion.xaml
    /// </summary>
    public partial class Page_Gestion : Page
    {
        DAO_Ecrans daoEcrans;
        Client_Accueil page_accueil;
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
            int mainPage = 5000; // 30000
            int normalPage = 5000; // 20000

            page_accueil = new Client_Accueil();

            MethodAsync(normalPage, mainPage);
        }


        public async void MethodAsync(int normalPage, int mainPage)
        {
            while (true)
            {
                await SwitchContent(page_accueil, mainPage);
                //await SwitchContent(page_meteo, normalPage);
                //await SwitchContent(page_offres, normalPage);
                //await SwitchContent(page_agenda, normalPage);
                //await SwitchContent(page_dates, normalPage);
                //await SwitchContent(page_media, normalPage);
            }
        }

        private async Task SwitchContent(UIElement newContent, int delay)
        {
            UIElement oldContent = null;

            if (GridContainer.Children.Count > 0)
            {
                oldContent = GridContainer.Children[0];
            }

            Grid.SetZIndex(newContent, 1);
            newContent.RenderTransform = new TranslateTransform();
            GridContainer.Children.Add(newContent);

            if (oldContent != null)
            {
                var storyboardOut = (Storyboard)this.Resources["PageSlideOut"];
                var storyboardIn = (Storyboard)this.Resources["PageSlideIn"];

                oldContent.RenderTransform = new TranslateTransform();
                Storyboard.SetTarget(storyboardOut, oldContent);
                storyboardOut.Begin();

                Storyboard.SetTarget(storyboardIn, newContent);
                storyboardIn.Begin();

                await Task.Delay(1500);
                GridContainer.Children.Remove(oldContent);
            }

            await Task.Delay(delay);
        }
    }
}

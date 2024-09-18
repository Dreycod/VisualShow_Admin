using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using VisualShow_Admin.Model;

namespace VisualShow_Admin.Controller
{
    public class DAO_Etages
    {
        public async Task<List<Etages>> GetEtages()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEtages");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Etages> etages = JsonConvert.DeserializeObject<List<Etages>>(content);
                    return etages;

                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

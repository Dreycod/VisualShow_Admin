using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VisualShow_Admin.Model;

namespace VisualShow_Admin.Controller
{
    public class DAO_TempHum
    {
        public async Task<List<Temp_Hum>> GetTemp_Hum(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/Temp_Hum/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Temp_Hum> temp_hum = JsonConvert.DeserializeObject<List<Temp_Hum>>(content);
                    return temp_hum;

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

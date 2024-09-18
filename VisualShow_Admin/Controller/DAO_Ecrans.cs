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
    public class DAO_Ecrans
    {
        public async Task<List<Ecrans>> GetEcrans()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getEcrans");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Ecrans> ecrans = JsonConvert.DeserializeObject<List<Ecrans>>(content);
                    return ecrans;

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

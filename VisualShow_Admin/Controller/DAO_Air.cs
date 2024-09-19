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
    public class DAO_Air
    {
        public async Task<List<Air>> GetAir(string id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync("https://drey.alwaysdata.net/getAir/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content.Contains("error"))
                    {
                        MessageBox.Show("Error: " + content);
                        return null;
                    }
                    List<Air> air = JsonConvert.DeserializeObject<List<Air>>(content);
                    return air;

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

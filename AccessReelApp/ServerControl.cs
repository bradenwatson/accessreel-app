using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Text.Json;
using AccessReelApp.database_structures;

namespace AccessReelApp
{
    public class ServerControl
    {
        public ServerControl()
        {
            var client = new HttpClient();
            var response = client.GetAsync("https://accessreel.com");
            var responseString = response.Result.Content.ReadAsStringAsync().Result;

            Debug.WriteLine(responseString);
        }
    }
}

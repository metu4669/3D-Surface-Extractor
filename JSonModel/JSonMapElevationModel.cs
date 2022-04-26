using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Petcocel.JSonModel
{
    public class JSonMapElevationModel
    {
        // Parsing URL Getting From BING Map To Collect Elevation
        public List<int> Parse_Url_Bing(string url)
        {
            System.Net.WebClient newClient = new System.Net.WebClient();
            Console.WriteLine(url);
            var json = newClient.DownloadString(url);
            dynamic Json_Parsed = JsonConvert.DeserializeObject<JsonElevationModel>(json);
            List<int> result = Json_Parsed.resourceSets[0].resources[0].elevations;
            return result;
        }

        // Parsing Units Definition
        public class resourceSets
        {
            public int estimatedTotal { get; set; }
            public List<Resources> resources { get; set; }
        }
        public class Resources
        {
            public string __type { get; set; }
            public List<int> elevations { get; set; }
            public int zoomLevel { get; set; }
        }
        public class JsonElevationModel
        {
            public string authenticationResultCode { get; set; }
            public string brandLogoUri { get; set; }
            public string copyright { get; set; }
            public List<resourceSets> resourceSets { get; set; }
            public int statusCode { get; set; }
            public string statusDescription { get; set; }
            public string traceId { get; set; }
        }

    }

}

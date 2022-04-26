using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petcocel.GMapControl
{
    class Bing_Elevation_DATA
    {
        public JSonModel.JSonMapElevationModel JSon_Parser = new JSonModel.JSonMapElevationModel();
        // BING Elevation API Key
        private static string bing_key = "your_api_key";


        // Creating Parsing URL
        public List<int> Elevation_Collector_From_BING(double lat, double lng)
        {
            string lat_changed = lat.ToString().Replace(',', '.');
            string lng_changed = lng.ToString().Replace(',', '.');

            string url = "http://dev.virtualearth.net/REST/v1/Elevation/List?points=" + lat_changed + "," + lng_changed + "&key=" + bing_key;
            List<int> elevations = JSon_Parser.Parse_Url_Bing(url);
            return elevations;
        }

        public List<int> Elevation_Collector_For_Rectangle(double lat1, double lat2, double lng1, double lng2, double row, double col)
        {
            double min_lat = (lat1 > lat2) ? lat2 : lat1;
            double max_lat = (lat1 > lat2) ? lat1 : lat2;

            double min_lng = (lng1 > lng2) ? lng2 : lng1;
            double max_lng = (lng1 > lng2) ? lng1 : lng2;


            string lat1_changed = min_lat.ToString().Replace(',', '.');
            string lng1_changed = min_lng.ToString().Replace(',', '.');

            string lat2_changed = max_lat.ToString().Replace(',', '.');
            string lng2_changed = max_lng.ToString().Replace(',', '.');


            string url = "http://dev.virtualearth.net/REST/v1/Elevation/Bounds?bounds=" + lat1_changed + "," + lng1_changed + "," + lat2_changed + "," + lng2_changed + "&rows=" + row + "&cols=" + col + "&key=" + bing_key;
            List<int> elevations = JSon_Parser.Parse_Url_Bing(url);
            //Console.WriteLine("Left: " + min_lng + " Right: " + max_lng + " Bottom: " + min_lat + " Top: " + max_lat + "\n" + String.Join("-", elevations.ToArray()));

            return elevations;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing.Imaging;
using GMap;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.Projections;
using GMap.NET.WindowsForms.Markers;

namespace Petcocel.GMapControl
{
    class GMapController
    {
        GMapControl.Bing_Elevation_DATA Bing_Elevation_JSON_Collect = new Bing_Elevation_DATA();
        public GMap.NET.WindowsForms.GMapControl gMapControl;
        
        public List<PointLatLng> point_markers_coord = new List<PointLatLng>();
        public List<PointLatLng> well_markers_coord = new List<PointLatLng>();

        public List<GMapOverlay> all_overlays = new List<GMapOverlay>();
        public GMapOverlay all_polygons = new GMapOverlay();
        public GMapOverlay all_marker = new GMapOverlay();
        public GMapOverlay well_marker = new GMapOverlay();

        public GMapProvider current_gmap_provider;

        // Creating GMap Provider Array
        public void Map_Provider_Creator(List<GMapProvider> gmap_Array)
        {
            gmap_Array.Add(GMapProviders.GoogleMap);
            gmap_Array.Add(GMapProviders.GoogleTerrainMap);
            gmap_Array.Add(GMapProviders.GoogleSatelliteMap);
            gmap_Array.Add(GMapProviders.OpenStreetMap);
        }


        // GMap Initializer
        public void GMap_Initialize()
        {
            Initial_Component_SetUp();

            all_overlays.Add(all_polygons);
            all_overlays.Add(all_marker);
            all_overlays.Add(well_marker);
        }

        // Initial Component Setting
        public void Initial_Component_SetUp()
        {
            gMapControl.DragButton = MouseButtons.Right;
            gMapControl.ShowCenter = false;
            gMapControl.MapProvider = current_gmap_provider;
            gMapControl.Position = new PointLatLng(35.886342, 44.587616);
            gMapControl.Zoom = 10;
            gMapControl.MaxZoom = 18;
            gMapControl.MinZoom = 0;
            Gmap_Refresh();
        }

        // Refresh Map
        public void Gmap_Refresh()
        {
            gMapControl.Zoom++;
            gMapControl.Zoom--;
        }

        public void Add_Marker_With_Button(double lat, double lng)
        {
            PointLatLng point_cursor = new PointLatLng(lat, lng);
            point_markers_coord.Add(point_cursor);

            List<int> elevations = Bing_Elevation_JSON_Collect.Elevation_Collector_From_BING(lat, lng);

            GMarkerGoogle marker_current = new GMarkerGoogle(point_cursor, GMarkerGoogleType.red_dot);
            Editing_Marker(marker_current, elevations[0]);
            Adding_Marker(marker_current);

        }


        // Clear All Overlays
        public void Clear_All_Overlays()
        {
            Clear_Wells();
            Clear_All_Markers();

            gMapControl.Overlays.Clear();
            Gmap_Refresh();

        }
        public void Clear_All_Markers()
        {
            point_markers_coord.Clear();
            all_marker.Clear();
            all_polygons.Clear();
        }
        public void Clear_Wells()
        {
            well_markers_coord.Clear();
            well_marker.Clear();
        }


        // Draw Polygons
        public void Draw_Polygons(List<PointLatLng> corner_points)
        {
            gMapControl.Overlays.Remove(all_polygons);

            //point_markers = point_markers.OrderBy(x => Math.Atan2(x.Lat, x.Lng)).ToList();

            var new_polygon = new GMapPolygon(corner_points, "Polygon "+all_polygons.Polygons.Count())
            {
                Stroke = new Pen(Color.Red, 2),
                Fill = new SolidBrush(Color.DarkGray)
            };

            all_polygons.Polygons.Add(new_polygon);
            gMapControl.Overlays.Add(all_polygons);
            Gmap_Refresh();

        }

        // Mouse Control On Map
        public void Mouse_Click_Marker(double lat, double lng)
        {
            PointLatLng point_cursor = new PointLatLng(lat, lng);
            point_markers_coord.Add(point_cursor);
            Image input = Properties.Resources.oil_pump;
            Bitmap btmp = new Bitmap(input, new Size(20, 20));
            GMarkerGoogle marker_current = new GMarkerGoogle(point_cursor, GMarkerGoogleType.red_dot);

            List<int> elevations = Bing_Elevation_JSON_Collect.Elevation_Collector_From_BING(lat, lng);
            //GMarkerGoogle marker_current = new GMarkerGoogle(point_cursor, btmp);
            Editing_Marker(marker_current, elevations[0]);
            Adding_Marker(marker_current);
        }

        public void Mouse_Move_Drawing(double lat, double lng)
        {
            gMapControl.Overlays.Remove(all_polygons);

            PointLatLng point_cursor = new PointLatLng(lat, lng);
            point_markers_coord.Add(point_cursor);

            var polygon = new GMapPolygon(point_markers_coord, "New Polygon")
            {
                Stroke = new Pen(Color.Red, 2),
                Fill = new SolidBrush(Color.DarkGray)
            };
            all_polygons.Polygons.Add(polygon);
            gMapControl.Overlays.Add(all_polygons);
        }


        // Editing Well
        public void Editing_Well(GMarkerGoogle selected_marker , int marker_elevation)
        {
            selected_marker.ToolTipText = "\nWell Tag\n\nLatitude: " + selected_marker.Position.Lat + "\nLongitude: " + selected_marker.Position.Lng + "\nElevation: " + marker_elevation + " Meters";
            selected_marker.ToolTip.Fill = Brushes.Black;
            selected_marker.ToolTip.Foreground = Brushes.White;
            selected_marker.ToolTip.Stroke = Pens.Black;
            selected_marker.ToolTip.TextPadding = new Size(20, 20);
            selected_marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
        }


        // Editing and Adding Marker To Overlay
        public void Editing_Marker(GMarkerGoogle selected_marker , int marker_elevation)
        {
            selected_marker.ToolTipText = "\nMarker Tag\n\nLatitude: " + selected_marker.Position.Lat + "\nLongitude: " + selected_marker.Position.Lng + "\nElevation: " + marker_elevation + " Meters";
            selected_marker.ToolTip.Fill = Brushes.Black;
            selected_marker.ToolTip.Foreground = Brushes.White;
            selected_marker.ToolTip.Stroke = Pens.Black;
            selected_marker.ToolTip.TextPadding = new Size(20, 20);
            selected_marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
        }

        public void Adding_Marker(GMarkerGoogle selected_marker)
        {
            all_marker.Markers.Add(selected_marker);
            gMapControl.Overlays.Add(all_marker);
            Gmap_Refresh();
        }

        public void Adding_New_Well(double lat, double lng)
        {
            PointLatLng point_cursor = new PointLatLng(lat, lng);
            gMapControl.Position = point_cursor;
            gMapControl.Zoom = 10;
            well_markers_coord.Add(point_cursor);

            Image input = Properties.Resources.oil_pump;
            Bitmap btmp = new Bitmap(input, new Size(40, 40));
            GMarkerGoogle marker_current = new GMarkerGoogle(point_cursor, btmp);
            List<int> elevations = Bing_Elevation_JSON_Collect.Elevation_Collector_From_BING(lat, lng);

            Editing_Well(marker_current, elevations[0]);
            well_marker.Markers.Add(marker_current);
            gMapControl.Overlays.Add(well_marker);
            Gmap_Refresh();
        }

        
        public void Save_Polygon()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.DefaultExt = "poly";
            dialog.Filter = "Polygon File(*.poly)|*.poly|Txt File(*.txt)|*.txt";
            dialog.Title = "Select Save Folder";
            string file_name = dialog.FileName;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dialog.FileName))
                {
                    MessageBox.Show("File exist. Create new one!" +
                        "");
                }
                else
                {
                    file_name = dialog.FileName;
                    StreamWriter sw = new StreamWriter(File.Create(file_name));
                    string fold = "";
                    for (int i = 0; i < point_markers_coord.Count(); i++)
                    {
                        fold = fold + "" + "Lat:" + point_markers_coord[i].Lat + "/Lng:" + point_markers_coord[i].Lng + "\n";
                    }
                    sw.WriteLine(fold);
                    sw.Dispose();
                }
            }
        }

        public void Load_Polygon()
        {
            OpenFileDialog open_poly_file = new OpenFileDialog();
            open_poly_file.Filter = "Polygon files (*.poly)|*.poly|txt files (*.txt*)|*.txt*";
            open_poly_file.FilterIndex = 1;
            open_poly_file.RestoreDirectory = true;
            open_poly_file.CheckFileExists = false;
            open_poly_file.Multiselect = false;
            open_poly_file.Title = "Please Choose Poly File or Txt File";

            if (open_poly_file.ShowDialog() == DialogResult.OK)
            {
                string _filePath = open_poly_file.FileName;
                string _fileName = open_poly_file.SafeFileName;
                string _fileText = File.ReadAllText(_filePath);

                //Readed Text Splitting
                string[] lines = _fileText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                point_markers_coord.Clear();
                foreach (string line in lines)
                {
                    if (!string.Equals(line, ""))
                    {
                        string lat_holder = line.Split('/')[0];
                        string lng_holder = line.Split('/')[1];

                        double lat = Convert.ToDouble(lat_holder.Split(':')[1]);
                        double lng = Convert.ToDouble(lng_holder.Split(':')[1]);

                        point_markers_coord.Add(new PointLatLng(lat, lng));
                    }
                }

                gMapControl.Overlays.Remove(all_polygons);

                //point_markers = point_markers.OrderBy(x => Math.Atan2(x.Lat, x.Lng)).ToList();

                var polygon = new GMapPolygon(point_markers_coord, "New Polygon")
                {
                    Stroke = new Pen(Color.Red, 2),
                    Fill = new SolidBrush(Color.DarkGray)
                };

                all_polygons.Polygons.Add(polygon);
                gMapControl.Overlays.Add(all_polygons);
                Gmap_Refresh();
            }
        }
    }
}
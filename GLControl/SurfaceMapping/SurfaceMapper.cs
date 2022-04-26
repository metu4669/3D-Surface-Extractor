using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using System.Drawing;

namespace Petcocel.GLControl.SurfaceMapping
{
    class SurfaceMapper
    {
        GMapControl.Bing_Elevation_DATA Bing_Elevation_JSON_Collect = new GMapControl.Bing_Elevation_DATA();
        public bool create_surface_model = false;
        
        int division = 12;
        int max_bing_division = 32;
        List<int> temp_elevation = new List<int>();

        List<double[]> main_regions = new List<double[]>();

        public List<List<double>> main_elevations_doble = new List<List<double>>();
        public List<List<double>> main_elevations_real = new List<List<double>>();

        GMapControl.DistanceMeasure.DistanceMeasure distance_measure_tool = new GMapControl.DistanceMeasure.DistanceMeasure();

        public double scale = 10;
        public double width = 1;
        public double height = 1;

        public Vector3d[,] normal_vectors;

        public void Create_Regions(double min_lat, double max_lat, double min_lng, double max_lng, int lat_division, int lng_division)
        {
            // Min_Lat Max_Lat Min_Lng Max_Lng

            double inc_lat = (max_lat - min_lat) / lat_division;
            double inc_lng = (max_lng - min_lng) / lng_division;

            for (int j = 0; j < lng_division; j++)
            {
                for (int i = 0; i < lat_division; i++)
                {
                    double[] temp_coor_holder = new double[4];
                    temp_coor_holder[0] = min_lat + i * inc_lat;
                    temp_coor_holder[1] = min_lat + (i + 1) * inc_lat;
                    temp_coor_holder[2] = min_lng + j * inc_lng;
                    temp_coor_holder[3] = min_lng + (j + 1) * inc_lng;

                    main_regions.Add(temp_coor_holder);

                    temp_elevation = Bing_Elevation_JSON_Collect.Elevation_Collector_For_Rectangle(temp_coor_holder[0], temp_coor_holder[1], temp_coor_holder[2], temp_coor_holder[3], max_bing_division, max_bing_division);

                    int min = temp_elevation.Min();
                    List<double> t = new List<double>();

                    for (int k = 0; k < temp_elevation.Count; k++)
                    {
                        t.Add(temp_elevation[k]);
                    }
                    main_elevations_real.Add(t);
                }
            }

        }

        public void DrawGrid()
        {
            int side_count = division * (max_bing_division - 1) + 2;
            normal_vectors = new Vector3d[side_count, side_count];
            /*
            for(int m =0; m < side_count; m++)
            {
                for (int n = 0; n < side_count; n++)
                {
                    normal_vectors[m, n] = Vector3d.Zero;
                }
            }
            */
            Color color1 = Color.Blue;
            Color color2 = Color.Blue;
            Color color3 = Color.Blue;
            Color color4 = Color.Blue;

            GL.Translate(-max_bing_division * division * width / 2, 0, -max_bing_division * division * height / 2);
            int counter = 0;

            for (int sj = 0; sj < division; sj++)
            {
                for (int si = 0; si < division; si++)
                {
                    double startingI = sj * (max_bing_division - 1) * width;
                    double startingJ = si * (max_bing_division - 1) * height;

                    double max_I = (max_bing_division - 1) * width * (division - 1) + (max_bing_division - 1) * width;
                    double max_J = (max_bing_division - 1) * height * (division - 1) + (max_bing_division - 1) * height;

                    List<double> temp_carrier = main_elevations_doble[counter++];

                    for (int j = 0; j < max_bing_division - 1; j++)
                    {
                        for (int i = 0; i < max_bing_division - 1; i++)
                        {
                            int current_index = i + j * max_bing_division;

                            double left = startingI + 0 + i * width;
                            double right = startingI + (1 + i) * width;
                            double bottom = startingJ + 0 + j * height;
                            double top = startingJ + (1 + j) * height;

                            double height1 = temp_carrier[current_index];
                            double height2 = temp_carrier[current_index + 1];
                            double height3 = temp_carrier[current_index + max_bing_division + 1];
                            double height4 = temp_carrier[current_index + max_bing_division];


                            Vector3d left_bottom = new Vector3d(left, bottom, height1);
                            Vector3d right_bottom = new Vector3d(right, bottom, height2);
                            Vector3d right_top = new Vector3d(right, top, height3);
                            Vector3d left_top = new Vector3d(left, top, height4);

                            Vector3d normal = -Vector3d.Cross(right_bottom - left_bottom, left_top - left_bottom);
                            normal_vectors[(int)bottom + 1, (int)left + 1] = normal;

                        }
                    }
                }
            }
            /*
                        for (int m = 0; m < side_count; m++)
                        {
                            for (int n = 0; n < side_count; n++)
                            {
                                Console.WriteLine("Index: " +  m + "," + n + "\nValue: " + normal_vectors[m, n]);
                            }
                        }
                        */
            counter = 0;

            for (int sj = 0; sj < division; sj++)
            {
                for (int si = 0; si < division; si++)
                {
                    double startingI = sj * (max_bing_division - 1) * width;
                    double startingJ = si * (max_bing_division - 1) * height;

                    double max_I = (max_bing_division - 1) * width * (division - 1) + (max_bing_division - 1) * width;
                    double max_J = (max_bing_division - 1) * height * (division - 1) + (max_bing_division - 1) * height;

                    List<double> temp_carrier = main_elevations_doble[counter++];

                    for (int j = 0; j < max_bing_division - 1; j++)
                    {
                        for (int i = 0; i < max_bing_division - 1; i++)
                        {
                            int current_index = i + j * max_bing_division;

                            double left = startingI + 0 + i * width;
                            double right = startingI + (1 + i) * width;
                            double bottom = startingJ + 0 + j * height;
                            double top = startingJ + (1 + j) * height;

                            double height1 = temp_carrier[current_index];
                            double height2 = temp_carrier[current_index + 1];
                            double height3 = temp_carrier[current_index + max_bing_division + 1];
                            double height4 = temp_carrier[current_index + max_bing_division];

                            double avg = (height1 + height2 + height3 + height4) / 4;

                            color1 = Color.Red;

                            if (avg * 10 > 3 && avg * 10 < 500)
                            {
                                color1 = Color.Green;
                            }
                            else if (avg * 10 >= 500 && avg * 10 < 1000)
                            {
                                color1 = Color.Red;
                            }
                            else if (avg * 10 >= 1000)
                            {
                                color1 = Color.Brown;
                            }

                            Vector3d left_bottom = new Vector3d(left, bottom, height1);
                            Vector3d right_bottom = new Vector3d(right, bottom, height2);
                            Vector3d right_top = new Vector3d(right, top, height3);
                            Vector3d left_top = new Vector3d(left, top, height4);

                            Vector3d normal = -Vector3d.Cross(right_bottom - left_bottom, left_top - left_bottom);

                            GL.Begin(PrimitiveType.Quads);
                            GL.PushMatrix();

                            GL.Normal3(normal);
                            GL.Color3(color1);
                            GL.Vertex3(left_bottom);

                            //GL.Color3(color2);
                            GL.Vertex3(right_bottom);

                            //GL.Color3(color3);
                            GL.Vertex3(right_top);

                            //GL.Color3(color4);
                            GL.Vertex3(left_top);

                            GL.PopMatrix();
                            GL.End();



                        }
                    }
                }
            }
        }

        public void Get_Marker_Data(double lt1, double ln1, double lt2, double ln2)
        {

            main_regions.Clear();
            main_elevations_real.Clear();
            main_elevations_doble.Clear();
            temp_elevation.Clear();

            double min_lat = (lt1 > lt2) ? lt2 : lt1;
            double max_lat = (lt1 > lt2) ? lt1 : lt2;

            double min_lng = (ln1 > ln2) ? ln2 : ln1;
            double max_lng = (ln1 > ln2) ? ln1 : ln2;

            double l1 = distance_measure_tool.GetDistanceBetweenPoints(min_lat, min_lng, min_lat, max_lng);
            double l2 = distance_measure_tool.GetDistanceBetweenPoints(max_lat, min_lng, max_lat, max_lng);

            double h1 = distance_measure_tool.GetDistanceBetweenPoints(min_lat, min_lng, max_lat, min_lng);
            double h2 = distance_measure_tool.GetDistanceBetweenPoints(min_lat, max_lng, max_lat, max_lng);
            double avg_width = (l1 + l2) / 2;
            double avg_height = (h1 + h2) / 3;

            height = avg_height / avg_width;

            Create_Regions(min_lat, max_lat, min_lng, max_lng, division, division);
            set_scaling();
            create_surface_model = true;
        }

        public void set_scaling()
        {
            main_elevations_doble.Clear();
            main_elevations_doble = new List<List<double>>();

            for (int i = 0; i < main_elevations_real.Count; i++)
            {
                List<double> temp = new List<double>();
                for (int j = 0; j < main_elevations_real[i].Count; j++)
                {
                    temp.Add(main_elevations_real[i][j] / scale);
                }
                main_elevations_doble.Add(temp);
            }
            create_surface_model = true;
        }
        
    }
}

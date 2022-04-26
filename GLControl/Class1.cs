/*
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace Petcocel.GLControl
{
    class GLControlUnit
    {
        GMapControl.Bing_Elevation_DATA Bing_Elevation_JSON_Collect = new GMapControl.Bing_Elevation_DATA();
        MouseController.MouseController mouse_controller = new MouseController.MouseController();
        KeyboardController.KeyboardController keyboard_controller = new KeyboardController.KeyboardController();
        public OpenTK.GLControl glControl1;


        public double offset_x = 0, offset_y = 0;
        public double current_x = 0, current_y = 0;
        public double z = 12f;

        public List<Color> color = new List<Color>();

        public Label lx;
        public Label ly;
        public Label dx;
        public Label dy;
        public Label nx;
        public Label ny;

        public List<Vector3> corners;
        public bool called_draw_polygon = false;

        Random rnd = new Random();
        int width = 1, max_block_division = 32, side_region = 4;
        List<int> elevations;
        List<List<double>> elevation_holder = new List<List<double>>();
        List<List<double>> main_elevation = new List<List<double>>();
        List<double[]> holder = new List<double[]>();


        // Random Color Creating
        public void Color_Random_Creator() {
            for (int i = 0; i < 20; i++)
            {
                color.Add(Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255)));
            }
        }
        // Creating Mouse and Keyboard Control on GLControl Screen
        public void Mouse_Control_Variable_Setting()
        {
            mouse_controller.glControl1 = glControl1;
            mouse_controller.glControlUnit = this;
            mouse_controller.Mouse_Control_Initialize();
        }
        public void Keyboard_Control_Variable_Setting()
        {
            keyboard_controller.glControl1 = glControl1;
            keyboard_controller.glControlUnit = this;
            keyboard_controller.Keyboard_Control_Initialize();
        }

        //Fires First Start
        public void Starting()
        {
            double right = 36.0402158688011f;
            double bottom = 44.2790222167969f;

            double left = 36.0082839912877f;
            double top = 44.3669128417969f;
            int row = side_region + 1;
            int col = side_region + 1;

            //Create_Regions(left, right, bottom, top, row, col);

            call_trigger_bing();


            Color_Random_Creator();

            glControl1.Load += GL_Control_Load;
            glControl1.Paint += GL_Control_Paint;
            glControl1.Resize += GL_Control_Resize;

            Mouse_Control_Variable_Setting();
            Keyboard_Control_Variable_Setting();


        }

        public void GL_Viewport_Setup(float zNear, float zFar)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, glControl1.Width / glControl1.Height, zNear, zFar);
            GL.LoadMatrix(ref matrix);

            GL.Translate(0, 0, -z);
            GL.Rotate(current_x, 0, 1, 0);
            GL.Rotate(current_y, 1, 0, 0);
        }

        // GLControl Frame Controller -> First Loading, Painting, Resizing
        public void GL_Control_Resize(object o, EventArgs e)
        {
            GL_Viewport_Setup(0.01f, 500f);
        }
        public void GL_Control_Load(object o, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            GL_Viewport_Setup(0.01f, 500f);
        }
        public void GL_Control_Paint(object o, EventArgs e)
        {
            GL_Viewport_Setup(0.01f, 500f);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);


            DrawGrid();
            glControl1.SwapBuffers();

        }




        /// <summary>
        /// GLControl Section Over
        /// 
        /// Drawing UNITS starting
        /// </summary>


        List<double[]> corner_point_regions = new List<double[]>();
        public List<double[]> region_corner_coordinates = new List<double[]>();
        public List<List<double>> elevation_for_region = new List<List<double>>();

        public void Create_Regions(double left, double right, double bottom, double top, int row, int col)
        {
            int width_division = col - 1;
            int heigth_division = row - 1;

            double delta_width = (right - left) / width_division;
            double delta_height = (top - bottom) / heigth_division;

            for(int j = 0; j < heigth_division; j++)
            {
                for(int i = 0; i< width_division; i++)
                {
                    double[] corners = new double[4];

                    corners[0] = left + i * delta_width; // Left
                    corners[1] = left + (i + 1) * delta_width; // Right
                    corners[2] = bottom + j * delta_height; // Bottom
                    corners[3] = bottom + (j + 1) * delta_height; // Top

                    corner_point_regions.Add(corners);
                    string temp = (corner_point_regions.Count % 5 == 0) ? " ---" : "";

                    List<double> temp_elevation = create_elevation_list(corners[0], corners[1], corners[2], corners[3], max_block_division, max_block_division);
                    elevation_for_region.Add(temp_elevation);

                    Console.WriteLine("Current: " + corner_point_regions.Count + " Left: " + corners[0] + " Right: " + corners[1] + " Bottom: " + corners[2] + " Top: " + corners[3]+" "+ temp);
                    Console.WriteLine(String.Join(", ", temp_elevation.ToArray()) + "\n-----------------------------------------------------------");
                }
            }


        }

        public List<double> create_elevation_list(double left, double right, double bottom, double top, double row, double col)
        {
            List<double> temp_elevation = new List<double>();
            elevations = new List<int>();
            elevations.Clear();

            elevations = Bing_Elevation_JSON_Collect.Elevation_Collector_For_Rectangle(left, right, top, bottom, row, col);

            int min = elevations.Min();

            for (int k = 0; k < elevations.Count; k++)
            {
                temp_elevation.Add(elevations[k]);
            }

            int max = elevations.Max();

            for (int k = 0; k < elevations.Count; k++)
            {
                temp_elevation[k] = temp_elevation[k]/10;
            }
            return temp_elevation;
        }


        public void DrawGrid()
        {
            GL.Translate(-max_block_division * side_region * width / 2, -max_block_division * side_region * width / 2, 0);

            for (int sj = 0; sj < side_region; sj++)
            {
                for (int si = 0; si < side_region; si++)
                {
                    int startingI = si * max_block_division;
                    int startingJ = sj * max_block_division;

                    List<double> temp_carrier = main_elevation[si + sj * side_region];

                    for (int j = 0; j < max_block_division - 1; j++)
                    {
                        for (int i = 0; i < max_block_division - 1; i++)
                        {
                            int current_index = i + j * max_block_division;

                            GL.Begin(PrimitiveType.LineLoop);
                            GL.PushMatrix();
                            GL.Color3(Color.White);
                            GL.Vertex3(startingI + 0 + i, startingJ + 0 + j, temp_carrier[current_index]);
                            GL.Vertex3(startingI + 1 + i, startingJ + 0 + j, temp_carrier[current_index + 1]);
                            GL.Vertex3(startingI + 1 + i, startingJ + 1 + j, temp_carrier[current_index + max_block_division + 1]);
                            GL.Vertex3(startingI + 0 + i, startingJ + 1 + j, temp_carrier[current_index + max_block_division]);
                            GL.PopMatrix();
                            GL.End();
                        }
                    }
                }
            }



        }

        public void call_trigger_bing()
        {

            double top = 36.0402158688011f;
            double left = 44.2790222167969f;

            double bottom = 36.0082839912877f;
            double right = 44.3669128417969f;


            create_division(left, right, top, bottom, side_region);

        }

        public void create_division(double left, double right, double top, double bottom, int division)
        {
            double fraction_width = (right - left) / (division-1);
            double fraction_heigth = (top - bottom) / (division-1);


            for (int j = 0; j < division; j++)
            {
                for (int i = 0; i < division; i++)
                {
                    double[] temp_double = new double[4];

                    temp_double[0] = left + i * fraction_width;
                    temp_double[1] = left + (i + 1) * fraction_width;

                    temp_double[2] = bottom + j * fraction_heigth;
                    temp_double[3] = bottom + (j + 1) * fraction_heigth;

                    holder.Add(temp_double);
                }
            }


            for (int i = 0; i < holder.Count; i++)
            {
                elevation_holder.Add(create_elevation_list(holder[i][0], holder[i][1], holder[i][2], holder[i][3], max_block_division, max_block_division));
            }


            for (int j = 0; j < side_region; j++)
            {
                for (int i = 0; i < side_region; i++)
                {
                    main_elevation.Add(elevation_holder[j + i * side_region]);
                }
            }

            // MessageBox.Show(String.Join("- ", elevation_holder[0].ToArray()) + "\n" + String.Join("- ", elevation_holder[1].ToArray()));
        }

    }
}

/*
 
            double left = 36.0402158688011f;
            double top = 44.2790222167969f;

            double right = 36.0082839912877f;
            double bottom = 44.3669128417969f;

    
            double left = 31.0f;
            double bottom = 41.0f;

            double right = 31.2f;
            double top = 41.2f;
            
        private void DrawBox()
        {
            DrawBoxLine();

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3(-1.0, 1.0, 1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);

            GL.Color3(1.0, 0.0, 1.0);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(1.0, -1.0, 1.0);

            GL.Color3(0.0, 1.0, 1.0);
            GL.Vertex3(1.0, -1.0, 1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);

            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, 1.0);

            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);

            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, -1.0, 1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);
            GL.Vertex3(-1.0, 1.0, 1.0);
            GL.End();
        }
        private void DrawBoxLine()
        {

            GL.Begin(PrimitiveType.Lines);
            GL.PushMatrix();
            GL.Color3(Color.Black);
            GL.Vertex3(-1.0, 1.0, 1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);

            GL.Color3(Color.Black);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(1.0, -1.0, 1.0);

            GL.Color3(Color.Black);
            GL.Vertex3(1.0, -1.0, 1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);

            GL.Color3(Color.Black);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, 1.0);

            GL.Color3(Color.Black);
            GL.Vertex3(1.0, 1.0, -1.0);
            GL.Vertex3(1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, -1.0, -1.0);
            GL.Vertex3(-1.0, 1.0, -1.0);

            GL.Color3(Color.Black);
            GL.Vertex3(1.0, 1.0, 1.0);
            GL.Vertex3(1.0, -1.0, 1.0);
            GL.Vertex3(-1.0, -1.0, 1.0);
            GL.Vertex3(-1.0, 1.0, 1.0);
            GL.PopMatrix();
            GL.End();
        }
        public void DrawPolygon(List<Vector3> corners)
        {
            Random rnd = new Random();
            float x_Max = 0, y_Max = 0, z_Max = 0;

            for (int i = 0; i < corners.Count; i++)
            {
                if (Math.Abs(corners[i].X) > x_Max)
                {
                    x_Max = Math.Abs(corners[i].X);
                }
                if (Math.Abs(corners[i].Y) > y_Max)
                {
                    y_Max = Math.Abs(corners[i].Y);
                }
                if (Math.Abs(corners[i].Z) > z_Max)
                {
                    z_Max = Math.Abs(corners[i].Z);
                }
            }

            GL.Ortho(0, glControl1.Width, glControl1.Height, 0, 0, 1);


            GL.Begin(PrimitiveType.Polygon);
            for (int i = 0; i < corners.Count; i++)
            {
                GL.Color3(color[rnd.Next(0, 10)]);

                GL.Vertex3(corners[i].X, corners[i].Z, corners[i].Y);
            }

            GL.End();
        }
        public void DrawNb()
        {
            Random rnd = new Random();
            GL.Begin(PrimitiveType.Quads);

            GL.Color3(color[0]);
            GL.Vertex2(2, 2);
            GL.Color3(color[1]);
            GL.Vertex2(-2, 2);
            GL.Color3(color[2]);
            GL.Vertex2(-2, -2);
            GL.Color3(color[3]);
            GL.Vertex2(2, -2);

            GL.End();
        }
 */


/*


    public void DrawGrid()
    {
        GL.Translate(-max_block_division * side_region * width / 2, -max_block_division * side_region * width / 2, 0);

        for(int sj = 0; sj < side_region; sj++)
        {
            for (int si = 0; si < side_region; si++)
            {
                int startingI = si * max_block_division;
                int startingJ = sj * max_block_division;

                List<double> temp_carrier = main_elevation[si + sj * side_region];

                for (int j = 0; j < max_block_division - 1; j++)
                {
                    for (int i = 0; i < max_block_division - 1; i++)
                    {
                        int current_index = i + j * max_block_division;

                        GL.Begin(PrimitiveType.LineLoop);
                        GL.PushMatrix();
                        GL.Color3(Color.White);
                        GL.Vertex3(startingI + 0 + i, startingJ + 0 + j, temp_carrier[current_index]);
                        GL.Vertex3(startingI + 1 + i, startingJ + 0 + j, temp_carrier[current_index + 1]);
                        GL.Vertex3(startingI + 1 + i, startingJ + 1 + j, temp_carrier[current_index + max_block_division + 1]);
                        GL.Vertex3(startingI + 0 + i, startingJ + 1 + j, temp_carrier[current_index + max_block_division]);
                        GL.PopMatrix();
                        GL.End();
                    }
                }
            }
        }



    }

    public void call_trigger_bing()
    {
        double right = 36.0402158688011f;
        double bottom = 44.2790222167969f;

        double left = 36.0082839912877f;
        double top = 44.3669128417969f;

        create_division(left, right, top, bottom, side_region);

    }


    public void create_division(double left, double right, double top, double bottom, int division)
    {
        double fraction_width = (right - left) / (division);
        double fraction_heigth = (top - bottom) / (division);


        for(int j=0; j<division; j++)
        {
            for (int i = 0; i < division; i++)
            {
                double[] temp_double = new double[4];

                temp_double[0] = left + i * fraction_width;
                temp_double[1] = left + (i + 1) * fraction_width;

                temp_double[2] = bottom + j * fraction_heigth;
                temp_double[3] = bottom + (j + 1) * fraction_heigth;

                holder.Add(temp_double);
            }
        }


        for (int i = 0; i < holder.Count; i++)
        {
            elevation_holder.Add(create_elevation_list(holder[i][0], holder[i][1], holder[i][2], holder[i][3]));
        }


        for(int j = 0; j<side_region; j++)
        {
            for (int i = 0; i < side_region; i++)
            {
                main_elevation.Add(elevation_holder[j + i * side_region]);
            }
        }

       // MessageBox.Show(String.Join("- ", elevation_holder[0].ToArray()) + "\n" + String.Join("- ", elevation_holder[1].ToArray()));
    }

    public List<double> create_elevation_list(double left, double right, double bottom, double top)
    {
        List<double> temp_elevation = new List<double>();
        elevations = new List<int>();
        elevations.Clear();

        elevations = Bing_Elevation_JSON_Collect.Elevation_Collector_For_Rectangle(left, right, top, bottom, max_block_division, max_block_division);

        int min = elevations.Min();

        for (int k = 0; k < elevations.Count; k++)
        {
            temp_elevation.Add(elevations[k] - min);
        }

        int max = elevations.Max();

        for (int k = 0; k < elevations.Count; k++)
        {
            temp_elevation[k] = temp_elevation[k]/5;
        }
        return temp_elevation;
    }
}
 */
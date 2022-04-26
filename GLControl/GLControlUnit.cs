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
        MouseController.MouseController mouse_controller = new MouseController.MouseController();
        KeyboardController.KeyboardController keyboard_controller = new KeyboardController.KeyboardController();
        SurfaceMapping.SurfaceMapper surface_mapper = new SurfaceMapping.SurfaceMapper();

        public OpenTK.GLControl glControl1;


        public double offset_x = -10, offset_y = -10;
        public double current_x = 10, current_y = 10;
        public double object_current_x = 0, object_current_y = 0, object_current_z = 0;
        public Vector3d current_rotation = Vector3d.One;
        public double z = 30f;
        public double scale = 10;

        public List<Color> color = new List<Color>();


         public bool called_draw_polygon = false;
        public bool create_surface_model = false;
        public bool initial_draw_called = false;

        Random rnd = new Random();     
        
        private int block_size = 100, grid_size = 32;



        // Random Color Creating
        public void Color_Random_Creator()
        {
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


        Main form;
        //Fires First Start
        public void Starting()
        {
            Color_Random_Creator();

            glControl1.Load += GL_Control_Load;
            glControl1.Paint += GL_Control_Paint;
            glControl1.Resize += GL_Control_Resize;

            Mouse_Control_Variable_Setting();
            Keyboard_Control_Variable_Setting();


        }
        // Setting Viewport
        public void GL_Viewport_Setup(float zNear, float zFar)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Fog Enable
            GL.Enable(EnableCap.Fog);
            GL.Fog(FogParameter.FogColor, new float[4] { 0.0f, 0.0f, 0.0f, 1.0f });//Same color as clear.
            GL.Fog(FogParameter.FogMode, (int)FogMode.Linear);
            GL.Fog(FogParameter.FogEnd, (float)(block_size * grid_size / 3));


            // Setting ViewPort/Projection
            GL.Enable(EnableCap.DepthTest);
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 matrix = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 3, glControl1.Width / glControl1.Height, zNear, zFar);
            GL.LoadMatrix(ref matrix);
            GL.MatrixMode(MatrixMode.Modelview);

            //Translating and Rotating
            GL.Rotate(current_x, Vector3d.UnitY);
            GL.Rotate(current_y, Vector3d.UnitX);
            initial_draw_called = !initial_draw_called;


            z = (z > 500) ? 500 : z;
            GL.Translate(offset_x, offset_y, -z);

            // Enable Lighting

            //GL.Enable(EnableCap.Lighting);
            float[] light_pos = { 80, 20, -20 };
            float[] light_diffuse = { 1.0f, 1.0f, 2.0f };
            float[] light_ambient = { 1.0f, 1.0f, 1.0f };

            //GL.Light(LightName.Light0, LightParameter.Position, light_pos);
            //GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuse);
            // GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            //GL.Enable(EnableCap.Light0);
        }



        // GLControl Frame Controller -> First Loading, Painting, Resizing
        public void GL_Control_Resize(object o, EventArgs e)
        {
            GL_Viewport_Setup(0.01f, 500f);
        }
        public void GL_Control_Load(object o, EventArgs e)
        {
            //GL.ClearColor(Color.FromArgb(71, 71, 71));
            GL.ClearColor(Color.Black);

            GL_Viewport_Setup(1f, 500000f);
        }
        public void GL_Control_Paint(object o, EventArgs e)
        {
            GL_Viewport_Setup(1f, 500000f);

            //Draw Grid Plane
           // DrawGrid(System.Drawing.Color.White, grid_size, block_size);

           // DrawCircle(0, 0, 0, 2, Color.Red, Color.Blue, Color.Yellow);

           // Vector3d n_direct;
            GL.Rotate(object_current_x, Vector3d.UnitY);
            GL.Rotate(object_current_y, Vector3d.UnitX);
            GL.Rotate(object_current_z, Vector3d.UnitZ);

           
            DrawBox();

            if (create_surface_model)
            {

            }
            else if (called_draw_polygon)
            {

            }


            glControl1.SwapBuffers();

        }

        public static void DrawCircle(float x, float y, float z, float radius, Color c1, Color c2, Color c3)
        {
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(c1);

            for (int i = 0; i < 360; i++)
            {
                GL.Vertex3(x + Math.Cos(i * Math.PI / 180) * radius, y + Math.Sin(i * Math.PI / 180) * radius, 0);
            }

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(c2);

            for (int i = 0; i < 360; i++)
            {
                GL.Vertex3(0, y + Math.Sin(i * Math.PI / 180) * radius,  + z - Math.Cos(i * Math.PI / 180) * radius);
            }

            GL.End();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Color3(c3);

            for (int i = 0; i < 360; i++)
            {
                GL.Vertex3(x + Math.Cos(i * Math.PI / 180) * radius, 0, z - Math.Sin(i * Math.PI / 180) * radius);
            }

            GL.End();




            GL.Disable(EnableCap.Blend);
        }

        public void DrawGrid(System.Drawing.Color color, int cell_size, int block_number)
        {
            GL.PushMatrix();

            GL.Translate(-cell_size * block_number / 2, -cell_size * block_number / 2,0);

            int i;

            GL.Color3(color);
            GL.Begin(PrimitiveType.Lines);
            
            for(int j=0; j<block_number; j++)
            {
                for(i=0; i<block_number; i++)
                {
                    GL.Vertex2(i * cell_size, j * cell_size);
                    GL.Vertex2((i + 1) * cell_size, j * cell_size);

                    GL.Vertex2((i + 1) * cell_size, j * cell_size);
                    GL.Vertex2((i + 1) * cell_size, (j + 1) * cell_size);

                    GL.Vertex2((i + 1) * cell_size, (j + 1) * cell_size);
                    GL.Vertex2(i * cell_size, (j + 1) * cell_size);

                    GL.Vertex2(i * cell_size, (j + 1) * cell_size);
                    GL.Vertex2(i * cell_size, j * cell_size);
                }
            }

            GL.End();

            GL.PopMatrix();
        }


        private void DrawBox()
        {
            GL.LineWidth(1);
            DrawBoxLine();

            GL.LineWidth(3);
            GL.Begin(PrimitiveType.Lines);
            GL.PushMatrix();
            GL.Color3(Color.Green);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 2, 0);
            GL.PopMatrix();
            GL.End();

            GL.Begin(PrimitiveType.Quads);
            GL.PushMatrix();

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
            GL.PopMatrix();
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
    }
}
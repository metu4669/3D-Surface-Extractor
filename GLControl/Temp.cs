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

namespace Petcocel.GLControl
{
    class GLControlUnit
    {
        public double angle = 20f;
        public OpenTK.GLControl glControl1;

        public bool create_eclipse_model = false;

        public double eclipse_pos1 = 0.0f;
        public double eclipse_pos2 = 0.0f;
        public double eclipse_pos3 = 0.0f;
        public int eclipse_thickness = 0;
        public Color eclipse_box_color = Color.Black;
        public Color eclipse_box_line_color = Color.White;

        public void Starting()
        {
            glControl1.Load += GL_Control_Load;
            glControl1.Paint += GL_Control_Paint;
            //glControl1.MouseMove += GL_Control_Paint;
            glControl1.Resize += GL_Control_Resize;
        }

        private void GL_Control_Load(Object o, EventArgs e)
        {
            GL.ClearColor(Color.Black);
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -10000, 10000);
            GL.Viewport(0, 0, w, h);
            GL.End();
            glControl1.SwapBuffers();
        }
        private void mouse_move_rotation()
        {
            angle += 0.01f;
            angle = (angle > 360) ? -360 : angle;
        }


        private void GL_Control_Paint(Object o, EventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Color lineColorr = Color.Black;


            //DrawBox(-200, -300, 0, 100, Color.Red, lineColorr);
            DrawSurface();

            glControl1.SwapBuffers();

        }

        private void GL_Control_Resize(Object o, EventArgs e)
        {
        }
        public void DrawSurface()
        {
            GL.Begin(PrimitiveType.Polygon);
            GL.Translate(2.5f, 2.5f, 2.5f);
            GL.PushMatrix();
            GL.Scale(100, 100, 100);
            GL.Rotate(0.01f, 0.0, 0.0, 1.0);
            GL.Color3(Color.Green);
            GL.Vertex3(new Vector3(0, 0, 0));
            GL.Vertex3(new Vector3(0, 1, 0));
            GL.Vertex3(new Vector3(0, 2, 3));
            GL.Vertex3(new Vector3(5, 5, -2));
            GL.PopMatrix();
        }

        public void DrawBox(double pos1, double pos2, double pos3, int thickness, Color col, Color lineColor)
        {
            DrawLineBox(pos1, pos2, pos3, thickness, lineColor);

            GL.PushMatrix();

            GL.Translate(pos1, pos2, pos3);

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(col);
            // front face
            GL.Normal3(0, 0, thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);

            // back face
            GL.Normal3(0, 0, -thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);

            // top face
            GL.Normal3(0, thickness, 0);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);

            // bottom face
            GL.Normal3(0, -thickness, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);

            // right face
            GL.Normal3(thickness, 0, 0);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);

            // left face
            GL.Normal3(-thickness, 0, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.End();
            GL.PopMatrix();
        }

        private void DrawLineBox(double pos1, double pos2, double pos3, int thickness, Color col)
        {
            GL.PushMatrix();

            GL.Translate(pos1, pos2, pos3);

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(col);
            // front face
            GL.Normal3(0, 0, thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);

            // back face
            GL.Normal3(0, 0, -thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);

            // top face
            GL.Normal3(0, thickness, 0);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);

            // bottom face
            GL.Normal3(0, -thickness, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);

            // right face
            GL.Normal3(thickness, 0, 0);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);

            // left face
            GL.Normal3(-thickness, 0, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.End();
            GL.PopMatrix();

        }

        private void DrawTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(Color.Red);
            GL.Vertex2(point1);

            GL.Color3(Color.Yellow);
            GL.Vertex2(point2);

            GL.Color3(Color.Blue);
            GL.Vertex2(point3);

        }

        public void CallDrawBox(double pos1, double pos2, double pos3, int thickness, Color col, Color lineColor)
        {
            create_eclipse_model = true;

            eclipse_pos1 = pos1;
            eclipse_pos2 = pos2;
            eclipse_pos3 = pos3;
            eclipse_thickness = thickness;
            eclipse_box_color = col;
            eclipse_box_line_color = lineColor;
        }

    }
}
*/


/*
 * using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Petcocel.GLControl
{
    class GLControlUnit
    {
        public double angle = 20f;
        public OpenTK.GLControl glControl1;

        public bool create_eclipse_model = false;

        public double eclipse_pos1 = 0.0f;
        public double eclipse_pos2 = 0.0f;
        public double eclipse_pos3 = 0.0f;
        public int eclipse_thickness = 0;
        public Color eclipse_box_color = Color.Black;
        public Color eclipse_box_line_color = Color.White;

        public void Starting()
        {
            glControl1.Load += GL_Control_Load;
            glControl1.Paint += GL_Control_Paint;
            //glControl1.MouseMove += GL_Control_Paint;
            glControl1.Resize += GL_Control_Resize;
        }

        private void GL_Control_Load(Object o, EventArgs e)
        {
            //GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GL.ClearColor(Color.Black);
        }
        private void mouse_move_rotation()
        {
            angle+=0.01f;
            angle = (angle > 360) ? -360 : angle;
        }


        private void GL_Control_Paint(Object o, EventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Color lineColorr = Color.Black;

            
            DrawBox(-200, -300, 0, 100, Color.Red, lineColorr);
            DrawBox(-200, -200, 0, 100, Color.Blue, lineColorr);
            DrawBox(-200, -100, 0, 100, Color.Red, lineColorr);
            DrawBox(-200, 0, 0, 100, Color.Blue, lineColorr);
            DrawBox(-200, 100, 0, 100, Color.Red, lineColorr);
            DrawBox(-200, 200, 0, 100, Color.Blue, lineColorr);
            
            DrawBox(-100, -50, 0, 100, Color.Blue, lineColorr);
            DrawBox( 0, -50, 0, 100, Color.Red, lineColorr);
            DrawBox(100, -50, 0, 100, Color.Blue, lineColorr);

            
            DrawBox(200, -300, 0, 100, Color.Red, lineColorr);
            DrawBox(200, -200, 0, 100, Color.Blue, lineColorr);
            DrawBox(200, -100, 0, 100, Color.Red, lineColorr);
            DrawBox(200, 0, 0, 100, Color.Blue, lineColorr);
            DrawBox(200, 100, 0, 100, Color.Red, lineColorr);
            DrawBox(200, 200, 0, 100, Color.Blue, lineColorr);

          
            glControl1.SwapBuffers();
            
        }

        private void GL_Control_Resize(Object o, EventArgs e)
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            glControl1.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-w / 2, w / 2, -h / 2, h / 2, -10000, 10000);
            GL.Viewport(0, 0, w, h);
            GL.End();
            glControl1.SwapBuffers();
        }

        public void DrawBox(double pos1, double pos2, double pos3, int thickness, Color col, Color lineColor)
        {
            DrawLineBox(pos1, pos2, pos3, thickness, lineColor);

            GL.PushMatrix();

            GL.Translate(pos1, pos2, pos3);

            GL.Begin(PrimitiveType.Quads);
            GL.Color3(col);
            // front face
            GL.Normal3(0, 0, thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);

            // back face
            GL.Normal3(0, 0, -thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);

            // top face
            GL.Normal3(0, thickness, 0);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);

            // bottom face
            GL.Normal3(0, -thickness, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);

            // right face
            GL.Normal3(thickness, 0, 0);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);

            // left face
            GL.Normal3(-thickness, 0, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.End();
            GL.PopMatrix();
        }

        private void DrawLineBox(double pos1, double pos2, double pos3, int thickness, Color col)
        {
            GL.PushMatrix();

            GL.Translate(pos1, pos2, pos3);

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(col);
            // front face
            GL.Normal3(0, 0, thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);

            // back face
            GL.Normal3(0, 0, -thickness);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);

            // top face
            GL.Normal3(0, thickness, 0);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);

            // bottom face
            GL.Normal3(0, -thickness, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);

            // right face
            GL.Normal3(thickness, 0, 0);
            GL.Vertex3(thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, -thickness / 2);
            GL.Vertex3(thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(thickness / 2, -thickness / 2, thickness / 2);

            // left face
            GL.Normal3(-thickness, 0, 0);
            GL.Vertex3(-thickness / 2, -thickness / 2, -thickness / 2);
            GL.Vertex3(-thickness / 2, -thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, thickness / 2);
            GL.Vertex3(-thickness / 2, thickness / 2, -thickness / 2);
            GL.End();
            GL.PopMatrix();

        }

        private void DrawTriangle(Vector2 point1, Vector2 point2, Vector2 point3)
        {
            GL.Begin(PrimitiveType.Lines);

            GL.Color3(Color.Red);
            GL.Vertex2(point1);

            GL.Color3(Color.Yellow);
            GL.Vertex2(point2);

            GL.Color3(Color.Blue);
            GL.Vertex2(point3);

        }

        public void CallDrawBox(double pos1, double pos2, double pos3, int thickness, Color col, Color lineColor)
        {
            create_eclipse_model = true;

            eclipse_pos1 = pos1;
            eclipse_pos2 = pos2;
            eclipse_pos3 = pos3;
            eclipse_thickness = thickness;
            eclipse_box_color = col;
            eclipse_box_line_color = lineColor;
        }

    }
}
*/


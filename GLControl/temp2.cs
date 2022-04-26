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
    class temp2
    {
        public double rot_angle_x = 0, rot_angle_y = 0, rot_angle_z = 0; // Rotation Angle of X,Y,Z

        public List<double> PointAfterRotation(double xi, double yi, double zi)
        {
            double radius_xz = Math.Sqrt(xi * xi + zi * zi);


            double init_sin_xz = xi / radius_xz;
            double init_cos_xz = zi / radius_xz;



            // RotY will rotate in XZ plane
            if (rot_angle_y >= 0)
            {
                init_sin_xz = init_sin_xz * Math.Cos(rot_angle_y * 0.0174532925) + init_cos_xz * Math.Sin(rot_angle_y * 0.0174532925);
                init_cos_xz = init_cos_xz * Math.Cos(rot_angle_y * 0.0174532925) - init_sin_xz * Math.Sin(rot_angle_y * 0.0174532925);
            }
            else
            {
                init_sin_xz = init_sin_xz * Math.Cos(rot_angle_y * 0.0174532925) - init_cos_xz * Math.Sin(rot_angle_y * 0.0174532925);
                init_cos_xz = init_cos_xz * Math.Cos(rot_angle_y * 0.0174532925) + init_sin_xz * Math.Sin(rot_angle_y * 0.0174532925);
            }

            xi = init_sin_xz * radius_xz;
            zi = init_cos_xz * radius_xz;
            

            double radius_yz = Math.Sqrt(yi * yi + zi * zi);
            double init_sin_yz = yi / radius_yz;
            double init_cos_yz = zi / radius_yz;

            // RotX will rotate in yZ plane
            if (rot_angle_x >= 0)
            {
                init_sin_yz = init_sin_yz * Math.Cos(rot_angle_x * 0.0174532925) + init_cos_yz * Math.Sin(rot_angle_x * 0.0174532925);
                init_cos_yz = init_cos_yz * Math.Cos(rot_angle_x * 0.0174532925) - init_sin_yz * Math.Sin(rot_angle_x * 0.0174532925);
            }
            else
            {
                init_sin_yz = init_sin_yz * Math.Cos(rot_angle_x * 0.0174532925) - init_cos_yz * Math.Sin(rot_angle_x * 0.0174532925);
                init_cos_yz = init_cos_yz * Math.Cos(rot_angle_x * 0.0174532925) + init_sin_yz * Math.Sin(rot_angle_x * 0.0174532925);
            }


            yi = init_sin_yz * radius_yz;
            zi = init_cos_yz * radius_yz;
            
            List<double> final_pos = new List<double>();
            final_pos.Add(xi);
            final_pos.Add(yi);
            final_pos.Add(zi);
            return final_pos;
        }




        public List<double> PointAfterRotation2(double xi, double yi, double zi)
        {
            // ---- RotY Section ----
            double radius_xz = Math.Sqrt(xi * xi + zi * zi);
            double init_sin_xz = xi / radius_xz;
            double init_cos_xz = zi / radius_xz;

            // RotY will rotate in XZ plane
            double final_sin_xz = init_sin_xz * Math.Cos(rot_angle_y * 0.0174532925) + init_cos_xz * Math.Sin(rot_angle_y * 0.0174532925);
            double final_cos_xz = init_cos_xz * Math.Cos(rot_angle_y * 0.0174532925) - init_sin_xz * Math.Sin(rot_angle_y * 0.0174532925);

            xi = final_sin_xz * radius_xz;
            zi = final_cos_xz * radius_xz;

            // ---- RotY Section Ends ----


            // ---- RotX Section ----
            double radius_yz = Math.Sqrt(yi * yi + zi * zi);
            double init_sin_yz = yi / radius_yz;
            double init_cos_yz = zi / radius_yz;

            // RotY will rotate in YZ plane
            double final_sin_yz = init_sin_yz * Math.Cos(-rot_angle_x * 0.0174532925) + init_cos_yz * Math.Sin(-rot_angle_x * 0.0174532925);
            double final_cos_yz = init_cos_yz * Math.Cos(-rot_angle_x * 0.0174532925) - init_sin_yz * Math.Sin(-rot_angle_x * 0.0174532925);

            yi = final_sin_yz * radius_yz;
            zi = final_cos_yz * radius_yz;

            // ---- RotX Section Ends ----




            List<double> final_pos = new List<double>();
            final_pos.Add(xi);
            final_pos.Add(yi);
            final_pos.Add(zi);
            return final_pos;
        }

        private void DrawBox(double rot_x, double rot_y, double rot_z)
        {
            GL.Begin(PrimitiveType.Quads);
            GL.PushMatrix();


            List<double> temp_points = PointAfterRotation(-1.0, 1.0, 1.0);
            GL.Color3(1.0, 1.0, 0.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            temp_points = PointAfterRotation(-1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            temp_points = PointAfterRotation(-1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            temp_points = PointAfterRotation(-1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);



            GL.Color3(1.0, 0.0, 1.0);
            temp_points = PointAfterRotation(1.0, 1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            GL.Color3(0.0, 1.0, 1.0);
            temp_points = PointAfterRotation(1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            GL.Color3(1.0, 0.0, 0.0);
            temp_points = PointAfterRotation(1.0, 1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, 1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            GL.Color3(0.0, 1.0, 0.0);
            temp_points = PointAfterRotation(1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, -1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, 1.0, -1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);

            GL.Color3(0.0, 0.0, 1.0);
            temp_points = PointAfterRotation(1.0, 1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, -1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);
            temp_points = PointAfterRotation(-1.0, 1.0, 1.0);
            GL.Vertex3(temp_points[0], temp_points[1], temp_points[2]);


            GL.PopMatrix();
            GL.End();

        }


    }
}

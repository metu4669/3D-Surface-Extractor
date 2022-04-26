using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Petcocel.GLControl.MouseController
{
    class MouseController
    {
        public OpenTK.GLControl glControl1;
        public GLControlUnit glControlUnit;

        public double angle_X = 0, angle_Y = 0;
        public double temp_current_x = 0, temp_current_y = 0;
        public double tX=0, tY = 0;

        public double z = 30f;
        public double current_x = 0, current_y = 0;
        public double object_current_x = 0, object_current_y = 0;

        public bool inital = true, control_over = false;

        public void Mouse_Control_Initialize()
        {

            glControl1.MouseMove += GL_Mouse_Move;
            glControl1.MouseWheel += GL_Mouse_Wheel;
        }


        public void GL_Mouse_Wheel(object o, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                z += 10f;
            }
            else
            {
                z -= 10f;
            }

            //z = (z <-10) ? -10 : z;
            glControlUnit.z = z;

            glControl1.Invalidate();
        }
        public void GL_Mouse_Move(object o, MouseEventArgs e)
        {
            if (e.X < 0 || e.X > glControl1.Width || e.Y < 0 || e.Y > glControl1.Height)
            {
                control_over = false;
            }
            else
            {
                control_over = true;
            }

            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
            {
                inital = true;
            }

            if (e.Button == MouseButtons.Left && control_over)
            {
                if (inital)
                {
                    inital = false;
                    temp_current_x = current_x;
                    temp_current_y = current_y;

                    tX = e.X;
                    tY = e.Y;
                }

                double dX = e.X - tX;
                double dY = e.Y - tY;

                angle_X = dX * (double)(360) / (double)glControl1.Width;
                angle_Y = dY * (double)(360) / (double)glControl1.Height;

                angle_X = (angle_X > 360) ? -360 : angle_X;
                angle_Y = (angle_Y > 360) ? -360 : angle_Y;

                angle_X = (angle_X < -360) ? 360 : angle_X;
                angle_Y = (angle_Y < -360) ? 360 : angle_Y;

                current_x = temp_current_x + angle_X;
                current_y = temp_current_y + angle_Y;

                current_x = (current_x > 360) ? -360 : current_x;
                current_x = (current_x < -360) ? 360 : current_x;

                current_y = (current_y > 360) ? -360 : current_y;
                current_y = (current_y < -360) ? 360 : current_y;

                glControlUnit.current_x = current_x;
                glControlUnit.current_y = current_y;
                glControl1.Invalidate();

            }
            else
            {
                angle_X = 0;
                angle_Y = 0;
            }

            if (e.Button == MouseButtons.Right && control_over)
            {
                if (inital)
                {
                    inital = false;
                    temp_current_x = object_current_x;
                    temp_current_y = object_current_y;

                    tX = e.X;
                    tY = e.Y;
                }

                double dX = e.X - tX;
                double dY = e.Y - tY;

                angle_X = dX * (double)(360) / (double)glControl1.Width;
                angle_Y = dY * (double)(360) / (double)glControl1.Height;

                angle_X = (angle_X > 360) ? -360 : angle_X;
                angle_Y = (angle_Y > 360) ? -360 : angle_Y;

                angle_X = (angle_X < -360) ? 360 : angle_X;
                angle_Y = (angle_Y < -360) ? 360 : angle_Y;

                object_current_x = temp_current_x + angle_X;
                object_current_y = temp_current_y + angle_Y;

                object_current_x = (current_x > 360) ? -360 : object_current_x;
                object_current_x = (current_x < -360) ? 360 : object_current_x;

                object_current_y = (current_y > 360) ? -360 : object_current_y;
                object_current_y = (current_y < -360) ? 360 : object_current_y;

                glControlUnit.object_current_x = object_current_x;
                glControlUnit.object_current_y = object_current_y;
                glControl1.Invalidate();

            }
            else
            {
                angle_X = 0;
                angle_Y = 0;
            }

        }
    }
}

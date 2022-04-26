using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petcocel.GLControl.KeyboardController
{
    class KeyboardController
    {
        public double offset_x = 0, offset_y = 0;
        public OpenTK.GLControl glControl1;
        public GLControlUnit glControlUnit;
        double increment = 5f;

        public void Keyboard_Control_Initialize()
        {

            glControl1.KeyPress += GL_Key_Press;
        }


        public void GL_Key_Press(object o, System.Windows.Forms.KeyPressEventArgs e)
        {
            string clicked = e.KeyChar + "";

            if (string.Equals(clicked, "w"))
            {
                offset_y -= increment;
            }
            else if (string.Equals(clicked, "s"))
            {
                offset_y += increment;
            }
            else if (string.Equals(clicked, "a"))
            {
                offset_x += increment;
            }
            else if (string.Equals(clicked, "d"))
            {
                offset_x -= increment;
            }
            glControlUnit.offset_x = offset_x;
            glControlUnit.offset_y = offset_y;
            glControl1.Invalidate();
        }
    }
}

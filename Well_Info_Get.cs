using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Petcocel
{
    public partial class Well_Info_Get : Form
    {
        public static string tex1 = "";
        public static string tex2 = "";
        public Well_Info_Get()
        {
            InitializeComponent();
        }

        public string Latitude
        {
            get { return (string.Equals(textBox1.Text,"")?"Empty":textBox1.Text); }
        }
        public string Longitude
        {
            get { return (string.Equals(textBox2.Text, "") ? "Empty" : textBox2.Text); }
        }
        private void Add_Well_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PetCoCel
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            this.SetVisibleCore(false);
            main.Show();
            main.FormClosed += new FormClosedEventHandler(form2_FormClosed);
        }
        void form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SetVisibleCore(true);
        }
    }
}

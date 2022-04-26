using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace PetCoCel
{
    public partial class Main : Form
    {
        public string[] getporosityZ = new string[0];
        public string[] getporosityX = new string[0];
        public string[] getporosityY = new string[0];

        public string[] getpermeabilityZ = new string[0];
        public string[] getpermeabilityX = new string[0];
        public string[] getpermeabilityY = new string[0];

        public string[] getSaturationZ = new string[0];
        public string[] getSaturationX = new string[0];
        public string[] getSaturationY = new string[0];

        public string[] getThicknessZ = new string[0];
        public string[] getThicknessX = new string[0];
        public string[] getThicknessY = new string[0];

        public string[] getwaterCutZ = new string[0];
        public string[] getwaterCutY = new string[0];
        public string[] getwaterCutX = new string[0];

        public string[] getporosityZResult = new string[0];
        public string[] getporosityXResult = new string[0];
        public string[] getporosityYResult = new string[0];

        public string[] getpermeabilityZResult = new string[0];
        public string[] getpermeabilityXResult = new string[0];
        public string[] getpermeabilityYResult = new string[0];

        public string[] getSaturationZResult = new string[0];
        public string[] getSaturationXResult = new string[0];
        public string[] getSaturationYResult = new string[0];

        public string[] getThicknessZResult = new string[0];
        public string[] getThicknessXResult = new string[0];
        public string[] getThicknessYResult = new string[0];

        public string[] getwaterCutZResult = new string[0];
        public string[] getwaterCutYResult = new string[0];
        public string[] getwaterCutXResult = new string[0];


        public float[] finalResult = new float[0];

        public int parameter = 0;

        public Main()
        {
            InitializeComponent();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                button5.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                button6.Enabled = true;
            }
            else
            {
                button6.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            TableSheet tableD = new TableSheet(this,0);
            this.SetVisibleCore(false);
            tableD.Show();

            tableD.FormClosed += new FormClosedEventHandler(form2_FormClosed);
        }

        void form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            comboBox2.Items.Clear();
            this.SetVisibleCore(true);
            propertUploader(parameter);
            if (getpermeabilityX.Length > 0)
            {
                button10.Enabled = true;
                comboBox2.Items.Add("Permeability");
            }
            else
            {
                button10.Enabled = false;
            }
            if (getporosityX.Length > 0)
            {
                button9.Enabled = true;
                comboBox2.Items.Add("Porosity");
            }
            else
            {
                button9.Enabled = false;
            }
            if (getwaterCutX.Length > 0)
            {
                button13.Enabled = true;
                comboBox2.Items.Add("Water-Cut");
            }
            else
            {
                button13.Enabled = false;
            }
            if (getSaturationX.Length > 0)
            {
                button12.Enabled = true;
                comboBox2.Items.Add("Saturation");
            }
            else
            {
                button12.Enabled = false;
            }
            if (getThicknessX.Length > 0)
            {
                button11.Enabled = true;
                comboBox2.Items.Add("Thickness");
            }
            else
            {
                button11.Enabled = false;
            }
            
        }
        
        private void propertUploader(int paramHolder)
        {
            dataGridView1.Rows.Clear();
            int tt = 0;
            if (paramHolder == 0)
            {
                tt = getporosityX.Length;
            }
            else if (paramHolder == 1)
            {
                tt = getpermeabilityX.Length;
            }
            else if (paramHolder == 2)
            {
                tt = getSaturationX.Length;
            }
            else if (paramHolder == 3)
            {
                tt = getThicknessX.Length;
            }
            else if (paramHolder == 4)
            {
                tt = getwaterCutX.Length;
            }

            if (tt > 0)
            {
                for (int i = 0; i < tt; i++)
                {
                    string tempX = "";
                    string tempY = "";
                    string tempZ = "";

                    if(paramHolder == 0)
                    {
                        tempX = getporosityX[i];
                        tempY = getporosityY[i];
                        tempZ = getporosityZ[i];
                    }else if(paramHolder == 1)
                    {
                        tempX = getpermeabilityX[i];
                        tempY = getpermeabilityY[i];
                        tempZ = getpermeabilityZ[i];
                    }else if(paramHolder == 2)
                    {
                        tempX = getSaturationX[i];
                        tempY = getSaturationY[i];
                        tempZ = getSaturationZ[i];
                    }else if(paramHolder == 3)
                    {
                        tempX = getThicknessX[i];
                        tempY = getThicknessY[i];
                        tempZ = getThicknessZ[i];
                    }else if(paramHolder == 4)
                    {
                        tempX = getwaterCutX[i];
                        tempY = getwaterCutY[i];
                        tempZ = getwaterCutZ[i];
                    }

                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = tempX;
                    dataGridView1[1, i].Value = tempY;
                    dataGridView1[2, i].Value = tempZ;
                }
            }
        }

        private void propertResultUploader(int paramHolder)
        {
            dataGridView1.Rows.Clear();
            int tt = 0;
            if (paramHolder == 0)
            {
                tt = getporosityXResult.Length;
            }
            else if (paramHolder == 1)
            {
                tt = getpermeabilityXResult.Length;
            }
            else if (paramHolder == 2)
            {
                tt = getSaturationXResult.Length;
            }
            else if (paramHolder == 3)
            {
                tt = getThicknessXResult.Length;
            }
            else if (paramHolder == 4)
            {
                tt = getwaterCutXResult.Length;
            }

            if (tt > 0)
            {
                for (int i = 0; i < tt; i++)
                {
                    string tempX = "";
                    string tempY = "";
                    string tempZ = "";

                    if (paramHolder == 0)
                    {
                        tempX = getporosityXResult[i];
                        tempY = getporosityYResult[i];
                        tempZ = getporosityZResult[i];
                    }
                    else if (paramHolder == 1)
                    {
                        tempX = getpermeabilityXResult[i];
                        tempY = getpermeabilityYResult[i];
                        tempZ = getpermeabilityZResult[i];
                    }
                    else if (paramHolder == 2)
                    {
                        tempX = getSaturationXResult[i];
                        tempY = getSaturationYResult[i];
                        tempZ = getSaturationZResult[i];
                    }
                    else if (paramHolder == 3)
                    {
                        tempX = getThicknessXResult[i];
                        tempY = getThicknessYResult[i];
                        tempZ = getThicknessZResult[i];
                    }
                    else if (paramHolder == 4)
                    {
                        tempX = getwaterCutXResult[i];
                        tempY = getwaterCutYResult[i];
                        tempZ = getwaterCutZResult[i];
                    }

                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = tempX;
                    dataGridView1[1, i].Value = tempY;
                    dataGridView1[2, i].Value = tempZ;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string[] parX = new string[0];
            string[] parY = new string[0];
            string[] parZ = new string[0];

            string param = comboBox2.SelectedItem.ToString();
            int paramm = 0;
            if (param == "Porosity")
            {
                parX = new string[getporosityX.Length];
                parY = new string[getporosityY.Length];
                parZ = new string[getporosityZ.Length];

                parX = getporosityX;
                parY = getporosityY;
                parZ = getporosityZ;
                paramm = 0;
            }
            else if (param == "Permeability")
            {
                parX = new string[getpermeabilityX.Length];
                parY = new string[getpermeabilityY.Length];
                parZ = new string[getpermeabilityZ.Length];

                parX = getpermeabilityX;
                parY = getpermeabilityY;
                parZ = getpermeabilityZ;
                paramm = 1;
            }
            else if (param == "Saturation")
            {
                parX = new string[getSaturationX.Length];
                parY = new string[getSaturationY.Length];
                parZ = new string[getSaturationZ.Length];

                parX = getSaturationX;
                parY = getSaturationY;
                parZ = getSaturationZ;
                paramm = 2;
            }
            else if (param == "Thickness")
            {
                parX = new string[getThicknessX.Length];
                parY = new string[getThicknessY.Length];
                parZ = new string[getThicknessZ.Length];

                parX = getThicknessX;
                parY = getThicknessY;
                parZ = getThicknessZ;
                paramm = 3;
            }
            else if (param == "Water-Cut")
            {
                parX = new string[getwaterCutX.Length];
                parY = new string[getwaterCutY.Length];
                parZ = new string[getwaterCutZ.Length];

                parX = getwaterCutX;
                parY = getwaterCutY;
                parZ = getwaterCutZ;
                paramm = 4;
            }

            Form1 generator;
            if (parX.Length > 0)
            {
                generator = new Form1(parX, parY, parZ, true, paramm, this);
            }
            else
            {
                generator = new Form1(parX, parY, parZ, false, paramm, this);
            }
            this.SetVisibleCore(false);
            generator.Show();

            generator.FormClosed += new FormClosedEventHandler(form2_FormClosed);

        }

        private void Main_Load(object sender, EventArgs e)
        {
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            propertUploader(0);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            propertUploader(1);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            propertUploader(2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            propertUploader(3);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            propertUploader(4);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            TableSheet tableD = new TableSheet(this, 1);
            this.SetVisibleCore(false);
            tableD.Show();

            tableD.FormClosed += new FormClosedEventHandler(form2_FormClosed);

        }

        private void button5_Click(object sender, EventArgs e)
        {

            TableSheet tableD = new TableSheet(this, 2);
            this.SetVisibleCore(false);
            tableD.Show();

            tableD.FormClosed += new FormClosedEventHandler(form2_FormClosed);

        }

        private void button4_Click(object sender, EventArgs e)
        {

            TableSheet tableD = new TableSheet(this, 3);
            this.SetVisibleCore(false);
            tableD.Show();

            tableD.FormClosed += new FormClosedEventHandler(form2_FormClosed);

        }

        private void button6_Click(object sender, EventArgs e)
        {

            TableSheet tableD = new TableSheet(this, 4);
            this.SetVisibleCore(false);
            tableD.Show();

            tableD.FormClosed += new FormClosedEventHandler(form2_FormClosed);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            propertResultUploader(0);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            propertResultUploader(1);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            propertResultUploader(2);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            propertResultUploader(3);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            propertResultUploader(4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            float[] tempPors = new float[getporosityZResult.Length];
            float[] tempThicks = new float[getporosityZResult.Length];
            float[] tempSats = new float[getporosityZResult.Length];

            float minPor = -1, maxPor = -1, minThick = -1, maxThick = -1, minSat = -1, maxSat = -1;

            for (int i = 0; i < getporosityZResult.Length; i++)
            {
                float tempPor = float.Parse(getporosityZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;
                float tempSat = float.Parse(getSaturationZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;
                float tempThick = float.Parse(getThicknessZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;

                tempPors[i] = tempPor;
                tempThicks[i] = tempThick;
                tempSats[i] = tempSat;

                if (minPor > tempPor)
                {
                    minPor = tempPor;
                }
                if (maxPor < tempPor)
                {
                    maxPor = tempPor;
                }

                if (minSat > tempSat)
                {
                    minSat = tempSat;
                }
                if (maxSat < tempSat)
                {
                    maxSat = tempSat;
                }

                if (minThick > tempThick)
                {
                    minThick = tempThick;
                }
                if (maxThick < tempThick)
                {
                    maxThick = tempThick;
                }
            }
            float porDiff = maxPor - minPor;
            float satDiff = maxSat - minSat;
            float thickDiff = maxThick - minThick;
            dataGridView1.Rows.Clear();
            finalResult = new float[getporosityZResult.Length];

            float maxResult = 0;
            int maxIndex = 0;
            for (int i=0; i< getporosityZResult.Length; i++)
            {
                float tempPor = float.Parse(getporosityZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;
                float tempSat = float.Parse(getSaturationZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;
                float tempThick = float.Parse(getThicknessZResult[i], CultureInfo.InvariantCulture.NumberFormat); ;

                float porRatio = (tempPor - minPor) / porDiff;
                float satRatio = (tempSat - minSat) / satDiff;
                float thickRatio = (tempThick - minThick) / thickDiff;

                finalResult[i] = porRatio * 5 + satRatio * 3 + thickRatio * 4;

                if(maxResult < finalResult[i])
                {
                    maxResult = finalResult[i];
                    maxIndex = i;
                }

                dataGridView1.Rows.Add();
                dataGridView1[0, i].Value = getporosityXResult[i];
                dataGridView1[1, i].Value = getporosityYResult[i];
                dataGridView1[2, i].Value = finalResult[i].ToString();
            }
            MessageBox.Show("Optimum Location:\n X: " + getporosityXResult[maxIndex] + " Y: " + getporosityYResult[maxIndex]);
        }
    }
}

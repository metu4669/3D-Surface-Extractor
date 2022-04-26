using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PetCoCel
{
    public partial class TableSheet : Form
    {
        Main formMain;
        public string[] valZ;
        public string[] valX;
        public string[] valY;

        string textBoxValue;
        int parameter = 0;

        public TableSheet(Main mainForm, int parameterValue)
        {
            formMain = mainForm;
            InitializeComponent();
            parameter = parameterValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxValue = textBox1.Text;

            int n;
            bool isNumeric = int.TryParse(textBoxValue, out n);

            int textBoxNumber = n;

            if (textBoxNumber > 0)
            {
                dataGridView1.Rows.Clear();
                for (int i = 0; i < textBoxNumber; i++)
                {
                    dataGridView1.Rows.Add();
                }
                dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            else
            {
                MessageBox.Show("Row number should be higher than 0");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int rowNumber = dataGridView1.Rows.Count;

            valZ = new string[rowNumber];
            valY = new string[rowNumber];
            valX = new string[rowNumber];


            for (int i = 0; i < rowNumber; i++)
            {
                valX[i] = dataGridView1[0, i].Value.ToString();
                valY[i] = dataGridView1[1, i].Value.ToString();
                valZ[i] = dataGridView1[2, i].Value.ToString();
            }

            if (valZ.Length > 0 && valY.Length > 0 && valX.Length > 0)
            {
                if (parameter == 0)
                {
                    formMain.getporosityZ = valZ;
                    formMain.getporosityX = valX;
                    formMain.getporosityY = valY;

                    formMain.parameter = 0;
                }
                else if (parameter == 1)
                {
                    formMain.getpermeabilityZ = valZ;
                    formMain.getpermeabilityX = valX;
                    formMain.getpermeabilityY = valY;

                    formMain.parameter = 1;
                }
                else if (parameter == 2)
                {
                    formMain.getSaturationZ = valZ;
                    formMain.getSaturationX = valX;
                    formMain.getSaturationY = valY;

                    formMain.parameter = 2;
                }
                else if (parameter == 3)
                {
                    formMain.getThicknessZ = valZ;
                    formMain.getThicknessX = valX;
                    formMain.getThicknessY = valY;

                    formMain.parameter = 3;
                }
                else if (parameter == 4)
                {
                    formMain.getwaterCutZ = valZ;
                    formMain.getwaterCutX = valX;
                    formMain.getwaterCutY = valY;

                    formMain.parameter = 4;
                }
                MessageBox.Show("Data inserted.");
            }
            else
            {
                MessageBox.Show("Error: Check your data.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s = Clipboard.GetText();

            string[] lines = s.Replace("\n", "").Split('\r');

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(lines.Length);
            string[] fields;
            int row = 0;
            int col = 0;

            foreach (string item in lines)
            {
                fields = item.Split('\t');
                foreach (string f in fields)
                {
                    Console.WriteLine(f);
                    dataGridView1[col, row].Value = f;
                    col++;
                }
                row++;
                col = 0;
            }
            dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
        }
    }
}

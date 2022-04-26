using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Petcocel.Eclipse
{
    class ReadDATAFile
    {
        public string DXV = "";
        public string DYV = "";
        public string DZ = "";
        public string TOPS = "";

        public List<double> _DXV = new List<double>();
        public List<double> _DYV = new List<double>();
        public List<double> _DZ = new List<double>();
        public List<double> _TOPS = new List<double>();

        public int error_empty = 0;

        public void CallDialog(OpenTK.GLControl glControl)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data files (*.DATA)|*.DATA|txt files (*.txt*)|*.txt*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.CheckFileExists = false;
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Please Choose Data File or Txt File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string _filePath = openFileDialog.FileName;
                string _fileName = openFileDialog.SafeFileName;
                string _fileText = File.ReadAllText(_filePath);

                DXV = Query_String_Part("DXV", _fileText);
                DYV = Query_String_Part("DYV", _fileText);
                DZ = Query_String_Part("DZ", _fileText);
                TOPS = Query_String_Part("TOPS", _fileText);


                _DXV = GET_DXV_DYV(_DXV, DXV);
                _DYV = GET_DXV_DYV(_DYV, DYV);

                _DZ = GET_DZ_TOPS(_DZ, DZ);
                _TOPS = GET_DZ_TOPS(_TOPS, TOPS);                
            }
        }

        private string Query_String_Part(string eclipse_component, string resource_text)
        {
            string query_text = eclipse_component;
            string temp_text_holder = resource_text;

            var startIndex = temp_text_holder.IndexOf(query_text);
            if(startIndex > 0)
            {
                startIndex = startIndex + query_text.Length;
                temp_text_holder = temp_text_holder.Substring(startIndex);
                startIndex = 1;
                var endIndex = temp_text_holder.IndexOf('/');
                var charsToRead = (endIndex - startIndex) - 1;
                string queried_string = temp_text_holder.Substring(startIndex, charsToRead);

                return queried_string;
            }
            else
            {
                error_empty = 1;
                return "";
            }
            
        }

        private List<double> GET_DXV_DYV(List<double> seperated_box, string seperating_text_target){
            int current_index = 0;
            double temp_holder = 0;
            string current_number = "";
            
            for(current_index = 0; current_index<seperating_text_target.Length; current_index++)
            {
                char current_char = seperating_text_target[current_index];

                if (current_char != ' ' && current_char != '*')
                {
                    if ((current_char >= '0' && current_char <= '9') || current_char == '.' || current_char == ',')
                    {
                        if (current_char == '.')
                        {
                            current_char = ',';
                        }
                        current_number = current_number + current_char;

                        if (current_index == seperating_text_target.Length - 1)
                        {
                            temp_holder = (double)double.Parse(current_number);
                            current_number = "";
                            seperated_box.Add(temp_holder);
                        }
                    }
                }
                else if (current_char == ' ' && current_number != "")
                {
                    temp_holder = (double)double.Parse(current_number);
                    current_number = "";
                    seperated_box.Add(temp_holder);
                }
            }

            return seperated_box;
        }

        private List<double> GET_DZ_TOPS(List<double> seperated_box, string seperating_text_target){
            int current_index = 0;
            double temp_holder = 0;
            string current_number = "";

            for (current_index = 0; current_index < seperating_text_target.Length; current_index++)
            {
                char current_char = seperating_text_target[current_index];

                if (current_char != ' ' && current_char != '*')
                {
                    if ((current_char >= '0' && current_char <= '9') || current_char == '.' || current_char == ',')
                    {
                        if(current_char == '.')
                        {
                            current_char = ',';
                        }
                        current_number = current_number + current_char;

                        if (current_index == seperating_text_target.Length - 1)
                        {
                            temp_holder = (double)double.Parse(current_number);
                            current_number = "";
                            seperated_box.Add(temp_holder);
                        }
                    }
                }
                else if (current_char == '*')
                {
                    temp_holder = 0;
                    current_number = "";
                    seperated_box.Add(temp_holder);
                }
                else if (current_char == ' ' && current_number != "")
                {
                    temp_holder = (double)double.Parse(current_number);
                    current_number = "";
                    seperated_box.Add(temp_holder);
                }
            }

            return seperated_box;
        }
    }
}

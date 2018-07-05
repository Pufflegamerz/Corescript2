/* 
TODO
input command
graphical
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Corescript
{
    public partial class ide : Form
    {
        public ide()
        {
            InitializeComponent();
            graphicspopup.Visible = false;
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string color = "Black";
            Bitmap bmp1 = new Bitmap(graphics.Width, graphics.Height);
            graphics.Image = bmp1;
            // Make some lists
            var variable_names = new List<string> { };
            var variable_values = new List<string> { };
            var label_names = new List<string> { };
            var label_values = new List<string> { };
            for (int line = 0; line < code.Lines.Count(); line++)
            {
                if (code.Lines[line].StartsWith(":"))
                {
                    label_names.Add(code.Lines[line].ToString().Substring(1));
                    label_values.Add(line.ToString());
                }
            }
            for (int line = 0; line < code.Lines.Count(); line++)
            {
                string current = code.Lines[line];
                if (current.StartsWith("print "))
                {
                    // Replace the variables.
                    var print = current.Substring(6);
                    MessageBox.Show(IntepretText(print, variable_names, variable_values));
                }
                else if (current.StartsWith("var "))
                {
                    string[] split = current.Substring(4).Split('=');
                    variable_names.Add(split[0]);
                    variable_values.Add(split[1]);
                }
                else if (current.StartsWith("goto "))
                {
                    var isNum = int.TryParse(current.Substring(5), out int n);
                    if (isNum)
                    {
                        string[] split = current.Substring(5).Split(' ');
                        line = Convert.ToInt32(split[1]) - 2;
                    }
                    else
                    {
                        for (int i = 0; i < label_names.Count; i++)
                        {
                            if (current.Substring(5) == label_names[i])
                            {
                                line = Convert.ToInt32(label_values[i]);
                            }
                        }
                    }
                }
                else if (current == "stop")
                {
                    // Set the loop variable higher than the number of lines
                    line = code.Lines.Count() + 1;
                }
                else if (current.StartsWith("input "))
                {
                    string[] split = current.Substring(6).Split('=');
                    string input = Microsoft.VisualBasic.Interaction.InputBox(split[1], "Input", "", -1, -1);
                    variable_names.Add(split[0]);
                    variable_values.Add(input);
                }
                else if (current.StartsWith("//") || current == "")
                {
                    // Allow comments, blank lines, and labels to pass though.
                }
                else if (current == "graphical")
                {
                    graphicspopup.Visible = true;
                    graphicspopup.Location = new Point((this.Width - graphicspopup.Width) / 2, (this.Height - graphicspopup.Height) / 2);
                }
                else if (current.StartsWith("line "))
                {
                    string[] split = current.Substring(5).Split(' ');
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] == "windowwidth")
                        {
                            split[i] = "453";
                        }
                        else if (split[i] == "windowheight")
                        {
                            split[i] = "247";
                        }
                    }
                    Bitmap bmp = new Bitmap(graphics.Image);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawLine(new Pen(Color.FromName(color)), Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
                    }
                    graphics.Image = bmp;
                }
                else if (current.StartsWith("rect "))
                {
                    string[] split = current.Substring(5).Split(' ');
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] == "windowwidth")
                        {
                            split[i] = "453";
                        }
                        else if (split[i] == "windowheight")
                        {
                            split[i] = "247";
                        }
                    }
                    Bitmap bmp = new Bitmap(graphics.Image);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.DrawRectangle(new Pen(Color.FromName(color)), Convert.ToInt32(split[0]), Convert.ToInt32(split[1]), Convert.ToInt32(split[2]), Convert.ToInt32(split[3]));
                    }
                    graphics.Image = bmp;
                }
                else if (current.StartsWith("setcolor "))
                {
                    string[] split = current.Substring(5).Split(' ');
                    color = split[1];
                }
                else
                {
                    MessageBox.Show("Syntax Error on line " + (line + 1).ToString());
                }
            }
            //static void intepretText(string text1)
            //{
            //    for (int i = 0; i < variable_names.Count; i++)
            //   {
            //      text1 = text1.Replace("(" + variable_names[i] + ")", variable_values[i]);
            //    }
            //}
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openfile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                System.IO.StreamReader(openfile.FileName);
                code.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            savefile.ShowDialog();
            string gcode = "";
            for (int i = 0; i < code.Lines.Count(); i++)
            {
                gcode = gcode + code.Lines[i] + "\r\n";
            }
            System.IO.File.WriteAllText(savefile.FileName, gcode);
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            docs frm2 = new docs();
            frm2.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            graphicspopup.Visible = false;
        }

        private void ide_Load(object sender, EventArgs e)
        {
            // Do things on load.
        }
        static string IntepretText(string text, List <string> vnames, List <string> vvalues)
        {
            string result = text;
            for (int i = 0; i < vnames.Count; i++)
            {
                result = result.Replace("(" + vnames[i] + ")", vvalues[i]);
            }
            result = result.Replace("(date)", DateTime.Now.ToString());
            var split = result.Split(' ');
            if (split.Length == 3 && split[0].StartsWith("(") && split[2].EndsWith(")"))
            {
                split[0] = split[0].Substring(1);
                split[2] = (split[2]).Remove(split[2].Length - 1);
                if (split[1] == "+")
                {
                    result = (Convert.ToInt32(split[0]) + Convert.ToInt32(split[2])).ToString();
                }
                else if (split[1] == "-")
                {
                    result = (Convert.ToInt32(split[0]) - Convert.ToInt32(split[2])).ToString();
                }
                else if (split[1] == "/")
                {
                    result = (Convert.ToInt32(split[0]) / Convert.ToInt32(split[2])).ToString();
                }
                else if (split[1] == "*" || split[1] == "x")
                {
                    result = (Convert.ToInt32(split[0]) / Convert.ToInt32(split[2])).ToString();
                }
            }
            return result;
        }
    }
}

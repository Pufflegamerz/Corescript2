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
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make some lists
            var variable_names = new List<string> {};
            var variable_values = new List<string> {};
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
                    for (int i = 0; i < variable_names.Count; i++)
                    {
                        print = print.Replace("(" + variable_names[i] + ")", variable_values[i]);
                    }
                    MessageBox.Show(print);
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
                        for(int i = 0; i < label_names.Count; i++)
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
                    line = code.Lines.Count() + 1;
                }
                else if (current.StartsWith("input "))
                {
                    string[] split = current.Substring(6).Split('=');
                    string input = Microsoft.VisualBasic.Interaction.InputBox(split[1], "Input", "", -1, -1);
                    variable_names.Add(split[0]);
                    variable_values.Add(input);
                }
                else if (current.StartsWith("//") || current == "" || current.StartsWith(":"))
                {
                    // Allow comments, blank lines, and labels to pass though.
                }
                else if (current == "graphical")
                {
                    new graphical().Show();
                    this.MdiParent.Controls["graphiccode"].Text = "test";
                }
                else
                {
                    MessageBox.Show("Syntax Error on line " + (line + 1).ToString());
                }
            }
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
    }
}

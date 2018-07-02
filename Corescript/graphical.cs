using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Corescript
{
    public partial class graphical : Form
    {
        public graphical()
        {
            InitializeComponent();
        }

        private void graphical_Load(object sender, EventArgs e)
        {
            graphiccode.Visible = false;
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawLine(new Pen(Color.Red), 0, 0, 100, 100);
            }
            pictureBox1.Image = bmp;

        }

    }
}

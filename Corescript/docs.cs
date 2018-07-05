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
    public partial class docs : Form
    {
        public docs()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void docs_Load(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = "hellos";
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GyF
{
    public partial class Main : Form
    {
        public Main(String username)
        {
            InitializeComponent();
            label1.Text = "Welcome, " + username;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}

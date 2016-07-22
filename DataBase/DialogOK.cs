using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataBase
{
    public partial class DialogOK : Form
    {
        public bool isClickOK = false;
        public DialogOK()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isClickOK = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

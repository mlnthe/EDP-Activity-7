using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void aboutbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form3();
            myform.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Add_account();
            myform.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form5();
            myform.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

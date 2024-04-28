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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void tenantsbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Add_account();
            myform.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void invoicebtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form5();
            myform.Show();
        }
    }
}

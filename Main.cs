using MySql.Data.MySqlClient;
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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {

            this.Hide();
            var myform = new LoginForm();
            myform.Show();

        }

        private void signupbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            btnSignup adding = new btnSignup();
            adding.Show();
        }
    }
}
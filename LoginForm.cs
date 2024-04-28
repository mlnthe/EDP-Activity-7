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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginbtn_Click(object sender, EventArgs e)
        {

            string Email = emailtxt.Text;
            string Password = passwordtxt.Text;

            {
                MySql.Data.MySqlClient.MySqlConnection conn;
                string connection;
                connection = "server=localhost;uid=root;" + "pwd=melinda;database=rent";
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    conn.Open();

                    string sql = "SELECT COUNT(*) FROM tenants_table WHERE Email = @myemail AND Password = @mypassword";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@myemail", Email);
                    cmd.Parameters.AddWithValue("@mypassword", Password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // Successful login, proceed to Form2 or other actions
                        this.Hide();
                        var myform = new Form2();
                        myform.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password");
                    }
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 0:
                            MessageBox.Show("Connection Failed");
                            break;
                        case 1045:
                            MessageBox.Show("Invalid Username or Password, Please Try Again");
                            break;
                    }
                }
            }
        }


                private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Login = new Main();
            Login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var Login = new Form7();
            Login.Show();
        }
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Forms
{
    public partial class MainForm : Form
    {
        //private readonly string connectionString = "server=localhost;uid=root;pwd=melinda;database=rent";
       // private object txtBox;

        public MainForm()
        {
            InitializeComponent();
        }

        private void clearbtn_Click(object sender, EventArgs e)
        {
            txtusername.Text = "";
            txtpassword.Text = "";
        }

        private void lgnbtn_Click(object sender, EventArgs e)
        {
            string Email = txtusername.Text;
            string Password = txtpassword.Text;

            {
                MySql.Data.MySqlClient.MySqlConnection conn;
                string connection;
                connection = "server=localhost;uid=root;" + "pwd=melinda;database=rent";
                try
                {
                    conn = new MySql.Data.MySqlClient.MySqlConnection(connection);
                    conn.Open();

                    string sql = "SELECT COUNT(*) FROM tenants_table WHERE Email = @myuser AND Password = @mypass";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@myuser", Email);
                    cmd.Parameters.AddWithValue("@mypass", Password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        // Successful login, proceed to Form2 or other actions
                        Form2 mainForm = new Form2();
                        Hide();
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
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
    private void forgotpassbtn_Click(object sender, EventArgs e)
    {
        this.Hide();
        Form7 go7 = new Form7();
        go7.Show();
    }

    private void btnSignup_Click(object sender, EventArgs e)
    {
        this.Hide();
        btnSignup adding = new btnSignup();
        adding.Show();
    }

        private void txtusername_TextChanged(object sender, EventArgs e)
        {

        }

        /*private string GetUserEmail(string username)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = "SELECT email FROM accounts WHERE username = @myuser";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@myuser", username);

                    object result = cmd.ExecuteScalar();

                    // Check if result is not null before converting to string
                    return result != null ? result.ToString() : null;
                }
            }
            catch (MySqlException ex)
            {
                HandleMySqlException(ex, "Error retrieving email");
                return null;
            }
        }
        */
        /* private void HandleMySqlException(MySqlException ex, string message)
         {
             switch (ex.Number)
             {
                 case 0:
                     MessageBox.Show("Cannot connect to the server. Contact administrator");
                     break;
                 case 1045:
                     MessageBox.Show("Invalid username or password, please try again");
                     break;
                 default:
                     MessageBox.Show($"{message}: {ex.Message}");
                     break;
             }
         }*/


    }
        }
    

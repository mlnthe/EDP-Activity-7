using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Forms
{
    public partial class btnSignup : Form
    {
        public btnSignup()
        {
            InitializeComponent();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            ClearFormFields();
            this.Hide();
            var Login = new Main();
            Login.Show();
        }

        private void signup_Click(object sender, EventArgs e)
        {

            // MySQL connection string
            string mySqlConnectionString = "server=localhost;uid=root;pwd=melinda;database=rent";

            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection(mySqlConnectionString))
                {
                    mySqlConnection.Open();

                    string Email = emailtxt.Text;

                    // Check for duplicate username
                    if (IsUsernameExists(mySqlConnection, Email))
                    {
                        MessageBox.Show("Username already exists. Please choose a different one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert the new user account into the database
                    string mySqlQuery = "INSERT INTO tenants_table (FirstName, LastName, Email, Address, Password) " +
                                        "VALUES (@FirstName, @LastName, @Email, @Address, @Password)";

                    using (MySqlCommand mySqlCommand = new MySqlCommand(mySqlQuery, mySqlConnection))
                    {

                        // Get user input
                        string FirstName = firstnametxt.Text;
                        string LastName = lastnametxt.Text;
                        string Address = addresstxt.Text;
                        string Password = passtxt.Text;

                        // Add parameters
                        mySqlCommand.Parameters.AddWithValue("@FirstName", FirstName);
                        mySqlCommand.Parameters.AddWithValue("@LastName", LastName);
                        mySqlCommand.Parameters.AddWithValue("@Email", Email);
                        mySqlCommand.Parameters.AddWithValue("@Address", Address);
                        mySqlCommand.Parameters.AddWithValue("@Password",Password);

                        // Execute the command
                        int rowsAffected = mySqlCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Optionally hide the form or navigate to another form
                            this.Hide();
                            var loginForm = new Main();
                            loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Failed to create user account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFormFields()
        {
            // Clear textboxes
            firstnametxt.Clear();
            lastnametxt.Clear();
            emailtxt.Clear();
            addresstxt.Clear();
            passtxt.Clear();
        }

        private bool IsUsernameExists(MySqlConnection connection, string Email)
        {
            string query = "SELECT COUNT(*) FROM tenants_table WHERE Email = @email";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", Email);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        
    }
}

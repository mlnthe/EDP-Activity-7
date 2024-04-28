using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using MySql.Data.MySqlClient;

namespace Forms
{
    public partial class Add_account : Form
    {
        MySqlConnection sqlConn = new MySqlConnection();
        MySqlCommand sqlCmd = new MySqlCommand();
        DataTable sqlDt = new DataTable();
        string sqlQuery;
        MySqlDataAdapter dataadap = new MySqlDataAdapter();
        MySqlDataReader sqlRd;

        DataSet DS = new DataSet();

        string server = "localhost";
        string username = "root";
        string password = "melinda";
        string database = "rent";
        public Add_account()
        {
            InitializeComponent();

            LoadUserData();
        }
        private void LoadUserData()
        {
            string connectionString = "server=localhost;uid=root;pwd=melinda;database=rent";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open(); // Open the connection

                    // Query to select all columns from the useraccounts table
                    string query = "SELECT * FROM members";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Create a DataAdapter to fetch data from the database
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the fetched data
                            System.Data.DataTable dataTable = new System.Data.DataTable();

                            // Fill the DataTable with data from the DataAdapter
                            adapter.Fill(dataTable);

                            // Bind the DataTable to the DataGridView
                            dataGridView1.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void uploadData()
        {
            sqlConn.ConnectionString = "server=" + server + ";"
                + "username =" + username + ";"
                + "password =" + password + ";"
                + "database =" + database;

            sqlConn.Open();
            sqlCmd.Connection = sqlConn;
            sqlCmd.CommandText = "select * from members.members";
            sqlRd = sqlCmd.ExecuteReader();
            sqlDt.Load(sqlRd);
            sqlRd.Close();
            sqlConn.Close();
            dataGridView1.DataSource = sqlDt;
        }



        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult iExit;
                iExit = MessageBox.Show("Confirm if you want to exit", "MySql Connector", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (iExit == DialogResult.Yes)
                {

                    this.Hide();
                    Form2 gf2 = new Form2();
                    gf2.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetbtn_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in panel2.Controls)
                {
                    if (c is TextBox)
                    {
                        ((TextBox)c).Clear();

                    }
                }
                searchtxt.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=localhost;uid=root;pwd=melinda;database=rent"; // connect to MySQL database

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open(); // Open the connection

                    string Email = emailbox.Text;

                    // Check for duplicate email
                    if (IsEmailExists(connection, Email))
                    {
                        MessageBox.Show("Email already exists. Please choose a different one.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insert the new user account into the database
                    string query = "INSERT INTO members (FirstName, LastName, Email,Address, Tenant_status) VALUES (@FirstName, @LastName, @Email, @Address, @TenantStatus)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Retrieve user input

                        string FirstName = fnamebox.Text;
                        string LastName = lnamebox.Text;
                        string Address = addressbox.Text;
                        string selectedStatus = comboBox1.SelectedItem.ToString();

                        // Add parameters
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@TenantStatus", selectedStatus);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("User account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Hide();
                            var acc = new Add_account();
                            acc.Show();
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

        private bool IsEmailExists(MySqlConnection connection, string Email)
        {
            string query = "SELECT COUNT(*) FROM members WHERE Email = @Email";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", Email);

                // Open the connection if it's not already open
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                // Execute the query and return true if count is greater than 0
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void delbtn_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    // Assuming sqlConn and sqlCmd are declared and initialized somewhere in your code

                    // Establish the connection string
                    sqlConn.ConnectionString = "server=" + server + ";"
                + "username =" + username + ";"
                + "password =" + password + ";"
                + "database =" + database;

                    // Open the connection
                    sqlConn.Open();
                    sqlCmd.CommandText = "delete from rent.members where UserID =@userid";

                    // Prepare the SQL command
                    sqlCmd = new MySqlCommand(sqlQuery, sqlConn);


                    // Close the connection
                    sqlConn.Close();
                    resetbtn_Click(sender, e);

                    foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                    {
                        dataGridView1.Rows.RemoveAt(item.Index);


                        // Iterate over selected rows and remove them from the DataGridView
                        foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                        {
                            dataGridView1.Rows.Remove(row);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }//end delete button
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // Accessing values from the selected row and setting them to textboxes
                fnamebox.Text = selectedRow.Cells[1].Value.ToString();
                lnamebox.Text = selectedRow.Cells[2].Value.ToString();
                emailbox.Text = selectedRow.Cells[3].Value.ToString();
                addressbox.Text = selectedRow.Cells[4].Value.ToString();
                comboBox1.Text = selectedRow.Cells[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchbtn_Click(object sender, EventArgs e)
        {
            string ConnectionString = "server=localhost;uid=root;pwd=melinda;database=rent";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Validate and convert searchtxt.Text to an integer
                    if (int.TryParse(searchtxt.Text, out int userID))
                    {
                        sqlQuery = "SELECT * FROM rent.members WHERE UserID = @UserID";
                        sqlCmd = new MySqlCommand(sqlQuery, connection);
                        sqlCmd.Parameters.AddWithValue("@UserID", userID);

                        sqlRd = sqlCmd.ExecuteReader();

                        if (sqlRd.Read())
                        {
                            fnamebox.Text = sqlRd.GetString("FirstName");
                            lnamebox.Text = sqlRd.GetString("LastName");
                            emailbox.Text = sqlRd.GetString("Email");
                            addressbox.Text = sqlRd.GetString("Address");
                            comboBox1.Text = sqlRd.GetString("Tenant_status");
                        }
                        else
                        {
                            MessageBox.Show("No Data Found", "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid UserID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void displaybtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = sqlDt.DefaultView;

                // Case-insensitive search for any column containing the search text
                dv.RowFilter = $"Convert(FirstName, 'System.String') LIKE '%{searchtxt.Text}%' OR " +
                               $"Convert(LastName, 'System.String') LIKE '%{searchtxt.Text}%' OR " +
                               $"Convert(Email, 'System.String') LIKE '%{searchtxt.Text}%' OR " +
                               $"Convert(Address, 'System.String') LIKE '%{searchtxt.Text}%' OR " +
                               $"Convert(Tenant_status, 'System.String') LIKE '%{searchtxt.Text}%'";

                dataGridView1.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void updatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the connection string
                string ConnectionString = "server=localhost;uid=root;pwd=melinda;database=rent";

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Check if there's a selected row
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        // Get the UserID from the first column of the selected row
                        int userID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

                        // Update the database with the new values
                        string updateQuery = "UPDATE rent.members SET FirstName = @FirstName, LastName = @LastName, " +
                                              "Email = @Email, Address = @Address, Tenant_status = @TenantStatus " +
                                              "WHERE UserID = @UserID";

                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                        {
                            // Retrieve user input
                            updateCmd.Parameters.AddWithValue("@FirstName", fnamebox.Text);
                            updateCmd.Parameters.AddWithValue("@LastName", lnamebox.Text);
                            updateCmd.Parameters.AddWithValue("@Email", emailbox.Text);
                            updateCmd.Parameters.AddWithValue("@Address", addressbox.Text);
                            updateCmd.Parameters.AddWithValue("@TenantStatus", comboBox1.SelectedItem?.ToString());
                            updateCmd.Parameters.AddWithValue("@UserID", userID);

                            // Execute the update command
                            int rowsAffected = updateCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadUserData();  // Refresh the DataGridView after the update
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No row selected for update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form2();
            myform.Show();
        }

        private void invoicebtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form5();
            myform.Show();
        }

        private void tenantsbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Add_account();
            myform.Show();
        }

        private void fnamebox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}










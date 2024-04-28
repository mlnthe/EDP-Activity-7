using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Forms
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string recEmail = this.txtbox.Text;

            try
            {
                MySqlConnection conn;
                string myConnectionString;
                myConnectionString = "server=localhost;uid=root;" +
                    "pwd=melinda;database=rent";

                conn = new MySqlConnection(myConnectionString);
                conn.Open();

                string sql = "SELECT COUNT(*) FROM tenants_table WHERE Email = @myemail";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@myemail", recEmail);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    // If the email exists, prompt the user to reset the password
                    DialogResult result = MessageBox.Show("Do you want to reset your password?", "Reset Password", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        // If the user chooses Yes, delete the password from the database
                        string deleteSql = "UPDATE tenants_table SET Password = NULL WHERE Email = @email";
                        MySqlCommand deleteCmd = new MySqlCommand(deleteSql, conn);
                        deleteCmd.Parameters.AddWithValue("@email", recEmail);
                        deleteCmd.ExecuteNonQuery();

                        // Prompt the user to input a new password
                        string newPassword = Prompt.ShowDialog("Enter your new password: ", "New Password");

                        // You can add validation here to ensure newPassword is not empty

                        // Update the database with the new password
                        string updateSql = "UPDATE tenants_table SET Password = @newPassword WHERE Email = @email";
                        MySqlCommand updateCmd = new MySqlCommand(updateSql, conn);
                        updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@email", recEmail);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Password updated successfully! Please log in with your new password.");
                        this.Hide();
                        var mainForm = new Main();
                        mainForm.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email.");
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
                        MessageBox.Show("Invalid Recovery Email, Please Try Again");
                        break;
                    // Add more cases if needed
                    default:
                        MessageBox.Show($"An error occurred: {ex.Message}");
                        break;
                }
            }
        }

        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form();
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.Text = caption;
                prompt.StartPosition = FormStartPosition.CenterScreen; // Center the form

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 300 };

                Button confirmation = new Button() { Text = "Ok", Left = 250, Width = 100, Top = 70 };
                confirmation.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);

                prompt.ShowDialog();

                return textBox.Text;
            }
        }

        private void txtbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

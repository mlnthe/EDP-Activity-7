using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Forms
{
    public partial class Revenue : Form
    {
        private MySqlConnection connection;

        public Revenue()
        {
            InitializeComponent();
            string connectionString = "server=localhost;uid=root;pwd=melinda;database=rent";
            connection = new MySqlConnection(connectionString);
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO revenue (month, rooms, total_sales) VALUES (@month, @rooms, @total_sales)";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@month", monthbox.Text);
                    command.Parameters.AddWithValue("@rooms", Convert.ToInt32(occupiedbox.Text));
                    command.Parameters.AddWithValue("@total_sales", Convert.ToDouble(totalbox.Text));
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

      
        private void exportbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Load the existing Excel template
                string templatePath = @"C:\Users\Melinda\Downloads\Revenue.xlsx"; // Replace with your template file path
                FileInfo templateFile = new FileInfo(templatePath);

                using (ExcelPackage excelPackage = new ExcelPackage(templateFile))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Revenue"];

                    // Check if the worksheet exists
                    if (worksheet != null)
                    {
                        // Retrieve data from the database for the specific invoice
                        string query = "SELECT * FROM revenue ";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        // Check if data is retrieved
                        if (reader.Read())
                        {
                            // Replace placeholders with actual data
                           // worksheet.Cells["C7"].Value = reader[""].ToString();
                            worksheet.Cells["D9"].Value = reader["rooms"].ToString();
                            worksheet.Cells["D11"].Value = reader["total_sales"].ToString();
                    



                            // Save the modified Excel package
                            FileInfo outputFile = new FileInfo("Revenue.xlsx");
                            excelPackage.SaveAs(outputFile);

                            MessageBox.Show("Revenue report exported successfully.");

                            // Open the Excel file with the default application
                            Process.Start(outputFile.FullName);
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified revenue.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Worksheet 'Billing' does not exist in the template.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
    }


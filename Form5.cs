using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using OfficeOpenXml;

namespace Forms
{
    public partial class Form5 : Form
    {
        MySqlConnection connection;
        private int invoiceId; // Declare invoiceId as a class-level variable

        public Form5()
        {
            InitializeComponent();
            string connectionString = "server=localhost;uid=root;pwd=melinda;database=rent";
            connection = new MySqlConnection(connectionString);
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if the connection is already open
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                // Retrieve values from text boxes
                string tenantName = tenantnametxtbox.Text;
                string dateText = datebox.Text;

                // Attempt to parse the date string
                DateTime dueDate;
                if (!DateTime.TryParseExact(dateText, new[] { "MM/dd/yy", "MM-dd-yy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dueDate))
                {
                    // Date parsing failed, show error message
                    MessageBox.Show("Error: Due date is not in the correct format. Please enter the date in the format 'MM/dd/yy' or 'MM-dd-yy'.");
                    return; // Exit method
                }

                string description = descriptionbox.Text;
                string month = monthtxtbox.Text; // Change data type to string
                int year = Convert.ToInt32(yeartxtbox.Text);
                string paymentMethod = comboBoxpaymentmethod.SelectedItem.ToString();
                double subtotal = Convert.ToDouble(subtotaltxtbox.Text);
                double lateFee = Convert.ToDouble(latetxtbox.Text);
                double total = Convert.ToDouble(totaltxtbox.Text);
                DateTime currentDate = DateTime.Now;

                // Insert data into the database
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO Invoice (TenantName, DueDate, Description, Month, Year, PaymentMethod, Subtotal, LateFee, Total, Date) VALUES (@TenantName, @DueDate, @Description, @Month, @Year, @PaymentMethod, @Subtotal, @LateFee, @Total, @Date)";
                    cmd.Parameters.AddWithValue("@TenantName", tenantName);
                    cmd.Parameters.AddWithValue("@DueDate", dueDate);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@Subtotal", subtotal);
                    cmd.Parameters.AddWithValue("@LateFee", lateFee);
                    cmd.Parameters.AddWithValue("@Total", total);
                    cmd.Parameters.AddWithValue("@Date", currentDate); // Add current date

                    cmd.ExecuteNonQuery();

                    // Retrieve the last inserted invoice ID
                    invoiceId = GetLastInsertedInvoiceId(); // Update the class-level invoiceId

                    // Clear the textboxes after successful save
                    // tenantnametxtbox.Text = "";
                    //duedatetxtbox.Text = "";
                    // datebox.Text = "";
                    // descriptionbox.Text = "";
                    // monthtxtbox.Text = "";
                    // yeartxtbox.Text = "";
                    //  comboBoxpaymentmethod.SelectedIndex = -1; // Clear selected item
                    // subtotaltxtbox.Text = "";
                    // latetxtbox.Text = "";
                    //  totaltxtbox.Text = "";

                    // Optionally, set focus to the first textbox for convenience
                    //   tenantnametxtbox.Focus();
                    MessageBox.Show("Data saved successfully.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Error: Input format was not correct. Please check your inputs.");
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Form3();
            myform.Show();
        }

        private void tenantsbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Add_account();
            myform.Show();
        }

        private void reportbtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            var myform = new Report();
            myform.Show();
        }

        private void exportbtn_Click(object sender, EventArgs e)  // export the invoice to Excel template
        {
            try
            {
                // Load the existing Excel template
                string templatePath = @"C:\Users\Melinda\Downloads\Billing.xlsx"; // Replace with your template file path
                FileInfo templateFile = new FileInfo(templatePath);

                using (ExcelPackage excelPackage = new ExcelPackage(templateFile))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Billing"];

                    // Check if the worksheet exists
                    if (worksheet != null)
                    {
                        // Retrieve data from the database for the specific invoice
                        string query = "SELECT * FROM Invoice WHERE InvoiceId = @InvoiceId";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@InvoiceId", invoiceId); // Use class-level invoiceId
                        connection.Open();
                        MySqlDataReader reader = cmd.ExecuteReader();

                        // Check if data is retrieved
                        if (reader.Read())
                        {
                            // Replace placeholders with actual data
                            worksheet.Cells["C7"].Value = reader["TenantName"].ToString();
                            worksheet.Cells["E12"].Value = reader["DueDate"].ToString();
                           // worksheet.Cells["B5"].Value = reader["Description"].ToString();
                            worksheet.Cells["D12"].Value = reader["PaymentMethod"].ToString();
                            worksheet.Cells["E14"].Value = reader["Subtotal"].ToString();
                            worksheet.Cells["E15"].Value = reader["LateFee"].ToString();
                            worksheet.Cells["E7"].Value = reader["Date"].ToString();
                            worksheet.Cells["E16"].Value = reader["Total"].ToString();
                            worksheet.Cells["C8"].Value = reader["Month"].ToString();// this is for the address

                   

                            // Save the modified Excel package
                            FileInfo outputFile = new FileInfo("Billing.xlsx");
                            excelPackage.SaveAs(outputFile);

                            MessageBox.Show("Invoice exported successfully.");

                            // Open the Excel file with the default application
                            Process.Start(outputFile.FullName);
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified invoice.");
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

        private int GetLastInsertedInvoiceId()
        {
            int lastId = -1; // Default value in case no ID is found

            try
            {
                // Check if the connection is already open
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                // Execute a query to get the last inserted invoice ID
                string query = "SELECT MAX(InvoiceId) FROM Invoice";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    lastId = Convert.ToInt32(result);
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

            return lastId;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void duedatetxtbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

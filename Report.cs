using MySql.Data.MySqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;

namespace Forms
{
    public partial class Report : Form
    {
        MySqlConnection connection;

        public Report()
        {
            InitializeComponent();
            string connectionString = "server=localhost;uid=root;pwd=melinda;database=rent";
            connection = new MySqlConnection(connectionString);
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                connection.Open();
                string query = "SELECT * FROM expenses";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void generatebtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Load the existing Excel file or create a new one if it doesn't exist
                string filePath = @"C:\Users\Melinda\Downloads\Expenses.xlsx"; // Replace with your Excel file path
                FileInfo existingFile = new FileInfo(filePath);
                ExcelPackage excelPackage;

                if (existingFile.Exists)
                    excelPackage = new ExcelPackage(existingFile);
                else
                    excelPackage = new ExcelPackage();

                // Get the existing "Report" worksheet or add it if it doesn't exist
                ExcelWorksheet reportWorksheet = excelPackage.Workbook.Worksheets["Report"];
                if (reportWorksheet == null)
                    reportWorksheet = excelPackage.Workbook.Worksheets.Add("Report");

                // Add data to the "Report" worksheet
                reportWorksheet.Cells["B9"].Value = monthtxt.Text;
                reportWorksheet.Cells["I9"].Value = totaltxt.Text;
                reportWorksheet.Cells["C9"].Value = watertxt.Text;
                reportWorksheet.Cells["D9"].Value = electricitytxt.Text;
                reportWorksheet.Cells["E9"].Value = maintenancetxt.Text;
                reportWorksheet.Cells["F9"].Value = repairstxt.Text;
                reportWorksheet.Cells["G9"].Value = renovationstxt.Text;
                reportWorksheet.Cells["H9"].Value = utilitiestxt.Text;

                // Get the existing "Chart" worksheet or add it if it doesn't exist
                ExcelWorksheet chartWorksheet = excelPackage.Workbook.Worksheets["Chart"];
                if (chartWorksheet == null)
                    chartWorksheet = excelPackage.Workbook.Worksheets.Add("Chart");

                // Create and configure the chart
                Chart chart = new Chart();
                chart.Size = new Size(800, 600);
                chart.BackColor = Color.White; // Set background color to white
                chart.Titles.Add("Expense Chart");
                chart.ChartAreas.Add("ChartArea").BackColor = Color.White; // Set background color of chart area to white
                chart.ChartAreas["ChartArea"].AxisX.LineColor = Color.LightGray; // Set X-axis line color to light gray
                chart.ChartAreas["ChartArea"].AxisY.LineColor = Color.LightGray; // Set Y-axis line color to light gray
                chart.ChartAreas["ChartArea"].AxisX.MajorGrid.LineColor = Color.LightGray; // Set X-axis grid line color to light gray
                chart.ChartAreas["ChartArea"].AxisY.MajorGrid.LineColor = Color.LightGray; // Set Y-axis grid line color to light gray

                Series expensesSeries = chart.Series.Add("Expenses");
                expensesSeries.ChartType = SeriesChartType.Column; // Set chart type to column
                expensesSeries.Color = Color.LightBlue; // Set column color to light blue
                expensesSeries.BorderWidth = 1; // Set border width to 1
                expensesSeries.IsValueShownAsLabel = true; // Show data labels

                // Add chart data as needed with labels
                expensesSeries.Points.AddXY("Total", Convert.ToDouble(totaltxt.Text));
                expensesSeries.Points.AddXY("Water", Convert.ToDouble(watertxt.Text));
                expensesSeries.Points.AddXY("Electricity", Convert.ToDouble(electricitytxt.Text));
                expensesSeries.Points.AddXY("Maintenance", Convert.ToDouble(maintenancetxt.Text));
                expensesSeries.Points.AddXY("Repairs", Convert.ToDouble(repairstxt.Text));
                expensesSeries.Points.AddXY("Renovations", Convert.ToDouble(renovationstxt.Text));
                expensesSeries.Points.AddXY("Utilities", Convert.ToDouble(utilitiestxt.Text));

                // Convert chart to image
                using (MemoryStream ms = new MemoryStream())
                {
                    chart.SaveImage(ms, ChartImageFormat.Png);

                    // Add the chart image to the "Chart" worksheet
                    ExcelPicture excelChart = chartWorksheet.Drawings.AddPicture("Chart", ms);
                    excelChart.SetPosition(1, 0, 1, 0); // Adjust position as needed
                }

                // Save the Excel file
                excelPackage.Save();

                // Open the Excel file with the default application
                Process.Start(filePath);

                MessageBox.Show("Chart generated and added to the 'Chart' worksheet successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void exportbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Load the existing Excel file or create a new one if it doesn't exist
                string filePath = @"C:\Users\Melinda\Downloads\Expenses.xlsx"; // Replace with your Excel file path
                FileInfo existingFile = new FileInfo(filePath);
                ExcelPackage excelPackage;

                if (existingFile.Exists)
                    excelPackage = new ExcelPackage(existingFile);
                else
                    excelPackage = new ExcelPackage();

                // Get the existing "Report" worksheet or add it if it doesn't exist
                ExcelWorksheet reportWorksheet = excelPackage.Workbook.Worksheets["Report"];
                if (reportWorksheet == null)
                    reportWorksheet = excelPackage.Workbook.Worksheets.Add("Report");

                // Add data to the "Report" worksheet
                reportWorksheet.Cells["B9"].Value = monthtxt.Text;
                reportWorksheet.Cells["I9"].Value = totaltxt.Text;
                reportWorksheet.Cells["C9"].Value = watertxt.Text;
                reportWorksheet.Cells["D9"].Value = electricitytxt.Text;
                reportWorksheet.Cells["E9"].Value = maintenancetxt.Text;
                reportWorksheet.Cells["F9"].Value = repairstxt.Text;
                reportWorksheet.Cells["G9"].Value = renovationstxt.Text;
                reportWorksheet.Cells["H9"].Value = utilitiestxt.Text;

                // Save the Excel file
                excelPackage.Save();

                // Open the Excel file with the default application
                Process.Start(filePath);

                MessageBox.Show("Data exported successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                monthtxt.Text = selectedRow.Cells[1].Value.ToString();
                totaltxt.Text = selectedRow.Cells[2].Value.ToString();
                watertxt.Text = selectedRow.Cells[3].Value.ToString();
                electricitytxt.Text = selectedRow.Cells[4].Value.ToString();
                maintenancetxt.Text = selectedRow.Cells[5].Value.ToString();
                repairstxt.Text = selectedRow.Cells[6].Value.ToString();
                renovationstxt.Text = selectedRow.Cells[7].Value.ToString();
                utilitiestxt.Text = selectedRow.Cells[8].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void revenuebtn_Click(object sender, EventArgs e)
        {

            this.Hide();
            var myform = new Revenue();
            myform.Show();
        }
    }
}

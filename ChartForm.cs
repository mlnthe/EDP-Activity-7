using System;
using System.Drawing;
using System.Windows.Forms;

namespace Forms
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();
            // Set the size of the PictureBox
         
        }

        // Method to set the chart image in the PictureBox
        public void SetChartImage(Bitmap image)
        {
            // Check if the provided image is not null
            if (image != null)
            {
                // Assign the image to the PictureBox control
                pictureBox1.Image = image;
            }
            else
            {
                // If the image is null, display a message or handle the error as needed
                MessageBox.Show("Error: Chart image is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

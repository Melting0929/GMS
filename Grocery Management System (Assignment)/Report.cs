
//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)
// Generate report dialog

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grocery_Management_System__Assignment_
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";

        // Handle button click events for generate report
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            // Get the selected value from a ComboBox and the start and end dates from DateTimePickers
            string selectedValue = comboBox1.Text;

            DateTime vstartDate = dateTimePicker1.Value;
            DateTime vendDate = dateTimePicker2.Value;

            // Check if vendDate is earlier than vstartDate
            if (vendDate < vstartDate)
            {
                MessageBox.Show("End date cannot be earlier than start date.");
                return; // Exit the method
            }

            // Format the dates format.
            string startDate = vstartDate.ToString("yyyy-MM-dd");
            string endDate = vendDate.ToString("yyyy-MM-dd");

            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();
                string query = "";

                // Determine the SQL query based on the selected value from the ComboBox
                if (selectedValue == "Staff")
                {
                    query = "SELECT * FROM staff WHERE dateofEmployment >= @StartDate AND dateofEmployment <= @EndDate";
                }
                else if (selectedValue == "Product")
                {
                    query = "SELECT * FROM product WHERE latestOrder >= @StartDate AND latestOrder <= @EndDate";
                }
                else if (selectedValue == "Supplier")
                {
                    query = "SELECT * FROM supplier WHERE latestOrder >= @StartDate AND latestOrder <= @EndDate";
                }
                else if (selectedValue == "Order")
                {
                    query = "SELECT * FROM [order] WHERE latestorder >= @StartDate AND latestorder <= @EndDate";
                }

                // Create a SqlCommand with parameters for the selected date range
                comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@StartDate", startDate);
                comm.Parameters.AddWithValue("@EndDate", endDate);
                comm.ExecuteNonQuery();

                // Create a data adapter to fetch data based on the SqlCommand and fill it into a DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Set the DataGridView's data source to the DataTable to display the results
                dataGridView1.DataSource = dataTable;
            }
            // Exception handling
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}


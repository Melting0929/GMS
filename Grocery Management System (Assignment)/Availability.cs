
//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)

using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Grocery_Management_System__Assignment_
{
    public partial class Availability : Form
    {
        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";

        public Availability()
        {
            InitializeComponent();
        }

        // Handle button click events for searching records
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();

                // Construct a SQL query to search for a product by its ID
                String str1 = "select product_id, product_name, product_category, " +
                    "current_quantity from product where product_id = ('"
                    + textBox1.Text + "')";

                comm = new SqlCommand(str1, conn);

                // Execute the query and fill the result in a DataTable
                SqlDataAdapter da = new SqlDataAdapter(str1, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // If records are found, display in DataGridView, clear input field and show success message
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    textBox1.Clear();
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    // Error Message
                    MessageBox.Show("No records found with the provided product_id: " + textBox1.Text);
                }
            }
            // Exception handling
            catch (Exception)
            {
                MessageBox.Show("Record Not Displayed!");
            }

            // Close Connection
            finally
            {
                conn.Close();
            }
        }

        //ignore
        private void label4_Click(object sender, EventArgs e)
        {
            //ignore
        }

        //ignore
        private void label5_Click(object sender, EventArgs e)
        {
            //ignore
        }

        // Handle button click event for updating total num of product and total grocery value
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int total = 0;
            double values = 0;

            // Establish a database connection
            conn = new SqlConnection(connstr);
            conn.Open();

            // Query the database to calculate the total number of products and total grocery value
            String query1 = "SELECT SUM(current_quantity) FROM product";
            comm = new SqlCommand(query1, conn);
            total = Convert.ToInt32(comm.ExecuteScalar());

            //Display value
            label4.Text = "Total Number of Products in Store: " + total;

            String query2 = "SELECT SUM(current_quantity * store_price) FROM product";
            comm = new SqlCommand(query2, conn);
            values = Convert.ToDouble(comm.ExecuteScalar());

            //Display value
            label5.Text = "Total Grocery Value: RM " + values.ToString("0.00");
        }

        // Handle button click event for displaying all records in a DataGridView
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            // Construct a SQL query to select all products
            String str1 = "select product_id, product_name, product_category, " +
                    "current_quantity from product";

            comm = new SqlCommand(str1, conn);

            // Execute the query and fill the result in a DataTable
            SqlDataAdapter da = new SqlDataAdapter(str1, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // If records are found, display in DataGridView and show success message
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                MessageBox.Show("Record Displayed!");
            }
            else
            {
                // Error message
                MessageBox.Show("No records found");
            }
        }
    }
}

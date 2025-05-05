
//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Grocery_Management_System__Assignment_
{
    public partial class Order : Form
    {
        // Declare global variable
        private string id;

        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";

        // Constructor
        public Order(String ID)
        {
            InitializeComponent();
            this.id = ID;
        }

        // This method is executed when the form is loaded
        private void Order_Load(object sender, EventArgs e)
        { 
            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();

                // Load all records from the "order" table into a DataGridView
                SqlDataAdapter da = new SqlDataAdapter("select * from [order]", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                // Query the "supplier" table to populate a ListBox with supplier IDs
                string query = "SELECT supplier_id FROM supplier";
                comm = new SqlCommand(query, conn);
                SqlDataReader reader = comm.ExecuteReader();

                // Set the text and make a label visible
                label9.Text = id;
                label9.Visible = true;

                // Clear the items in the ListBox
                listBox1.Items.Clear();

                // Populate the ListBox with supplier IDs
                while (reader.Read())
                {
                    string supplierName = reader["supplier_id"].ToString();
                    listBox1.Items.Add(supplierName);
                }

                reader.Close();
            }
            // Exception handling
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            // Close connection
            finally
            {
                conn.Close();
            }
        }

        // Handle the search button click event
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Check if any of the text fields is null or empty
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return; // Exit the method if any field is empty
            }
            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();

                // Construct a SQL query to search for an order by its ID
                string str1 = "SELECT * FROM [order] WHERE order_id =(" 
                    + "'" + textBox1.Text + "')";;

                SqlDataAdapter da = new SqlDataAdapter(str1, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Display the matching record in the DataGridView, clear the input field, and show success message
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    clearText();
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    // Error message
                    MessageBox.Show("No records found with the order_id: " + textBox1.Text);
                }
            }
            // Exception handling
            catch (Exception ex)
            {
                MessageBox.Show("Error searching record: " + ex.Message);
            }
            // Close connection
            finally
            {
                conn.Close();
            }
        }

        // Handle the place button click event
        private void btnPlace_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox3.Text) ||
            string.IsNullOrEmpty(textBox3.Text) || listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }
            // Get the selected date and format it
            DateTime selectedDate = dateTimePicker1.Value;
            string formattedDate = selectedDate.ToString("yyyy-MM-dd");
            int quantity = 0;

            // Establish a database connection
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;

            /* Select current_quantity from the database to make sure the current quantity of the product 
               before place a new order */
            String str1 = "SELECT current_quantity from product WHERE product_id = @ProductID";
            comm = new SqlCommand(str1, conn);
            comm.Parameters.AddWithValue("@ProductID", textBox3.Text);

            // Execute the query and save to the result
            object result = comm.ExecuteScalar();

            // If the current quantity record exixts
            if (result != null)
            {
                // Convert result to int
                quantity = Convert.ToInt32(result);

                // Check current quantity if it is zero to place order
                if (quantity == 0)
                {
                    // Insert a new order record into the "order" table
                    String str = "insert into [order] values(";
                    str += "'" + textBox1.Text + "',";
                    str += "'" + textBox3.Text + "',";
                    str += "'" + textBox4.Text + "',";
                    str += "'" + listBox1.SelectedItem + "',";
                    str += "'" + formattedDate + "',";
                    str += "'" + label9.Text + "')";
                    comm = new SqlCommand(str, conn);
                    comm.ExecuteNonQuery();

                    try
                    {
                        // Update product information
                        String DateQuery = "UPDATE product SET latestOrder = '" + formattedDate + "' WHERE product_id = @ProductID";
                        comm = new SqlCommand(DateQuery, conn);
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@ProductID", textBox3.Text);
                        comm.ExecuteNonQuery();

                        String UnitQuery = "UPDATE product SET unitsOrderFromSupplier =  @CurrentQuantity WHERE latestOrder = @date";
                        comm = new SqlCommand(UnitQuery, conn);
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@date", formattedDate);
                        comm.Parameters.AddWithValue("@CurrentQuantity", textBox4.Text);
                        comm.ExecuteNonQuery();

                        string updateQuery = "UPDATE product SET current_quantity = @CurrentQuantity WHERE product_id = @ProductID";
                        comm = new SqlCommand(updateQuery, conn);
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@ProductID", textBox3.Text);
                        comm.Parameters.AddWithValue("@CurrentQuantity", textBox4.Text);
                        comm.ExecuteNonQuery();

                        String resetQuery = "UPDATE product SET sold_quantity = 0 WHERE product_id = @ProductID";
                        comm = new SqlCommand(resetQuery, conn);
                        comm.Parameters.Clear();
                        comm.Parameters.AddWithValue("@ProductID", textBox3.Text);
                        comm.ExecuteNonQuery();

                        // Show record in data grid view
                        SqlDataAdapter da = new SqlDataAdapter("select * from [order]", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        MessageBox.Show("Record saved...");
                    }
                    // Exception handling
                    catch (Exception ex)
                    {
                        MessageBox.Show("Record not saved..." + ex.Message);
                    }
                }
                else
                {
                    // If current quantity is more than zero, display error message
                    MessageBox.Show("Quantity of the product is still available, " +
                        "kindly proceed to the product availability page.");
                }
            }
            else
            {
                // If the record does not exist, display an error message
                MessageBox.Show("Product ID not found in the database. Please enter a valid product ID.");
            }

            conn.Close();
        }

        // Handle button click events for deleting records
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the order ID to delete
            string orderId = textBox1.Text;
            
            if (!string.IsNullOrEmpty(orderId))
            {
                string str1 = "DELETE FROM [order] WHERE order_id = @OrderId";
                SqlCommand comm = new SqlCommand(str1, conn);
                comm.Parameters.AddWithValue("@OrderId", orderId);
                try
                {
                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [order]", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    clearText();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Record deleted...");
                    }
                    else
                    {
                        MessageBox.Show("Record not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Record delete failed: " + ex.Message);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                    comm.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid order_id.");
            }

        }

        // To clear Text
        public void clearText()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        // Handle DataGridView cell click events to show on specific text views
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Get the selected row in the DataGridView
            DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

            // Populate text boxes with data from the selected record
            textBox1.Text = selectedRow.Cells["order_id"].Value.ToString();
            label9.Text = selectedRow.Cells["orderperson"].Value.ToString();
            textBox3.Text = selectedRow.Cells["product_id"].Value.ToString();
            textBox4.Text = selectedRow.Cells["unitsOrderFromSupplier"].Value.ToString();
            listBox1.Text = selectedRow.Cells["supplier_id"].Value.ToString();
            string order = selectedRow.Cells["latestOrder"].Value.ToString();

            // Parse and set the date value from the "latestOrder" column to a DateTimePicker
            if (DateTime.TryParse(order, out DateTime parsedOrder))
            {
                dateTimePicker1.Value = parsedOrder;
            }
            else
            {
                // Error message
                MessageBox.Show("Error: Time displayed failed");
            }
        }

        // Handle button click event to show all records in the DataGridView
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [order]", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        // Handle button click event to clear text views's text
        private void btnClear_Click(object sender, EventArgs e)
        {
            clearText();
        }
    }
}

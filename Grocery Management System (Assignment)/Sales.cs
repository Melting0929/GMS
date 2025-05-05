
//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Grocery_Management_System__Assignment_
{
    public partial class Sales : Form
    {
        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";

        public Sales()
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
                string str1 = "SELECT product_id, product_name, product_category, sold_quantity, " +
                    "supplier_price, store_price FROM product WHERE product_id = ('"
                     + textBox1.Text + "')";

                comm = new SqlCommand(str1, conn);

                //To show in data grid view
                SqlDataAdapter da = new SqlDataAdapter(str1, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    // Display the result in DataGridView, clear input fields, update label text,
                    // and show success message
                    dataGridView1.DataSource = dt;
                    textBox1.Clear();
                    textBox2.Clear();
                    label2.Text = "Sales of the Product:";
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    // Clear input fields and show error message
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("No records found with the provided product_id: " + textBox1.Text);
                }
            }
            // Exception handling
            catch (Exception)
            {
                MessageBox.Show("Record Not Displayed!");
            }
            // Close connection
            finally
            {                
                conn.Close();                
            }
        }

        // Handle button click events for updating records
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return; // Exit the method if any field is empty
            }
            // Establish a database connection
            conn = new SqlConnection(connstr);
            conn.Open();

            // SQL query to retrieve the current quantity of a product by its ID
            String str1 = "SELECT current_quantity from product WHERE product_id = @ProductID";
            comm = new SqlCommand(str1, conn);
            comm.Parameters.Clear();
            comm.Parameters.AddWithValue("@ProductID", textBox1.Text);

            object result = comm.ExecuteScalar();

            // Convert the result and input to integer.
            int cquantity = Convert.ToInt32(result);
            int squantity = int.Parse(textBox2.Text);

            // Proceed to update the product sales, quantities, and calculate sales amount
            if (squantity <= cquantity)
            {
                try
                {
                    conn = new SqlConnection(connstr);
                    conn.Open();
                    string updateQuery = "UPDATE product SET sold_quantity = sold_quantity + @SoldQuantity, " +
                        "current_quantity = current_quantity - @SoldQuantity WHERE product_id = @ProductID";                    

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SoldQuantity", textBox2.Text);
                        cmd.Parameters.AddWithValue("@ProductID", textBox1.Text);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        double sales = 0;

                        conn = new SqlConnection(connstr);
                        conn.Open();

                        string query1 = "SELECT SUM(sold_quantity * (store_price - supplier_price)) " +
                            "FROM product WHERE product_id = ('" + textBox1.Text + "')";
                        comm = new SqlCommand(query1, conn);
                        sales = Convert.ToDouble(comm.ExecuteScalar());

                        label2.Text = "Sales of the Product (" + textBox1.Text + "): RM " + sales.ToString("0.00");                        
                        
                        // Update Successful
                        if (rowsAffected > 0)
                        {
                            textBox1.Clear();
                            textBox2.Clear();
                            MessageBox.Show("Sales recorded successfully.");
                        }
                        else
                        {
                            // Update Failed
                            textBox1.Clear();
                            textBox2.Clear();
                            MessageBox.Show("Failed to record sales.");
                        }
                    }
                }
                // Exception handling
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                // Show error message
                MessageBox.Show("Your sold quantity exceeds the current quantity available in store.");
            }
        }

        // Handle button click events for showing all records
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();

                // SQL query to select all product records
                String str1 = "select product_id, product_name, product_category, " +
                    "sold_quantity,supplier_price,store_price from product";

                comm = new SqlCommand(str1, conn);

                SqlDataAdapter da = new SqlDataAdapter(str1, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // if record exists
                if (dt.Rows.Count > 0)
                {
                    // Display the result in a DataGridView
                    dataGridView1.DataSource = dt;
                    double totalSales = 0;

                    // calculate the total sales, update label text, and clear input fields
                    String query2 = "SELECT SUM(sold_quantity * (store_price - supplier_price)) FROM product";
                    comm = new SqlCommand(query2, conn);
                    totalSales = Convert.ToDouble(comm.ExecuteScalar());

                    label5.Text = "Total Sales: RM " + totalSales.ToString("0.00");
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("No records found with the provided product_id: " + textBox1.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Record Not Displayed!");
            }

            finally
            {
                conn.Close();
            }
        }

        // Handle button click events for showing sales of specified product
        private void btnShowU_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new SqlConnection(connstr);
                conn.Open();

                // Fetch the data for a specific product_id
                String query = "SELECT * FROM product WHERE product_id = @ProductId";
                comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@ProductId", textBox1.Text); 

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(comm);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;

                    // Calculate the sum for the specific product_id
                    conn = new SqlConnection(connstr);
                    conn.Open();

                    String query2 = "SELECT SUM(sold_quantity * (store_price - supplier_price)) " +
                        "FROM product WHERE product_id = @ProductId";
                    comm = new SqlCommand(query2, conn);
                    comm.Parameters.AddWithValue("@ProductId", textBox1.Text);

                    double totalSales = Convert.ToDouble(comm.ExecuteScalar());

                    label2.Text = "Total Sales for the Product: RM " + 
                        totalSales.ToString("0.00");

                    textBox2.Clear();
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    textBox1.Clear();
                    textBox2.Clear();
                    MessageBox.Show("No records found with the provided product_id: " + textBox1.Text);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Record Not Displayed!");
            }

            finally
            {
                conn.Close();
            }
        }
    }
}

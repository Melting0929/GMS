
//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)

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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Grocery_Management_System__Assignment_
{
    public partial class StaffPage : Form
    {
        // Declare global variable staffid
        private string staffid;

        // Constructor
        public StaffPage(string staffid)
        {
            InitializeComponent();
            this.staffid = staffid;
        }

        String selectedRole;// declare as global variable to use in combo box
        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";

        // Handle the event when the role is selected from a ComboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected role from the ComboBox
            selectedRole = comboBox1.SelectedItem.ToString();
            // Depending on the selected role, show different panels and data from database on the data grid view
            if (selectedRole == "Staff")
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                btnSearch.Visible = false;
                btnShowAll.Visible = false;
                label1.Text = "Staff Detail Page";

                conn = new SqlConnection(connstr);
                conn.Open();

                string query = "SELECT * FROM staff WHERE staff_id = @staffid";
                comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@staffid", staffid);

                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                SqlDataReader dr = comm.ExecuteReader();

                // If the data reader has records to read
                if (dr.Read())
                {
                    label27.Text = dr["staff_id"].ToString();
                    label28.Text = dr["employmentStatus"].ToString();
                    DateTime dateofEmployment = Convert.ToDateTime(dr["dateofEmployment"]);
                    label29.Text = dateofEmployment.ToString("dd/MM/yyyy");
                }

                dr.Close();

            }
            else if (selectedRole == "Supplier")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                btnAdd.Visible = true;
                btnDelete.Visible = true;
                btnSearch.Visible = true;
                btnShowAll.Visible = true;
                label1.Text = "Staff Page";

                conn = new SqlConnection(connstr);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from supplier", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else if (selectedRole == "Product")
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                btnAdd.Visible = true;
                btnDelete.Visible = true;
                btnSearch.Visible = true;
                btnShowAll.Visible = true;
                label1.Text = "Staff Page";

                conn = new SqlConnection(connstr);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from product", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }            
        }

        // Handle button click events for adding records
        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected role from the ComboBox
            selectedRole = comboBox1.SelectedItem.ToString();

            if (selectedRole == "Supplier")
            {
                // Check if any of the text fields is null or empty
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return; // Exit the method if any field is empty
                }
                try
                {
                    conn = new SqlConnection(connstr);
                    conn.Open();
                    // Get value from the date time picker
                    DateTime selectedDateTime = dateTimePicker1.Value;

                    // Database query to add record into supplier table
                    String str = "insert into supplier values(";
                    str += "'" + textBox1.Text + "',";
                    str += "'" + textBox2.Text + "',";
                    str += "'" + textBox3.Text + "',";
                    str += "'" + textBox4.Text + "',";
                    str += "'" + selectedDateTime.ToString() + "')";

                    // Execute command
                    comm = new SqlCommand(str, conn);
                    comm.ExecuteNonQuery();

                    //To show in data grid view
                    SqlDataAdapter da = new SqlDataAdapter("select * from supplier", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // If successful add, show all record in data grid view and clear text
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                        clearText();
                        MessageBox.Show("Record Saved!");
                    }
                    else
                    {
                        MessageBox.Show("No records found with the supplier_id: " + textBox1.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding record:" + ex.Message);
                }

                //Close connection
                finally
                {
                    conn.Close();
                }
            }
            //Similar with supplier
            else if (selectedRole == "Product")
            {                
                if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) ||
                string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrEmpty(textBox8.Text) ||
                string.IsNullOrEmpty(textBox9.Text) || string.IsNullOrEmpty(textBox10.Text) ||
                string.IsNullOrEmpty(textBox11.Text) || string.IsNullOrEmpty(textBox12.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return; // Exit the method if any field is empty
                }
                try
                {
                    conn = new SqlConnection(connstr);//connecting database
                    conn.Open();//open the connection
                    DateTime selectedDateTime = dateTimePicker2.Value;

                    //SQL command
                    String str = "insert into product values(";
                    str += "'" + textBox5.Text + "',";
                    str += "'" + textBox6.Text + "',";
                    str += "'" + textBox7.Text + "',";
                    str += "'" + textBox8.Text + "',";
                    str += "'" + textBox9.Text + "',";
                    str += "'" + textBox10.Text + "',";
                    str += "'" + textBox11.Text + "',";
                    str += "'" + textBox12.Text + "',";
                    str += "'" + selectedDateTime.ToString() + "')";

                    //To excute the command write above
                    comm = new SqlCommand(str, conn);
                    comm.ExecuteNonQuery();
                    clearText();

                    SqlDataAdapter da = new SqlDataAdapter("select * from product", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                        clearText();
                        MessageBox.Show("Record Saved!");
                    }
                    else
                    {
                        MessageBox.Show("No records found with the supplier_id: " + textBox5.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding record:" + ex.Message);
                }

                //Close connection
                finally
                {
                    conn.Close();
                }
            }
        }

        // To clear txt in text views
        public void clearText()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox15.Clear();
            textBox16.Clear();
        }

        // Handle button click events for update records
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            selectedRole = comboBox1.SelectedItem.ToString();
            if(selectedRole == "Staff")
            {
                // Check if any of the text fields is null or empty
                if (string.IsNullOrEmpty(textBox15.Text) || string.IsNullOrEmpty(textBox14.Text) ||
                string.IsNullOrEmpty(textBox13.Text) || string.IsNullOrEmpty(textBox16.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return; // Exit the method if any field is empty
                }
                try
                {
                    // Get the selected date from dateTimePicker3 and format it
                    DateTime fdate = dateTimePicker3.Value;
                    string selectedDateTime = fdate.ToString("yyyy-MM-dd");

                    // Establish a database connection
                    conn = new SqlConnection(connstr);
                    conn.Open();

                    // Check if a staff record with the specified staff_id (label27.Text) exists
                    string str1 = "SELECT * FROM staff WHERE staff_id = @StaffID";
                    comm = new SqlCommand(str1, conn);
                    comm.Parameters.Clear();
                    comm.Parameters.AddWithValue("@StaffID", label27.Text);


                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();

                        // Define an update query for the "staff" table
                        string updateQuery = "UPDATE staff SET staff_uname = @StaffUName,";
                        updateQuery += "staff_pass = @StaffPass,";
                        updateQuery += "staff_dob = @StaffDOB,";
                        updateQuery += "staff_phone = @StaffPhone,";
                        updateQuery += "staff_email = @StaffEmail";                                              
                        updateQuery += " WHERE staff_id = @StaffID";

                        // Create a SqlCommand for the update query
                        SqlCommand updateComm = new SqlCommand(updateQuery, conn);
                        comm.Parameters.Clear();

                        // Set parameters for the update query
                        updateComm.Parameters.AddWithValue("@StaffUName", textBox15.Text);
                        updateComm.Parameters.AddWithValue("@StaffPass", textBox14.Text);
                        updateComm.Parameters.AddWithValue("@StaffDOB", selectedDateTime);
                        updateComm.Parameters.AddWithValue("@StaffPhone", textBox13.Text);
                        updateComm.Parameters.AddWithValue("@StaffEmail", textBox16.Text);
                        updateComm.Parameters.AddWithValue("@StaffID", label27.Text);

                        // Execute the update query
                        updateComm.ExecuteNonQuery();

                        // Refresh the DataGridView with the updated data
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM staff where staff_id= @StaffID", conn);
                        da.SelectCommand.Parameters.AddWithValue("@StaffID", label27.Text);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        // Clear input fields
                        clearText();

                        // Display a success message
                        MessageBox.Show("Record Updated!");
                    }
                    else
                    {
                        // If the record is not found, display an error message
                        MessageBox.Show("Record with staff_id " + label27.Text + " not found.");
                    }
                }
                // Exception Handling
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
                // Close connection
                finally
                {
                    conn.Close();
                }
            }
            //Similar with staff
            else if (selectedRole == "Supplier")
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) ||
                string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }
                try
                {
                    DateTime selectedDateTime = dateTimePicker1.Value;
                    conn = new SqlConnection(connstr);
                    conn.Open();

                    string str1 = "SELECT * FROM supplier WHERE supplier_id = @SupplierID";
                    comm = new SqlCommand(str1, conn);
                    comm.Parameters.AddWithValue("@SupplierID", textBox1.Text);

                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        
                        string updateQuery = "UPDATE supplier SET supplier_name = @SupplierName, ";
                        updateQuery += "supplier_phone  = @SupplierPhone, ";
                        updateQuery += "supplier_address  = @SupplierAddress, ";
                        updateQuery += "latestOrder  = @LatestOrder ";
                        updateQuery += " WHERE supplier_id = @SupplierID";

                        SqlCommand updateComm = new SqlCommand(updateQuery, conn);
                        updateComm.Parameters.AddWithValue("@SupplierName", textBox2.Text);
                        updateComm.Parameters.AddWithValue("@SupplierPhone", textBox3.Text);
                        updateComm.Parameters.AddWithValue("@SupplierAddress", textBox4.Text);
                        updateComm.Parameters.AddWithValue("@LatestOrder", selectedDateTime);
                        updateComm.Parameters.AddWithValue("@SupplierID", textBox1.Text);

                        updateComm.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM supplier", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                        clearText();

                        MessageBox.Show("Record Updated!");
                    }
                    else
                    {
                        MessageBox.Show("Record with supplier_id " + textBox1.Text + " not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            //Similar with staff
            else if (selectedRole == "Product")
            {
                if (string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) ||
                string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrEmpty(textBox8.Text) ||
                string.IsNullOrEmpty(textBox9.Text) || string.IsNullOrEmpty(textBox10.Text) ||
                string.IsNullOrEmpty(textBox11.Text) || string.IsNullOrEmpty(textBox12.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return; // Exit the method if any field is empty
                }
                try
                {
                    DateTime selectedDateTime = dateTimePicker2.Value;
                    conn = new SqlConnection(connstr);
                    conn.Open();

                    string str1 = "SELECT * FROM product WHERE product_id = @ProductID";
                    comm = new SqlCommand(str1, conn);
                    comm.Parameters.AddWithValue("@ProductID", textBox5.Text);

                    SqlDataReader reader = comm.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        
                        string updateQuery = "UPDATE product SET product_name = @ProductName, ";
                        updateQuery += "product_category  = @ProductCategory, ";
                        updateQuery += "unitsOrderFromSupplier  = @UnitsOrderFromSupplier, ";
                        updateQuery += "current_quantity  = @CurrentQuantity, ";
                        updateQuery += "sold_quantity  = @SoldQuantity, ";
                        updateQuery += "supplier_price  = @SupplierPrice, ";
                        updateQuery += "store_price  = @StorePrice, ";
                        updateQuery += "latestOrder  = @LatestOrder ";
                        updateQuery += " WHERE product_id = @ProductID";

                        SqlCommand updateComm = new SqlCommand(updateQuery, conn);
                        updateComm.Parameters.AddWithValue("@ProductName", textBox6.Text);
                        updateComm.Parameters.AddWithValue("@ProductCategory", textBox7.Text);
                        updateComm.Parameters.AddWithValue("@UnitsOrderFromSupplier", textBox8.Text);
                        updateComm.Parameters.AddWithValue("@CurrentQuantity", textBox9.Text);
                        updateComm.Parameters.AddWithValue("@SoldQuantity", textBox10.Text);
                        updateComm.Parameters.AddWithValue("@SupplierPrice", textBox11.Text);
                        updateComm.Parameters.AddWithValue("@StorePrice", textBox12.Text);
                        updateComm.Parameters.AddWithValue("@LatestOrder", selectedDateTime);
                        updateComm.Parameters.AddWithValue("@ProductID", textBox5.Text);

                        updateComm.ExecuteNonQuery();

                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM product", conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                        clearText();

                        MessageBox.Show("Record Updated!");
                    }
                    else
                    {
                        MessageBox.Show("Record with product_id " + textBox5.Text + " not found.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        // Handle button click events for deleting records
        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Call the DeleteRecord() method
            if (selectedRole == "Supplier")
            {
                DeleteRecord("supplier", "supplier_id", textBox1.Text);
            }
            else if (selectedRole == "Product")
            {
                DeleteRecord("product", "product_id", textBox5.Text);
            }
        }

        // Handle button click events for deleting records
        private void DeleteRecord(string tableName, string columnName, string textBoxValue)
        {
            try
            {
                // Establish a database connection
                conn = new SqlConnection(connstr);
                conn.Open();

                // Define a SQL command to delete records where a specified column matches a given value
                string sql = $"DELETE FROM {tableName} WHERE {columnName} = @Value";
                comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@Value", textBoxValue);

                // Execute the DELETE command and store the number of affected rows
                int rowsAffected = comm.ExecuteNonQuery();

                // Refresh the DataGridView with the updated data
                SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridView1.DataSource = dt;

                // Check if any rows were deleted
                if (rowsAffected > 0)
                {
                    // ClearText and show success message
                    clearText();
                    MessageBox.Show("Record Deleted!");
                }
                else
                {
                    // Show Error message
                    MessageBox.Show($"No records found with the {columnName}: {textBoxValue}");
                }
            }
            // Exception handling
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message);
            }
            //Close connection
            finally
            {
                conn.Close();
            }
        }

        // Handle button click events for searching records
        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Call the RetrieveRecord() method
            if (selectedRole == "Supplier")
            {
                RetrieveRecord("supplier", "supplier_id", textBox1.Text);
            }
            else if (selectedRole == "Product")
            {
                RetrieveRecord("product", "product_id", textBox5.Text);
            }
        }

        // Handle button click events for searching records
        //Similar with DeleteRecord()
        private void RetrieveRecord(string tableName, string columnName, string textBoxValue)
        {
            try
            {
                conn = new SqlConnection(connstr);
                conn.Open();

                string query = $"SELECT * FROM {tableName} WHERE {columnName} = @Value";
                SqlCommand comm = new SqlCommand(query, conn);
                comm.Parameters.AddWithValue("@Value", textBoxValue);

                SqlDataAdapter da = new SqlDataAdapter(comm);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                    clearText();
                    MessageBox.Show("Record Displayed!");
                }
                else
                {
                    MessageBox.Show($"No records found with the {columnName}: {textBoxValue}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching record: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        // Handle DataGridView cell click events to show on specific text views
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a cell in the DataGridView was clicked
            if (e.RowIndex >= 0)
            {
                selectedRole = comboBox1.SelectedItem.ToString();
                if (selectedRole == "Staff")
                {
                    // Get the selected row in the DataGridView
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Populate text boxes with data from the selected record for the "Supplier" role
                    textBox15.Text = selectedRow.Cells["staff_uname"].Value.ToString();
                    textBox14.Text = selectedRow.Cells["staff_pass"].Value.ToString();
                    textBox13.Text = selectedRow.Cells["staff_phone"].Value.ToString();
                    textBox16.Text = selectedRow.Cells["staff_email"].Value.ToString();
                    string DOB = selectedRow.Cells["staff_dob"].Value.ToString();

                    // Parse and set the date value from the "latestOrder" column to a DateTimePicker
                    if (DateTime.TryParse(DOB, out DateTime parsedDOB))
                    {
                        dateTimePicker3.Value = parsedDOB;
                    }
                    else
                    {
                        MessageBox.Show("Error: Time displayed failed");
                    }
                }
                else if (selectedRole == "Supplier")
                {
                    // Get the selected row in the DataGridView
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    // Populate text boxes with data from the selected record for the "Supplier" role
                    textBox1.Text = selectedRow.Cells["supplier_id"].Value.ToString();
                    textBox2.Text = selectedRow.Cells["supplier_name"].Value.ToString();
                    textBox3.Text = selectedRow.Cells["supplier_phone"].Value.ToString();
                    textBox4.Text = selectedRow.Cells["supplier_address"].Value.ToString();
                    string order = selectedRow.Cells["latestOrder"].Value.ToString();

                    // Parse and set the date value from the "latestOrder" column to a DateTimePicker
                    if (DateTime.TryParse(order, out DateTime parsedOrder))
                    {
                        dateTimePicker1.Value = parsedOrder;
                    }
                    else
                    {
                        MessageBox.Show("Error: Time displayed failed");
                    }
                }
                // Similar with supplier
                else if (selectedRole == "Product")
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                    textBox5.Text = selectedRow.Cells["product_id"].Value.ToString();
                    textBox6.Text = selectedRow.Cells["product_name"].Value.ToString();
                    textBox7.Text = selectedRow.Cells["product_category"].Value.ToString();
                    textBox8.Text = selectedRow.Cells["unitsOrderFromSupplier"].Value.ToString();
                    textBox9.Text = selectedRow.Cells["current_quantity"].Value.ToString();
                    textBox10.Text = selectedRow.Cells["sold_quantity"].Value.ToString();
                    textBox11.Text = selectedRow.Cells["supplier_price"].Value.ToString();
                    textBox12.Text = selectedRow.Cells["store_price"].Value.ToString();
                    string order = selectedRow.Cells["latestOrder"].Value.ToString();

                    if (DateTime.TryParse(order, out DateTime parsedOrder))
                    {
                        dateTimePicker2.Value = parsedOrder;
                    }
                    else
                    {
                        MessageBox.Show("Error: Time displayed failed");
                    }
                }
            }
        }

        // Menu item click events for opening availablility forms
        private void availabilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Availability availability = new Availability();
            availability.Show();
        }

        // Menu item click events for opening sales forms
        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sales sales = new Sales ();
            sales.Show();
        }

        // Menu item click events for opening placeOrder forms
        private void placeOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order order = new Order(staffid);
            order.Show();
        }

        // Menu item click events for opening report forms
        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }

        // Handle button click event to show all records in the DataGridView
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            if (selectedRole == "Supplier")
            {
                ShowRecord("supplier");
            }
            else if (selectedRole == "Product")
            {
                ShowRecord("product");
            }
        }

        // Handle button click event to show all records in the DataGridView
        public void ShowRecord(string tableName)
        {
            SqlDataAdapter da = new SqlDataAdapter($"SELECT * FROM {tableName}", conn);
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

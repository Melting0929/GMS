//CHAN MEI TING_SUKD2101220 & SIM XIN YI_SUKD2101863 (Window Programming 16/10/2023)
// Login Page

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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Grocery_Management_System__Assignment_
{
    public partial class LoginPage : Form
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // Declare SQL connection, command, data reader objects, and connection string
        SqlConnection conn;
        SqlCommand comm;
        SqlDataReader dr;
        String connstr = @"Data Source=LAPTOP-FINCDPMD;Initial Catalog=ChanMeiTIng;Integrated Security=True";
        String selectedRole;//declare as global variable to use in combo box

        // Handle the event when the role is selected from a ComboBox
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected role from the ComboBox
            selectedRole = comboBox1.SelectedItem.ToString();
            // Show or hide panels based on the selected role and call clearText()
            if (selectedRole == "Admin")
            {
                panel1.Visible = true;
                panel2.Visible = false;
                ClearText();
            }
            else if (selectedRole == "Staff")
            {
                panel1.Visible = false;
                panel2.Visible = true;
                ClearText();
            }
        }

        // Handle the event when a button is clicked for user login
        private void button1_Click(object sender, EventArgs e)
         {
            // Create a SQL connection and open it.
            conn = new SqlConnection(connstr);
            conn.Open();

            // Check the selected role and perform user authentication
            if (selectedRole == "Admin")
            {
                try
                {
                    // Check if username and password fields are not empty
                    if (textBox1.Text != string.Empty || textBox2.Text != string.Empty)
                    {
                        // Create a SQL command to query the admin table with query
                        comm = new SqlCommand("select * from admin where admin_id='" + textBox1.Text +
                            "' and admin_pass='" + textBox2.Text + "'", conn);
                        dr = comm.ExecuteReader();

                        // If a record is found, hide the current form and open the AdminPage
                        if (dr.Read())
                        {
                            dr.Close();
                            this.Hide();
                            AdminPage adminPage = new AdminPage(textBox1.Text);
                            adminPage.ShowDialog();
                            Application.Exit();
                        }
                        else
                        {
                            dr.Close();
                            ClearText();
                            // Display an error message for invalid input
                            MessageBox.Show("No Account avilable with this username and password ", 
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        // Display an error message if empty fields
                        MessageBox.Show("Please enter value in all field.", 
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }catch (Exception ex)
                {
                    //Exception handling
                    MessageBox.Show("Error: " + ex.Message);
                }
            }else if (selectedRole == "Staff")
                {
                    try
                    {
                        // Check if username and password fields are not empty
                        if (textBox4.Text != string.Empty || textBox3.Text != string.Empty)
                        {
                            // Create a SQL command to query the admin table with query
                            comm = new SqlCommand("select * from staff where staff_id='" + textBox4.Text +
                                "' and staff_pass='" + textBox3.Text + "'", conn);
                            dr = comm.ExecuteReader();

                            // If a record is found, hide the current form and open the StaffPage
                            if (dr.Read())
                            {
                                dr.Close();
                                this.Hide();

                                StaffPage staffPage = new StaffPage(textBox4.Text);
                                staffPage.ShowDialog();
                                Application.Exit();
                            }
                            else
                            {
                                dr.Close();
                                ClearText();
                                // Display an error message for invalid input
                                MessageBox.Show("No Account avilable with this username and password ",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            // Display an error message if empty fields
                            MessageBox.Show("Please enter value in all field.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Exception handling
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }

        //To clear text box
        public void ClearText()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }
    }
    }
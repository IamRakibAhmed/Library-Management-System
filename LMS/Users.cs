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
using MySql.Data.MySqlClient;

namespace LMS
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            populate();
        }

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1483;database=lms-c#");

        private void populate()
        {
            connection.Open();
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserDataGridView.DataSource = ds.Tables[0];
            connection.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (userNameBox.Text == "" || phoneBox.Text == "" || passwordBox.Text == "" || addressBox.Text == "")
            {
                MessageBox.Show("Informations Are Missing!");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "insert into usertable(user_name, user_phone, user_address, user_password, entry_date) values('" + userNameBox.Text + "','" + phoneBox.Text + "','" + addressBox.Text + "','" + passwordBox.Text + "','" + dateTimePicker.Text + "');";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    populate();
                    reset();
                    MessageBox.Show("Manager Added Succeessfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        int key = 0;
        private void UserDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            userNameBox.Text = UserDataGridView[1, UserDataGridView.CurrentRow.Index].Value.ToString();
            phoneBox.Text = UserDataGridView[2, UserDataGridView.CurrentRow.Index].Value.ToString();
            addressBox.Text = UserDataGridView[3, UserDataGridView.CurrentRow.Index].Value.ToString();
            passwordBox.Text = UserDataGridView[4, UserDataGridView.CurrentRow.Index].Value.ToString();
            //dateTimePicker.Text = (string)UserDataGridView[5, UserDataGridView.CurrentRow.Index].Value;

            if (userNameBox.Text != "")
                key = Convert.ToInt32(UserDataGridView[0, UserDataGridView.CurrentRow.Index].Value.ToString());
        }

        private void reset()
        {
            userNameBox.Text = "";
            phoneBox.Text = "";
            passwordBox.Text = "";
            addressBox.Text = "";
            dateTimePicker.Value = DateTime.Today;
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            reset();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (key == 0) MessageBox.Show("No item selected!");
            else
            {
                try
                {
                    connection.Open();
                    string query = "delete from usertable where user_ID=" + key + ";";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    populate();
                    reset();
                    MessageBox.Show("User Deleted Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (userNameBox.Text == "" || phoneBox.Text == "" || addressBox.Text == "" || passwordBox.Text == "")
            {
                MessageBox.Show("Informations Are Missing!");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "update usertable set user_name='" + userNameBox.Text + "',user_phone='" + phoneBox.Text + "',user_address='" + addressBox.Text + "',user_password='" + passwordBox.Text + "',entry_date='" + dateTimePicker.Text + "' where user_ID=" + key + "";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    populate();
                    reset();
                    MessageBox.Show("Manager Updated Succeessfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void searchByUserName()
        {
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable where user_name='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            UserDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByPhoneNumber()
        {
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable where user_phone='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            UserDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByUserID()
        {
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable where user_ID='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            UserDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByUserAddress()
        {
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable where user_address='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            UserDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByEntryDate()
        {
            string query = "select user_ID as 'User ID', user_name as 'User Name', user_phone as 'Phone Number', user_address as 'Address', user_password as 'Password', entry_date as 'Entry Date' from usertable where entry_date='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            UserDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchFromTable()
        {
            string searchValue = getSearchValue();
            if (searchValue == "Search by User Name") searchByUserName();
            else if (searchValue == "Search by Phone Number") searchByPhoneNumber();
            else if (searchValue == "Search by User ID") searchByUserID();
            else if (searchValue == "Search by Address") searchByUserAddress();
            else if (searchValue == "Search by Entry Date") searchByEntryDate();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            searchFromTable();
        }

        string searchItem = "";
        private void searchSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchItem = searchSelector.Text;
        }

        private string getSearchValue()
        {
            return searchItem;
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginLabel_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}

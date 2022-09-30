using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace LMS
{
    public partial class Books : Form
    {
        public Books()
        {
            InitializeComponent();
            populate();
        }

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1483;database=lms-c#");

        private void populate()
        {
            connection.Open();
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDataGridView.DataSource = ds.Tables[0];
            connection.Close();
        }

        private void filterByCategory()
        {
            connection.Open();
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where book_category = '" + categoryFilterBox.SelectedItem.ToString() + "'";
            MySqlDataAdapter sda = new MySqlDataAdapter(query, connection);
            MySqlCommandBuilder builder = new MySqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookDataGridView.DataSource = ds.Tables[0];
            connection.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (bookTitleBox.Text == "" || bookAuthorBox.Text == "" || bookQuantityBox.Text == "" || bookPriceBox.Text == "" || bookPriceBox.Text == "0" || bookCategoryBox.SelectedIndex == -1 || publishYearBox.Text == "")
            {
                if (bookQuantityBox.Text == "0") MessageBox.Show("Quantity Can't be 0");
                else MessageBox.Show("Informations Are Missing!");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "insert into booktable(book_title, book_author, publish_year, book_category, book_qty, book_price, entry_date) values('" + bookTitleBox.Text + "','" + bookAuthorBox.Text + "'," + publishYearBox.Text + ",'" + bookCategoryBox.SelectedItem.ToString() + "'," + bookQuantityBox.Text + "," + bookPriceBox.Text + ",'" + dateTimePicker.Text + "')";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    populate();
                    reset();
                    MessageBox.Show("Book Saved Succeessfully!");
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void categoryFilterBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            filterByCategory();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            populate();
            categoryFilterBox.SelectedIndex = -1;
            categoryFilterBox.Text = "Filter by Category";
        }

        private void categoryFilterBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoryFilterBox.SelectedIndex != -1) filterByCategory();
        }

        private void reset()
        {
            bookTitleBox.Text = "";
            bookAuthorBox.Text = "";
            publishYearBox.Text = "";
            bookCategoryBox.SelectedIndex = -1;
            bookCategoryBox.Text = "Select Category";
            bookPriceBox.Text = "";
            bookQuantityBox.Text = "";
            dateTimePicker.Value = DateTime.Today;
        }

        int key = 0;
        private void BookDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bookTitleBox.Text = BookDataGridView[1, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookAuthorBox.Text = BookDataGridView[2, BookDataGridView.CurrentRow.Index].Value.ToString();
            publishYearBox.Text = BookDataGridView[3, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookCategoryBox.SelectedItem = BookDataGridView[4, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookQuantityBox.Text = BookDataGridView[5, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookPriceBox.Text = BookDataGridView[6, BookDataGridView.CurrentRow.Index].Value.ToString();
            //dateTimePicker.Text = BookDataGridView[7, BookDataGridView.CurrentRow.Index].Value.ToString();

            if (bookTitleBox.Text != "")
                key = Convert.ToInt32(BookDataGridView[0, BookDataGridView.CurrentRow.Index].Value.ToString());
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
                    string query = "delete from booktable where book_ID=" + key + ";";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    if (categoryFilterBox.Text != "Filter by Category")
                    {
                        string cat = categoryFilterBox.SelectedItem.ToString();
                        categoryFilterBox.Text = cat;
                        filterByCategory();
                    }
                    else populate();
                    reset();
                    MessageBox.Show("Book Deleted Successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void searchByBookTitle()
        {
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where book_title='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            BookDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByBookAuthor()
        {
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where book_author='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            BookDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByBookID()
        {
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where book_ID='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            BookDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByPublishYear()
        {
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where publish_year='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            BookDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchByEntryDate()
        {
            string query = "select book_ID as 'Book ID', book_title as 'Book Title', book_author as 'Author', publish_year as 'Publish Year', book_category as 'Category', book_qty as 'Quantity', book_price as 'Price', entry_date as 'Entry Date' from booktable where entry_date='" + searchBox.Text + "'";
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter sdr = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sdr.Fill(dt);
            BookDataGridView.DataSource = dt;
            connection.Close();
            if (searchBox.Text == "") populate();
        }

        private void searchFromTable()
        {
            string searchValue = getSearchValue();
            if (searchValue == "Search by Book Title") searchByBookTitle();
            else if (searchValue == "Search by Book Author") searchByBookAuthor();
            else if (searchValue == "Search by Book ID") searchByBookID();
            else if (searchValue == "Search by Publish Year") searchByPublishYear();
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

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (bookTitleBox.Text == "" || bookAuthorBox.Text == "" || bookQuantityBox.Text == "" || bookPriceBox.Text == "" || bookPriceBox.Text == "0" || bookCategoryBox.SelectedIndex == -1 || publishYearBox.Text == "")
            {
                if (bookQuantityBox.Text == "0") MessageBox.Show("Quantity Can't be 0");
                else MessageBox.Show("Informations Are Missing!");
            }
            else
            {
                try
                {
                    connection.Open();
                    string query = "update booktable set book_title='" + bookTitleBox.Text + "',book_author='" + bookAuthorBox.Text + "',publish_year=" + publishYearBox.Text + ",book_category='" + bookCategoryBox.SelectedItem.ToString() + "',book_qty=" + bookQuantityBox.Text + ",book_price=" + bookPriceBox.Text + ",entry_date='" + dateTimePicker.Text +"' where book_ID=" + key + ";";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    populate();
                    reset();
                    MessageBox.Show("Book Updated Succeessfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Books_Load(object sender, EventArgs e)
        {
            userNameLabel.Text = Login.userName;
        }

        private void LoginLabel_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void billingLabel_Click(object sender, EventArgs e)
        {
            Billing obj = new Billing();
            obj.Show();
            this.Hide();
        }

        private void dashboardLabel_Click(object sender, EventArgs e)
        {
            Dashboard obj = new Dashboard();
            obj.Show();
            this.Hide();
        }

        private void aboutBox_Click(object sender, EventArgs e)
        {
            About obj = new About();
            obj.ShowDialog();
        }
    }
}

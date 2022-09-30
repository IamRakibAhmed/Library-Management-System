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
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
            populate();
        }

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1483;database=lms-c#");

        private int key = 0, stock = 0, n = 0, invoice_tot = 0;
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

        private void updateBook()
        {
            stock -= Convert.ToInt32(bookQuantityBox.Text);
            try
            {
                connection.Open();
                string query = "update booktable set book_qty=" + stock + " where book_ID=" + key + ";";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                populate();
                //reset();
                //MessageBox.Show("Book Updated Succeessfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addToBillBtn_Click(object sender, EventArgs e)
        {
            if (bookQuantityBox.Text == "")
            {
                MessageBox.Show("Quantity can't be Empty!");
            }
            else if (Convert.ToInt32(bookQuantityBox.Text) > stock)
            {
                MessageBox.Show("Not Enough Stock Available!");
            }
            else
            {
                int total = Convert.ToInt32(bookQuantityBox.Text) * Convert.ToInt32(bookPriceBox.Text);
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(BillDataGridView);
                row.Cells[0].Value = n + 1;
                row.Cells[1].Value = bookTitleBox.Text;
                row.Cells[2].Value = bookPriceBox.Text;
                row.Cells[3].Value = bookQuantityBox.Text;
                row.Cells[4].Value = total;
                BillDataGridView.Rows.Add(row);
                n++;

                updateBook();
                invoice_tot += total;
                totalBillLabel.Text = "Total:   $" + invoice_tot;
            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (customerNameBox.Text == "" || bookTitleBox.Text == "" || customerPhoneBox.Text == "" || customerEmailBox.Text == "")
            {
                MessageBox.Show("Informations Are Missing!");
            }
            else
            {
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("pprnm", 550, 600);
                if (printPreviewDialog1.ShowDialog() == DialogResult.OK) printDocument1.Print();

                try
                {
                    connection.Open();
                    string query = "insert into billtable(user_name, customer_name, customer_phone, customer_email, amount) values('" + userNameLabel.Text + "','" + customerNameBox.Text + "'," + customerPhoneBox.Text + ",'" + customerEmailBox.Text + "'," + invoice_tot + ")";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    MessageBox.Show("Bill Saved Succeessfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private int productID, productQTY, productPrice, tot, pos = 60;

        private void loginLabel_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            Books obj = new Books();
            obj.Show();
            this.Hide();
        }

        private void dashboardLabel_Click(object sender, EventArgs e)
        {
            Dashboard obj = new Dashboard();
            obj.Show();
            this.Hide();
        }

        private void aboutLabel_Click(object sender, EventArgs e)
        {
            About obj = new About();
            obj.ShowDialog();
        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            userNameLabel.Text = Login.userName;
        }

        string productName;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("Invoice", new Font("Poppins", 12, FontStyle.Bold), Brushes.Red, new Point(250));
            e.Graphics.DrawString("ID       Product     Price    Quantity    Total", new Font("Poppins", 10, FontStyle.Bold), Brushes.Red, new Point(26, 40));
            foreach (DataGridViewRow row in BillDataGridView.Rows)
            {
                productID = Convert.ToInt32(row.Cells["Column1"].Value);
                productName = "" + row.Cells["Column2"].Value;
                productPrice = Convert.ToInt32(row.Cells["Column3"].Value);
                productQTY = Convert.ToInt32(row.Cells["Column4"].Value);
                tot = Convert.ToInt32(row.Cells["Column5"].Value);
                e.Graphics.DrawString("" + productID, new Font("Poppins", 8, FontStyle.Bold), Brushes.Blue, new Point(29, pos));
                e.Graphics.DrawString("" + productName, new Font("Poppins", 8, FontStyle.Bold), Brushes.Blue, new Point(58, pos));
                e.Graphics.DrawString("" + productPrice, new Font("Poppins", 8, FontStyle.Bold), Brushes.Blue, new Point(143, pos));
                e.Graphics.DrawString("" + productQTY, new Font("Poppins", 8, FontStyle.Bold), Brushes.Blue, new Point(210, pos));
                e.Graphics.DrawString("" + tot, new Font("Poppins", 8, FontStyle.Bold), Brushes.Blue, new Point(270, pos));
                pos += 20;
            }
            e.Graphics.DrawString("Grand Total: " + invoice_tot + " $", new Font("Poppins", 12, FontStyle.Bold), Brushes.Crimson, new Point(26, pos + 50));
            e.Graphics.DrawString("***********Library Book Store Management System***********", new Font("Poppins", 10, FontStyle.Bold), Brushes.Crimson, new Point(40, pos + 85));
            BillDataGridView.Rows.Clear();
            BillDataGridView.Refresh();
            pos = 100;
            //invoice_tot = 0;
        }

        private void BookDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bookTitleBox.Text = BookDataGridView[1, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookAuthorBox.Text = BookDataGridView[2, BookDataGridView.CurrentRow.Index].Value.ToString();
            //bookQuantityBox.Text = BookDataGridView[5, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookCategoryBox.SelectedItem = BookDataGridView[4, BookDataGridView.CurrentRow.Index].Value.ToString();
            bookPriceBox.Text = BookDataGridView[6, BookDataGridView.CurrentRow.Index].Value.ToString();

            if (bookTitleBox.Text != "")
            {
                key = Convert.ToInt32(BookDataGridView[0, BookDataGridView.CurrentRow.Index].Value.ToString());
                stock = Convert.ToInt32(BookDataGridView[5, BookDataGridView.CurrentRow.Index].Value.ToString());
            }
        }

        private void reset()
        {
            bookTitleBox.Text = "";
            bookAuthorBox.Text = "";
            bookQuantityBox.Text = "";
            bookPriceBox.Text = "";
            bookCategoryBox.Text = "Select Category";
            dateTimePicker.Value = DateTime.Today;
            customerNameBox.Text = "";
            customerPhoneBox.Text = "";
            customerEmailBox.Text = "";
        }
        private void resetBtn_Click(object sender, EventArgs e)
        {
            reset();
        }
    }
}

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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1483;database=lms-c#");

        private void Dashboard_Load(object sender, EventArgs e)
        {
            userNameLabel.Text = Login.userName;
            connection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("select sum(book_qty) from booktable", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            bookStockLabel.Text = dt.Rows[0][0].ToString();

            MySqlDataAdapter sda1 = new MySqlDataAdapter("select sum(amount) from billtable", connection);
            DataTable dt1 = new DataTable();
            sda1.Fill(dt1);
            totalAmountLabel.Text = dt1.Rows[0][0].ToString();

            MySqlDataAdapter sda2 = new MySqlDataAdapter("select count(*) from usertable", connection);
            DataTable dt2 = new DataTable();
            sda2.Fill(dt2);
            managersTotalLabel.Text = dt2.Rows[0][0].ToString();

            connection.Close();
        }

        private void booksLabel_Click(object sender, EventArgs e)
        {
            Books obj = new Books();
            obj.Show();
            this.Hide();
        }

        private void billingLabel_Click(object sender, EventArgs e)
        {
            Billing obj = new Billing();
            obj.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logoutLabel_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void aboutLabel_Click(object sender, EventArgs e)
        {
            About obj = new About();
            obj.ShowDialog();
        }
    }
}

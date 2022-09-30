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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;username=root;password=1483;database=lms-c#");

        public static string userName = "";
        private void loginBtn_Click(object sender, EventArgs e)
        {
            connection.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("select count(*) from usertable where user_name='" + userNameBox.Text + "' and user_password='" + passwordBox.Text + "'", connection);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows[0][0].ToString() == "1")
            {
                userName = userNameBox.Text;
                Books obj = new Books();
                obj.Show();
                this.Hide();
                connection.Close();
            }
            else MessageBox.Show("Wrong UserName or Password");
            connection.Close();
        }

        private void AdminLoginbutton_Click(object sender, EventArgs e)
        {
            AdminLogin obj = new AdminLogin();
            obj.Show();
            this.Hide();
        }

        private void hideBtn_Click(object sender, EventArgs e)
        {
            if (passwordBox.PasswordChar == '\0')
            {
                unhideBtn.BringToFront();
                passwordBox.PasswordChar = '*';
            }
        }

        private void unhideBtn_Click(object sender, EventArgs e)
        {
            if (passwordBox.PasswordChar == '*')
            {
                hideBtn.BringToFront();
                passwordBox.PasswordChar = '\0';
            }
        }
    }
}

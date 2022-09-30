using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LMS
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            if (userNameBox.Text == "admin" && passwordBox.Text == "admin")
            {
                Users obj = new Users();
                obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Wrong UserName or Password!");
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void unhideBtn_Click(object sender, EventArgs e)
        {
            if (passwordBox.PasswordChar == '*')
            {
                hideBtn.BringToFront();
                passwordBox.PasswordChar = '\0';
            }
        }

        private void hideBtn_Click(object sender, EventArgs e)
        {
            if (passwordBox.PasswordChar == '\0')
            {
                unhideBtn.BringToFront();
                passwordBox.PasswordChar = '*';
            }
        }
    }
}

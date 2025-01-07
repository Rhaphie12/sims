using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Users
{
    public partial class New_Staff : Form
    {
        public New_Staff()
        {
            InitializeComponent();
            ConfirmPasswordTextBox();
        }
        private void ConfirmPasswordTextBox()
        {
            passwordTxt.PlaceholderText = "Password";
            passwordTxt.PasswordChar = '\0';
            passwordTxt.TextChanged += (sender, e) =>
            {
                string enteredPassword = passwordTxt.Text;
                if (string.IsNullOrEmpty(enteredPassword))
                {
                    passwordTxt.PlaceholderText = "Password";
                    passwordTxt.PasswordChar = '\0';
                }
                else
                {
                    passwordTxt.PlaceholderText = "";
                    passwordTxt.PasswordChar = '●';
                }
            };
        }
        private void New_Staff_Load(object sender, EventArgs e)
        {
            userRoleCmb.Items.Clear();
            userRoleCmb.Items.Add("Select user role");
            userRoleCmb.Items.Add("Owner");
            userRoleCmb.Items.Add("Staff");
            userRoleCmb.SelectedIndex = 0;

        }

        private void backStaffBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void showPasswordChk_OnChange(object sender, EventArgs e)
        {
            if (showPasswordChk.Checked)
            {
                passwordTxt.PasswordChar = '\0';
            }
            else
            {
                passwordTxt.PasswordChar = '●';
            }
        }
    }
}

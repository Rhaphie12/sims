using MySql.Data.MySqlClient;
using sims.Admin_Side.Items;
using sims.Messages_Boxes;
using sims.Staff_Side;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims
{
    public partial class Login_Form : Form
    {
        public Login_Form()
        {
            InitializeComponent();
            ConfirmPasswordTextBox();
            showPasswordChk.OnChange += new EventHandler(showPasswordChk_OnChange);
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

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            dbModule db = new dbModule();
            string username = usernameTxt.Text.Trim();
            string password = passwordTxt.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                usernameTxt.Focus();
                new Field_Required().Show();
                usernameTxt.Clear();
                passwordTxt.Clear();
                return;
            }

            string userQuery = "SELECT COUNT(*) FROM users WHERE BINARY username = @Username AND BINARY password = @Password";
            string staffQuery = "SELECT COUNT(*) FROM staff WHERE BINARY username = @Username AND BINARY password = @Password";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(userQuery, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Username", usernameTxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@Password", passwordTxt.Text.Trim());

                        int isOwner = Convert.ToInt32(cmd.ExecuteScalar());

                        if (isOwner > 0)
                        {
                            new ownerLogin().Show();
                            this.Hide();
                        }
                        else
                        {
                            cmd.CommandText = staffQuery;
                            int isStaff = Convert.ToInt32(cmd.ExecuteScalar());

                            if (isStaff > 0)
                            {
                                new Staff_Login().Show();
                                this.Hide();
                            }
                            else
                            {
                                usernameTxt.Focus();
                                new Invalid_Account().Show();
                                usernameTxt.Clear();
                                passwordTxt.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void forgotPasswordLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Forgot_Password.Forgot_Password().Show();
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

using MySql.Data.MySqlClient;
using sims.Messages_Boxes;
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

        public void Login()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

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

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT username, password FROM users WHERE BINARY username = @uname AND BINARY password = @password LIMIT 1";
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@uname", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        usernameTxt.Clear();
                        passwordTxt.Clear();
                        this.Hide();
                        new ownerLogin().Show();
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Invalid Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.UI.WinForms;
using MySql.Data.MySqlClient;

namespace sims.Forgot_Password
{
    public partial class Forgot_Password : Form
    {
        public Forgot_Password()
        {
            InitializeComponent();
        }

        public BunifuTextBox UsernameTxt
        {
            get { return usernameTxt; }
        }

        private void ContinueBtn_Click(object sender, EventArgs e)
        {
            string Username = usernameTxt.Text.Trim();

            if (string.IsNullOrEmpty(Username))
            {
                new Messages_Boxes.Forgot_Password_msgbox.UsernameRequired().Show();
                return;
            }

            if (CheckUsername(Username))
            {
                //MessageBox.Show("Username found in the database.", "Check Username", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new Messages_Boxes.Forgot_Password_msgbox.Username_Exists(this).Show();
                this.Hide();
            }
            else
            {
                //MessageBox.Show("Username not found in the database.");
                usernameTxt.Focus();
                new Messages_Boxes.Forgot_Password_msgbox.UsernameNotFound().Show();
                usernameTxt.Clear();
            }
        }
        private bool CheckUsername(string username)
        {
            dbModule db = new dbModule();
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM users WHERE BINARY username = @username";
                using (MySqlCommand command = new MySqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private void BackToSigninLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Login_Form().Show();
        }
    }
}

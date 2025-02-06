using MySql.Data.MySqlClient;
using sims.Messages_Boxes;
using System;
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

            usernameTxt.KeyDown += new KeyEventHandler(OnEnterKeyPress);
            passwordTxt.KeyDown += new KeyEventHandler(OnEnterKeyPress);
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

        private void OnEnterKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevents the 'ding' sound
                Login(); // Call the login method
            }
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

            string userQuery = "SELECT Staff_Name FROM users WHERE BINARY username = @Username AND BINARY password = @Password";
            string staffQuery = "SELECT Staff_Name, Action FROM staff WHERE BINARY username = @Username AND BINARY password = @Password";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    using (MySqlCommand cmd = new MySqlCommand(userQuery, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);

                        // Check if the user is an owner
                        string ownerName = cmd.ExecuteScalar()?.ToString();

                        if (!string.IsNullOrEmpty(ownerName))
                        {
                            // Log owner activity
                            AddLoginActivity(ownerName, "Owner");
                            new ownerLogin().Show();
                            this.Hide();
                        }
                        else
                        {
                            using (MySqlDataReader staffReader = cmd.ExecuteReader())
                            {
                                if (staffReader.Read())
                                {
                                    string staffName = staffReader["Staff_Name"].ToString();
                                    string accountStatus = staffReader["Action"].ToString();

                                    if (accountStatus == "Inactive")
                                    {
                                        MessageBox.Show("This account is inactive. Please contact the administrator.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }

                                    // Log staff activity
                                    AddLoginActivity(staffName, "Staff");
                                    new Staff_Login().Show();
                                    this.Hide();
                                    return;
                                }
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

        private void AddLoginActivity(string staffName, string role)
        {
            dbModule db = new dbModule();

            string query = "INSERT INTO activitylogs (Staff_Name, Role, Activity, Date_Logged_In) VALUES (@StaffName, @Role, @Activity, @DateLoggedIn)";

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffName", staffName);
                        cmd.Parameters.AddWithValue("@Role", role);
                        cmd.Parameters.AddWithValue("@Activity", "Logged In");
                        cmd.Parameters.AddWithValue("@DateLoggedIn", DateTime.Now);

                        cmd.ExecuteNonQuery();
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
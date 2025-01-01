using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Category
{
    public partial class Edit_Category : Form
    {
        private string _categoryID;
        private Manage_Category dashboardForm;
        private Manage_Category flow;
        private int categoryID;
        private string categoryName;
        private Manage_Category manage_Category;

        public Edit_Category(string categoryID, Manage_Category dashboardForm, Manage_Category flow)
        {
            InitializeComponent();
            _categoryID = categoryID;
            this.dashboardForm = dashboardForm;
            this.flow = flow;
        }

        public Edit_Category(int categoryID, string categoryName, Manage_Category manage_Category)
        {
            this.categoryID = categoryID;
            this.categoryName = categoryName;
            this.manage_Category = manage_Category;
        }

        private void Edit_Category_Load(object sender, EventArgs e)
        {
            Populate();
            LoadProductDetails(_categoryID);
        }

        private void Populate()
        {
            dbModule db = new dbModule();
            MySqlDataAdapter adapter = db.GetAdapter();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM categories";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dashboardForm.RecentlyAddedDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadProductDetails(string itemID)
        {
            dbModule db = new dbModule();
            string query = "SELECT Category_ID, Category_Name " +
                           "FROM items WHERE Category_ID = @Category_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Category_ID", _categoryID);
                    try
                    {
                        conn.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Fetch textual data
                                categoryIDTxt.Text = reader["Category_ID"].ToString();
                                categoryNameTxt.Text = reader["Category_Name"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("Category not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while fetching categgories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void backNewCatBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}

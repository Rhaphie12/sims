using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using sims.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace sims.Admin_Side.Category
{
    public partial class New_Category : Form
    {
        private Manage_Category dashboardForm;
        private Manage_Category flow;
        private Inventory_Dashboard countCategory;

        public New_Category(Manage_Category dashboardForm, Manage_Category flow, Inventory_Dashboard countCategory)
        {
            InitializeComponent();
            this.dashboardForm = dashboardForm;
            this.flow = flow;
            this.countCategory = countCategory;
        }

        public class Categories
        {
            public int CategoryID { get; set; }
            public string Category { get; set; }
        }

        public void Alert(string msg)
        {
            Category_Added frm = new Category_Added();
            frm.showalert(msg);
        }

        private void New_Category_Load(object sender, EventArgs e)
        {
            LoadItemsPanel();
            GenerateRandomItemID();
            CategoriesCount();
        }

        private void GenerateRandomItemID()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            categoryIDTxt.Text = randomNumber.ToString();
        }

        private void Populate()
        {
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string queryCategories = "SELECT * FROM categories";
                    using (MySqlCommand commandCategories = new MySqlCommand(queryCategories, conn))
                    using (MySqlDataAdapter adapterCategories = new MySqlDataAdapter(commandCategories))
                    {
                        DataTable dtCategories = new DataTable();
                        adapterCategories.Fill(dtCategories);
                        dashboardForm.RecentlyAddedDgv.DataSource = dtCategories;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CategoriesCount()
        {
            dbModule db = new dbModule();
            string query = "SELECT COUNT(*) FROM categories";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                        countCategory.CategoriesCountLabel.Text = itemCount.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            addCategory();
        }

        private void addCategory()
        {
            dbModule db = new dbModule();

            string categoryID = categoryIDTxt.Text.Trim();
            string categoryName = categoryNameTxt.Text.Trim();
            string categoryDescription = categoryDescriptionTxt.Text.Trim();

            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(categoryDescription))
            {
                new Messages_Boxes.Field_Required().Show();
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO categories (Category_ID, Category_Name, Category_Description) VALUES (@Category_ID, @Category_Name, @Category_Description)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category_ID", categoryID);
                        cmd.Parameters.AddWithValue("@Category_Name", categoryName);
                        cmd.Parameters.AddWithValue("@Category_Description", categoryDescription);
                        cmd.ExecuteNonQuery();
                        this.Hide();
                        categoryNameTxt.Clear();
                        categoryDescriptionTxt.Clear();
                        CategoriesCount();
                        GenerateRandomItemID();
                        Populate();
                        AddItemPanel(categoryName);
                        this.Alert("Category Added Successfully");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void backNewCatBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void LoadItemsPanel()
        {
            dbModule db = new dbModule();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Category_Name FROM categories";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        flow.CategoriesPanel.Controls.Clear();

                        while (reader.Read())
                        {
                            string category = reader.GetString("Category_Name");
                            AddItemPanel(category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddItemPanel(string category)
        {
            GunaElipsePanel productPanel = new GunaElipsePanel
            {
                Width = 395,
                Height = 100,
                Radius = 8,
                BackColor = Color.FromArgb(222, 196, 125),
                Tag = new Categories
                {
                    Category = category
                }
            };

            Label categoryLabel = new Label
            {
                Text = category,
                Font = new Font("Poppins", 14),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false
            };

            productPanel.Controls.Add(categoryLabel);
            flow.CategoriesPanel.Controls.Add(productPanel);
        }

        private void categoryNameTxt_TextChanged(object sender, EventArgs e)
        {
            string newText = categoryNameTxt.Text;

            if (System.Text.RegularExpressions.Regex.IsMatch(newText, @"\d"))
            {
                MessageBox.Show("Numbers are not allowed!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                categoryNameTxt.Text = System.Text.RegularExpressions.Regex.Replace(newText, @"\d", "");
                categoryNameTxt.SelectionStart = categoryNameTxt.Text.Length;
            }
        }
    }
}

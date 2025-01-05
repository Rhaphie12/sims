using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
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

        public New_Category(Manage_Category dashboardForm, Manage_Category flow)
        {
            InitializeComponent();
            this.dashboardForm = dashboardForm;
            this.flow = flow;
        }

        public class Categories
        {
            public int CategoryID { get; set; }
            public string Category { get; set; }
        }

        private void New_Category_Load(object sender, EventArgs e)
        {
            LoadItemsPanel();
            GenerateRandomItemID();
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
                    string queryCategories = "SELECT Category_ID, Category_Name FROM categories";
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

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            addCategory();
        }

        private void addCategory()
        {
            dbModule db = new dbModule();

            string categoryID = categoryIDTxt.Text.Trim();
            string categoryName = categoryNameTxt.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                //MessageBox.Show("Category name are required!",
                //                "Information Required",
                //                MessageBoxButtons.OK,
                //                MessageBoxIcon.Warning);
                new Messages_Boxes.Field_Required().Show();
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO categories (Category_ID, Category_Name) VALUES (@Category_ID, @Category_Name)";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@Category_ID", categoryID);
                        command.Parameters.AddWithValue("@Category_Name", categoryName);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Category added successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        categoryNameTxt.Clear();
                        GenerateRandomItemID();
                        Populate();
                        AddItemPanel(categoryName);
                        categoryNameTxt.Focus();
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
    }
}

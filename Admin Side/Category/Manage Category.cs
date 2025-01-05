using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static sims.Admin_Side.Category.New_Category;

namespace sims.Admin_Side.Category
{
    public partial class Manage_Category : UserControl
    {
        private DataTable itemsTable;
        private BindingSource itemsBindingSource = new BindingSource();

        public Manage_Category()
        {
            InitializeComponent();
        }

        public DataGridView RecentlyAddedDgv
        {
            get { return recentlyAddedDgv; }
        }

        public FlowLayoutPanel CategoriesPanel
        {
            get { return categoriesPanel; }
        }

        private void newCategoryBtn_Click(object sender, EventArgs e)
        {
            New_Category newCategory = new New_Category(this, this);
            newCategory.Show();
        }

        private void Manage_Category_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadItemsPanel();
        }

        private void LoadData()
        {
            try
            {
                dbModule db = new dbModule();
                string query = "SELECT Category_ID, Category_Name FROM categories";

                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            recentlyAddedDgv.DataSource = dataTable;
                            recentlyAddedDgv.ClearSelection();
                            recentlyAddedDgv.CurrentCell = null;

                            categoriesPanel.Controls.Clear();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                int categoryID = Convert.ToInt32(row["Category_ID"]);
                                string categoryName = row["Category_Name"].ToString();
                                AddItemPanel(categoryID, categoryName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter(DataTable dataTable, BindingSource bindingSource, string searchText)
        {
            if (dataTable != null)
            {
                try
                {
                    if (string.IsNullOrEmpty(searchText))
                    {
                        bindingSource.RemoveFilter();
                    }
                    else
                    {
                        string filterExpression = string.Join(" OR ", dataTable.Columns
                            .OfType<DataColumn>()
                            .Select(column => $"CONVERT([{column.ColumnName}], 'System.String') LIKE '%{searchText}%'"));

                        bindingSource.Filter = filterExpression;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error filtering data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void searchCategoryTxt_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchCategoryTxt.Text.Trim();
            ApplyFilter(itemsTable, itemsBindingSource, searchText);
        }

        private void LoadItemsPanel()
        {
            dbModule db = new dbModule();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Category_ID, Category_Name FROM categories";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        categoriesPanel.Controls.Clear();

                        while (reader.Read())
                        {
                            int categoryID = reader.GetInt32("Category_ID");
                            string category = reader.GetString("Category_Name");
                            AddItemPanel(categoryID, category);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddItemPanel(int categoryID, string category)
        {
            GunaElipsePanel productPanel = new GunaElipsePanel
            {
                Width = 280,
                Height = 100,
                Radius = 8,
                BackColor = Color.FromArgb(222, 196, 125),
                Tag = new Categories
                {
                    CategoryID = categoryID,
                    Category = category
                },
                Margin = new Padding(73, 3, 3, 3) // Adds a left margin of 73 pixels
            };


            Label categoryLabel = new Label
            {
                Text = category,
                Font = new Font("Poppins", 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                AutoSize = false
            };

            productPanel.Controls.Add(categoryLabel);
            categoriesPanel.Controls.Add(productPanel);
        }

        private void DeleteCategoryBtn_Click(object sender, EventArgs e)
        {
            if (recentlyAddedDgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string selectedCategoryId = recentlyAddedDgv.SelectedRows[0].Cells["Category_ID"]?.Value?.ToString();
                    DeleteRecord(selectedCategoryId);
                    recentlyAddedDgv.Rows.RemoveAt(recentlyAddedDgv.SelectedRows[0].Index);
                    DeleteCategoryPanel(selectedCategoryId);
                    MessageBox.Show("Record successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeleteRecord(string categoryID)
        {
            dbModule db = new dbModule();
            string query = "DELETE FROM categories WHERE Category_ID = @Category_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category_ID", categoryID);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while deleting the record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeleteCategoryPanel(string categoryID)
        {
            foreach (Control control in categoriesPanel.Controls)
            {
                if (control.Tag != null && control.Tag.ToString() == categoryID)
                {
                    categoriesPanel.Controls.Remove(control);
                    control.Dispose();
                    break;
                }
            }
        }

    }
}

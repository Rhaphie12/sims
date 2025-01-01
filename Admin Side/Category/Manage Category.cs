using Guna.UI.WinForms;
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
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                // Load Items
                string itemsQuery = "SELECT Category_ID, Category_Name FROM categories";
                MySqlDataAdapter itemsAdapter = new MySqlDataAdapter(itemsQuery, conn);
                itemsTable = new DataTable();
                itemsAdapter.Fill(itemsTable);
                itemsBindingSource.DataSource = itemsTable;
                recentlyAddedDgv.DataSource = itemsBindingSource;

                categoriesPanel.Controls.Clear();
                foreach (DataRow row in itemsTable.Rows)
                {
                    int categoryID = Convert.ToInt32(row["Category_ID"]);
                    string categoryName = row["Category_Name"].ToString();
                    AddItemPanel(categoryID, categoryName);
                }

                if (recentlyAddedDgv.Columns.Contains("Category_ID"))
                {
                    recentlyAddedDgv.Columns["Category_ID"].Visible = false;
                }

                // Add a Delete Button Column
                if (recentlyAddedDgv != null)
                {
                    if (!recentlyAddedDgv.Columns.Contains("DeleteButton"))
                    {
                        DataGridViewImageColumn deleteIconColumn = new DataGridViewImageColumn
                        {
                            Name = "DeleteButton",
                            HeaderText = "Delete",
                            Image = Properties.Resources.delete, // Ensure the resource exists
                            ImageLayout = DataGridViewImageCellLayout.Zoom
                        };

                        recentlyAddedDgv.Columns.Add(deleteIconColumn);
                        recentlyAddedDgv.Columns["DeleteButton"].Width = 100; // Set desired width
                    }
                }

                recentlyAddedDgv.CellMouseMove += (s, e) =>
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        // Check if the hovered column is the DeleteButton
                        if (recentlyAddedDgv.Columns[e.ColumnIndex].Name == "DeleteButton")
                        {
                            recentlyAddedDgv.Cursor = Cursors.Hand; // Set cursor to hand
                        }
                        else
                        {
                            recentlyAddedDgv.Cursor = Cursors.Default; // Reset cursor
                        }
                    }
                    else
                    {
                        recentlyAddedDgv.Cursor = Cursors.Default; // Reset cursor
                    }
                };
            }
        }
        private void recentlyAddedDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (recentlyAddedDgv.Columns.Contains("DeleteButton") &&
                    e.ColumnIndex == recentlyAddedDgv.Columns["DeleteButton"].Index)
                {
                    // Handle Delete
                    int categoryID = Convert.ToInt32(recentlyAddedDgv.Rows[e.RowIndex].Cells["Category_ID"].Value);

                    DialogResult result = MessageBox.Show("Are you sure you want to delete this category?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        DeleteCategory(categoryID);
                        RemovePanelFromFlowLayout(categoryID);
                        LoadData();
                    }
                }
            }
        }

        private void RemovePanelFromFlowLayout(int categoryID)
        {
            Control panelToRemove = null;

            // Find the panel with the matching category ID in the ItemDetails object
            foreach (Control control in categoriesPanel.Controls)
            {
                if (control is GunaElipsePanel panel &&
                    panel.Tag is Categories details &&
                    details.CategoryID == categoryID)
                {
                    panelToRemove = panel;
                    break;
                }
            }

            if (panelToRemove != null)
            {
                categoriesPanel.Controls.Remove(panelToRemove);
                panelToRemove.Dispose();
            }
        }

        private void DeleteCategory(int categoryID)
        {
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string deleteQuery = "DELETE FROM categories WHERE Category_ID = @CategoryID";
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CategoryID", categoryID);
                    cmd.ExecuteNonQuery();
                }
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
                        // Clear existing controls in the panel
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
                Width = 395,
                Height = 100,
                Radius = 8,
                BackColor = Color.FromArgb(222, 196, 125),
                Tag = new Categories
                {
                    CategoryID = categoryID,
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
            categoriesPanel.Controls.Add(productPanel);
        }
    }
}

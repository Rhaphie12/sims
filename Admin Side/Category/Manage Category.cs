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
                string itemsQuery = "SELECT Category_Name FROM categories";
                MySqlDataAdapter itemsAdapter = new MySqlDataAdapter(itemsQuery, conn);
                itemsTable = new DataTable();
                itemsAdapter.Fill(itemsTable);
                itemsBindingSource.DataSource = itemsTable;
                recentlyAddedDgv.DataSource = itemsBindingSource;

                if (!recentlyAddedDgv.Columns.Contains("DeleteButton"))
                {
                    DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "DeleteButton",
                        HeaderText = "Action",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true
                    };
                    recentlyAddedDgv.Columns.Add(deleteButtonColumn);
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
                    string query = "SELECT Category_Name FROM categories";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Clear existing controls in the panel
                        categoriesPanel.Controls.Clear();

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
            // Create the main panel
            GunaElipsePanel productPanel = new GunaElipsePanel
            {
                Width = 395,
                Height = 100,
                Radius = 8,
                BackColor = Color.FromArgb(222, 196, 125),
                Tag = new ItemDetails
                {
                    Category = category
                }
            };

            // Create and center the label
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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Sales
{
    public partial class Manage_Sales : UserControl
    {
        private DataTable originalDataTable;
        private BindingSource bindingSource = new BindingSource();

        public Manage_Sales()
        {
            InitializeComponent();
        }

        public DataGridView ProductsDgv
        {
            get { return productsDgv; }
        }

        public Label CountProduct
        {
            get { return productCountTxt; }
        }

        private void Manage_Sales_Load(object sender, EventArgs e)
        {
            LoadProducts();
            ProductsCount();
            searchComboBox();
            searchFunction();
        }

        private void LoadProducts()
        {
            dbModule db = new dbModule();
            MySqlDataAdapter adapter = db.GetAdapter();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM products";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    productsDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ProductsCount()
        {
            dbModule db = new dbModule();
            string query = "SELECT COUNT(*) FROM products";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                        productCountTxt.Text = itemCount.ToString();
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
        private void searchFunction()
        {
            dbModule db = new dbModule();
            string query = "SELECT * FROM products";
            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        originalDataTable = dataTable;
                        bindingSource.DataSource = originalDataTable;
                        productsDgv.DataSource = bindingSource;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchComboBox()
        {
            searchCategoryCmb.Items.Clear();

            string query = "SELECT Category_Name FROM categoriesproducts";
            dbModule db = new dbModule();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                searchCategoryCmb.Items.Add(reader["Category_Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            if (originalDataTable == null) return;

            string searchText = searchProductTxt.Text.Trim();
            string selectedCategory = searchCategoryCmb.SelectedItem?.ToString();

            // Construct the filter dynamically
            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(selectedCategory))
            {
                filters.Add($"Category = '{selectedCategory.Replace("'", "''")}'"); // Escape single quotes
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                filters.Add($"Product_Name LIKE '%{searchText.Replace("'", "''")}%'"); // Escape single quotes
            }

            string combinedFilter = string.Join(" AND ", filters);

            DataView dv = originalDataTable.DefaultView;
            dv.RowFilter = combinedFilter;

            productsDgv.DataSource = dv.ToTable();
        }

        private void ResetFilters()
        {
            searchCategoryCmb.SelectedIndex = -1;
            searchProductTxt.Clear();               
            ApplyFilters();                      
        }

        private void NewProductBtn_Click(object sender, EventArgs e)
        {
            Product_Sales product_Sales = new Product_Sales(this);
            Add_Product addProduct = new Add_Product(this, this, product_Sales, product_Sales, product_Sales);
            addProduct.Show();
        }

        private void searchCategoryCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void searchItemTxt_TextChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            ResetFilters();
        }
    }
}

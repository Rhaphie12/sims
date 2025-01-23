using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using sims.Admin_Side.Stocks;
using sims.Messages_Boxes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Bunifu.UI.WinForms;

namespace sims.Admin_Side.Sales
{
    public partial class Sales_Form : Form
    {
        private string _productID;
        private Manage_Stockk _stock;
        private Inventory_Dashboard _inventoryDashboard;

        public Sales_Form(string productID, Manage_Stockk stock, Inventory_Dashboard inventoryDashboard)
        {
            InitializeComponent();
            _stock = stock;
            _inventoryDashboard = inventoryDashboard;
            _productID = productID;
        }

        private void previewStock()
        {
            if (_stock != null)
            {
                _stock.ViewStock();
            }
            else
            {
                MessageBox.Show("Inventory Dashboard is not available.");
            }
        }

        private void previewProductsDashboard()
        {
            if (_inventoryDashboard != null)
            {
                _inventoryDashboard.ProductsCount();
            }
            else
            {
                MessageBox.Show("Inventory Dashboard is not available.");
            }
        }
        private void previewStockDashboard()
        {
            if (_inventoryDashboard != null)
            {
                _inventoryDashboard.StockPreview();
            }
            else
            {
                MessageBox.Show("Inventory Dashboard is not available.");
            }
        }

        private void Sales_Form_Load(object sender, EventArgs e)
        {
            LoadProductDetails(_productID);
            previewStock();
            stocks();
            LoadCategoriesProducts();
            Timer timer = new Timer();
            timer.Tick += timer1_Tick;
            timer.Start();

            DateLbl.Text = DateTime.Now.ToString("ddd, d MMMM yyyy");
        }
        private void LoadCategoriesProducts()
        {
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
                                categoryCmb.Items.Add(reader["Category_Name"].ToString());
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

        private List<string> allStockItems = new List<string>();
        private bool isUpdatingComboBoxes = false;

        private void stocks()
        {
            string query = "SELECT Item_Name FROM stocks";
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
                            allStockItems.Clear();

                            while (reader.Read())
                            {
                                string itemName = reader["Item_Name"].ToString();
                                allStockItems.Add(itemName);
                            }
                        }
                    }
                }

                // Populate combo boxes
                PopulateComboBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stocks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateComboBoxes()
        {
            stockCmb.Items.Clear();
            stock2Cmb.Items.Clear();
            stock3Cmb.Items.Clear();
            stock4Cmb.Items.Clear();
            stock5Cmb.Items.Clear();
            stock6Cmb.Items.Clear();

            foreach (var item in allStockItems)
            {
                stockCmb.Items.Add(item);
                stock2Cmb.Items.Add(item);
                stock3Cmb.Items.Add(item);
                stock4Cmb.Items.Add(item);
                stock5Cmb.Items.Add(item);
                stock6Cmb.Items.Add(item);
            }
        }

        private void UpdateComboBoxItems()
        {
            // Prevent recursive updates
            if (isUpdatingComboBoxes) return;

            isUpdatingComboBoxes = true;

            try
            {
                // Get selected items from all combo boxes
                var selectedItems = new HashSet<string>
        {
            stockCmb.SelectedItem?.ToString(),
            stock2Cmb.SelectedItem?.ToString(),
            stock3Cmb.SelectedItem?.ToString(),
            stock4Cmb.SelectedItem?.ToString(),
            stock5Cmb.SelectedItem?.ToString(),
            stock6Cmb.SelectedItem?.ToString()
        };

                // Update each combo box's items
                UpdateComboBox(stockCmb, selectedItems);
                UpdateComboBox(stock2Cmb, selectedItems);
                UpdateComboBox(stock3Cmb, selectedItems);
                UpdateComboBox(stock4Cmb, selectedItems);
                UpdateComboBox(stock5Cmb, selectedItems);
                UpdateComboBox(stock6Cmb, selectedItems);
            }
            finally
            {
                // Re-enable updates
                isUpdatingComboBoxes = false;
            }
        }

        private void UpdateComboBox(GunaComboBox comboBox, HashSet<string> selectedItems)
        {
            // Get the currently selected item
            var currentSelection = comboBox.SelectedItem?.ToString();

            // Clear and repopulate items
            comboBox.Items.Clear();
            foreach (var item in allStockItems)
            {
                if (!selectedItems.Contains(item) || item == currentSelection)
                {
                    comboBox.Items.Add(item);
                }
            }

            // Restore the current selection if still valid
            if (!string.IsNullOrEmpty(currentSelection))
            {
                comboBox.SelectedItem = currentSelection;
            }
        }

        private void LoadProductDetails(string productID)
        {
            dbModule db = new dbModule();
            string query = "SELECT Product_ID, Product_Name, Category " +
                           "FROM products WHERE Product_ID = @Product_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Product_ID", _productID);
                    try
                    {
                        conn.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                productIDTxt.Text = reader["Product_ID"].ToString();
                                productNameTxt.Text = reader["Product_Name"].ToString();
                                string categoryValue = reader["Category"].ToString();

                                if (!string.IsNullOrEmpty(categoryValue))
                                {
                                    categoryCmb.SelectedItem = categoryValue;
                                    if (!categoryCmb.Items.Contains(categoryValue))
                                    {
                                        categoryCmb.Items.Add(categoryValue);
                                        categoryCmb.SelectedItem = categoryValue;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while fetching product details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void addProductBtn_Click(object sender, EventArgs e)
        {
            addProduct();
        }

        private void addProduct()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();

            string productID = productIDTxt.Text.Trim();
            string productName = productNameTxt.Text.Trim();
            string category = categoryCmb.SelectedItem?.ToString() ?? string.Empty;
            string productPrice = productPriceTxt.Text.Trim();
            string stockQuantity = quantityStockTxt.Text.Trim();
            string quantitySold = quantitySoldTxt.Text.Trim();

            List<string> stockItems = new List<string>();
            if (!string.IsNullOrEmpty(stockCmb.SelectedItem?.ToString())) stockItems.Add(stockCmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock2Cmb.SelectedItem?.ToString())) stockItems.Add(stock2Cmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock3Cmb.SelectedItem?.ToString())) stockItems.Add(stock3Cmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock4Cmb.SelectedItem?.ToString())) stockItems.Add(stock4Cmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock5Cmb.SelectedItem?.ToString())) stockItems.Add(stock5Cmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock6Cmb.SelectedItem?.ToString())) stockItems.Add(stock6Cmb.SelectedItem.ToString());
            // Add more stock items if necessary...

            string stockNeeded = string.Join(", ", stockItems);

            if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(stockQuantity) || string.IsNullOrEmpty(quantitySold))
            {
                new Field_Required().Show();
                return;
            }

            if (!int.TryParse(stockQuantity, out var parsedStockQuantity))
            {
                MessageBox.Show("Invalid stock quantity. Please enter a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(quantitySold, out var parsedQuantitySold))
            {
                MessageBox.Show("Invalid quantity sold. Please enter a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                conn.Open();
                cmd.Connection = conn;

                // Insert the product
                cmd.CommandText = "INSERT INTO products(Product_ID, Product_Name, Category)" +
                                  "VALUES (@Product_ID, @Product_Name, @Category)";
                cmd.Parameters.AddWithValue("@Product_ID", productID);
                cmd.Parameters.AddWithValue("@Product_Name", productName);
                cmd.Parameters.AddWithValue("@Category", category);
                //cmd.Parameters.AddWithValue("@Product_Price", parsedPrice); // Use the parsed decimal price
                //cmd.Parameters.AddWithValue("@Stock_Quantity", parsedStockQuantity); // Use parsed integer
                //cmd.Parameters.AddWithValue("@Quantity_Sold", parsedQuantitySold); // Use parsed integer
                //cmd.Parameters.AddWithValue("@Stock_Needed", stockNeeded);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Reduce stock quantities
                    foreach (var stockItem in stockItems)
                    {
                        cmd.CommandText = "UPDATE stocks SET Stock_In = Stock_In - @Stock_In WHERE Item_Name = @ItemName";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Stock_In", parsedStockQuantity);  // Use parsed stock quantity
                        cmd.Parameters.AddWithValue("@ItemName", stockItem);
                        cmd.ExecuteNonQuery();
                        previewStock();
                        previewStockDashboard();
                    }

                    MessageBox.Show("Product added successfully, and stock quantities updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Reset the form
                    productNameTxt.Clear();
                    categoryCmb.SelectedIndex = -1;
                    productPriceTxt.Clear();
                    quantitySoldTxt.Clear();
                    stockCmb.SelectedIndex = -1;
                    stock2Cmb.SelectedIndex = -1;
                    stock3Cmb.SelectedIndex = -1;
                    stock4Cmb.SelectedIndex = -1;
                    stock5Cmb.SelectedIndex = -1;
                    stock6Cmb.SelectedIndex = -1;
                    this.Hide();
                    previewProductsDashboard();
                }
                else
                {
                    MessageBox.Show("Failed to add product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                cmd.Dispose();
                conn.Dispose();
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private decimal parsedPrice;
        private void productPriceTxt_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(productPriceTxt.Text))
            {
                // Remove peso sign and other formatting for processing
                string rawInput = productPriceTxt.Text.Replace("₱", "").Trim();

                if (decimal.TryParse(rawInput, out parsedPrice))
                {
                    // Format the input with the peso sign and two decimal places for display
                    productPriceTxt.Text = $"₱{parsedPrice:0.00}";
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    productPriceTxt.Clear();
                    parsedPrice = 0; // Reset the parsed value to ensure no invalid data is stored
                }
            }
        }

        private void ValidateTextBoxForNumbersOnly(BunifuTextBox textBox)
        {
            string newText = textBox.Text;

            if (System.Text.RegularExpressions.Regex.IsMatch(newText, @"[a-zA-Z]"))
            {
                MessageBox.Show("Letters are not allowed!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox.Text = System.Text.RegularExpressions.Regex.Replace(newText, @"[a-zA-Z]", "");
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
        private void quantitySoldTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(quantitySoldTxt);

        }

        private void productPriceTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(productPriceTxt);
        }

        private void quantityStockTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(quantityStockTxt);
            quantitySoldTxt.Text = quantityStockTxt.Text;
        }

        private void stockCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void stock2Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void stock3Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void stock4Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void stock5Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void stock6Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateComboBoxItems();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeLbl.Text = DateTime.Now.ToString("h:mm:ss tt");
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            stockCmb.SelectedIndex = -1;
            stock2Cmb.SelectedIndex = -1;
            stock3Cmb.SelectedIndex = -1;
            stock4Cmb.SelectedIndex = -1;
            stock5Cmb.SelectedIndex = -1;
            stock6Cmb.SelectedIndex = -1;
        }
    }
}

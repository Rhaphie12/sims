using Bunifu.UI.WinForms;
using MySql.Data.MySqlClient;
using sims.Admin_Side.Stocks;
using sims.Messages_Boxes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace sims.Admin_Side.Sales
{
    public partial class Add_Product : Form
    {
        private Manage_Sales count;
        private Manage_Sales dashboard;
        private Product_Sales CoffeeLayoutPanel;
        private Product_Sales NonCoffeeLayoutPanel;
        private Product_Sales HotCoffeeLayoutPanel;
        private Manage_Stock stock;

        public Add_Product(Manage_Sales count, Manage_Sales dashboard, Product_Sales CoffeeLayoutPanel, Product_Sales NonCoffeeLayoutPanel, Product_Sales HotCoffeeLayoutPanel, Manage_Stock stock)
        {
            InitializeComponent();
            this.count = count;
            this.dashboard = dashboard;
            this.CoffeeLayoutPanel = CoffeeLayoutPanel;
            this.NonCoffeeLayoutPanel = NonCoffeeLayoutPanel;
            this.HotCoffeeLayoutPanel = HotCoffeeLayoutPanel;
            this.stock = stock;
        }

        public class ProductDetails
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductPrice { get; set; }
            public string category { get; set; }
        }

        private void Add_Product_Load(object sender, EventArgs e)
        {
            stocks();
            Populate();
            ProductsCount();
            LoadProductButtons();
            GenerateRandomItemID();
            LoadCategoriesProducts();
            ViewStock();
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
                            while (reader.Read())
                            {
                                stockCmb.Items.Add(reader["Item_Name"].ToString());
                                stock2Cmb.Items.Add(reader["Item_Name"].ToString());
                                stock3Cmb.Items.Add(reader["Item_Name"].ToString());
                                stock4Cmb.Items.Add(reader["Item_Name"].ToString());
                                stock5Cmb.Items.Add(reader["Item_Name"].ToString());
                                stock6Cmb.Items.Add(reader["Item_Name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stocks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateRandomItemID()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            productIDTxt.Text = randomNumber.ToString();
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
                    string query = "SELECT * FROM products";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dashboard.ProductsDgv.DataSource = dt;
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
                        count.productCountTxt.Text = itemCount.ToString();
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

        private void ViewStock()
        {
            if (stock == null || stock.ItemsStockDgv == null)
            {
                MessageBox.Show("The stock object or ItemsStockDgv is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DataTable dataTable = new DataTable();
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();

                    string query = "SELECT Stock_ID, Item_ID, Item_Name, Stock_In, Unit_Type, Date_Added, Item_Price, Item_Total, Item_Image FROM stocks";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    // Bind the data table to the DataGridView
                    stock.ItemsStockDgv.Invoke((MethodInvoker)(() =>
                    {
                        stock.ItemsStockDgv.DataSource = dataTable;
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to populate stock data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string quantitySold = quantitySoldTxt.Text.Trim();
            string stockQuantity = quantityStockTxt.Text.Trim();

            List<string> stockItems = new List<string>();
            if (!string.IsNullOrEmpty(stockCmb.SelectedItem?.ToString())) stockItems.Add(stockCmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock2Cmb.SelectedItem?.ToString())) stockItems.Add(stock2Cmb.SelectedItem.ToString());
            if (!string.IsNullOrEmpty(stock3Cmb.SelectedItem?.ToString())) stockItems.Add(stock3Cmb.SelectedItem.ToString());

            string stockNeeded = string.Join(", ", stockItems);

            if (string.IsNullOrEmpty(productID) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(category))
            {
                new Field_Required().Show();
                return;
            }

            try
            {
                conn.Open();
                cmd.Connection = conn;

                // Insert the product
                cmd.CommandText = "INSERT INTO products(Product_ID, Product_Name, Category, Product_Price, Quantity_Sold, Stock_Needed)" +
                                  "VALUES (@Product_ID, @Product_Name, @Category, @Product_Price, @Quantity_Sold, @Stock_Needed)";
                cmd.Parameters.AddWithValue("@Product_ID", productID);
                cmd.Parameters.AddWithValue("@Product_Name", productName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Product_Price", decimal.TryParse(productPrice, out var price) ? price : 0);
                cmd.Parameters.AddWithValue("@Quantity_Sold", int.TryParse(quantitySold, out var qtySold) ? qtySold : 0);
                cmd.Parameters.AddWithValue("@Stock_Needed", stockNeeded);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Reduce stock quantities
                    foreach (var stockItem in stockItems)
                    {
                        cmd.CommandText = "UPDATE stocks SET Stock_In = Stock_In - @Stock_In WHERE Item_Name = @ItemName";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Stock_In", int.TryParse(stockQuantity, out var qty) ? qty : 0);
                        cmd.Parameters.AddWithValue("@ItemName", stockItem);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product added successfully, and stock quantities updated", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear fields and perform other cleanup
                    Populate();
                    this.Hide();
                    ProductsCount();
                    GenerateRandomItemID();
                    ViewStock();

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

                    LoadProductButtons();
                    AddProductButton(productID, productName, productPrice, category);
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

        private void AddProductButton(string productID, string productName, string productPrice, string category)
        {
            Button productButton = new Button
            {
                Width = 150,
                Height = 100,
                Text = $"{productName}\nPrice: ₱ {productPrice}",
                Tag = new ProductDetails
                {
                    ProductID = productID,
                    ProductName = productName,
                    ProductPrice = productPrice
                },
                BackColor = Color.FromArgb(222, 196, 125),
                Font = new Font("Poppins", 12),
                TextAlign = ContentAlignment.MiddleCenter
            };

            switch (category)
            {
                case "Coffee":
                    CoffeeLayoutPanel.Controls.Add(productButton);
                    break;
                case "Non-Coffee":
                    NonCoffeeLayoutPanel.Controls.Add(productButton);
                    break;
                case "Hot":
                    HotCoffeeLayoutPanel.Controls.Add(productButton);
                    break;
                default:
                    MessageBox.Show($"Unknown category: {category}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
        }

        private void LoadProductButtons()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT Product_ID, Product_Name, Product_Price, Category FROM products";

                MySqlDataReader reader = cmd.ExecuteReader();

                // Clear existing controls from the panels
                CoffeeLayoutPanel.Controls.Clear();
                NonCoffeeLayoutPanel.Controls.Clear();
                HotCoffeeLayoutPanel.Controls.Clear();

                while (reader.Read())
                {
                    string productID = reader.GetInt32("Product_ID").ToString();
                    string productName = reader.GetString("Product_Name");
                    string productPrice = reader.GetDecimal("Product_Price").ToString("F2");
                    string category = reader.GetString("Category");

                    AddProductButton(productID, productName, productPrice, category);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
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

        private void productNameTxt_TextChanged(object sender, EventArgs e)
        {
            string newText = productNameTxt.Text;

            if (System.Text.RegularExpressions.Regex.IsMatch(newText, @"\d"))
            {
                MessageBox.Show("Numbers are not allowed!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                productNameTxt.Text = System.Text.RegularExpressions.Regex.Replace(newText, @"\d", "");
                productNameTxt.SelectionStart = productNameTxt.Text.Length;
            }
        }

        private void productPriceTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(productPriceTxt);
        }

        private void quantitySoldTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(quantitySoldTxt);
        }

        private void quantityStockTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(quantityStockTxt);
        }
    }
}
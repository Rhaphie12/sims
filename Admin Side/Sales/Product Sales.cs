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
using static sims.Admin_Side.Sales.Add_Product;

namespace sims.Admin_Side.Sales
{
    public partial class Product_Sales : UserControl
    {
        private Manage_Sales dashboard;

        public Product_Sales(Manage_Sales dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        public FlowLayoutPanel CoffeeLayoutPanel
        {
            get { return coffeeLayoutPanel; }
        }

        public FlowLayoutPanel NonCoffeeLayoutPanel
        {
            get { return nonCoffeeLayoutPanel; }
        }

        public FlowLayoutPanel HotCoffeeLayoutPanel
        {
            get { return hotCoffeeLayoutPanel; }
        }

        private void Product_Sales_Load(object sender, EventArgs e)
        {
            Populate();
            LoadProductButtons();
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
            // productButton.Click += ProductButton_Click;

            // Add product button to the appropriate layout panel
            if (category.Equals("Coffee", StringComparison.OrdinalIgnoreCase))
            {
                CoffeeLayoutPanel.Controls.Add(productButton);
            }
            else if (category.Equals("Non-Coffee", StringComparison.OrdinalIgnoreCase))
            {
                NonCoffeeLayoutPanel.Controls.Add(productButton);
            }
            else if (category.Equals("Hot", StringComparison.OrdinalIgnoreCase))
            {
                HotCoffeeLayoutPanel.Controls.Add(productButton);
            }
            else
            {
                MessageBox.Show($"Category '{category}' is not recognized.", "Unknown Category", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Populate();
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
                CoffeeLayoutPanel.Controls.Clear();
                NonCoffeeLayoutPanel.Controls.Clear();
                HotCoffeeLayoutPanel.Controls.Clear();

                while (reader.Read())
                {
                    string productID = reader.GetInt32("Product_ID").ToString(); // Corrected here
                    string productName = reader.GetString("Product_Name");
                    string productPrice = reader.GetDecimal("Product_Price").ToString("F2"); // Assuming Product_Price is a decimal
                    string category = reader.GetString("Category"); // Retrieve the category

                    AddProductButton(productID, productName, productPrice, category);
                    Populate();
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
    }
}

using MySql.Data.MySqlClient;
using sims.Admin_Side.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Bunifu.UI.WinForms;

namespace sims.Admin_Side.Stocks
{
    public partial class Edit_Stock : Form
    {
        private string _itemID;
        private Manage_Stock dashboard;

        public Edit_Stock(string itemID, Manage_Stock dashboard)
        {
            InitializeComponent();
            _itemID = itemID;
            this.dashboard = dashboard;

            itemQuantityTxt.TextChanged += (s, e) => CalculateTotalValue();
            itemPriceTxt.TextChanged += (s, e) => CalculateTotalValue();
        }

        private void Edit_Stock_Load(object sender, EventArgs e)
        {
            LoadProductDetails(_itemID);
            UnitType();
            CalculateTotalValue();
        }

        private void UnitType()
        {
            string query = "SELECT Unit_Type FROM unittype";
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
                                unitTypeCmb.Items.Add(reader["Unit_Type"].ToString());
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

        private void Populate()
        {
            dbModule db = new dbModule();
            MySqlDataAdapter adapter = db.GetAdapter();
            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM stocks";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dashboard.ItemsStockDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CalculateTotalValue()
        {
            try
            {
                if (int.TryParse(itemQuantityTxt.Text, out int quantity) &&
                    decimal.TryParse(itemPriceTxt.Text, out decimal price))
                {
                    decimal totalValue = quantity * price;

                    // Store the plain numeric value in a variable for future use
                    itemTotalTxt.Tag = totalValue;

                    // Display the value with a peso sign and formatting
                    itemTotalTxt.Text = $"₱ {totalValue:0.00}";
                }
                else
                {
                    itemTotalTxt.Text = string.Empty;
                    itemTotalTxt.Tag = null; // Clear the stored value
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void LoadProductDetails(string stockID)
        {
            dbModule db = new dbModule();
            string query = "SELECT Stock_ID, Item_ID, Item_Name, Stock_In, Unit_Type, Date_Added, Item_Price, Item_Total, Item_Image " +
                           "FROM stocks WHERE Stock_ID = @Stock_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Stock_ID", stockID); // Use Stock_ID instead of itemID
                    try
                    {
                        conn.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                itemIDTxt.Text = reader["Item_ID"].ToString();
                                itemQuantityTxt.Text = reader["Stock_In"].ToString();
                                itemPriceTxt.Text = reader["Item_Price"].ToString();
                                itemTotalTxt.Text = reader["Item_Total"].ToString();

                                string itemNameValue = reader["Item_Name"].ToString();
                                if (!string.IsNullOrEmpty(itemNameValue))
                                {
                                    if (!selectItemNameCmb.Items.Contains(itemNameValue))
                                    {
                                        selectItemNameCmb.Items.Add(itemNameValue);
                                    }
                                    selectItemNameCmb.SelectedItem = itemNameValue;
                                }

                                string unitValue = reader["Unit_Type"].ToString();
                                if (!string.IsNullOrEmpty(unitValue))
                                {
                                    if (!unitTypeCmb.Items.Contains(unitValue))
                                    {
                                        unitTypeCmb.Items.Add(unitValue);
                                    }
                                    unitTypeCmb.SelectedItem = unitValue;
                                }

                                if (reader["Date_Added"] != DBNull.Value)
                                {
                                    dateAddedDtp.Value = Convert.ToDateTime(reader["Date_Added"]);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("Item_Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Item_Image"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        itemImagePic.Image = System.Drawing.Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    itemImagePic.Image = null;
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

        private void backNewStockBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void addStockBtn_Click(object sender, EventArgs e)
        {
            updateStock();
        }

        private void updateStock()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();

            // Assuming itemIDTxt.Text is your Stock_ID, but make sure you have the correct value
            string stockID = itemIDTxt.Text.Trim();  // This should be your Stock_ID from the selected item, not itemID
            string itemName = selectItemNameCmb.SelectedItem?.ToString() ?? string.Empty;
            string stockIn = itemQuantityTxt.Text.Trim();
            string unitType = unitTypeCmb.SelectedItem?.ToString() ?? string.Empty;
            string dateAdded = dateAddedDtp.Value.ToString("yyyy-MM-dd");
            string itemPrice = itemPriceTxt.Text.Trim();
            decimal itemTotal = itemTotalTxt.Tag is decimal value ? value : 0;

            if (string.IsNullOrEmpty(stockID) || string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(stockIn) || string.IsNullOrEmpty(unitType) || string.IsNullOrEmpty(itemPrice))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE stocks SET " +
                    "Item_Name = @Item_Name, " +
                    "Stock_In = @Stock_In, " +
                    "Unit_Type = @Unit_Type, " +
                    "Date_Added = @Date_Added, " +
                    "Item_Price = @Item_Price, " +
                    "Item_Total = @Item_Total, " +
                    "Item_Image = @Item_Image " +
                    "WHERE Stock_ID = @Stock_ID"; // Use the correct Stock_ID

                // Add parameters
                cmd.Parameters.AddWithValue("@Item_Name", itemName);
                cmd.Parameters.AddWithValue("@Stock_In", int.TryParse(stockIn, out var stock) ? stock : 0);
                cmd.Parameters.AddWithValue("@Unit_Type", unitType);
                cmd.Parameters.AddWithValue("@Date_Added", dateAdded);
                cmd.Parameters.AddWithValue("@Item_Price", decimal.TryParse(itemPrice, out var price) ? price : 0);
                cmd.Parameters.AddWithValue("@Item_Total", itemTotal);
                cmd.Parameters.AddWithValue("@Stock_ID", stockID); // Ensure this is the correct Stock_ID

                // If image is provided, convert it to bytes
                if (itemImagePic.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (Bitmap bitmap = new Bitmap(itemImagePic.Image))
                        {
                            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            byte[] imageBytes = ms.ToArray();
                            cmd.Parameters.AddWithValue("@Item_Image", imageBytes);
                        }
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Item_Image", DBNull.Value);
                }

                // Execute the command
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Stock updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    selectItemNameCmb.SelectedIndex = -1;
                    itemQuantityTxt.Clear();
                    unitTypeCmb.SelectedIndex = -1;
                    dateAddedDtp.Value = DateTime.Now;
                    itemPriceTxt.Clear();
                    itemTotalTxt.Clear();
                    itemImagePic.Image = null;
                    Populate();
                }
                else
                {
                    MessageBox.Show("Failed to update stock. Please check the Stock ID and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void itemQuantityTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(itemQuantityTxt);

            if (int.TryParse(itemQuantityTxt.Text, out int quantity))
            {
                if (quantity > 20)
                {
                    MessageBox.Show("The maximum stock quantity is 20.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    itemQuantityTxt.Clear();
                }
            }
        }

        private void itemPriceTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(itemPriceTxt);
        }

        private void totalInfoBtn_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show("Item Total is calculated by multiplying Item Quantity and Item Price", "Item Total of Item Quantity and Item Price", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

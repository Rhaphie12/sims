using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Items
{
    public partial class Edit_Items : Form
    {
        private string _itemID;
        private Manage_Items dashboard;
        public Edit_Items(string itemID, Manage_Items dashboard)
        {
            InitializeComponent();
            _itemID = itemID;
            this.dashboard = dashboard;

            itemQuantityTxt.TextChanged += (s, e) => CalculateTotalValue();
            itemPriceTxt.TextChanged += (s, e) => CalculateTotalValue();
        }
        private void Edit_Items_Load(object sender, EventArgs e)
        {
            Populate();
            LoadProductDetails(_itemID);
            LoadComboBoxData();
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
                    string query = "SELECT * FROM items";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dashboard.ItemsDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void LoadComboBoxData()
        {
            string query = "SELECT Category_Name FROM categories";
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

        private void LoadProductDetails(string itemID)
        {
            dbModule db = new dbModule();
            string query = "SELECT Item_ID, Item_Name, Category, Date_Added, Item_Quantity, Weight_Unit, Item_Price, Item_Total, Item_Description, Item_Image " +
                           "FROM items WHERE Item_ID = @Item_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Item_ID", itemID);
                    try
                    {
                        conn.Open();

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                itemIDTxt.Text = reader["Item_ID"].ToString();
                                itemNameTxt.Text = reader["Item_Name"].ToString();
                                string categoryValue = reader["Category"].ToString();
                                dateAddedTxt.Text = reader["Date_Added"].ToString();
                                string itemQuantityValue = reader["Item_Quantity"].ToString();
                                string weightUnitValue = reader["Weight_Unit"].ToString();
                                itemPriceTxt.Text = reader["Item_Price"].ToString();
                                totalValueTxt.Text = reader["Item_Total"].ToString();
                                itemDescTxt.Text = reader["Item_Description"].ToString();

                                if (!string.IsNullOrEmpty(itemQuantityValue))
                                {
                                    string[] parts = itemQuantityValue.Split(' ');
                                    if (parts.Length == 2)
                                    {
                                        itemQuantityTxt.Text = parts[0];
                                        string unitType = parts[1];  
                                        if (!unitTypeCmb.Items.Contains(unitType))
                                        {
                                            unitTypeCmb.Items.Add(unitType);
                                        }
                                        unitTypeCmb.SelectedItem = unitType;
                                    }
                                    else
                                    {
                                        itemQuantityTxt.Text = itemQuantityValue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(categoryValue))
                                {
                                    categoryCmb.SelectedItem = categoryValue;
                                    if (!categoryCmb.Items.Contains(categoryValue))
                                    {
                                        categoryCmb.Items.Add(categoryValue);
                                        categoryCmb.SelectedItem = categoryValue;
                                    }
                                }

                                if (!string.IsNullOrEmpty(weightUnitValue))
                                {
                                    weightUnitCmb.SelectedItem = weightUnitValue;
                                    if (!weightUnitCmb.Items.Contains(weightUnitValue))
                                    {
                                        weightUnitCmb.Items.Add(weightUnitValue);
                                        weightUnitCmb.SelectedItem = weightUnitValue;
                                    }
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("Item_Image")))
                                {
                                    byte[] imageBytes = (byte[])reader["Item_Image"];
                                    using (MemoryStream ms = new MemoryStream(imageBytes))
                                    {
                                        itemImagePic.Image = Image.FromStream(ms);
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

        private void CalculateTotalValue()
        {
            try
            {
                if (int.TryParse(itemQuantityTxt.Text, out int quantity) &&
                    decimal.TryParse(itemPriceTxt.Text, out decimal price))
                {
                    decimal totalValue = quantity * price;

                    totalValueTxt.Text = totalValue.ToString("0.00");
                }
                else
                {
                    totalValueTxt.Text = string.Empty;
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

        private void EditItemBtn_Click(object sender, EventArgs e)
        {
            UpdateItem();
        }

        private void UpdateItem()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();

            string itemID = itemIDTxt.Text.Trim();
            string itemName = itemNameTxt.Text.Trim();
            string category = categoryCmb.SelectedItem?.ToString() ?? string.Empty;
            string dateAdded = dateAddedTxt.Text.Trim();
            string itemQuantity = itemQuantityTxt.Text.Trim();
            string unitType = unitTypeCmb.SelectedItem?.ToString() ?? string.Empty;
            string weightUnit = weightUnitCmb.SelectedItem?.ToString() ?? string.Empty;
            string itemPrice = itemPriceTxt.Text.Trim();
            string itemDescription = itemDescTxt.Text.Trim();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "UPDATE items SET Item_name = @Item_name, Category = @Category, Date_Added = @Date_Added, " +
                                  "Item_Quantity = @Item_Quantity, Weight_Unit = @Weight_Unit, Item_Price = @Item_Price, Item_Description = @Item_Description " +
                                  "WHERE Item_ID = @Item_ID";

                cmd.Parameters.AddWithValue("@Item_ID", itemID);
                cmd.Parameters.AddWithValue("@Item_name", itemName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Date_Added", dateAdded);
                cmd.Parameters.AddWithValue("@Item_Quantity", itemQuantity);
                cmd.Parameters.AddWithValue("@Weight_Unit", weightUnit);
                cmd.Parameters.AddWithValue("@Item_Price", itemPrice);
                cmd.Parameters.AddWithValue("@Item_Description", itemDescription);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Item updated successfully!", "Item Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    itemNameTxt.Clear();
                    dateAddedTxt.Clear();
                    categoryCmb.SelectedIndex = -1;
                    itemQuantityTxt.Clear();
                    weightUnitCmb.SelectedIndex = -1;
                    itemPriceTxt.Clear();
                    itemDescTxt.Clear();
                    itemImagePic.Image = null;
                    Populate();
                }
                else
                {
                    MessageBox.Show("Failed to update item. Item ID might not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void backNewItemsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void totalInfoBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Item Total is calculated by muliplying Item Quantity and Item Price", "Item Total of Item Quantity and Item Price", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

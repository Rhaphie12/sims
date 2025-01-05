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
using Bunifu.UI.WinForms;

namespace sims.Admin_Side.Items
{
    public partial class New_Items : Form
    {
        private Manage_Items dashboard;
        private Manage_Items count;

        public New_Items(Manage_Items dashboard, Manage_Items count)
        {
            InitializeComponent();
            this.dashboard = dashboard;
            this.count = count;

            itemQuantityTxt.TextChanged += (s, e) => CalculateTotalValue();
            itemPriceTxt.TextChanged += (s, e) => CalculateTotalValue();
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void New_Items_Load(object sender, EventArgs e)
        {
            ItemsCount();
            GenerateRandomItemID();
            LoadComboBoxData();
            CalculateTotalValue();
        }
        private void ItemsCount()
        {
            dbModule db = new dbModule();
            string query = "SELECT COUNT(*) FROM items";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                        count.CountItem.Text = itemCount.ToString();
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

        private void GenerateRandomItemID()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            itemIDTxt.Text = randomNumber.ToString();
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

        private void CalculateTotalValue()
        {
            try
            {
                // Parse the quantity and price from the textboxes
                if (int.TryParse(itemQuantityTxt.Text, out int quantity) &&
                    decimal.TryParse(itemPriceTxt.Text, out decimal price))
                {
                    // Calculate the total value
                    decimal totalValue = quantity * price;

                    // Display the total value in the totalValueTxt textbox
                    totalValueTxt.Text = totalValue.ToString("0.00");
                }
                else
                {
                    // If parsing fails, clear the totalValueTxt
                    totalValueTxt.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                MessageBox.Show($"An error occurred: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void backNewItemsBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void addItemBtn_Click(object sender, EventArgs e)
        {
            AddItem();
        }

        private void AddItem()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();

            string itemID = itemIDTxt.Text.Trim();
            string itemName = itemNameTxt.Text.Trim();
            string category = categoryCmb.SelectedItem?.ToString() ?? string.Empty;
            string dateAdded = dateAddedTxt.Text.Trim();
            string itemQuantity = itemQuantityTxt.Text.Trim();
            string itemPrice = itemPriceTxt.Text.Trim();
            string totalValue = totalValueTxt.Text.Trim();
            string itemDescription = itemDescTxt.Text.Trim();
            Image itemImage = itemImagePic.Image;

            // Validation for empty fields
            if (string.IsNullOrEmpty(itemID) || string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(dateAdded) ||
                string.IsNullOrEmpty(itemQuantity) || string.IsNullOrEmpty(itemPrice) || string.IsNullOrEmpty(totalValue) || string.IsNullOrEmpty(itemDescription))
            {
                new Messages_Boxes.Field_Required().Show();
                return;
            }

            // Date format validation - only accepts MM/dd/yyyy format (no time)
            //if (!DateTime.TryParseExact(dateAdded, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            //{
            //    MessageBox.Show("Please enter the date in the correct format (MM/dd/yyyy).", "Invalid Date Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //string formattedDate = parsedDate.ToString("yyyy-MM-dd");
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO items (Item_ID, Item_Name, Category, Date_Added, Item_Quantity, Item_Price, Item_Total, Item_Description, Item_Image) " +
                    "VALUES (@Item_ID, @Item_name, @Category, @Date_Added, @Item_Quantity, @Item_Price, @Item_Total, @Item_Description, @Item_Image)";
                cmd.Parameters.AddWithValue("@Item_ID", itemID);
                cmd.Parameters.AddWithValue("@Item_name", itemName);
                cmd.Parameters.AddWithValue("@Category", category);
                cmd.Parameters.AddWithValue("@Date_Added", dateAdded);
                cmd.Parameters.AddWithValue("@Item_Quantity", itemQuantity);
                cmd.Parameters.AddWithValue("@Item_Price", itemPrice);
                cmd.Parameters.AddWithValue("@Item_Description", itemDescription);
                cmd.Parameters.AddWithValue("@Item_Total", totalValue);
                cmd.Parameters.AddWithValue("@Item_Image",
                    itemImage != null ? ImageToByteArray(itemImage) : (object)DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Item added successfully!", "Item Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    GenerateRandomItemID();
                    ItemsCount();
                    itemNameTxt.Clear();
                    categoryCmb.SelectedIndex = -1;
                    itemQuantityTxt.Clear();
                    dateAddedTxt.Clear();
                    itemPriceTxt.Clear();
                    totalValueTxt.Clear();
                    itemDescTxt.Clear();
                    itemImagePic.Image = null;
                    Populate();
                }
                else
                {
                    MessageBox.Show("Failed to add item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dateAddedTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(dateAddedTxt);
        }

        private void itemQuantityTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(itemQuantityTxt);
        }

        private void itemPriceTxt_TextChanged(object sender, EventArgs e)
        {
            ValidateTextBoxForNumbersOnly(itemPriceTxt);
        }

        private void browseImageBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All Files|*.*";
                openFileDialog.Title = "Select an Image";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;

                    try
                    {
                        itemImagePic.Image = Image.FromFile(imagePath);
                        itemImagePic.SizeMode = PictureBoxSizeMode.StretchImage;
                        itemImageLabel.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

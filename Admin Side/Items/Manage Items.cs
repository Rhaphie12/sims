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
using Bunifu.UI.WinForms;

namespace sims.Admin_Side.Items
{
    public partial class Manage_Items : UserControl
    {
        private DataTable originalDataTable;
        private BindingSource bindingSource = new BindingSource();

        public Manage_Items()
        {
            InitializeComponent();
        }
        public DataGridView ItemsDgv
        {
            get { return itemsDgv; }
        }

        public Label CountItem
        {
            get { return itemCountTxt; }
        }
        private void Manage_Items_Load(object sender, EventArgs e)
        {
            Populate();
            ItemsCount();
            searchFunction();
            searchComboBox();
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
                    itemsDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void searchFunction()
        {
            dbModule db = new dbModule();
            string query = "SELECT * FROM items";
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
                        itemsDgv.DataSource = bindingSource;
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
            searchCategoryCmb.DropDownStyle = ComboBoxStyle.DropDown;
            searchCategoryCmb.Items.Clear();

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
            finally
            {
                searchCategoryCmb.SelectedIndex = -1;
            }
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
                        itemCountTxt.Text = itemCount.ToString();
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

        private void NewItemBtn_Click(object sender, EventArgs e)
        {
            New_Items newProductForm = new New_Items(this, this);
            newProductForm.Show();
        }

        private void UpdateItemBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to update this record?", "Update Item!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = itemsDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = itemsDgv.Rows[selectedRowIndex];
                    string itemID = selectedRow.Cells["Item_ID"]?.Value?.ToString();
                    if (!string.IsNullOrEmpty(itemID))
                    {
                        Edit_Items updateProductForm = new Edit_Items(itemID, this);
                        updateProductForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Item_ID. Unable to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteItemBtn_Click(object sender, EventArgs e)
        {
            if (itemsDgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this Item?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = itemsDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = itemsDgv.Rows[selectedRowIndex];
                    string selectedItemID = selectedRow.Cells["Item_ID"]?.Value?.ToString();

                    if (!string.IsNullOrEmpty(selectedItemID))
                    {
                        DeleteRecord(selectedItemID);
                        itemsDgv.Rows.RemoveAt(selectedRowIndex);
                        MessageBox.Show("Item successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Populate();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Item_ID. Unable to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteRecord(string itemID)
        {
            dbModule db = new dbModule();
            string query = "DELETE FROM items WHERE Item_ID = @Item_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Item_ID", itemID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("No record found to delete.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while deleting the record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void searchItemTxt_TextChanged(object sender, EventArgs e)
        {
            if (originalDataTable == null) return;
            string searchText = searchItemTxt.Text.Trim();
            DataView dv = originalDataTable.DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
                ResetFilters();
            }
            else
            {
                dv.RowFilter = $"Item_Name LIKE '%{searchText}%'";
            }

            itemsDgv.DataSource = dv.ToTable();
        }

        private void searchDateTxt_TextChanged(object sender, EventArgs e)
        {
            if (originalDataTable == null) return;
            string searchText = searchDateTxt.Text.Trim();
            DataView dv = originalDataTable.DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
                ResetFilters();
            }
            else
            {
                dv.RowFilter = $"Date_Added LIKE '%{searchText}%' OR Category LIKE '%{searchText}%'";
            }

            itemsDgv.DataSource = dv.ToTable();
            ValidateTextBoxForNumbersOnly(searchDateTxt);
        }

        private void searchCategoryCmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = searchCategoryCmb.SelectedItem?.ToString();
            string query = "SELECT * FROM items WHERE Category = @CategoryName";
            dbModule db = new dbModule();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", selectedCategory);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            itemsDgv.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFilters()
        {
            try
            {
                string query = "SELECT * FROM items";
                dbModule db = new dbModule();

                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            itemsDgv.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}

﻿using MySql.Data.MySqlClient;
using sims.Admin_Side.Inventory_Report;
using sims.Notification.Stock_notification;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace sims.Admin_Side.Stocks
{
    public partial class Manage_Stockk : Form
    {
        private Dashboard_Inventory _inventoryDashboard;
        private Add_Stock _addStock;
        private DashboardOwner _dashboardOwner;
        private Inventory_Reportt _reportt;

        public Manage_Stockk(Dashboard_Inventory inventoryDashboard, Add_Stock _addStock, DashboardOwner dashboardOwner, Inventory_Reportt reportt)
        {
            InitializeComponent();
            itemStockDgv.CellFormatting += itemStockDgv_CellFormatting;
            _inventoryDashboard = inventoryDashboard;
            _dashboardOwner = dashboardOwner;
            _reportt = reportt;
            itemStockDgv.DataBindingComplete += itemStockDgv_DataBindingComplete;
        }

        public DataGridView ItemsStockDgv
        {
            get { return itemStockDgv; }
        }

        public void Alert(string msg)
        {
            Stock_Deleted frm = new Stock_Deleted();
            frm.showalert(msg);
        }

        private byte[] ImageToByteArray(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "Image cannot be null.");

            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void Manage_Stockk_Load(object sender, EventArgs e)
        {
            ViewStock();
        }

        public void ViewStock()
        {
            dbModule db = new dbModule();
            MySqlConnection conn = db.GetConnection();
            MySqlCommand cmd = db.GetCommand();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dataTable = new DataTable();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM stocks";

                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);

                itemStockDgv.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to populate stock data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void previewStock()
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

        private void TotalSales()
        {
            if (_inventoryDashboard != null)
            {
                _inventoryDashboard.TotalSalesItems();
            }
            else
            {
                MessageBox.Show("Inventory Dashboard is not available.");
            }
        }

        private void StocksReport()
        {
            if (_reportt != null)
            {
                _reportt.PopulateStocks();
            }
        }
        
        private DataTable SearchInDatabase(string searchTerm)
        {
            DataTable dataTable = new DataTable();
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT Stock_ID, Item_ID, Item_Name, Category, Stock_In, Unit_Type, Date_Added, Item_Price, Item_Total, Item_Image " +
                               "FROM stocks WHERE Item_Name LIKE @SearchTerm";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        private void searchCategoryTxt_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = searchCategoryTxt.Text.Trim();

            // Search for results in the database
            DataTable searchResultDataTable = SearchInDatabase(searchTerm);

            // Bind the search results to the DataGridView
            itemStockDgv.DataSource = searchResultDataTable;

            // Clear selection in the DataGridView
            itemStockDgv.ClearSelection();
        }

        private void NewStockBtn_Click(object sender, EventArgs e)
        {
            // Check if the form is already open
            if (_addStock == null || _addStock.IsDisposed)
            {
                _addStock = new Add_Stock(this, _inventoryDashboard, _dashboardOwner, _reportt);
                _addStock.Show();
            }
            else
            {
                // If the form is already open, bring it to the front
                _addStock.BringToFront();
            }
        }

        private void DeleteStockBtn_Click(object sender, EventArgs e)
        {
            removeStock();
        }

        private void removeStock()
        {
            if (itemStockDgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete stock?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = itemStockDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = itemStockDgv.Rows[selectedRowIndex];
                    string selectedItemID = selectedRow.Cells["Stock_ID"]?.Value?.ToString();

                    if (!string.IsNullOrEmpty(selectedItemID))
                    {
                        DeleteRecord(selectedItemID);
                        itemStockDgv.Rows.RemoveAt(selectedRowIndex);
                        this.Alert("Stock successfully deleted.");
                        ViewStock();
                        previewStock();
                        TotalSales();
                        StocksReport();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Stock ID. Unable to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DeleteRecord(string stockID)
        {
            dbModule db = new dbModule();
            string query = "DELETE FROM stocks WHERE Stock_ID = @Stock_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Stock_ID", stockID);
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

        public void Notification(bool hasNotification)
        {
            if (_dashboardOwner == null)
            {
                MessageBox.Show("Dashboard owner is not set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_dashboardOwner.bellIcon == null)
            {
                MessageBox.Show("Bell icon is not set!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update the bell icon based on the notification state
            _dashboardOwner.bellIcon.Image = hasNotification
                ? Properties.Resources.Active_white // Set active icon
                : Properties.Resources.bell__1____white; // Set default icon

            if (hasNotification)
            {
                // Set a timer to reset the icon after 1 second
                Timer timer = new Timer();
                timer.Interval = 6000; // 6 seconds
                timer.Tick += (sender, e) =>
                {
                    _dashboardOwner.bellIcon.Image = Properties.Resources.bell__1____white; // Set default icon
                    timer.Stop(); // Stop the timer after it ticks
                };
                timer.Start();
            }
        }

        private void itemStockDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int stockColumnIndex = itemStockDgv.Columns["Stock_In"].Index;

            if (e.ColumnIndex == stockColumnIndex && e.Value != null)
            {
                int stockLevel = Convert.ToInt32(e.Value);

                // Define stock level thresholds
                int lowStockThreshold = 5;    // Stock is low if ≤ 0
                int normalStockThreshold = 30; // Stock is normal if > 5 and ≤ 30

                // Set the background color based on stock level
                if (stockLevel <= lowStockThreshold)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
                    Notification(true);
                }
                else if (stockLevel <= normalStockThreshold)
                {
                    e.CellStyle.BackColor = Color.Green;
                    e.CellStyle.ForeColor = Color.White;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Orange;
                    e.CellStyle.ForeColor = Color.Black;
                }
            }
        }

        private void UpdateStockBtn_Click(object sender, EventArgs e)
        {
            if (itemStockDgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a item to re-stock.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to re-stock this item?", "Update Item!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = itemStockDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = itemStockDgv.Rows[selectedRowIndex];
                    string itemID = selectedRow.Cells["Stock_ID"]?.Value?.ToString();
                    if (!string.IsNullOrEmpty(itemID))
                    {
                        Edit_Stock updateProductForm = new Edit_Stock(itemID, _inventoryDashboard, this);
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

        private void StocksReportBtn_Click(object sender, EventArgs e)
        {
            var inventoryReport = new Inventory_Reportt(this);
            inventoryReport.Show();
        }

        private void selectAllBtn_Click(object sender, EventArgs e)
        {
            // Check if all rows are already selected
            bool allSelected = itemStockDgv.SelectedRows.Count == itemStockDgv.Rows.Count;

            // Loop through all rows and toggle the selection
            foreach (DataGridViewRow row in itemStockDgv.Rows)
            {
                row.Selected = !allSelected;
            }
        }

        private void itemStockDgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            itemStockDgv.ClearSelection();
        }
    }
}
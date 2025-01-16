using LiveCharts.Wpf;
using LiveCharts;
using MySql.Data.MySqlClient;
using sims.Notification;
using sims.Notification.Stock_notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Stocks
{
    public partial class Manage_Stock : UserControl
    {
        private DataTable originalDataTable;
        private BindingSource bindingSource = new BindingSource();
        private Inventory_Dashboard stockChart;

        public Manage_Stock(Inventory_Dashboard stockChart)
        {
            InitializeComponent();
            itemStockDgv.CellFormatting += itemStockDgv_CellFormatting;
            this.stockChart = stockChart;
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

        private void Manage_Stock_Load(object sender, EventArgs e)
        {
            ViewStock();
            searchFunction();
        }

        private void ViewStock()
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
                    itemStockDgv.DataSource = dt;
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
            string query = "SELECT * FROM stocks";
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
                        itemStockDgv.DataSource = bindingSource;
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
                    itemStockDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChartStock()
        {
            dbModule db = new dbModule();
            SeriesCollection series = new SeriesCollection();
            List<string> itemNames = new List<string>();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Item_Name, Stock_In FROM stocks";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        ChartValues<int> values = new ChartValues<int>();

                        while (reader.Read())
                        {
                            string itemName = reader["Item_Name"]?.ToString() ?? string.Empty;
                            if (int.TryParse(reader["Stock_In"]?.ToString(), out int itemQuantity))
                            {
                                itemNames.Add(itemName);
                                values.Add(itemQuantity);
                            }
                        }

                        series.Add(new ColumnSeries
                        {
                            Title = "Items",
                            Values = values,
                            DataLabels = true
                        });
                    }
                }

                if (stockChart.StockChart != null)
                {
                    stockChart.StockChart.Series.Clear(); // Clear existing series
                    stockChart.StockChart.Series = series;

                    stockChart.StockChart.AxisX.Clear();
                    stockChart.StockChart.AxisX.Add(new Axis
                    {
                        Title = "Item Name",
                        Labels = itemNames
                    });

                    stockChart.StockChart.AxisY.Clear();
                    stockChart.StockChart.AxisY.Add(new Axis
                    {
                        Title = "Item Stocks"
                    });

                    stockChart.StockChart.Update(true, true); // Force redraw
                }
                else
                {
                    MessageBox.Show("Cartesian chart is not initialized!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void NewStockBtn_Click(object sender, EventArgs e)
        {
            Inventory_Dashboard dashboardInventory = new Inventory_Dashboard();
            Add_Stock add_Stock = new Add_Stock(this, dashboardInventory);
            add_Stock.Show();
        }

        private void UpdateStockBtn_Click(object sender, EventArgs e)
        {
            Edit_Stock edit_Stock = new Edit_Stock();
            edit_Stock.Show();
        }

        private void DeleteStockBtn_Click(object sender, EventArgs e)
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
                        Populate();
                        ChartStock();
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

        private void searchCategoryTxt_TextChanged(object sender, EventArgs e)
        {
            if (originalDataTable == null) return;
            string searchText = searchCategoryTxt.Text.Trim();
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

            itemStockDgv.DataSource = dv.ToTable();
        }

        private void ResetFilters()
        {
            try
            {
                string query = "SELECT * FROM stocks";
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
                            itemStockDgv.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void itemStockDgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int stockColumnIndex = itemStockDgv.Columns["Stock_In"].Index;

            if (e.ColumnIndex == stockColumnIndex && e.Value != null)
            {
                int stockLevel = Convert.ToInt32(e.Value);

                // Define stock level thresholds
                int lowStockThreshold = 5;    // Example: stock is low if ≤ 10
                int normalStockThreshold = 30; // Example: stock is normal if > 10 and ≤ 50

                // Set the background color based on stock level
                if (stockLevel <= lowStockThreshold)
                {
                    e.CellStyle.BackColor = Color.Red;
                    e.CellStyle.ForeColor = Color.White;
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
    }
}

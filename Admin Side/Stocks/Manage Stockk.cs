using MySql.Data.MySqlClient;
using sims.Notification.Stock_notification;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace sims.Admin_Side.Stocks
{
    public partial class Manage_Stockk : Form
    {
        public Manage_Stockk()
        {
            InitializeComponent();
            itemStockDgv.CellFormatting += itemStockDgv_CellFormatting;
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

        private void Manage_Stockk_Load(object sender, EventArgs e)
        {
            ViewStock();
        }

        private void ViewStock()
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
        private DataTable SearchInDatabase(string searchTerm)
        {
            DataTable dataTable = new DataTable();
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT Stock_ID, Item_ID, Item_Name, Stock_In, Unit_Type, Date_Added, Item_Price, Item_Total, Item_Image " +
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
            Add_Stock add_Stock = new Add_Stock(this);
            add_Stock.Show();
        }

        private void UpdateStockBtn_Click(object sender, EventArgs e)
        {

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

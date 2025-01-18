using MySql.Data.MySqlClient;
using sims.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Category
{
    public partial class Manage_Categoryy : Form
    {
        public DataTable originalDataTable;

        public Manage_Categoryy()
        {
            InitializeComponent();
        }

        public DataGridView RecentlyAddedDgv
        {
            get { return recentlyAddedDgv; }
        }

        public void Alert(string msg)
        {
            Category_Deleted frm = new Category_Deleted();
            frm.showalert(msg);
        }

        private void Manage_Categoryy_Load(object sender, EventArgs e)
        {
            PopulateData();
        }

        private void PopulateData()
        {
            dbModule db = new dbModule();
            MySqlDataAdapter adapter = db.GetAdapter();

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM categories";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Use the adapter to fill the DataTable
                        DataTable dataTable = new DataTable();
                        adapter.SelectCommand = cmd; // Set the command to the adapter
                        adapter.Fill(dataTable);     // Fill the DataTable

                        recentlyAddedDgv.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void ResetFilters()
        {
            try
            {
                dbModule db = new dbModule();
                string query = "SELECT * FROM categories";

                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            recentlyAddedDgv.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting filters: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable SearchInDatabase(string searchTerm)
        {
            DataTable dataTable = new DataTable();
            dbModule db = new dbModule();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT Category_ID, Category_Name, Category_Description " +
                               "FROM categories WHERE Category_Name LIKE @SearchTerm";

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
            recentlyAddedDgv.DataSource = searchResultDataTable;

            // Clear selection in the DataGridView
            recentlyAddedDgv.ClearSelection();
        }

        private void newCategoryBtn_Click(object sender, EventArgs e)
        {
            New_Category newCategory = new New_Category(this);
            newCategory.Show();
        }

        private void editCategoryBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to update this record?", "Update Category?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = recentlyAddedDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = recentlyAddedDgv.Rows[selectedRowIndex];
                    string itemID = selectedRow.Cells["Category_ID"]?.Value?.ToString();
                    if (!string.IsNullOrEmpty(itemID))
                    {
                        Edit_Category updateCategoryForm = new Edit_Category(itemID, this, this);
                        updateCategoryForm.Show();
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

        private void DeleteCategoryBtn_Click(object sender, EventArgs e)
        {
            if (recentlyAddedDgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Please select a record to delete.", "Notice!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedRowIndex = recentlyAddedDgv.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = recentlyAddedDgv.Rows[selectedRowIndex];
                    string selectedItemID = selectedRow.Cells["Category_ID"]?.Value?.ToString();

                    if (!string.IsNullOrEmpty(selectedItemID))
                    {
                        DeleteRecord(selectedItemID);
                        recentlyAddedDgv.Rows.RemoveAt(selectedRowIndex);
                        //MessageBox.Show("Item successfully deleted.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Alert("Category successfully deleted.");
                        //searchFunction();
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
        private void DeleteRecord(string categoryID)
        {
            dbModule db = new dbModule();
            string query = "DELETE FROM categories WHERE Category_ID = @Category_ID";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category_ID", categoryID);
                        int rowsAffected = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error while deleting the record: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
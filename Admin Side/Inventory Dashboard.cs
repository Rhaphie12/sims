using LiveCharts.Wpf;
using LiveCharts;
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
using sims.Admin_Side.Items;
using System.Security.Permissions;

namespace sims.Admin_Side
{
    public partial class Inventory_Dashboard : Form
    {
        public Inventory_Dashboard()
        {
            InitializeComponent();
        }

        private void Inventory_Dashboard_Load(object sender, EventArgs e)
        {
            StockPreview();
            ItemsCount();
            CategoriesCount();
        }

        public void CategoriesCount()
        {
            dbModule db = new dbModule();
            string query = "SELECT COUNT(*) FROM categories";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                        categoriesCountTxt.Text = itemCount.ToString();
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

        public void ItemsCount()
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
                        itemsCountTxt.Text = itemCount.ToString();
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

        public void StockPreview()
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

                if (stockPreviewChart != null)
                {
                    stockPreviewChart.Series.Clear();
                    stockPreviewChart.Series = series;

                    stockPreviewChart.AxisX.Clear();
                    stockPreviewChart.AxisX.Add(new Axis
                    {
                        Title = "Item Name",
                        Labels = itemNames
                    });

                    stockPreviewChart.AxisY.Clear();
                    stockPreviewChart.AxisY.Add(new Axis
                    {
                        Title = "Item Stocks"
                    });

                    stockPreviewChart.Update(true, true);
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
    }
}

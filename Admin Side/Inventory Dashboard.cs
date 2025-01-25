using LiveCharts;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

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
            TotalSalesItems();
            ProductsCount();
            TotalSalesCoffeePreview();
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
                        categoriesCountLbl.Text = itemCount.ToString();
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
                        itemsCountLbl.Text = itemCount.ToString();
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
        public void ProductsCount()
        {
            dbModule db = new dbModule();
            string query = "SELECT COUNT(*) FROM products";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        int itemCount = Convert.ToInt32(cmd.ExecuteScalar());
                        productsCountLbl.Text = itemCount.ToString();
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

        public void TotalSalesItems()
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

                // Query to get all stocks and calculate the total of Item_Total
                cmd.CommandText = "SELECT *, SUM(Item_Total) AS TotalSales FROM stocks";

                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);

                //activityLogsBtn.DataSource = dataTable;

                // Check if the TotalSales column is present in the result
                if (dataTable.Rows.Count > 0 && dataTable.Columns.Contains("TotalSales"))
                {
                    object totalSalesValue = dataTable.Rows[0]["TotalSales"];
                    if (decimal.TryParse(totalSalesValue?.ToString(), out decimal totalSales))
                    {
                        // Format the total sales value with a peso sign
                        totalSalesLbl.Text = $"₱ {totalSales:0.00}";
                    }
                    else
                    {
                        totalSalesLbl.Text = "₱ 0.00";
                    }
                }
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
                    // Clear existing chart data
                    stockPreviewChart.Series.Clear();
                    stockPreviewChart.Series = series;

                    stockPreviewChart.AxisX.Clear();
                    stockPreviewChart.AxisX.Add(new Axis
                    {
                        Title = "Item Name",
                        Labels = itemNames,
                        Separator = new Separator
                        {
                            Step = 1 // Step controls the interval of labels shown
                        }
                    });

                    stockPreviewChart.AxisY.Clear();
                    stockPreviewChart.AxisY.Add(new Axis
                    {
                        Title = "Item Stocks"
                    });

                    // Set dynamic range for X-axis
                    stockPreviewChart.AxisX[0].MinValue = 0;
                    stockPreviewChart.AxisX[0].MaxValue = 10; // Initially display 10 items

                    // Attach event to dynamically update MinValue and MaxValue during scroll/zoom
                    stockPreviewChart.DataClick += (sender, args) =>
                    {
                        double viewWidth = stockPreviewChart.AxisX[0].MaxValue - stockPreviewChart.AxisX[0].MinValue;
                        double totalItems = itemNames.Count;

                        if (totalItems > viewWidth)
                        {
                            stockPreviewChart.AxisX[0].MinValue = Math.Max(0, stockPreviewChart.AxisX[0].MinValue);
                            stockPreviewChart.AxisX[0].MaxValue = Math.Min(totalItems, stockPreviewChart.AxisX[0].MaxValue);
                        }
                    };

                    // Update the chart
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

        public void TotalSalesCoffeePreview()
        {
            dbModule db = new dbModule();
            SeriesCollection series = new SeriesCollection();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Product_Name, SUM(Total_Product_Sale) AS TotalSales FROM productsales_coffee GROUP BY Product_Name";
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader["Product_Name"]?.ToString() ?? "Unknown Product";
                            if (decimal.TryParse(reader["TotalSales"]?.ToString(), out decimal totalSales))
                            {
                                series.Add(new PieSeries
                                {
                                    Title = productName,
                                    Values = new ChartValues<decimal> { totalSales },
                                    DataLabels = true
                                });
                            }
                        }
                    }
                }

                if (coffeeSalesChart != null)
                {
                    // Clear and set the pie chart series
                    coffeeSalesChart.Series.Clear();
                    coffeeSalesChart.Series = series;

                    // Optionally update chart properties
                    coffeeSalesChart.LegendLocation = LegendLocation.Bottom;
                    coffeeSalesChart.Width = 700; // Increase chart width
                    coffeeSalesChart.Height = 700; // Increase chart height
                    coffeeSalesChart.Update(true, true);
                }
                else
                {
                    MessageBox.Show("Pie chart is not initialized!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
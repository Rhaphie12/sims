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

namespace sims.Admin_Side.Products
{
    public partial class Sales_History_Form : Form
    {
        public Sales_History_Form()
        {
            InitializeComponent();
        }

        private void Sales_History_Form_Load(object sender, EventArgs e)
        {
            MonthlySalesHistoryPreview("productsaleshistory_coffee");
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void MonthlySalesHistoryPreview(string tableName)
        {
            dbModule db = new dbModule();

            try
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = $"SELECT * FROM {tableName}";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    salesHistoryDgv.DataSource = dt;
                }

                // Update the title label
                chartTitleLabel.Text = $"{tableName}";
                chartTitleLabel.Font = new Font("Poppins", 11);
                chartTitleLabel.TextAlign = ContentAlignment.MiddleCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales history: " + ex.Message);
            }
        }

        private void CoffeeMonthlyChart_Click(object sender, EventArgs e)
        {
            MonthlySalesHistoryPreview("productsaleshistory_coffee");
        }

        private void NonCoffeeMonthlyChart_Click(object sender, EventArgs e)
        {
            MonthlySalesHistoryPreview("productsaleshistory_noncoffee");
        }

        private void HotCoffeeMonthlyChart_Click(object sender, EventArgs e)
        {
            MonthlySalesHistoryPreview("productsaleshistory_hotcoffee");
        }

        private void pastriesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MonthlySalesHistoryPreview("productsaleshistory_pastries");
        }

    }
}

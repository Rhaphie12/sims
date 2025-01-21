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
using static sims.Admin_Side.Sales.Add_Product;

namespace sims.Admin_Side.Sales
{
    public partial class Product_Saless : Form
    {
        public Product_Saless()
        {
            InitializeComponent();
        }

        public FlowLayoutPanel CoffeeLayoutPanel
        {
            get { return coffeeLayoutPanel; }
        }

        private void Product_Saless_Load(object sender, EventArgs e)
        {

        }
    }
}

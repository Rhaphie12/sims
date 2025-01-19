using MySql.Data.MySqlClient;
using sims.Admin_Side.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Bunifu.UI.WinForms;

namespace sims.Admin_Side.Stocks
{
    public partial class Edit_Stock : Form
    {
        private string _itemID;

        public Edit_Stock(string itemID)
        {
            InitializeComponent();
            _itemID = itemID;
        }

        private void Edit_Stock_Load(object sender, EventArgs e)
        {

        }

        private void updateStockBtn_Click(object sender, EventArgs e)
        {

        }

        private void editStock()
        {

        }

        private void backNewStockBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void totalInfoBtn_Click(object sender, EventArgs e)
        {

        }
    }
}

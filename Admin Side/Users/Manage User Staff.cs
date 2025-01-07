using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims.Admin_Side.Users
{
    public partial class Manage_User_Staff : UserControl
    {
        public Manage_User_Staff()
        {
            InitializeComponent();
        }

        private void NewStaffBtn_Click(object sender, EventArgs e)
        {
            new New_Staff().Show();
        }
    }
}

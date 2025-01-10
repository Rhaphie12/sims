using FontAwesome.Sharp;
using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using sims.Admin_Side;
using sims.Admin_Side.Category;
using sims.Admin_Side.Inventory_Report;
using sims.Admin_Side.Items;
using sims.Admin_Side.Sales;
using sims.Admin_Side.Sales_Report;
using sims.Admin_Side.Stocks;
using sims.Admin_Side.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sims
{
    public partial class DashboardOwner : Form
    {
        private IconButton currentBtn;
        private GunaPanel leftBorderBtn;

        public DashboardOwner()
        {
            InitializeComponent();
            ShowUsernameWithGreeting();
            customizeDesign();
            leftBorderBtn = new GunaPanel
            {
                Size = new Size(10, 58)
            };
            PanelMenu.Controls.Add(leftBorderBtn);
        }

        private void ShowUsernameWithGreeting()
        {
            dbModule db = new dbModule();
            string query = "SELECT username FROM users LIMIT 1";

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string username = result.ToString();
                            greetingNameTxt.Text = $"HI! {username},";
                        }
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

        private void customizeDesign()
        {
            InventoryPanelSubMenu.Visible = false;
        }

        private void hideSubMenu()
        {
            if (InventoryPanelSubMenu.Visible == true)
                InventoryPanelSubMenu.Visible = false;
        }

        private void showSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                hideSubMenu();
                subMenu.Visible = true;
            }
            else
                subMenu.Visible = false;
        }

        private void ActivateButton(object senderBtn, Color customColor)
        {
            if (senderBtn != null)
            {
                DisableBtn();

                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(222, 196, 125);
                currentBtn.ForeColor = customColor;
                currentBtn.IconColor = customColor;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;

                leftBorderBtn.BackColor = customColor;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
            }
        }
        private void OpeninPanel(object formOpen)
        {
            // Clear existing controls in the DashboardPanel
            if (DashboardPanel.Controls.Count > 0)
            {
                DashboardPanel.Controls.RemoveAt(0);
            }

            // Check if the object is a UserControl
            if (formOpen is UserControl uc)
            {
                uc.Dock = DockStyle.Fill;
                DashboardPanel.Controls.Add(uc);
                DashboardPanel.Tag = uc;
            }
            // Check if the object is a Form
            else if (formOpen is Form dh)
            {
                dh.TopLevel = false;
                dh.FormBorderStyle = FormBorderStyle.None;
                dh.Dock = DockStyle.Fill;
                DashboardPanel.Controls.Add(dh);
                DashboardPanel.Tag = dh;
                dh.Show();
            }
        }

        private void DisableBtn()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(255, 255, 255);
                currentBtn.ForeColor = Color.Black;
                currentBtn.IconColor = Color.Black;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
            }
        }

        private void DashboardBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Dashboard_Inventory());
        }

        private void CategoriesBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_Category());
        }

        private void inventoryBtn_Click(object sender, EventArgs e)
        {
            showSubMenu(InventoryPanelSubMenu);
        }

        private void ItemsBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_Items());

        }

        private void StocksBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_Stock());

        }

        private void inventoryReport_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Inventory_Report());
        }

        private void SalesBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_Sales());

        }

        private void SalesReportBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_Sales_Report());

        }

        private void UserBtn_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.FromArgb(255, 255, 255));
            OpeninPanel(new Manage_User_Staff());
        }

        private void SignoutBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout?",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.OK)
            {
                this.Hide();
                new Login_Form().Show();
            }
        }
    }
}

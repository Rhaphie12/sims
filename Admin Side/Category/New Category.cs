using Guna.UI.WinForms;
using MySql.Data.MySqlClient;
using sims.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace sims.Admin_Side.Category
{
    public partial class New_Category : Form
    {
        public DataTable originalDataTable;
        private BindingSource bindingSource = new BindingSource();

        private Manage_Categoryy dashboardForm;

        public New_Category(Manage_Categoryy dashboardForm)
        {
            InitializeComponent();
            this.dashboardForm = dashboardForm;
        }

        public class Categories
        {
            public int CategoryID { get; set; }
            public string Category { get; set; }
        }

        public void Alert(string msg)
        {
            Category_Added frm = new Category_Added();
            frm.showalert(msg);
        }

        private void New_Category_Load(object sender, EventArgs e)
        {
            GenerateRandomItemID();
            Populate();
            //searchFunction();
        }

        private void GenerateRandomItemID()
        {
            Random random = new Random();
            int randomNumber = random.Next(10000000, 99999999);
            categoryIDTxt.Text = randomNumber.ToString();
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
                    string query = "SELECT * FROM categories";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    adapter.SelectCommand = command;
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dashboardForm.RecentlyAddedDgv.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void searchFunction()
        //{
        //    dbModule db = new dbModule();
        //    string query = "SELECT * FROM categories";
        //    MySqlConnection conn = null;

        //    try
        //    {
        //        conn = db.GetConnection();
        //        conn.Open();

        //        using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn))
        //        {
        //            DataTable dataTable = new DataTable();
        //            adapter.Fill(dataTable);
        //            originalDataTable = dataTable;
        //            bindingSource.DataSource = originalDataTable;
        //            dashboardForm.RecentlyAddedDgv.DataSource = bindingSource;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (conn != null && conn.State == ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //}

        private void addCategoryBtn_Click(object sender, EventArgs e)
        {
            addCategory();
        }

        private void addCategory()
        {
            dbModule db = new dbModule();

            string categoryID = categoryIDTxt.Text.Trim();
            string categoryName = categoryNameTxt.Text.Trim();
            string categoryDescription = categoryDescriptionTxt.Text.Trim();

            if (string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(categoryDescription))
            {
                new Messages_Boxes.Field_Required().Show();
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO categories (Category_ID, Category_Name, Category_Description) VALUES (@Category_ID, @Category_Name, @Category_Description)";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Category_ID", categoryID);
                        cmd.Parameters.AddWithValue("@Category_Name", categoryName);
                        cmd.Parameters.AddWithValue("@Category_Description", categoryDescription);
                        cmd.ExecuteNonQuery();

                        this.Hide();
                        categoryNameTxt.Clear();
                        categoryDescriptionTxt.Clear();
                        GenerateRandomItemID();
                        this.Alert("Category Added Successfully");
                        Populate();
                        //searchFunction();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding category: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void backNewCatBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void categoryNameTxt_TextChanged(object sender, EventArgs e)
        {
            string newText = categoryNameTxt.Text;

            if (System.Text.RegularExpressions.Regex.IsMatch(newText, @"\d"))
            {
                MessageBox.Show("Numbers are not allowed!", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                categoryNameTxt.Text = System.Text.RegularExpressions.Regex.Replace(newText, @"\d", "");
                categoryNameTxt.SelectionStart = categoryNameTxt.Text.Length;
            }
        }
    }
}

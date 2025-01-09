﻿namespace sims.Admin_Side.Stocks
{
    partial class Manage_Stock
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Manage_Stock));
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextBox.StateProperties();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gunaElipsePanel2 = new Guna.UI.WinForms.GunaElipsePanel();
            this.label4 = new System.Windows.Forms.Label();
            this.gunaGroupBox2 = new Guna.UI.WinForms.GunaGroupBox();
            this.searchCategoryTxt = new Bunifu.UI.WinForms.BunifuTextBox();
            this.DeleteStockBtn = new Guna.UI.WinForms.GunaButton();
            this.UpdateStockBtn = new Guna.UI.WinForms.GunaButton();
            this.NewStockBtn = new Guna.UI.WinForms.GunaButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.itemStockDgv = new Guna.UI.WinForms.GunaDataGridView();
            this.Column8 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Stock_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock_In = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock_Out = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight_Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date_Added = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.gunaElipsePanel2.SuspendLayout();
            this.gunaGroupBox2.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.itemStockDgv)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gunaElipsePanel2);
            this.panel1.Location = new System.Drawing.Point(11, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1170, 53);
            this.panel1.TabIndex = 42;
            // 
            // gunaElipsePanel2
            // 
            this.gunaElipsePanel2.BackColor = System.Drawing.Color.Transparent;
            this.gunaElipsePanel2.BaseColor = System.Drawing.Color.White;
            this.gunaElipsePanel2.Controls.Add(this.label4);
            this.gunaElipsePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gunaElipsePanel2.Location = new System.Drawing.Point(0, 0);
            this.gunaElipsePanel2.Margin = new System.Windows.Forms.Padding(0);
            this.gunaElipsePanel2.Name = "gunaElipsePanel2";
            this.gunaElipsePanel2.Radius = 7;
            this.gunaElipsePanel2.Size = new System.Drawing.Size(1170, 53);
            this.gunaElipsePanel2.TabIndex = 46;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Poppins", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(10, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(286, 34);
            this.label4.TabIndex = 21;
            this.label4.Text = "Dashboard / Manage Stocks";
            // 
            // gunaGroupBox2
            // 
            this.gunaGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.gunaGroupBox2.BaseColor = System.Drawing.Color.White;
            this.gunaGroupBox2.BorderColor = System.Drawing.Color.Gainsboro;
            this.gunaGroupBox2.Controls.Add(this.searchCategoryTxt);
            this.gunaGroupBox2.Controls.Add(this.DeleteStockBtn);
            this.gunaGroupBox2.Controls.Add(this.UpdateStockBtn);
            this.gunaGroupBox2.Controls.Add(this.NewStockBtn);
            this.gunaGroupBox2.Controls.Add(this.panel5);
            this.gunaGroupBox2.Font = new System.Drawing.Font("Poppins", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gunaGroupBox2.LineColor = System.Drawing.Color.White;
            this.gunaGroupBox2.LineTop = 0;
            this.gunaGroupBox2.Location = new System.Drawing.Point(11, 76);
            this.gunaGroupBox2.Name = "gunaGroupBox2";
            this.gunaGroupBox2.Radius = 10;
            this.gunaGroupBox2.Size = new System.Drawing.Size(1171, 607);
            this.gunaGroupBox2.TabIndex = 49;
            this.gunaGroupBox2.Text = "Item Stocks";
            this.gunaGroupBox2.TextLocation = new System.Drawing.Point(10, 15);
            this.gunaGroupBox2.TextRenderingHint = Guna.UI.WinForms.DrawingTextRenderingHint.AntiAlias;
            // 
            // searchCategoryTxt
            // 
            this.searchCategoryTxt.AcceptsReturn = false;
            this.searchCategoryTxt.AcceptsTab = false;
            this.searchCategoryTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchCategoryTxt.AnimationSpeed = 200;
            this.searchCategoryTxt.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.searchCategoryTxt.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.searchCategoryTxt.BackColor = System.Drawing.Color.Transparent;
            this.searchCategoryTxt.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("searchCategoryTxt.BackgroundImage")));
            this.searchCategoryTxt.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.searchCategoryTxt.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.searchCategoryTxt.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.searchCategoryTxt.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.searchCategoryTxt.BorderRadius = 30;
            this.searchCategoryTxt.BorderThickness = 1;
            this.searchCategoryTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.searchCategoryTxt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.searchCategoryTxt.DefaultFont = new System.Drawing.Font("Poppins", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchCategoryTxt.DefaultText = "";
            this.searchCategoryTxt.FillColor = System.Drawing.Color.White;
            this.searchCategoryTxt.HideSelection = true;
            this.searchCategoryTxt.IconLeft = null;
            this.searchCategoryTxt.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.searchCategoryTxt.IconPadding = 10;
            this.searchCategoryTxt.IconRight = null;
            this.searchCategoryTxt.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.searchCategoryTxt.Lines = new string[0];
            this.searchCategoryTxt.Location = new System.Drawing.Point(878, 11);
            this.searchCategoryTxt.MaximumSize = new System.Drawing.Size(282, 35);
            this.searchCategoryTxt.MaxLength = 32767;
            this.searchCategoryTxt.MinimumSize = new System.Drawing.Size(282, 35);
            this.searchCategoryTxt.Modified = false;
            this.searchCategoryTxt.Multiline = false;
            this.searchCategoryTxt.Name = "searchCategoryTxt";
            stateProperties1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.searchCategoryTxt.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.searchCategoryTxt.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.searchCategoryTxt.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            stateProperties4.FillColor = System.Drawing.Color.White;
            stateProperties4.ForeColor = System.Drawing.Color.Empty;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.searchCategoryTxt.OnIdleState = stateProperties4;
            this.searchCategoryTxt.Padding = new System.Windows.Forms.Padding(3);
            this.searchCategoryTxt.PasswordChar = '\0';
            this.searchCategoryTxt.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.searchCategoryTxt.PlaceholderText = "Search by Item ";
            this.searchCategoryTxt.ReadOnly = false;
            this.searchCategoryTxt.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.searchCategoryTxt.SelectedText = "";
            this.searchCategoryTxt.SelectionLength = 0;
            this.searchCategoryTxt.SelectionStart = 0;
            this.searchCategoryTxt.ShortcutsEnabled = true;
            this.searchCategoryTxt.Size = new System.Drawing.Size(282, 35);
            this.searchCategoryTxt.Style = Bunifu.UI.WinForms.BunifuTextBox._Style.Bunifu;
            this.searchCategoryTxt.TabIndex = 37;
            this.searchCategoryTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.searchCategoryTxt.TextMarginBottom = 0;
            this.searchCategoryTxt.TextMarginLeft = 3;
            this.searchCategoryTxt.TextMarginTop = 0;
            this.searchCategoryTxt.TextPlaceholder = "Search by Item ";
            this.searchCategoryTxt.UseSystemPasswordChar = false;
            this.searchCategoryTxt.WordWrap = true;
            this.searchCategoryTxt.TextChanged += new System.EventHandler(this.searchCategoryTxt_TextChanged);
            // 
            // DeleteStockBtn
            // 
            this.DeleteStockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteStockBtn.AnimationHoverSpeed = 0.07F;
            this.DeleteStockBtn.AnimationSpeed = 0.03F;
            this.DeleteStockBtn.BackColor = System.Drawing.Color.Transparent;
            this.DeleteStockBtn.BaseColor = System.Drawing.Color.Transparent;
            this.DeleteStockBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.DeleteStockBtn.BorderSize = 2;
            this.DeleteStockBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteStockBtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.DeleteStockBtn.FocusedColor = System.Drawing.Color.Empty;
            this.DeleteStockBtn.Font = new System.Drawing.Font("Poppins", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteStockBtn.ForeColor = System.Drawing.Color.Black;
            this.DeleteStockBtn.Image = null;
            this.DeleteStockBtn.ImageSize = new System.Drawing.Size(20, 20);
            this.DeleteStockBtn.Location = new System.Drawing.Point(1013, 552);
            this.DeleteStockBtn.Name = "DeleteStockBtn";
            this.DeleteStockBtn.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.DeleteStockBtn.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.DeleteStockBtn.OnHoverForeColor = System.Drawing.Color.White;
            this.DeleteStockBtn.OnHoverImage = null;
            this.DeleteStockBtn.OnPressedColor = System.Drawing.Color.White;
            this.DeleteStockBtn.Radius = 6;
            this.DeleteStockBtn.Size = new System.Drawing.Size(147, 35);
            this.DeleteStockBtn.TabIndex = 36;
            this.DeleteStockBtn.Text = "Delete Stock";
            this.DeleteStockBtn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.DeleteStockBtn.TextRenderingHint = Guna.UI.WinForms.DrawingTextRenderingHint.AntiAlias;
            this.DeleteStockBtn.Click += new System.EventHandler(this.DeleteStockBtn_Click);
            // 
            // UpdateStockBtn
            // 
            this.UpdateStockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdateStockBtn.AnimationHoverSpeed = 0.07F;
            this.UpdateStockBtn.AnimationSpeed = 0.03F;
            this.UpdateStockBtn.BackColor = System.Drawing.Color.Transparent;
            this.UpdateStockBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.UpdateStockBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.UpdateStockBtn.BorderSize = 2;
            this.UpdateStockBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.UpdateStockBtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.UpdateStockBtn.FocusedColor = System.Drawing.Color.Empty;
            this.UpdateStockBtn.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateStockBtn.ForeColor = System.Drawing.Color.White;
            this.UpdateStockBtn.Image = null;
            this.UpdateStockBtn.ImageSize = new System.Drawing.Size(20, 20);
            this.UpdateStockBtn.Location = new System.Drawing.Point(860, 552);
            this.UpdateStockBtn.Name = "UpdateStockBtn";
            this.UpdateStockBtn.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.UpdateStockBtn.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.UpdateStockBtn.OnHoverForeColor = System.Drawing.Color.White;
            this.UpdateStockBtn.OnHoverImage = null;
            this.UpdateStockBtn.OnPressedColor = System.Drawing.Color.White;
            this.UpdateStockBtn.Radius = 4;
            this.UpdateStockBtn.Size = new System.Drawing.Size(147, 35);
            this.UpdateStockBtn.TabIndex = 34;
            this.UpdateStockBtn.Text = "Edit Stock";
            this.UpdateStockBtn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UpdateStockBtn.TextRenderingHint = Guna.UI.WinForms.DrawingTextRenderingHint.AntiAlias;
            this.UpdateStockBtn.Click += new System.EventHandler(this.UpdateStockBtn_Click);
            // 
            // NewStockBtn
            // 
            this.NewStockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NewStockBtn.AnimationHoverSpeed = 0.07F;
            this.NewStockBtn.AnimationSpeed = 0.03F;
            this.NewStockBtn.BackColor = System.Drawing.Color.Transparent;
            this.NewStockBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.NewStockBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.NewStockBtn.BorderSize = 2;
            this.NewStockBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NewStockBtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.NewStockBtn.FocusedColor = System.Drawing.Color.Empty;
            this.NewStockBtn.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewStockBtn.ForeColor = System.Drawing.Color.White;
            this.NewStockBtn.Image = null;
            this.NewStockBtn.ImageSize = new System.Drawing.Size(20, 20);
            this.NewStockBtn.Location = new System.Drawing.Point(707, 552);
            this.NewStockBtn.Name = "NewStockBtn";
            this.NewStockBtn.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.NewStockBtn.OnHoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.NewStockBtn.OnHoverForeColor = System.Drawing.Color.White;
            this.NewStockBtn.OnHoverImage = null;
            this.NewStockBtn.OnPressedColor = System.Drawing.Color.White;
            this.NewStockBtn.Radius = 5;
            this.NewStockBtn.Size = new System.Drawing.Size(147, 35);
            this.NewStockBtn.TabIndex = 35;
            this.NewStockBtn.Text = "New Stock";
            this.NewStockBtn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NewStockBtn.TextRenderingHint = Guna.UI.WinForms.DrawingTextRenderingHint.AntiAlias;
            this.NewStockBtn.Click += new System.EventHandler(this.NewStockBtn_Click);
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.itemStockDgv);
            this.panel5.Location = new System.Drawing.Point(11, 59);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1149, 473);
            this.panel5.TabIndex = 31;
            // 
            // itemStockDgv
            // 
            this.itemStockDgv.AllowUserToAddRows = false;
            this.itemStockDgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.itemStockDgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.itemStockDgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.itemStockDgv.BackgroundColor = System.Drawing.Color.White;
            this.itemStockDgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.itemStockDgv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.itemStockDgv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.itemStockDgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.itemStockDgv.ColumnHeadersHeight = 40;
            this.itemStockDgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column8,
            this.Stock_ID,
            this.Item_ID,
            this.Column2,
            this.Stock_In,
            this.Stock_Out,
            this.Unit_Type,
            this.Weight_Unit,
            this.Date_Added,
            this.Item_Price,
            this.Item_Total});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.itemStockDgv.DefaultCellStyle = dataGridViewCellStyle3;
            this.itemStockDgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemStockDgv.EnableHeadersVisualStyles = false;
            this.itemStockDgv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.itemStockDgv.Location = new System.Drawing.Point(0, 0);
            this.itemStockDgv.Name = "itemStockDgv";
            this.itemStockDgv.ReadOnly = true;
            this.itemStockDgv.RowHeadersVisible = false;
            this.itemStockDgv.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.itemStockDgv.RowTemplate.Height = 100;
            this.itemStockDgv.RowTemplate.ReadOnly = true;
            this.itemStockDgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.itemStockDgv.Size = new System.Drawing.Size(1147, 471);
            this.itemStockDgv.TabIndex = 3;
            this.itemStockDgv.Theme = Guna.UI.WinForms.GunaDataGridViewPresetThemes.Guna;
            this.itemStockDgv.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.itemStockDgv.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.itemStockDgv.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.itemStockDgv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.itemStockDgv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.itemStockDgv.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.itemStockDgv.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.itemStockDgv.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(178)))), ((int)(((byte)(84)))));
            this.itemStockDgv.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.itemStockDgv.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemStockDgv.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.itemStockDgv.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.itemStockDgv.ThemeStyle.HeaderStyle.Height = 40;
            this.itemStockDgv.ThemeStyle.ReadOnly = true;
            this.itemStockDgv.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.itemStockDgv.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.itemStockDgv.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemStockDgv.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.itemStockDgv.ThemeStyle.RowsStyle.Height = 100;
            this.itemStockDgv.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.itemStockDgv.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "Item_Image";
            this.Column8.HeaderText = "Item Image";
            this.Column8.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Stock_ID
            // 
            this.Stock_ID.DataPropertyName = "Stock_ID";
            this.Stock_ID.HeaderText = "Stock ID";
            this.Stock_ID.Name = "Stock_ID";
            this.Stock_ID.ReadOnly = true;
            this.Stock_ID.Visible = false;
            // 
            // Item_ID
            // 
            this.Item_ID.DataPropertyName = "Item_ID";
            this.Item_ID.HeaderText = "Item ID";
            this.Item_ID.Name = "Item_ID";
            this.Item_ID.ReadOnly = true;
            this.Item_ID.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Item_Name";
            this.Column2.HeaderText = "Item Name";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Stock_In
            // 
            this.Stock_In.DataPropertyName = "Stock_In";
            this.Stock_In.HeaderText = "Stock IN";
            this.Stock_In.Name = "Stock_In";
            this.Stock_In.ReadOnly = true;
            // 
            // Stock_Out
            // 
            this.Stock_Out.DataPropertyName = "Stock_Out";
            this.Stock_Out.HeaderText = "Stock OUT";
            this.Stock_Out.Name = "Stock_Out";
            this.Stock_Out.ReadOnly = true;
            this.Stock_Out.Visible = false;
            // 
            // Unit_Type
            // 
            this.Unit_Type.DataPropertyName = "Unit_Type";
            this.Unit_Type.HeaderText = "Unit Type";
            this.Unit_Type.Name = "Unit_Type";
            this.Unit_Type.ReadOnly = true;
            // 
            // Weight_Unit
            // 
            this.Weight_Unit.DataPropertyName = "Weight_Unit";
            this.Weight_Unit.HeaderText = "Weight Unit";
            this.Weight_Unit.Name = "Weight_Unit";
            this.Weight_Unit.ReadOnly = true;
            // 
            // Date_Added
            // 
            this.Date_Added.DataPropertyName = "Date_Added";
            this.Date_Added.HeaderText = "Date Added";
            this.Date_Added.Name = "Date_Added";
            this.Date_Added.ReadOnly = true;
            // 
            // Item_Price
            // 
            this.Item_Price.DataPropertyName = "Item_Price";
            this.Item_Price.HeaderText = "Item Price";
            this.Item_Price.Name = "Item_Price";
            this.Item_Price.ReadOnly = true;
            // 
            // Item_Total
            // 
            this.Item_Total.DataPropertyName = "Item_Total";
            this.Item_Total.HeaderText = "Item Total";
            this.Item_Total.Name = "Item_Total";
            this.Item_Total.ReadOnly = true;
            // 
            // Manage_Stock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.Controls.Add(this.gunaGroupBox2);
            this.Controls.Add(this.panel1);
            this.Name = "Manage_Stock";
            this.Size = new System.Drawing.Size(1194, 700);
            this.Load += new System.EventHandler(this.Manage_Stock_Load);
            this.panel1.ResumeLayout(false);
            this.gunaElipsePanel2.ResumeLayout(false);
            this.gunaElipsePanel2.PerformLayout();
            this.gunaGroupBox2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.itemStockDgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI.WinForms.GunaElipsePanel gunaElipsePanel2;
        internal System.Windows.Forms.Label label4;
        private Guna.UI.WinForms.GunaGroupBox gunaGroupBox2;
        private System.Windows.Forms.Panel panel5;
        private Guna.UI.WinForms.GunaDataGridView itemStockDgv;
        private Guna.UI.WinForms.GunaButton DeleteStockBtn;
        private Guna.UI.WinForms.GunaButton UpdateStockBtn;
        private Guna.UI.WinForms.GunaButton NewStockBtn;
        private Bunifu.UI.WinForms.BunifuTextBox searchCategoryTxt;
        private System.Windows.Forms.DataGridViewImageColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stock_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stock_In;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stock_Out;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight_Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date_Added;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Total;
    }
}

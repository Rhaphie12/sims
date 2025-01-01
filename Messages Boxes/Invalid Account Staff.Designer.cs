namespace sims.Messages_Boxes
{
    partial class Invalid_Account_Staff
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gunaElipse1 = new Guna.UI.WinForms.GunaElipse(this.components);
            this.gunaDragControl1 = new Guna.UI.WinForms.GunaDragControl(this.components);
            this.continueBtn = new Guna.UI.WinForms.GunaButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gunaPictureBox1 = new Guna.UI.WinForms.GunaPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // gunaElipse1
            // 
            this.gunaElipse1.Radius = 12;
            this.gunaElipse1.TargetControl = this;
            // 
            // gunaDragControl1
            // 
            this.gunaDragControl1.TargetControl = null;
            // 
            // continueBtn
            // 
            this.continueBtn.AnimationHoverSpeed = 0.07F;
            this.continueBtn.AnimationSpeed = 0.03F;
            this.continueBtn.BackColor = System.Drawing.Color.Transparent;
            this.continueBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.continueBtn.BorderColor = System.Drawing.Color.Black;
            this.continueBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.continueBtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.continueBtn.FocusedColor = System.Drawing.Color.Empty;
            this.continueBtn.Font = new System.Drawing.Font("Poppins", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continueBtn.ForeColor = System.Drawing.Color.White;
            this.continueBtn.Image = null;
            this.continueBtn.ImageSize = new System.Drawing.Size(20, 20);
            this.continueBtn.Location = new System.Drawing.Point(345, 207);
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(196)))), ((int)(((byte)(125)))));
            this.continueBtn.OnHoverBorderColor = System.Drawing.Color.Black;
            this.continueBtn.OnHoverForeColor = System.Drawing.Color.White;
            this.continueBtn.OnHoverImage = null;
            this.continueBtn.OnPressedColor = System.Drawing.Color.Black;
            this.continueBtn.Radius = 7;
            this.continueBtn.Size = new System.Drawing.Size(185, 41);
            this.continueBtn.TabIndex = 30;
            this.continueBtn.Text = "CONTINUE";
            this.continueBtn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.continueBtn.Click += new System.EventHandler(this.continueBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Poppins", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(392, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 34);
            this.label2.TabIndex = 29;
            this.label2.Text = "As Staff";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Poppins", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(274, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 48);
            this.label1.TabIndex = 28;
            this.label1.Text = "LOGIN UNSUCCESSFUL!";
            // 
            // gunaPictureBox1
            // 
            this.gunaPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.gunaPictureBox1.BaseColor = System.Drawing.Color.White;
            this.gunaPictureBox1.Image = global::sims.Properties.Resources._1647744;
            this.gunaPictureBox1.Location = new System.Drawing.Point(28, 64);
            this.gunaPictureBox1.Name = "gunaPictureBox1";
            this.gunaPictureBox1.Radius = 6;
            this.gunaPictureBox1.Size = new System.Drawing.Size(230, 216);
            this.gunaPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.gunaPictureBox1.TabIndex = 31;
            this.gunaPictureBox1.TabStop = false;
            // 
            // Invalid_Account_Staff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(625, 344);
            this.Controls.Add(this.gunaPictureBox1);
            this.Controls.Add(this.continueBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Invalid_Account_Staff";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invalid_Account_Staff";
            ((System.ComponentModel.ISupportInitialize)(this.gunaPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI.WinForms.GunaElipse gunaElipse1;
        private Guna.UI.WinForms.GunaDragControl gunaDragControl1;
        private Guna.UI.WinForms.GunaPictureBox gunaPictureBox1;
        private Guna.UI.WinForms.GunaButton continueBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
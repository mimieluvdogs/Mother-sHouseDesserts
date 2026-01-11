namespace test68._10._17
{
    partial class ShopForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopForm));
            this.flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnProfile = new System.Windows.Forms.Button();
            this.btnCart = new System.Windows.Forms.Button();
            this.btnAboutUs = new System.Windows.Forms.Button();
            this.flpCategories = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flpProducts
            // 
            this.flpProducts.AutoScroll = true;
            this.flpProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.flpProducts.Location = new System.Drawing.Point(71, 162);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(1117, 435);
            this.flpProducts.TabIndex = 1;
            // 
            // btnProfile
            // 
            this.btnProfile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProfile.BackgroundImage")));
            this.btnProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProfile.Location = new System.Drawing.Point(1142, 12);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(46, 42);
            this.btnProfile.TabIndex = 7;
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // btnCart
            // 
            this.btnCart.BackColor = System.Drawing.SystemColors.Info;
            this.btnCart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCart.BackgroundImage")));
            this.btnCart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCart.Location = new System.Drawing.Point(1089, 12);
            this.btnCart.Name = "btnCart";
            this.btnCart.Size = new System.Drawing.Size(47, 42);
            this.btnCart.TabIndex = 6;
            this.btnCart.UseVisualStyleBackColor = false;
            this.btnCart.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // btnAboutUs
            // 
            this.btnAboutUs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAboutUs.BackgroundImage")));
            this.btnAboutUs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAboutUs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnAboutUs.Location = new System.Drawing.Point(846, 12);
            this.btnAboutUs.Name = "btnAboutUs";
            this.btnAboutUs.Size = new System.Drawing.Size(237, 42);
            this.btnAboutUs.TabIndex = 5;
            this.btnAboutUs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAboutUs.UseVisualStyleBackColor = true;
            this.btnAboutUs.Click += new System.EventHandler(this.btnAboutUs_Click);
            // 
            // flpCategories
            // 
            this.flpCategories.AutoScroll = true;
            this.flpCategories.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(205)))), ((int)(((byte)(161)))));
            this.flpCategories.Location = new System.Drawing.Point(71, 65);
            this.flpCategories.Name = "flpCategories";
            this.flpCategories.Size = new System.Drawing.Size(1116, 91);
            this.flpCategories.TabIndex = 0;
            this.flpCategories.WrapContents = false;
            // 
            // ShopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.btnCart);
            this.Controls.Add(this.btnAboutUs);
            this.Controls.Add(this.flpProducts);
            this.Controls.Add(this.flpCategories);
            this.Name = "ShopForm";
            this.Text = "ShopForm";
            this.Load += new System.EventHandler(this.ShopForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Button btnProfile;
        private System.Windows.Forms.Button btnCart;
        private System.Windows.Forms.Button btnAboutUs;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
    }
}
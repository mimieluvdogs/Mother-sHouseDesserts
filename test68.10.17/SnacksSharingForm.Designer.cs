namespace test68._10._17
{
    partial class SnacksSharingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnacksSharingForm));
            this.btnCart = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.flpProducts = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAboutUs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCart
            // 
            this.btnCart.BackColor = System.Drawing.SystemColors.Info;
            this.btnCart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCart.BackgroundImage")));
            this.btnCart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCart.Location = new System.Drawing.Point(1141, 38);
            this.btnCart.Name = "btnCart";
            this.btnCart.Size = new System.Drawing.Size(88, 72);
            this.btnCart.TabIndex = 22;
            this.btnCart.UseVisualStyleBackColor = false;
            this.btnCart.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(62, 607);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(197, 46);
            this.btnBack.TabIndex = 19;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // flpProducts
            // 
            this.flpProducts.AutoScroll = true;
            this.flpProducts.BackColor = System.Drawing.SystemColors.Info;
            this.flpProducts.Location = new System.Drawing.Point(71, 132);
            this.flpProducts.Name = "flpProducts";
            this.flpProducts.Size = new System.Drawing.Size(1117, 468);
            this.flpProducts.TabIndex = 18;
            // 
            // btnAboutUs
            // 
            this.btnAboutUs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAboutUs.BackgroundImage")));
            this.btnAboutUs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAboutUs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnAboutUs.Location = new System.Drawing.Point(869, 44);
            this.btnAboutUs.Name = "btnAboutUs";
            this.btnAboutUs.Size = new System.Drawing.Size(266, 61);
            this.btnAboutUs.TabIndex = 23;
            this.btnAboutUs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAboutUs.UseVisualStyleBackColor = true;
            this.btnAboutUs.Click += new System.EventHandler(this.btnAboutUs_Click);
            // 
            // SnacksSharingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            this.Controls.Add(this.btnAboutUs);
            this.Controls.Add(this.btnCart);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.flpProducts);
            this.Name = "SnacksSharingForm";
            this.Text = "SnacksSharingForm";
            this.Load += new System.EventHandler(this.SnacksSharingForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCart;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Button btnAboutUs;
    }
}
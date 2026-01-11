namespace test68._10._17
{
    partial class CategoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CategoryForm));
            this.btnAboutUs = new System.Windows.Forms.Button();
            this.btnCart = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSnacksSharing = new System.Windows.Forms.Button();
            this.btnForOne = new System.Windows.Forms.Button();
            this.btnProfile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAboutUs
            // 
            this.btnAboutUs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAboutUs.BackgroundImage")));
            this.btnAboutUs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAboutUs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnAboutUs.Location = new System.Drawing.Point(773, 24);
            this.btnAboutUs.Name = "btnAboutUs";
            this.btnAboutUs.Size = new System.Drawing.Size(266, 72);
            this.btnAboutUs.TabIndex = 0;
            this.btnAboutUs.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAboutUs.UseVisualStyleBackColor = true;
            this.btnAboutUs.Click += new System.EventHandler(this.btnAboutUs_Click);
            // 
            // btnCart
            // 
            this.btnCart.BackColor = System.Drawing.SystemColors.Info;
            this.btnCart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCart.BackgroundImage")));
            this.btnCart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnCart.Location = new System.Drawing.Point(1045, 24);
            this.btnCart.Name = "btnCart";
            this.btnCart.Size = new System.Drawing.Size(76, 72);
            this.btnCart.TabIndex = 2;
            this.btnCart.UseVisualStyleBackColor = false;
            this.btnCart.Click += new System.EventHandler(this.btnCart_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.btnSnacksSharing);
            this.panel1.Controls.Add(this.btnForOne);
            this.panel1.Location = new System.Drawing.Point(71, 277);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1114, 303);
            this.panel1.TabIndex = 3;
            // 
            // btnSnacksSharing
            // 
            this.btnSnacksSharing.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSnacksSharing.BackgroundImage")));
            this.btnSnacksSharing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSnacksSharing.Location = new System.Drawing.Point(594, 16);
            this.btnSnacksSharing.Name = "btnSnacksSharing";
            this.btnSnacksSharing.Size = new System.Drawing.Size(223, 255);
            this.btnSnacksSharing.TabIndex = 1;
            this.btnSnacksSharing.UseVisualStyleBackColor = true;
            this.btnSnacksSharing.Click += new System.EventHandler(this.btnSnacksSharing_Click);
            // 
            // btnForOne
            // 
            this.btnForOne.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnForOne.BackgroundImage")));
            this.btnForOne.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnForOne.Location = new System.Drawing.Point(281, 16);
            this.btnForOne.Name = "btnForOne";
            this.btnForOne.Size = new System.Drawing.Size(223, 255);
            this.btnForOne.TabIndex = 0;
            this.btnForOne.UseVisualStyleBackColor = true;
            this.btnForOne.Click += new System.EventHandler(this.btnForOne_Click);
            // 
            // btnProfile
            // 
            this.btnProfile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProfile.BackgroundImage")));
            this.btnProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProfile.Location = new System.Drawing.Point(1127, 24);
            this.btnProfile.Name = "btnProfile";
            this.btnProfile.Size = new System.Drawing.Size(75, 72);
            this.btnProfile.TabIndex = 4;
            this.btnProfile.UseVisualStyleBackColor = true;
            this.btnProfile.Click += new System.EventHandler(this.btnProfile_Click);
            // 
            // CategoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            this.Controls.Add(this.btnProfile);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCart);
            this.Controls.Add(this.btnAboutUs);
            this.Name = "CategoryForm";
            this.Text = "CategoryForm";
            this.Load += new System.EventHandler(this.CategoryForm_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAboutUs;
        private System.Windows.Forms.Button btnCart;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSnacksSharing;
        private System.Windows.Forms.Button btnForOne;
        private System.Windows.Forms.Button btnProfile;
    }
}
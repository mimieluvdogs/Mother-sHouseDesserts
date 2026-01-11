namespace test68._10._17
{
    partial class CartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CartForm));
            this.flpCartItems = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.pbQRCode = new System.Windows.Forms.PictureBox();
            this.btnUploadSlip = new System.Windows.Forms.Button();
            this.pbSlipPreview = new System.Windows.Forms.PictureBox();
            this.lblSubtotalValue = new System.Windows.Forms.Label();
            this.lblVatValue = new System.Windows.Forms.Label();
            this.lblGrandTotalValue = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSlipPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // flpCartItems
            // 
            this.flpCartItems.AutoScroll = true;
            this.flpCartItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.flpCartItems.Location = new System.Drawing.Point(71, 198);
            this.flpCartItems.Name = "flpCartItems";
            this.flpCartItems.Size = new System.Drawing.Size(620, 314);
            this.flpCartItems.TabIndex = 0;
            // 
            // btnPay
            // 
            this.btnPay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPay.BackgroundImage")));
            this.btnPay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPay.Enabled = false;
            this.btnPay.Location = new System.Drawing.Point(534, 606);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(157, 38);
            this.btnPay.TabIndex = 2;
            this.btnPay.UseVisualStyleBackColor = true;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.Location = new System.Drawing.Point(71, 606);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(197, 46);
            this.btnBack.TabIndex = 15;
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // pbQRCode
            // 
            this.pbQRCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbQRCode.Location = new System.Drawing.Point(882, 87);
            this.pbQRCode.Name = "pbQRCode";
            this.pbQRCode.Size = new System.Drawing.Size(208, 171);
            this.pbQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbQRCode.TabIndex = 16;
            this.pbQRCode.TabStop = false;
            // 
            // btnUploadSlip
            // 
            this.btnUploadSlip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.btnUploadSlip.Location = new System.Drawing.Point(918, 314);
            this.btnUploadSlip.Name = "btnUploadSlip";
            this.btnUploadSlip.Size = new System.Drawing.Size(167, 36);
            this.btnUploadSlip.TabIndex = 17;
            this.btnUploadSlip.Text = "อัปโหลดสลิป...";
            this.btnUploadSlip.UseVisualStyleBackColor = true;
            this.btnUploadSlip.Click += new System.EventHandler(this.btnUploadSlip_Click);
            // 
            // pbSlipPreview
            // 
            this.pbSlipPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbSlipPreview.Location = new System.Drawing.Point(882, 406);
            this.pbSlipPreview.Name = "pbSlipPreview";
            this.pbSlipPreview.Size = new System.Drawing.Size(226, 220);
            this.pbSlipPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbSlipPreview.TabIndex = 18;
            this.pbSlipPreview.TabStop = false;
            // 
            // lblSubtotalValue
            // 
            this.lblSubtotalValue.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtotalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblSubtotalValue.Location = new System.Drawing.Point(587, 514);
            this.lblSubtotalValue.Name = "lblSubtotalValue";
            this.lblSubtotalValue.Size = new System.Drawing.Size(80, 25);
            this.lblSubtotalValue.TabIndex = 19;
            this.lblSubtotalValue.Text = "0.00 B";
            this.lblSubtotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSubtotalValue.Click += new System.EventHandler(this.lblSubtotalValue_Click);
            // 
            // lblVatValue
            // 
            this.lblVatValue.BackColor = System.Drawing.Color.Transparent;
            this.lblVatValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblVatValue.Location = new System.Drawing.Point(587, 537);
            this.lblVatValue.Name = "lblVatValue";
            this.lblVatValue.Size = new System.Drawing.Size(80, 25);
            this.lblVatValue.TabIndex = 20;
            this.lblVatValue.Text = "0.00 B";
            this.lblVatValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblVatValue.Click += new System.EventHandler(this.lblVatValue_Click);
            // 
            // lblGrandTotalValue
            // 
            this.lblGrandTotalValue.BackColor = System.Drawing.Color.Transparent;
            this.lblGrandTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.lblGrandTotalValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.lblGrandTotalValue.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblGrandTotalValue.Location = new System.Drawing.Point(477, 561);
            this.lblGrandTotalValue.Name = "lblGrandTotalValue";
            this.lblGrandTotalValue.Size = new System.Drawing.Size(190, 30);
            this.lblGrandTotalValue.TabIndex = 21;
            this.lblGrandTotalValue.Text = "0.00 B";
            this.lblGrandTotalValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblGrandTotalValue.Click += new System.EventHandler(this.lblGrandTotalValue_Click);
            // 
            // CartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            this.Controls.Add(this.lblGrandTotalValue);
            this.Controls.Add(this.lblVatValue);
            this.Controls.Add(this.lblSubtotalValue);
            this.Controls.Add(this.pbSlipPreview);
            this.Controls.Add(this.btnUploadSlip);
            this.Controls.Add(this.pbQRCode);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.flpCartItems);
            this.Name = "CartForm";
            this.Text = "CartForm";
            this.Load += new System.EventHandler(this.CartForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbQRCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbSlipPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpCartItems;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.PictureBox pbQRCode;
        private System.Windows.Forms.Button btnUploadSlip;
        private System.Windows.Forms.PictureBox pbSlipPreview;
        private System.Windows.Forms.Label lblSubtotalValue;
        private System.Windows.Forms.Label lblVatValue;
        private System.Windows.Forms.Label lblGrandTotalValue;
    }
}
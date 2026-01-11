namespace test68._10._17 // ตรวจสอบว่า namespace ตรงกัน
{
    partial class AdminForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminForm));
            // --- แก้ไขชื่อตัวแปรปุ่มให้ถูกต้อง ---
            this.btnCustomer = new System.Windows.Forms.Button(); // เดิมชื่อ button1
            this.btnProduct = new System.Windows.Forms.Button();  // เดิมชื่อ btnProduct_Click
            this.btnOrder = new System.Windows.Forms.Button();    // เดิมชื่อ btnOrder_Click
            this.btnSalesReport = new System.Windows.Forms.Button(); // เดิมชื่อ btnSalesReport_Click
            this.SuspendLayout();
            //
            // btnCustomer
            //
            // ** หมายเหตุ: ควรตั้งชื่อ Image Resource ให้ตรงกับชื่อปุ่ม (เช่น btnCustomer.BackgroundImage) **
            // ** ถ้า Resource ชื่อ button1.BackgroundImage ให้ใช้ชื่อนั้นไปก่อน **
            this.btnCustomer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button1.BackgroundImage")));
            this.btnCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCustomer.Location = new System.Drawing.Point(136, 246);
            this.btnCustomer.Name = "btnCustomer"; // แก้ไข Name ให้ตรง
            this.btnCustomer.Size = new System.Drawing.Size(180, 166);
            this.btnCustomer.TabIndex = 0;
            this.btnCustomer.UseVisualStyleBackColor = true;
            this.btnCustomer.Click += new System.EventHandler(this.btnCustomer_Click); // เชื่อม Event Handler ที่ถูกต้อง
            //
            // btnProduct
            //
            this.btnProduct.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProduct_Click.BackgroundImage"))); // ** อาจจะต้องแก้ชื่อ Resource เป็น btnProduct.BackgroundImage **
            this.btnProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProduct.Location = new System.Drawing.Point(407, 246);
            this.btnProduct.Name = "btnProduct"; // แก้ไข Name ให้ตรง
            this.btnProduct.Size = new System.Drawing.Size(179, 166);
            this.btnProduct.TabIndex = 1;
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click); // เชื่อม Event Handler ที่ถูกต้อง
            //
            // btnOrder
            //
            this.btnOrder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOrder_Click.BackgroundImage"))); // ** อาจจะต้องแก้ชื่อ Resource เป็น btnOrder.BackgroundImage **
            this.btnOrder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnOrder.Location = new System.Drawing.Point(672, 246);
            this.btnOrder.Name = "btnOrder"; // แก้ไข Name ให้ตรง
            this.btnOrder.Size = new System.Drawing.Size(179, 166);
            this.btnOrder.TabIndex = 2;
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click); // เชื่อม Event Handler ที่ถูกต้อง
            //
            // btnSalesReport
            //
            this.btnSalesReport.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSalesReport_Click.BackgroundImage"))); // ** อาจจะต้องแก้ชื่อ Resource เป็น btnSalesReport.BackgroundImage **
            this.btnSalesReport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSalesReport.Location = new System.Drawing.Point(942, 246);
            this.btnSalesReport.Name = "btnSalesReport"; // แก้ไข Name ให้ตรง
            this.btnSalesReport.Size = new System.Drawing.Size(179, 166);
            this.btnSalesReport.TabIndex = 3;
            this.btnSalesReport.UseVisualStyleBackColor = true;
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click); // เชื่อม Event Handler ที่ถูกต้อง
            //
            // AdminForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1258, 664);
            // --- แก้ไขการ Add Control ให้ใช้ชื่อที่ถูกต้อง ---
            this.Controls.Add(this.btnSalesReport);
            this.Controls.Add(this.btnOrder);
            this.Controls.Add(this.btnProduct);
            this.Controls.Add(this.btnCustomer);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        // --- แก้ไขชื่อตัวแปรให้ถูกต้อง ---
        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnProduct;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnSalesReport;
    }
}
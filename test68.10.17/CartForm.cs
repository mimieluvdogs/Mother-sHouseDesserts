using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using QRCoder;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Diagnostics; // จำเป็นสำหรับ Process.Start

namespace test68._10._17
{
    public partial class CartForm : Form
    {
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private string uploadedSlipPath = null;
        private byte[] logoImageData;

        public CartForm()
        {
            InitializeComponent();
            InitCustomFont();
        }

        private void InitCustomFont()
        {
            try
            {
                pfc.AddFontFile("Fonts/Sarabun-Regular.ttf");

                if (pfc.Families.Length > 0)
                {
                    // ตั้งค่าฟอนต์
                    lblSubtotalValue.Font = new System.Drawing.Font(pfc.Families[0], 8, FontStyle.Regular);
                    lblVatValue.Font = new System.Drawing.Font(pfc.Families[0], 8, FontStyle.Regular);
                    lblGrandTotalValue.Font = new System.Drawing.Font(pfc.Families[0], 10, FontStyle.Bold);

                    btnPay.Font = new System.Drawing.Font(pfc.Families[0], 11, FontStyle.Bold);
                    btnBack.Font = new System.Drawing.Font(pfc.Families[0], 11, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("หาไฟล์ฟอนต์ไม่เจอ หรือ ยังไม่ได้สร้าง Label ใหม่ใน Designer: " + ex.Message);
            }
        }

        private void CartForm_Load(object sender, EventArgs e)
        {
            btnPay.Enabled = false;
            DisplayCartItems();

            try
            {
                string logoPath = Path.Combine(Application.StartupPath, "images", "Logo.png");
                if (File.Exists(logoPath))
                {
                    logoImageData = File.ReadAllBytes(logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ไม่สามารถโหลดโลโก้ร้านได้: " + ex.Message);
                logoImageData = null;
            }
        }

        #region "โค้ดจัดการตะกร้า (Display, Plus, Minus, GetStock)"

        private void DisplayCartItems()
        {
            foreach (Control ctrl in flpCartItems.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control pnlCtrl in panel.Controls)
                    {
                        if (pnlCtrl is PictureBox pb)
                        {
                            pb.Image?.Dispose();
                        }
                    }
                }
                ctrl.Dispose();
            }
            flpCartItems.Controls.Clear();


            foreach (var item in ShoppingCart.Items)
            {
                // Panel หลัก
                Panel itemPanel = new Panel { Size = new System.Drawing.Size(flpCartItems.Width - 30, 80), BackColor = System.Drawing.Color.FromArgb(240, 240, 240), Margin = new Padding(5) };

                // รูปภาพ
                PictureBox pb = new PictureBox { SizeMode = PictureBoxSizeMode.Zoom, Location = new System.Drawing.Point(10, 10), Size = new System.Drawing.Size(60, 60) };

                try
                {
                    string fullImagePath = Path.Combine(Application.StartupPath, item.ImagePath);
                    if (File.Exists(fullImagePath))
                    {
                        using (var bmpTemp = new Bitmap(fullImagePath))
                        {
                            pb.Image = new Bitmap(bmpTemp);
                        }
                    }
                }
                catch { pb.Image = null; }

                // ชื่อสินค้าและราคา
                Label nameLabel = new Label { Text = item.Name, Font = new System.Drawing.Font(pfc.Families[0], 10, FontStyle.Bold), Location = new System.Drawing.Point(80, 15), AutoSize = true };
                Label priceLabel = new Label { Text = string.Format("{0:N2} ฿", item.Price), Font = new System.Drawing.Font(pfc.Families[0], 9), ForeColor = System.Drawing.Color.DarkRed, Location = new System.Drawing.Point(80, 40), AutoSize = true };

                // ปุ่มลบ
                Button btnMinus = new Button { Text = "-", Location = new System.Drawing.Point(itemPanel.Width - 120, 27), Size = new System.Drawing.Size(25, 25), Tag = item.ProductID, Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold) };

                // จำนวนสินค้า
                Label lblQuantity = new Label { Text = item.Quantity.ToString(), Location = new System.Drawing.Point(itemPanel.Width - 90, 31), Size = new System.Drawing.Size(30, 20), TextAlign = ContentAlignment.MiddleCenter, Font = new System.Drawing.Font(pfc.Families[0], 9) };

                // ปุ่มบวก
                Button btnPlus = new Button { Text = "+", Location = new System.Drawing.Point(itemPanel.Width - 55, 27), Size = new System.Drawing.Size(25, 25), Tag = item.ProductID, Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold) };

                btnMinus.Click += BtnMinus_Click;
                btnPlus.Click += BtnPlus_Click;
                itemPanel.Controls.Add(pb);
                itemPanel.Controls.Add(nameLabel);
                itemPanel.Controls.Add(priceLabel);
                itemPanel.Controls.Add(btnMinus);
                itemPanel.Controls.Add(lblQuantity);
                itemPanel.Controls.Add(btnPlus);
                flpCartItems.Controls.Add(itemPanel);
            }
            UpdateTotal();
        }

        private int GetCurrentStock(int productId)
        {
            string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT stock FROM products WHERE product_id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", productId);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการตรวจสอบสต็อก: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return 0;
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int productId = (int)btn.Tag;
            var item = ShoppingCart.Items.Find(i => i.ProductID == productId);

            if (item != null)
            {
                int newQuantity = item.Quantity + 1;
                int currentStock = GetCurrentStock(productId);

                if (newQuantity > currentStock)
                {
                    MessageBox.Show($"ขออภัย, สินค้า '{item.Name}' มีสต็อกไม่เพียงพอ (มี {currentStock} ชิ้น)",
                                    "สต็อกไม่พอ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    item.Quantity++;
                }
            }
            DisplayCartItems();
        }

        private void BtnMinus_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int productId = (int)btn.Tag;
            var item = ShoppingCart.Items.Find(i => i.ProductID == productId);
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0) { ShoppingCart.RemoveItem(productId); }
            }
            DisplayCartItems();
        }
        #endregion

        private void UpdateTotal()
        {
            // 1. คำนวณค่าต่างๆ
            decimal subTotal = ShoppingCart.GetTotal(); // ราคาสินค้าทั้งหมด (ก่อน VAT)
            decimal vatRate = 0.07m; // 7%
            decimal vatAmount = subTotal * vatRate;
            decimal grandTotal = subTotal + vatAmount;

            // 2. แสดงผลบน Label
            lblSubtotalValue.Text = string.Format("{0:N2} ฿", subTotal);
            lblVatValue.Text = string.Format("{0:N2} ฿", vatAmount);
            lblGrandTotalValue.Text = string.Format("{0:N2} ฿", grandTotal);

            // 3. สร้าง QR Code
            if (grandTotal > 0 && pbSlipPreview.Image == null)
            {
                string promptPayId = "0640954757";
                try
                {
                    PromptPayQrCode.PromptPayQrCode qrCodePayload = new PromptPayQrCode.PromptPayQrCode(promptPayId, (double?)grandTotal);
                    string payload = qrCodePayload.PromptPayPayload;
                    using (QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator())
                    using (QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(payload, QRCoder.QRCodeGenerator.ECCLevel.Q))
                    using (QRCoder.QRCode qrCode = new QRCoder.QRCode(qrCodeData))
                    {
                        if (pbQRCode.Image != null) { pbQRCode.Image.Dispose(); }
                        pbQRCode.Image = qrCode.GetGraphic(20);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการสร้าง QR Code: " + ex.Message);
                    if (pbQRCode.Image != null) { pbQRCode.Image.Dispose(); }
                    pbQRCode.Image = null;
                }
            }
            else
            {
                if (pbQRCode.Image != null) { pbQRCode.Image.Dispose(); }
                pbQRCode.Image = null;
            }
        }

        private void btnUploadSlip_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    uploadedSlipPath = openFile.FileName;

                    using (var bmpTemp = new System.Drawing.Bitmap(uploadedSlipPath))
                    {
                        if (pbSlipPreview.Image != null)
                        {
                            pbSlipPreview.Image.Dispose();
                        }
                        pbSlipPreview.Image = new System.Drawing.Bitmap(bmpTemp);
                    }

                    btnPay.Enabled = true;
                    if (pbQRCode.Image != null) { pbQRCode.Image.Dispose(); }
                    pbQRCode.Image = null;
                    MessageBox.Show("อัปโหลดสลิปสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรูปสลิป: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    uploadedSlipPath = null;
                    pbSlipPreview.Image = null;
                    btnPay.Enabled = false;
                }
            }
        }

        // ⭐️⭐️⭐️ ส่วนที่แก้ไขหลัก: บันทึกและเปิด PDF อัตโนมัติ ⭐️⭐️⭐️
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(uploadedSlipPath) || pbSlipPreview.Image == null)
            {
                MessageBox.Show("กรุณาอัปโหลดสลิปการชำระเงินก่อนค่ะ", "ยังไม่ได้อัปโหลดสลิป", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ShoppingCart.Items.Count == 0)
            {
                MessageBox.Show("ตะกร้าของคุณว่างเปล่า", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlTransaction transaction = null;

            long newOrderId = 0;
            string slipPathToSaveInDb = "";

            // คำนวณยอด
            decimal subTotal = ShoppingCart.GetTotal();
            decimal vatAmount = subTotal * 0.07m;
            decimal grandTotal = subTotal + vatAmount;

            // คัดลอกข้อมูลไว้สำหรับ PDF
            string customerName = CurrentUser.Name;
            string shippingAddress = CurrentUser.FullAddress;
            string customerPhone = CurrentUser.Phone;
            List<CartItem> itemsForReceipt = new List<CartItem>(ShoppingCart.Items);

            try
            {
                conn.Open();
                transaction = conn.BeginTransaction();

                // 1. บันทึก Order
                string sqlOrder = "INSERT INTO orders (user_id, total_amount, shipping_address, status) VALUES (@user_id, @total, @address, 'คำสั่งซื้อใหม่')";
                MySqlCommand cmdOrder = new MySqlCommand(sqlOrder, conn, transaction);
                cmdOrder.Parameters.AddWithValue("@user_id", CurrentUser.UserID);
                cmdOrder.Parameters.AddWithValue("@total", grandTotal);
                cmdOrder.Parameters.AddWithValue("@address", shippingAddress);
                cmdOrder.ExecuteNonQuery();
                newOrderId = cmdOrder.LastInsertedId;

                // 2. บันทึกไฟล์สลิป
                #region "Save Slip File"
                try
                {
                    string targetDirectory = Path.Combine(Application.StartupPath, "slips");
                    if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }
                    string fileExtension = Path.GetExtension(uploadedSlipPath);
                    string newFileName = $"slip_{newOrderId}_{Guid.NewGuid().ToString().Substring(0, 8)}{fileExtension}";
                    string destinationPath = Path.Combine(targetDirectory, newFileName);
                    File.Copy(uploadedSlipPath, destinationPath, true);
                    slipPathToSaveInDb = Path.Combine("slips", newFileName).Replace("\\", "/");
                }
                catch (Exception fileEx)
                {
                    throw new Exception("เกิดข้อผิดพลาดในการบันทึกไฟล์สลิป: " + fileEx.Message);
                }

                string sqlUpdateSlip = "UPDATE orders SET payment_slip_path = @slipPath WHERE order_id = @orderId";
                MySqlCommand cmdUpdateSlip = new MySqlCommand(sqlUpdateSlip, conn, transaction);
                cmdUpdateSlip.Parameters.AddWithValue("@slipPath", slipPathToSaveInDb);
                cmdUpdateSlip.Parameters.AddWithValue("@orderId", newOrderId);
                cmdUpdateSlip.ExecuteNonQuery();
                #endregion

                // 3. บันทึก Order Details และตัดสต็อก
                foreach (var item in itemsForReceipt)
                {
                    string sqlDetail = "INSERT INTO order_details (order_id, product_id, quantity, price_per_item) VALUES (@order_id, @product_id, @qty, @price)";
                    MySqlCommand cmdDetail = new MySqlCommand(sqlDetail, conn, transaction);
                    cmdDetail.Parameters.AddWithValue("@order_id", newOrderId);
                    cmdDetail.Parameters.AddWithValue("@product_id", item.ProductID);
                    cmdDetail.Parameters.AddWithValue("@qty", item.Quantity);
                    cmdDetail.Parameters.AddWithValue("@price", item.Price);
                    cmdDetail.ExecuteNonQuery();

                    string sqlStock = "UPDATE products SET stock = stock - @qty WHERE product_id = @product_id AND stock >= @qty";
                    MySqlCommand cmdStock = new MySqlCommand(sqlStock, conn, transaction);
                    cmdStock.Parameters.AddWithValue("@qty", item.Quantity);
                    cmdStock.Parameters.AddWithValue("@product_id", item.ProductID);
                    int rowsAffected = cmdStock.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new Exception($"ขออภัย, สินค้า '{item.Name}' มีสต็อกไม่เพียงพอ (คุณต้องการ {item.Quantity} ชิ้น)");
                    }
                }

                transaction.Commit();
                MessageBox.Show("สั่งซื้อสินค้าสำเร็จ! กำลังเปิดใบเสร็จ...", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // --- 4. สร้างและเปิด PDF อัตโนมัติ ---
                try
                {
                    var receipt = new ReceiptDocument(
                        newOrderId,
                        customerName,
                        shippingAddress,
                        customerPhone,
                        grandTotal,
                        itemsForReceipt,
                        logoImageData
                    );

                    // กำหนดที่เก็บไฟล์ (โฟลเดอร์ Receipts)
                    string receiptsFolder = Path.Combine(Application.StartupPath, "Receipts");
                    if (!Directory.Exists(receiptsFolder))
                    {
                        Directory.CreateDirectory(receiptsFolder);
                    }

                    // ชื่อไฟล์และ Path
                    string fileName = $"Receipt-{newOrderId}.pdf";
                    string filePath = Path.Combine(receiptsFolder, fileName);

                    // สร้างไฟล์
                    receipt.GeneratePdf(filePath);

                    // สั่งเปิดไฟล์ทันที
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true // สำคัญมากสำหรับการเปิดไฟล์ด้วย Default Program
                    };
                    Process.Start(psi);
                }
                catch (Exception pdfEx)
                {
                    MessageBox.Show("บันทึกออเดอร์สำเร็จ แต่เกิดข้อผิดพลาดในการเปิดไฟล์ PDF อัตโนมัติ:\n" + pdfEx.Message, "PDF Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // -----------------------------

                ShoppingCart.ClearCart();
                this.Close();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show("เกิดข้อผิดพลาดในการสั่งซื้อ: \n\n" + ex.Message, "ทำรายการไม่สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // เคลียร์ Resource รูปภาพก่อนปิด
            if (pbSlipPreview != null && pbSlipPreview.Image != null)
            {
                pbSlipPreview.Image.Dispose();
                pbSlipPreview.Image = null;
            }
            if (pbQRCode != null && pbQRCode.Image != null)
            {
                pbQRCode.Image.Dispose();
                pbQRCode.Image = null;
            }

            foreach (Control ctrl in flpCartItems.Controls)
            {
                if (ctrl is Panel panel)
                {
                    foreach (Control pnlCtrl in panel.Controls)
                    {
                        if (pnlCtrl is PictureBox pb)
                        {
                            pb.Image?.Dispose();
                        }
                    }
                }
                ctrl.Dispose();
            }

            this.Close();
        }

        // Event ว่าง (ปล่อยไว้กัน Error ถ้าใน Designer มีการลิงก์ไว้)
        private void lblSubtotalValue_Click(object sender, EventArgs e) { }
        private void lblVatValue_Click(object sender, EventArgs e) { }
        private void lblGrandTotalValue_Click(object sender, EventArgs e) { }
    }
}
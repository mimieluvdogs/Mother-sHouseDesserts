using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class OrderForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";

        public OrderForm()
        {
            InitializeComponent();
            // --- ตั้งค่าเริ่มต้น ---
            txtOrderId.ReadOnly = true;
            txtCustomerName.ReadOnly = true;
            txtShippingAddress.ReadOnly = true;
            txtTotalAmount.ReadOnly = true;

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("คำสั่งซื้อใหม่");
            cmbStatus.Items.Add("กำลังจัดเตรียมสินค้า"); // (เพิ่มสถานะย่อยได้ตามต้องการ)
            cmbStatus.Items.Add("จัดส่งแล้ว");
            cmbStatus.Items.Add("ยกเลิก");
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            // ปรับรูปให้พอดีกับกรอบ
            if (pbSlipPreview != null) pbSlipPreview.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            LoadOrders();
        }

        // ======================================================
        // ส่วนที่เพิ่มใหม่: ฟังก์ชันค้นหา (Real-time)
        // ======================================================

        // Event เมื่อพิมพ์ข้อความในช่อง txtSearch
        // อย่าลืมไปสร้าง TextBox ชื่อ txtSearch และ Link Event TextChanged ที่หน้า Design ก่อนนะครับ
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadOrders(); // ถ้าลบจนว่าง ให้โหลดข้อมูลทั้งหมด
            }
            else
            {
                SearchOrders(keyword); // ถ้ามีตัวอักษร ให้ค้นหา
            }
        }

        // เมธอดค้นหาออเดอร์
        private void SearchOrders(string keyword)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // SQL: ค้นหาจาก Order ID หรือ ชื่อลูกค้า (ต้อง JOIN ตาราง users)
                    string sql = @"SELECT 
                                     o.order_id, 
                                     o.order_date, 
                                     u.name AS customer_name, 
                                     o.total_amount, 
                                     o.status, 
                                     o.payment_slip_path 
                                   FROM orders AS o
                                   JOIN users AS u ON o.user_id = u.user_id
                                   WHERE o.order_id LIKE @keyword OR u.name LIKE @keyword
                                   ORDER BY o.order_date DESC"; // เรียงจากใหม่ไปเก่า (DESC) จะดูง่ายกว่า

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvOrders.DataSource = dt;
                    SetupOrderGridColumns(dgvOrders);
                }
                ClearDetails(); // ล้างหน้าจอรายละเอียดเมื่อผลการค้นหาเปลี่ยน
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Search Error: " + ex.Message);
            }
        }

        // ======================================================
        // จบส่วนที่เพิ่มใหม่
        // ======================================================

        private void btnLoadOrders_Click(object sender, EventArgs e)
        {
            txtSearch.Clear(); // ล้างช่องค้นหา
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT 
                                     o.order_id, 
                                     o.order_date, 
                                     u.name AS customer_name, 
                                     o.total_amount, 
                                     o.status, 
                                     o.payment_slip_path 
                                   FROM orders AS o
                                   JOIN users AS u ON o.user_id = u.user_id
                                   ORDER BY o.order_date DESC"; // แนะนำให้เรียง DESC (ล่าสุดขึ้นก่อน)

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvOrders.DataSource = dt;
                    SetupOrderGridColumns(dgvOrders);
                }
                ClearDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายการสั่งซื้อ: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupOrderGridColumns(DataGridView dgv)
        {
            // ตรวจสอบว่าคอลัมน์มีอยู่จริงก่อนกำหนดค่า เพื่อป้องกัน Error
            if (dgv.Columns.Contains("order_id")) { dgv.Columns["order_id"].HeaderText = "ID ออเดอร์"; dgv.Columns["order_id"].Width = 80; }
            if (dgv.Columns.Contains("order_date")) { dgv.Columns["order_date"].HeaderText = "วันที่สั่งซื้อ"; dgv.Columns["order_date"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; dgv.Columns["order_date"].Width = 120; }
            if (dgv.Columns.Contains("customer_name")) { dgv.Columns["customer_name"].HeaderText = "ชื่อลูกค้า"; dgv.Columns["customer_name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; }
            if (dgv.Columns.Contains("total_amount")) { dgv.Columns["total_amount"].HeaderText = "ยอดรวม"; dgv.Columns["total_amount"].DefaultCellStyle.Format = "N2"; dgv.Columns["total_amount"].Width = 100; }
            if (dgv.Columns.Contains("status")) { dgv.Columns["status"].HeaderText = "สถานะ"; dgv.Columns["status"].Width = 100; }
            if (dgv.Columns.Contains("payment_slip_path")) { dgv.Columns["payment_slip_path"].Visible = false; }
        }

        // ⭐️ เมื่อคลิกตาราง ให้โหลดรายละเอียด + โชว์รูปสลิปทันที
        private void dgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrders.Rows[e.RowIndex];

                // ป้องกัน Error กรณีคลิกที่ว่างๆ หรือข้อมูลยังไม่โหลด
                if (row.Cells["order_id"].Value == DBNull.Value) return;

                int selectedOrderId = Convert.ToInt32(row.Cells["order_id"].Value);

                // 1. ดึง Path รูปจากตาราง
                string slipPath = "";
                if (dgvOrders.Columns.Contains("payment_slip_path") && row.Cells["payment_slip_path"].Value != DBNull.Value)
                {
                    slipPath = row.Cells["payment_slip_path"].Value.ToString();
                }

                if (txtSlipPath != null) txtSlipPath.Text = slipPath;

                // 2. เรียกฟังก์ชันโชว์รูป
                ShowSlipImage(slipPath);

                // 3. โหลดรายละเอียดสินค้า
                LoadOrderDetails(selectedOrderId);
            }
        }

        // ⭐️ ฟังก์ชันโชว์รูปสลิป
        private void ShowSlipImage(string relativePath)
        {
            if (pbSlipPreview.Image != null)
            {
                pbSlipPreview.Image.Dispose();
                pbSlipPreview.Image = null;
            }

            if (string.IsNullOrWhiteSpace(relativePath)) return;

            try
            {
                // ใช้ Path.Combine เพื่อความถูกต้องของ path ในแต่ละเครื่อง
                string fullSlipPath = Path.Combine(Application.StartupPath, relativePath);

                if (File.Exists(fullSlipPath))
                {
                    // เทคนิค: โหลดรูปผ่าน FileStream เพื่อไม่ให้ไฟล์ถูก Lock (เผื่อมีการลบ/แก้รูป)
                    using (FileStream fs = new FileStream(fullSlipPath, FileMode.Open, FileAccess.Read))
                    {
                        pbSlipPreview.Image = Image.FromStream(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                // แสดง Error ใน Console แทน MessageBox เพื่อไม่ให้รบกวน
                System.Diagnostics.Debug.WriteLine("Load Image Error: " + ex.Message);
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. ข้อมูลหลัก
                    string sqlOrderInfo = @"SELECT o.order_id, u.name, o.shipping_address, o.total_amount, o.status
                                            FROM orders AS o
                                            JOIN users AS u ON o.user_id = u.user_id
                                            WHERE o.order_id = @orderId";

                    MySqlCommand cmdOrderInfo = new MySqlCommand(sqlOrderInfo, conn);
                    cmdOrderInfo.Parameters.AddWithValue("@orderId", orderId);

                    using (MySqlDataReader readerInfo = cmdOrderInfo.ExecuteReader())
                    {
                        if (readerInfo.Read())
                        {
                            txtOrderId.Text = readerInfo["order_id"].ToString();
                            txtCustomerName.Text = readerInfo["name"].ToString();
                            txtShippingAddress.Text = readerInfo["shipping_address"].ToString();
                            txtTotalAmount.Text = string.Format("{0:N2} ฿", readerInfo.GetDecimal("total_amount"));

                            string status = readerInfo["status"].ToString();
                            // เช็คว่าสถานะที่มีใน DB มีอยู่ใน ComboBox หรือไม่ ถ้าไม่มีให้เพิ่มเข้าไป (กัน Error)
                            if (!cmbStatus.Items.Contains(status))
                            {
                                cmbStatus.Items.Add(status);
                            }
                            cmbStatus.SelectedItem = status;
                        }
                    }

                    // 2. รายการสินค้า
                    string sqlOrderItems = @"SELECT p.name AS product_name, od.quantity, od.price_per_item
                                             FROM order_details AS od
                                             JOIN products AS p ON od.product_id = p.product_id
                                             WHERE od.order_id = @orderId";

                    MySqlDataAdapter adapterItems = new MySqlDataAdapter(sqlOrderItems, conn);
                    adapterItems.SelectCommand.Parameters.AddWithValue("@orderId", orderId);
                    DataTable dtItems = new DataTable();
                    adapterItems.Fill(dtItems);
                    dgvOrderDetails.DataSource = dtItems;
                    SetupDetailsGridColumns(dgvOrderDetails);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายละเอียด: " + ex.Message);
                ClearDetails();
            }
        }

        private void SetupDetailsGridColumns(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgv.Columns.Contains("product_name")) { dgv.Columns["product_name"].HeaderText = "ชื่อสินค้า"; dgv.Columns["product_name"].FillWeight = 70; }
            if (dgv.Columns.Contains("quantity")) { dgv.Columns["quantity"].HeaderText = "จำนวน"; dgv.Columns["quantity"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
            if (dgv.Columns.Contains("price_per_item")) { dgv.Columns["price_per_item"].HeaderText = "ราคาต่อหน่วย"; dgv.Columns["price_per_item"].DefaultCellStyle.Format = "N2"; dgv.Columns["price_per_item"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtOrderId.Text)) { MessageBox.Show("กรุณาเลือกออเดอร์ก่อน", "แจ้งเตือน"); return; }
            if (cmbStatus.SelectedItem == null) { MessageBox.Show("กรุณาเลือกสถานะ", "แจ้งเตือน"); return; }

            int orderIdToUpdate = Convert.ToInt32(txtOrderId.Text);
            string newStatus = cmbStatus.SelectedItem.ToString();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "UPDATE orders SET status = @status WHERE order_id = @orderId";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@status", newStatus);
                    cmd.Parameters.AddWithValue("@orderId", orderIdToUpdate);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("อัปเดตสถานะสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // เช็คว่าค้นหาอยู่หรือไม่ เพื่อโหลดข้อมูลให้ตรงสถานะ
                        if (string.IsNullOrEmpty(txtSearch.Text)) LoadOrders();
                        else SearchOrders(txtSearch.Text);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("อัปเดตสถานะผิดพลาด: " + ex.Message); }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearDetails()
        {
            txtOrderId.Clear();
            txtCustomerName.Clear();
            txtShippingAddress.Clear();
            txtTotalAmount.Clear();
            dgvOrderDetails.DataSource = null;
            cmbStatus.SelectedIndex = -1;
            if (txtSlipPath != null) txtSlipPath.Clear();

            if (pbSlipPreview.Image != null)
            {
                pbSlipPreview.Image.Dispose();
                pbSlipPreview.Image = null;
            }
        }
    }
}
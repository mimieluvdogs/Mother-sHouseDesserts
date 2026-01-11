using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.IO; // <-- [เพิ่ม] สำหรับจัดการไฟล์รูป

namespace test68._10._17
{
    public partial class ProfileForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private string _selectedImagePath = null; // <-- [เพิ่ม] ตัวแปรเก็บ path รูปใหม่

        public ProfileForm()
        {
            InitializeComponent();
            InitCustomFont();
        }

        private void InitCustomFont()
        {
            try { pfc.AddFontFile("Fonts/Sarabun-Regular.ttf"); } catch { }
        }

        private void ProfileForm_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
            LoadOrderHistory();
        }

        // --- 1. ส่วนจัดการข้อมูลผู้ใช้ (User Info) ---

        private void LoadUserInfo()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // [แก้ไข] ดึง profile_image_path มาด้วย
                    string sql = "SELECT name, phone_number, province, district, address, zip_code, profile_image_path FROM users WHERE user_id = @userId";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@userId", CurrentUser.UserID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["name"].ToString();
                            txtPhone.Text = reader["phone_number"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                            txtDistrict.Text = reader["district"].ToString();
                            txtProvince.Text = reader["province"].ToString();
                            txtZipCode.Text = reader["zip_code"].ToString();

                            // --- [เพิ่ม] โหลดรูปโปรไฟล์ ---
                            string imagePath = reader["profile_image_path"].ToString();
                            if (!string.IsNullOrEmpty(imagePath))
                            {
                                string fullPath = Path.Combine(Application.StartupPath, imagePath);
                                if (File.Exists(fullPath))
                                {
                                    // โหลดแบบไม่ล็อกไฟล์ (เผื่อเปลี่ยนรูป)
                                    using (var bmpTemp = new Bitmap(fullPath))
                                    {
                                        pbProfileImage.Image = new Bitmap(bmpTemp);
                                    }
                                }
                            }
                            // -----------------------------
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลผู้ใช้: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- [เพิ่ม] ปุ่มเปลี่ยนรูป ---
        // **อย่าลืมเชื่อม Event Click ของปุ่ม btnChangeImage ในหน้า Designer**
        private void btnChangeImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _selectedImagePath = openFileDialog.FileName;
                // แสดงตัวอย่างรูป
                using (var bmpTemp = new Bitmap(_selectedImagePath))
                {
                    if (pbProfileImage.Image != null) pbProfileImage.Image.Dispose();
                    pbProfileImage.Image = new Bitmap(bmpTemp);
                }
            }
        }

        // ปุ่มบันทึกการแก้ไขข้อมูลส่วนตัว
        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("กรุณากรอกชื่อและเบอร์โทรศัพท์", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string savedImagePathInDb = null;

            // --- [เพิ่ม] บันทึกรูปภาพ (ถ้ามีการเลือกใหม่) ---
            if (_selectedImagePath != null)
            {
                try
                {
                    string targetDirectory = Path.Combine(Application.StartupPath, "Images", "Profiles");
                    if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(_selectedImagePath);
                    string destinationPath = Path.Combine(targetDirectory, fileName);

                    File.Copy(_selectedImagePath, destinationPath, true);

                    savedImagePathInDb = Path.Combine("Images", "Profiles", fileName).Replace("\\", "/");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("บันทึกรูปภาพไม่สำเร็จ: " + ex.Message);
                    return;
                }
            }
            // ---------------------------------------------

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // [แก้ไข] เพิ่มการอัปเดต profile_image_path (เฉพาะถ้ามีการเลือกรูปใหม่)
                    string sql = @"UPDATE users SET 
                                   name = @name, 
                                   phone_number = @phone, 
                                   address = @address, 
                                   district = @district, 
                                   province = @province, 
                                   zip_code = @zipcode";

                    if (savedImagePathInDb != null)
                    {
                        sql += ", profile_image_path = @imagePath"; // เพิ่ม field รูปภาพถ้ามี
                    }

                    sql += " WHERE user_id = @userId";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@district", txtDistrict.Text);
                    cmd.Parameters.AddWithValue("@province", txtProvince.Text);
                    cmd.Parameters.AddWithValue("@zipcode", txtZipCode.Text);
                    cmd.Parameters.AddWithValue("@userId", CurrentUser.UserID);

                    if (savedImagePathInDb != null)
                    {
                        cmd.Parameters.AddWithValue("@imagePath", savedImagePathInDb);
                    }

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("บันทึกข้อมูลเรียบร้อยแล้ว!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CurrentUser.Name = txtName.Text;
                        CurrentUser.Phone = txtPhone.Text;
                        CurrentUser.FullAddress = $"{txtAddress.Text} อ.{txtDistrict.Text} จ.{txtProvince.Text} {txtZipCode.Text}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("บันทึกข้อมูลล้มเหลว: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // (ส่วนอื่นๆ LoadOrderHistory, SetupHistoryGridColumns, CellClick, LoadOrderDetails, SetupDetailsGridColumns, btnBack_Click เหมือนเดิมทุกประการ)
        #region "Order History Code (Unchanged)"
        private void LoadOrderHistory()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT o.order_id AS 'ID ออเดอร์', o.order_date AS 'วันที่สั่งซื้อ', o.total_amount AS 'ยอดรวม', o.status AS 'สถานะ' FROM orders AS o WHERE o.user_id = @userId ORDER BY o.order_date DESC";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@userId", CurrentUser.UserID);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvOrderHistory.DataSource = dt;
                    SetupHistoryGridColumns(dgvOrderHistory);
                }
            }
            catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการโหลดประวัติการสั่งซื้อ: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SetupHistoryGridColumns(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgv.Columns.Contains("ID ออเดอร์")) { dgv.Columns["ID ออเดอร์"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
            if (dgv.Columns.Contains("วันที่สั่งซื้อ")) { dgv.Columns["วันที่สั่งซื้อ"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"; dgv.Columns["วันที่สั่งซื้อ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
            if (dgv.Columns.Contains("ยอดรวม")) { dgv.Columns["ยอดรวม"].DefaultCellStyle.Format = "N2"; dgv.Columns["ยอดรวม"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
            if (dgv.Columns.Contains("สถานะ")) { dgv.Columns["สถานะ"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
        }

        private void dgvOrderHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvOrderHistory.Rows[e.RowIndex];
                int selectedOrderId = Convert.ToInt32(row.Cells["ID ออเดอร์"].Value);
                LoadOrderDetails(selectedOrderId);
            }
        }

        private void LoadOrderDetails(int orderId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT p.name AS 'ชื่อสินค้า', od.quantity AS 'จำนวน', od.price_per_item AS 'ราคาต่อหน่วย', (od.quantity * od.price_per_item) AS 'ราคารวม' FROM order_details AS od JOIN products AS p ON od.product_id = p.product_id WHERE od.order_id = @orderId";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvOrderDetails.DataSource = dt;
                    SetupDetailsGridColumns(dgvOrderDetails);
                }
            }
            catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายละเอียดออเดอร์: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SetupDetailsGridColumns(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgv.Columns.Contains("ชื่อสินค้า")) { dgv.Columns["ชื่อสินค้า"].FillWeight = 60; }
            if (dgv.Columns.Contains("จำนวน")) { dgv.Columns["จำนวน"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; dgv.Columns["จำนวน"].FillWeight = 15; }
            if (dgv.Columns.Contains("ราคาต่อหน่วย")) { dgv.Columns["ราคาต่อหน่วย"].DefaultCellStyle.Format = "N2"; dgv.Columns["ราคาต่อหน่วย"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; dgv.Columns["ราคาต่อหน่วย"].FillWeight = 25; }
            if (dgv.Columns.Contains("ราคารวม")) { dgv.Columns["ราคารวม"].DefaultCellStyle.Format = "N2"; dgv.Columns["ราคารวม"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; dgv.Columns["ราคารวม"].FillWeight = 25; }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (pbProfileImage.Image != null) { pbProfileImage.Image.Dispose(); } // ล้างรูปก่อนปิด
            this.Close();
        }
        #endregion
    }
}
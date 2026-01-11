using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class CustomerForm : Form
    {
        // Connection string
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";

        public CustomerForm()
        {
            InitializeComponent();
        }

        // --- โหลดข้อมูลอัตโนมัติเมื่อฟอร์มเปิด ---
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        // ======================================================
        // โซนฟังก์ชันค้นหา (เพิ่มใหม่)
        // ======================================================

        // 1. Event เมื่อพิมพ์ข้อความในช่องค้นหา (ทำงานทันทีแบบ Real-time)
        // *อย่าลืม: ต้องสร้าง TextBox ชื่อ txtSearch และ Link Event TextChanged ที่หน้า Design ก่อน
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadCustomerData(); // ถ้าลบจนว่าง ให้โหลดข้อมูลทั้งหมด
            }
            else
            {
                SearchCustomerData(keyword); // ถ้ามีตัวอักษร ให้ค้นหา
            }
        }

        // 2. เมธอดสำหรับค้นหาข้อมูลใน Database (SQL LIKE)
        private void SearchCustomerData(string keyword)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // ค้นหาจาก 'ชื่อ' หรือ 'เบอร์โทร'
                    string sql = "SELECT user_id, name, province, district, address, zip_code, phone_number " +
                                 "FROM users " +
                                 "WHERE name LIKE @keyword OR phone_number LIKE @keyword";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    // ใส่ % หน้า-หลัง เพื่อให้เจอคำที่พิมพ์แค่บางส่วน
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                // แสดง Error เฉพาะตอน Debug เพื่อไม่ให้รบกวน User เวลาพิมพ์เร็วๆ
                System.Diagnostics.Debug.WriteLine("Search Error: " + ex.Message);
            }
        }

        // ======================================================
        // โซนฟังก์ชันเดิม (โหลด/แก้ไข/ลบ)
        // ======================================================

        // --- ปุ่มโหลดข้อมูล (Manual Refresh) ---
        private void btnLoadData_Click(object sender, EventArgs e)
        {
            txtSearch.Clear(); // ล้างช่องค้นหา
            LoadCustomerData();
        }

        // --- เมธอดโหลดข้อมูลทั้งหมด ---
        private void LoadCustomerData()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT user_id, name, province, district, address, zip_code, phone_number FROM users";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลลูกค้า: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Event เมื่อคลิกแถวใน DataGridView ---
        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
                txtName.Text = row.Cells["name"].Value.ToString();
                txtProvince.Text = row.Cells["province"].Value.ToString();
                txtDistrict.Text = row.Cells["district"].Value.ToString();
                txtAddress.Text = row.Cells["address"].Value.ToString();
                txtZipCode.Text = row.Cells["zip_code"].Value.ToString();
                txtPhoneNumber.Text = row.Cells["phone_number"].Value.ToString();
            }
        }

        // --- ปุ่มบันทึกการเปลี่ยนแปลง ---
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกข้อมูลลูกค้าที่ต้องการแก้ไข", "ยังไม่ได้เลือก", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลที่จำเป็นให้ครบถ้วน", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userIdToUpdate = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["user_id"].Value);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE users SET 
                                   name = @name, 
                                   province = @province, 
                                   district = @district, 
                                   address = @address, 
                                   zip_code = @zip_code, 
                                   phone_number = @phone 
                                 WHERE user_id = @userId";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@province", txtProvince.Text);
                    cmd.Parameters.AddWithValue("@district", txtDistrict.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@zip_code", txtZipCode.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@userId", userIdToUpdate);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลลูกค้าสำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // เช็คว่ากำลังค้นหาอยู่ไหม เพื่อโหลดข้อมูลกลับมาให้ถูกสถานะ
                        if (string.IsNullOrEmpty(txtSearch.Text)) LoadCustomerData();
                        else SearchCustomerData(txtSearch.Text);

                        ClearTextBoxes();
                    }
                    else
                    {
                        MessageBox.Show("ไม่สามารถแก้ไขข้อมูลลูกค้าได้", "แก้ไขล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062) MessageBox.Show("เบอร์โทรศัพท์นี้ถูกใช้โดยลูกค้าคนอื่นแล้ว", "เบอร์โทรซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- ปุ่มลบลูกค้า ---
        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกข้อมูลลูกค้าที่ต้องการลบ", "ยังไม่ได้เลือก", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userIdToDelete = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["user_id"].Value);
            string customerNameToDelete = dgvCustomers.SelectedRows[0].Cells["name"].Value.ToString();

            DialogResult confirmation = MessageBox.Show($"คุณแน่ใจหรือไม่ว่าต้องการลบลูกค้า '{customerNameToDelete}' (ID: {userIdToDelete})?\nการกระทำนี้ไม่สามารถย้อนกลับได้",
                                                        "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "DELETE FROM users WHERE user_id = @userId";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@userId", userIdToDelete);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("ลบข้อมูลลูกค้าสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // เช็คสถานะค้นหาเพื่อรีเฟรชตาราง
                            if (string.IsNullOrEmpty(txtSearch.Text)) LoadCustomerData();
                            else SearchCustomerData(txtSearch.Text);

                            ClearTextBoxes();
                        }
                        else
                        {
                            MessageBox.Show("ไม่สามารถลบข้อมูลลูกค้าได้", "ลบล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1451) MessageBox.Show($"ไม่สามารถลบลูกค้าได้ เนื่องจากมีข้อมูลออเดอร์เชื่อมโยงอยู่", "ลบล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ClearTextBoxes()
        {
            txtName.Clear();
            txtProvince.Clear();
            txtDistrict.Clear();
            txtAddress.Clear();
            txtZipCode.Clear();
            txtPhoneNumber.Clear();
        }
    }
}
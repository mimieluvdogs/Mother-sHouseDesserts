using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Keep this import

namespace test68._10._17 // Make sure namespace matches
{
    public partial class LoginForm : Form
    {
        // --- Placeholder Text variables and methods (Keep if you added them) ---
        string phonePlaceholder = "เบอร์โทรศัพท์"; // Or your placeholder text
        string passPlaceholder = "รหัสผ่าน";      // Or your placeholder text
        //----------------------------------------------------------------------

        public LoginForm()
        {
            InitializeComponent();
            // txtPassword.PasswordChar = '•'; // Keep commented out if you want visible password

            // --- Initialize Placeholders (Keep if you added them) ---
            // SetPlaceholder(txtPhoneNumber, phonePlaceholder);
            // SetPlaceholder(txtPassword, passPlaceholder);
            // txtPassword.PasswordChar = '\0'; // Ensure password placeholder is visible
            //-------------------------------------------------------------
        }

        private void btnGoToRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredPhone = txtPhoneNumber.Text;
            string enteredPassword = txtPassword.Text;

            // --- Validation for empty fields or placeholders (Adjust if needed) ---
            if (string.IsNullOrWhiteSpace(enteredPhone) || enteredPhone == phonePlaceholder ||
                string.IsNullOrWhiteSpace(enteredPassword) || enteredPassword == passPlaceholder)
            {
                MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์และรหัสผ่าน", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //--------------------------------------------------------------------------


            // ----- [เพิ่ม] ส่วนตรวจสอบแอดมิน -----
            string adminPhone = "1111111111";
            string adminPassword = "1111111111"; // Consider storing this more securely later

            if (enteredPhone == adminPhone && enteredPassword == adminPassword)
            {
                // ถ้าตรงกับข้อมูลแอดมิน
                MessageBox.Show("เข้าสู่ระบบแอดมินสำเร็จ!", "Admin Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide(); // ซ่อนหน้า Login
                AdminForm adminForm = new AdminForm(); // สร้างหน้า Admin
                adminForm.Show(); // เปิดหน้า Admin
                return; // ออกจากการทำงานของเมธอดนี้ ไม่ต้องไปเช็คฐานข้อมูลลูกค้าต่อ
            }
            // ----- [จบส่วนตรวจสอบแอดมิน] -----


            // ... (โค้ดส่วนตรวจสอบแอดมิน เหมือนเดิม) ...

            // ----- โค้ดเดิมสำหรับตรวจสอบลูกค้าทั่วไป -----
            string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // (SQL query และ Parameter เหมือนเดิม)
                    string sql = "SELECT user_id, name, province, district, address, zip_code, phone_number " +
                                 "FROM users WHERE phone_number = @phone AND password = @password";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@phone", enteredPhone);
                    cmd.Parameters.AddWithValue("@password", enteredPassword);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Login สำเร็จ (ลูกค้าทั่วไป)
                        {
                            // เก็บข้อมูลผู้ใช้ (เหมือนเดิม)
                            CurrentUser.UserID = reader.GetInt32("user_id");
                            CurrentUser.Name = reader.GetString("name");
                            CurrentUser.Phone = reader.GetString("phone_number");
                            CurrentUser.FullAddress = $"{reader.GetString("address")} " +
                                                        $"อ.{reader.GetString("district")} " +
                                                        $"จ.{reader.GetString("province")} " +
                                                        $"{reader.GetString("zip_code")}";

                            // --- [แก้ไข] เปลี่ยนจุดหมายปลายทาง ---
                            this.Hide();
                            ShopForm shopForm = new ShopForm(); // สร้าง ShopForm
                            shopForm.Show(); // เปิด ShopForm แทน CategoryForm
                                             // ---------------------------------
                        }
                        else
                        {
                            MessageBox.Show("เบอร์โทรศัพท์หรือรหัสผ่านไม่ถูกต้อง", "เข้าสู่ระบบไม่สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }// Connection is automatically closed here
            // ----- จบโค้ดลูกค้าทั่วไป -----
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // --- Call SetPlaceholder if you added it ---
            // SetPlaceholder(txtPhoneNumber, phonePlaceholder);
            // SetPlaceholder(txtPassword, passPlaceholder);
            // txtPassword.PasswordChar = '\0';
            //---------------------------------------------
        }

        // --- Placeholder Text methods (Keep if you added them) ---
        // private void SetPlaceholder(TextBox textBox, string placeholder) { ... }
        // private void TextBox_Enter(object sender, EventArgs e) { ... }
        // private void TextBox_Leave(object sender, EventArgs e) { ... }
        //----------------------------------------------------------
    }
}
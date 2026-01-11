using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO; // <-- [เพิ่ม] สำหรับจัดการไฟล์
using System.Linq;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class RegisterForm : Form
    {
        private Color customTextColor;
        private PrivateFontCollection pfc = new PrivateFontCollection();
        private Font customFontRegular;
        private string _selectedImagePath = null; // <-- [เพิ่ม] ตัวแปรสำหรับเก็บ Path ของรูปที่เลือก

        public RegisterForm()
        {
            InitializeComponent();
            try
            {
                customTextColor = System.Drawing.ColorTranslator.FromHtml("#f2fae1");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Hex color code: " + ex.Message);
                customTextColor = Color.Black;
            }
            InitCustomFont();
        }

        private void InitCustomFont()
        {
            try
            {
                pfc.AddFontFile("Fonts/Sarabun-Regular.ttf");
                customFontRegular = new Font(pfc.Families[0], 12, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not find font file Sarabun-Regular.ttf: " + ex.Message);
                customFontRegular = this.Font;
            }
        }

        // --- Placeholder Text Methods (เหมือนเดิม) ---
        private void SetPlaceholder(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.ForeColor = customTextColor;
            textBox.Font = customFontRegular;
            textBox.Enter += TextBox_Enter;
            textBox.Leave += TextBox_Leave;
            textBox.Tag = placeholder;
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string placeholder = textBox.Tag as string;

            if (textBox.Text == placeholder && textBox.ForeColor == customTextColor)
            {
                textBox.Text = "";
                textBox.ForeColor = SystemColors.WindowText; // สีข้อความปกติ
                textBox.Font = customFontRegular;
            }
        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string placeholder = textBox.Tag as string;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = placeholder;
                textBox.ForeColor = customTextColor;
                textBox.Font = customFontRegular;
            }
        }
        // --- End Placeholder Methods ---

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            SetPlaceholder(txtName, "ชื่อ-สกุล");
            SetPlaceholder(txtProvince, "จังหวัด");
            SetPlaceholder(txtDistrict, "อำเภอ");
            SetPlaceholder(txtAddress, "ที่อยู่ (บ้านเลขที่, ถนน, ตำบล)");
            SetPlaceholder(txtZipCode, "รหัสไปรษณีย์");
            SetPlaceholder(txtPhoneNumber, "เบอร์โทรศัพท์ (10 หลัก)");
            SetPlaceholder(txtPassword, "รหัสผ่าน (อย่างน้อย 6 ตัว)");

            // [เพิ่ม] ตั้งค่ารูปโปรไฟล์เริ่มต้น
            try
            {
                string defaultImagePath = Path.Combine(Application.StartupPath, "Images", "default_profile.png"); // ตรวจสอบ Path ดีๆ
                if (File.Exists(defaultImagePath))
                {
                    pbProfileImage.Image = Image.FromFile(defaultImagePath);
                }
                else
                {
                    // ถ้าหา default_profile.png ไม่เจอ ก็ใช้รูปว่างเปล่า
                    pbProfileImage.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading default profile image: " + ex.Message, "Error");
            }


            // (โค้ดตั้งค่าสี Label และ Button เหมือนเดิม)
            try
            {
                foreach (Control c in this.Controls)
                {
                    if (c is Label)
                    {
                        c.Font = new Font(pfc.Families[0], 16, FontStyle.Bold);
                        c.ForeColor = customTextColor;
                    }
                    else if (c is Button && c != btnSelectImage) // [แก้ไข] ไม่ต้องตั้งค่าปุ่มเลือกรูป
                    {
                        c.Font = new Font(pfc.Families[0], 12, FontStyle.Bold);
                        // [เพิ่ม] ตั้งค่าสีปุ่มเลือกรูปเอง
                        if (c == btnSelectImage)
                        {
                            c.BackColor = Color.LightGreen; // สีเขียวอ่อนๆ
                            c.ForeColor = Color.DarkGreen;
                        }
                    }
                }
                // [เพิ่ม] ตั้งค่าฟอนต์และสีปุ่มเลือกรูปแยกต่างหาก
                if (pfc.Families.Length > 0)
                {
                    btnSelectImage.Font = new Font(pfc.Families[0], 10, FontStyle.Regular);
                }
                else
                {
                    btnSelectImage.Font = new Font(this.Font.FontFamily, 10, FontStyle.Regular);
                }
                btnSelectImage.BackColor = Color.LightGray;
                btnSelectImage.ForeColor = Color.Black;

            }
            catch (Exception ex)
            {
                if (pfc.Families.Length == 0)
                { MessageBox.Show("Custom font not loaded. Controls will use default font."); }
                else
                { MessageBox.Show("Error setting font/color for Labels/Buttons: " + ex.Message); }
            }
        }

        // --- [เพิ่ม] เมธอดสำหรับเลือกรูปโปรไฟล์ ---
        // **อย่าลืมเชื่อม Event Click ของปุ่ม btnSelectImage ในหน้า Designer**
        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
            openFileDialog.Title = "เลือกรูปโปรไฟล์";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _selectedImagePath = openFileDialog.FileName; // เก็บ Path ต้นฉบับ
                pbProfileImage.Image = Image.FromFile(_selectedImagePath); // แสดงรูปใน PictureBox
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // --- Input Validation --- (เหมือนเดิม)
            TextBox[] allTextBoxes = { txtName, txtProvince, txtDistrict, txtAddress, txtZipCode, txtPhoneNumber, txtPassword };
            foreach (var textBox in allTextBoxes)
            {
                string fieldName = textBox.Tag as string ?? "Field";

                if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == fieldName)
                {
                    MessageBox.Show($"กรุณากรอกข้อมูล '{fieldName}' ให้ครบถ้วน", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (txtPhoneNumber.Text.Length != 10 || !txtPhoneNumber.Text.All(char.IsDigit)) { MessageBox.Show("กรุณากรอกเบอร์โทรศัพท์ให้ถูกต้อง (ตัวเลข 10 หลัก)", "ข้อมูลผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (txtZipCode.Text.Length != 5 || !txtZipCode.Text.All(char.IsDigit)) { MessageBox.Show("กรุณากรอกรหัสไปรษณีย์ให้ถูกต้อง (ตัวเลข 5 หลัก)", "ข้อมูลผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (txtPassword.Text.Length < 6) { MessageBox.Show("กรุณาตั้งรหัสผ่านอย่างน้อย 6 ตัวอักษร", "ข้อมูลผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }


            // --- Database Saving ---
            string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // [แก้ไข] เพิ่มการบันทึก profile_image_path
                    string sql = "INSERT INTO users (name, province, district, address, zip_code, phone_number, password, profile_image_path) VALUES (@name, @province, @district, @address, @zip_code, @phone_number, @password, @profileImagePath)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@province", txtProvince.Text);
                    cmd.Parameters.AddWithValue("@district", txtDistrict.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@zip_code", txtZipCode.Text);
                    cmd.Parameters.AddWithValue("@phone_number", txtPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);

                    // --- [เพิ่ม] ส่วนจัดการรูปภาพก่อนบันทึกลง DB ---
                    string savedImagePath = null;
                    if (_selectedImagePath != null)
                    {
                        // สร้างโฟลเดอร์ Images/Profiles ถ้ายังไม่มี
                        string targetDirectory = Path.Combine(Application.StartupPath, "Images", "Profiles");
                        if (!Directory.Exists(targetDirectory))
                        {
                            Directory.CreateDirectory(targetDirectory);
                        }

                        // ตั้งชื่อไฟล์ใหม่ (ใช้ GUID เพื่อให้ชื่อไม่ซ้ำกัน)
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(_selectedImagePath);
                        savedImagePath = Path.Combine(targetDirectory, fileName);

                        // คัดลอกรูปจากต้นฉบับไปยังโฟลเดอร์ของโปรแกรม
                        File.Copy(_selectedImagePath, savedImagePath);

                        // บันทึก Path แบบสัมพัทธ์ (Relative Path) เพื่อความยืดหยุ่น
                        savedImagePath = Path.Combine("Images", "Profiles", fileName);
                    }
                    cmd.Parameters.AddWithValue("@profileImagePath", (object)savedImagePath ?? DBNull.Value); // ถ้าไม่มีรูป ก็บันทึกเป็น NULL
                    // --- [จบส่วนเพิ่ม] ---

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("สมัครสมาชิกสำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnBack_Click(sender, e);
                }
                catch (MySqlException ex)
                {
                    if (ex.Number == 1062) { MessageBox.Show("เบอร์โทรศัพท์นี้ถูกใช้งานแล้ว กรุณาใช้เบอร์อื่น", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else { MessageBox.Show("Database connection error: " + ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
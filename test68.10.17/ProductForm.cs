using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class ProductForm : Form
    {
        // ตั้งค่า Database Connection
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
        private string selectedImagePath = null;

        public ProductForm()
        {
            InitializeComponent();

            numStock.Minimum = -9999;
            txtProductId.Enabled = false;

            // ตั้งค่า ComboBox ให้เป็นแบบ DropDownList (ห้ามพิมพ์เอง ต้องเลือกเท่านั้น)
            cmbCategorys.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;

            chkIsAvailable.Checked = true;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            // เชื่อม Event ให้ตัวกรองทำงานเมื่อเลือกเปลี่ยนค่า
            this.cmbCategoryFilter.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryFilter_SelectedIndexChanged);

            // 1. โหลดรายชื่อหมวดหมู่จากตาราง categories ก่อน
            LoadCategoriesFromDatabase();

            // 2. จากนั้นค่อยโหลดข้อมูลสินค้า
            LoadProductData();
        }

        // --- [แก้ไขใหม่] โหลดหมวดหมู่จากตาราง 'categories' ---
        private void LoadCategoriesFromDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // ดึงข้อมูลจากตาราง categories (ตามรูปที่ 2)
                    string sql = "SELECT category_name FROM categories ORDER BY category_id ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // เคลียร์ค่าเก่า
                    cmbCategorys.Items.Clear();
                    cmbCategoryFilter.Items.Clear();

                    // ใส่ค่า Default ให้ตัวกรอง "ทั้งหมด"
                    cmbCategoryFilter.Items.Add("ทั้งหมด");

                    while (reader.Read())
                    {
                        string catName = reader["category_name"].ToString();
                        cmbCategorys.Items.Add(catName);      // ใส่ในช่องเลือกหมวดหมู่ (หน้ากรอกข้อมูล)
                        cmbCategoryFilter.Items.Add(catName); // ใส่ในช่องตัวกรอง (ข้างบน)
                    }

                    // ถ้าตัวกรองยังไม่ได้เลือกอะไร ให้เลือก "ทั้งหมด" เป็นค่าเริ่มต้น
                    if (cmbCategoryFilter.Items.Count > 0) cmbCategoryFilter.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดหมวดหมู่: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- [แก้ไขใหม่] ปุ่มกดเพิ่มหมวดหมู่ (บันทึกลงตาราง categories) ---
        // *** ต้องมีปุ่มชื่อ btnAddCategory ในหน้า Form ***
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            // เรียกใช้ InputDialog (ฟังก์ชันอยู่ล่างสุดของไฟล์)
            string newCategory = ShowInputDialog("กรุณากรอกชื่อหมวดหมู่ใหม่:", "เพิ่มหมวดหมู่");

            if (!string.IsNullOrWhiteSpace(newCategory))
            {
                // เช็คเบื้องต้นว่ามีใน List หรือยัง
                if (!cmbCategorys.Items.Contains(newCategory))
                {
                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            conn.Open();
                            // สั่ง INSERT ลงตาราง categories
                            string sql = "INSERT INTO categories (category_name) VALUES (@catName)";
                            MySqlCommand cmd = new MySqlCommand(sql, conn);
                            cmd.Parameters.AddWithValue("@catName", newCategory);

                            int result = cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show($"เพิ่มหมวดหมู่ '{newCategory}' เรียบร้อยแล้ว", "สำเร็จ");

                                // โหลดหมวดหมู่ใหม่ทันที เพื่อให้ List อัปเดต
                                LoadCategoriesFromDatabase();

                                // เลือกหมวดหมู่ใหม่ให้อัตโนมัติ
                                cmbCategorys.SelectedItem = newCategory;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ไม่สามารถบันทึกหมวดหมู่ลงฐานข้อมูลได้: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("หมวดหมู่นี้มีอยู่แล้ว", "แจ้งเตือน");
                    cmbCategorys.SelectedItem = newCategory;
                }
            }
        }

        private void cmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductData();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadProductData();
        }

        // --- ฟังก์ชันโหลดข้อมูลสินค้า (Load Product Data) ---
        private void LoadProductData()
        {
            if (cmbCategoryFilter.SelectedItem == null) return;

            string filterCategory = cmbCategoryFilter.SelectedItem.ToString();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // ดึงข้อมูลจากตาราง products
                    string baseSql = "SELECT product_id, category, name, price, stock, image_path, is_available FROM products";
                    string sqlProducts;

                    if (filterCategory == "ทั้งหมด")
                    {
                        sqlProducts = baseSql;
                    }
                    else
                    {
                        sqlProducts = baseSql + " WHERE category = @category";
                    }

                    MySqlDataAdapter adapterProducts = new MySqlDataAdapter(sqlProducts, conn);

                    if (filterCategory != "ทั้งหมด")
                    {
                        adapterProducts.SelectCommand.Parameters.AddWithValue("@category", filterCategory);
                    }

                    DataTable dtProducts = new DataTable();
                    adapterProducts.Fill(dtProducts);

                    // สร้างคอลัมน์ DisplayID สำหรับแสดงรหัสสินค้าสวยๆ (F01, S01, etc.)
                    dtProducts.Columns.Add("DisplayID", typeof(string));

                    foreach (DataRow row in dtProducts.Rows)
                    {
                        string category = row["category"].ToString();
                        int productId = Convert.ToInt32(row["product_id"]);

                        if (category == "For One")
                        {
                            row["DisplayID"] = "F" + productId.ToString("D2");
                        }
                        else if (category == "Snacks Sharing")
                        {
                            row["DisplayID"] = "S" + productId.ToString("D2");
                        }
                        else
                        {
                            // กรณีเป็นหมวดหมู่อื่นๆ ให้เอาตัวอักษรแรกภาษาอังกฤษ หรือใช้ 'G' ถ้าไม่มี
                            // (ถ้าชื่อหมวดเป็นภาษาไทย อาจจะแสดงผลแปลกๆ ตรงนี้อาจจะต้องปรับ logic เพิ่มถ้าอยากได้ตัวย่อภาษาไทย)
                            string prefix = string.IsNullOrEmpty(category) ? "G" : category.Substring(0, 1).ToUpper();
                            row["DisplayID"] = prefix + productId.ToString("D2");
                        }
                    }

                    dgvProducts.DataSource = dtProducts;
                    SetupDataGridViewColumns(dgvProducts);
                }
                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสินค้า: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridViewColumns(DataGridView dgv)
        {
            // ซ่อนคอลัมน์ที่ไม่จำเป็น
            if (dgv.Columns.Contains("product_id")) { dgv.Columns["product_id"].Visible = false; }
            if (dgv.Columns.Contains("image_path")) { dgv.Columns["image_path"].Visible = false; }
            if (dgv.Columns.Contains("category")) { dgv.Columns["category"].Visible = false; }

            // ตั้งค่าคอลัมน์ที่จะแสดง
            if (dgv.Columns.Contains("DisplayID"))
            {
                dgv.Columns["DisplayID"].HeaderText = "รหัสสินค้า";
                dgv.Columns["DisplayID"].DisplayIndex = 0;
                dgv.Columns["DisplayID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgv.Columns.Contains("is_available")) { dgv.Columns["is_available"].HeaderText = "พร้อมขาย"; dgv.Columns["is_available"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; }
            if (dgv.Columns.Contains("name")) dgv.Columns["name"].HeaderText = "ชื่อสินค้า";
            if (dgv.Columns.Contains("price")) { dgv.Columns["price"].HeaderText = "ราคา"; dgv.Columns["price"].DefaultCellStyle.Format = "N2"; }
            if (dgv.Columns.Contains("stock")) dgv.Columns["stock"].HeaderText = "สต็อก";

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- เมื่อคลิกที่ตารางสินค้า ---
        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv != null && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                txtProductId.Text = row.Cells["product_id"].Value.ToString();
                txtName.Text = row.Cells["name"].Value.ToString();
                numPrice.Value = Convert.ToDecimal(row.Cells["price"].Value);
                numStock.Value = Convert.ToInt32(row.Cells["stock"].Value);

                // ดึงชื่อหมวดหมู่จากข้อมูลสินค้า
                string cat = row.Cells["category"].Value.ToString();

                // เช็คว่าหมวดหมู่นี้มีใน Dropdown มั้ย (กันพลาด)
                if (!cmbCategorys.Items.Contains(cat))
                {
                    cmbCategorys.Items.Add(cat);
                }
                cmbCategorys.SelectedItem = cat;

                txtImagePath.Text = row.Cells["image_path"].Value.ToString();

                try
                {
                    object isAvailableValue = row.Cells["is_available"].Value;
                    chkIsAvailable.Checked = isAvailableValue != DBNull.Value && Convert.ToBoolean(isAvailableValue);
                }
                catch { chkIsAvailable.Checked = false; }

                try
                {
                    string fullImagePath = Path.Combine(Application.StartupPath, txtImagePath.Text);
                    if (File.Exists(fullImagePath))
                    {
                        using (var bmpTemp = new Bitmap(fullImagePath)) { pbImagePreview.Image = new Bitmap(bmpTemp); }
                    }
                    else { pbImagePreview.Image = null; }
                }
                catch { pbImagePreview.Image = null; }
                selectedImagePath = null;
            }
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    selectedImagePath = openFileDialog.FileName;
                    using (var bmpTemp = new Bitmap(selectedImagePath)) { pbImagePreview.Image = new Bitmap(bmpTemp); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ไม่สามารถโหลดรูปภาพได้: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    selectedImagePath = null;
                    pbImagePreview.Image = null;
                }
            }
        }

        // --- ปุ่มบันทึกการแก้ไข (Update) ---
        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text)) { MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไขจากตาราง", "ยังไม่ได้เลือก", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrWhiteSpace(txtName.Text) || cmbCategorys.SelectedItem == null || pbImagePreview.Image == null) { MessageBox.Show("กรุณากรอกข้อมูลสินค้าให้ครบถ้วน (ชื่อ, หมวดหมู่, รูปภาพ)", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int productIdToUpdate = Convert.ToInt32(txtProductId.Text);
            string imagePathInDb = txtImagePath.Text;

            // ถ้ามีการเลือกรูปใหม่ ให้ Copy ไปเก็บไว้
            if (selectedImagePath != null)
            {
                try
                {
                    string targetDirectory = Path.Combine(Application.StartupPath, "images");
                    if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }
                    string fileName = Path.GetFileName(selectedImagePath);
                    string destinationPath = Path.Combine(targetDirectory, fileName);
                    File.Copy(selectedImagePath, destinationPath, true);
                    imagePathInDb = Path.Combine("images", fileName).Replace("\\", "/");
                }
                catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกไฟล์รูปภาพ: " + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"UPDATE products SET
                                    name = @name,
                                    price = @price,
                                    stock = @stock,
                                    image_path = @image_path,
                                    category = @category,
                                    is_available = @is_available
                                    WHERE product_id = @productId";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", numPrice.Value);
                    cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(numStock.Value));
                    cmd.Parameters.AddWithValue("@image_path", imagePathInDb);
                    // บันทึกเป็นชื่อหมวดหมู่ (String) ลงตาราง products
                    cmd.Parameters.AddWithValue("@category", cmbCategorys.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@productId", productIdToUpdate);
                    cmd.Parameters.AddWithValue("@is_available", chkIsAvailable.Checked ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("แก้ไขข้อมูลสินค้าสำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // โหลดข้อมูลใหม่
                        LoadProductData();
                    }
                    else { MessageBox.Show("ไม่สามารถแก้ไขข้อมูลสินค้าได้", "แก้ไขล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            }
            catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // --- ปุ่มเพิ่มสินค้าใหม่ (Add New) ---
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || cmbCategorys.SelectedItem == null || selectedImagePath == null || pbImagePreview.Image == null) { MessageBox.Show("กรุณากรอกข้อมูลสินค้าใหม่ให้ครบถ้วน (ชื่อ, หมวดหมู่, รูปภาพ)", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!string.IsNullOrWhiteSpace(txtProductId.Text)) { MessageBox.Show("กรุณากด 'ล้างข้อมูล' ก่อนเพิ่มสินค้าใหม่", "ข้อมูลค้าง", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            string imagePathToSave = "";
            try
            {
                string targetDirectory = Path.Combine(Application.StartupPath, "images");
                if (!Directory.Exists(targetDirectory)) { Directory.CreateDirectory(targetDirectory); }
                string fileName = Path.GetFileName(selectedImagePath);
                string destinationPath = Path.Combine(targetDirectory, fileName);
                File.Copy(selectedImagePath, destinationPath, true);
                imagePathToSave = Path.Combine("images", fileName).Replace("\\", "/");
            }
            catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกไฟล์รูปภาพ: " + ex.Message, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string sql = @"INSERT INTO products (name, price, stock, image_path, category, is_available)
                                   VALUES (@name, @price, @stock, @image_path, @category, @is_available)";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@price", numPrice.Value);
                    cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(numStock.Value));
                    cmd.Parameters.AddWithValue("@image_path", imagePathToSave);
                    cmd.Parameters.AddWithValue("@category", cmbCategorys.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@is_available", chkIsAvailable.Checked ? 1 : 0);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("เพิ่มสินค้าใหม่สำเร็จ!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProductData();
                    }
                    else { MessageBox.Show("ไม่สามารถเพิ่มสินค้าใหม่ได้", "เพิ่มล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            }
            catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาดในการเพิ่มข้อมูลสินค้า: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductId.Text)) { MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการลบจากตาราง", "ยังไม่ได้เลือก", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            int productIdToUpdate = Convert.ToInt32(txtProductId.Text);
            string productNameToUpdate = txtName.Text;

            DialogResult confirmation = MessageBox.Show($"คุณแน่ใจหรือไม่ว่าต้องการ 'ซ่อน/ยกเลิกการขาย' สินค้า '{productNameToUpdate}' (ID: {productIdToUpdate})?",
                                                        "ยืนยันการยกเลิกการขาย", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmation == DialogResult.Yes)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string sql = "UPDATE products SET is_available = 0 WHERE product_id = @productId";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@productId", productIdToUpdate);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("ซ่อน/ยกเลิกการขายสินค้าสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProductData();
                        }
                        else { MessageBox.Show("ไม่สามารถอัปเดตสถานะสินค้าได้", "ล้มเหลว", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    }
                }
                catch (Exception ex) { MessageBox.Show("เกิดข้อผิดพลาด: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void btnClearFields_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (pbImagePreview.Image != null) { pbImagePreview.Image.Dispose(); pbImagePreview.Image = null; }
            this.Close();
        }

        private void ClearTextBoxes()
        {
            txtProductId.Clear();
            txtName.Clear();
            numPrice.Value = 0;
            numStock.Value = 0;

            if (cmbCategorys.Items.Count > 0) cmbCategorys.SelectedIndex = -1;

            if (pbImagePreview.Image != null) { pbImagePreview.Image.Dispose(); }
            pbImagePreview.Image = null;
            txtImagePath.Clear();
            selectedImagePath = null;
            chkIsAvailable.Checked = true;

            dgvProducts.ClearSelection();
        }

        // --- Helper Class สำหรับสร้าง Input Dialog (Popup กรอกชื่อหมวดหมู่) ---
        public static string ShowInputDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };
            Label textLabel = new Label() { Left = 20, Top = 20, Text = text, AutoSize = true, Font = new Font("Segoe UI", 10) };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, Font = new Font("Segoe UI", 10) };
            Button confirmation = new Button() { Text = "ตกลง", Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK, Height = 30 };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
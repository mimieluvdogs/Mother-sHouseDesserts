using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class SalesReportForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";

        public SalesReportForm()
        {
            InitializeComponent();
        }

        private void SalesReportForm_Load(object sender, EventArgs e)
        {
            // ⭐️ [แก้ไขใหม่] โหลดหมวดหมู่จาก Database แทนการพิมพ์เอง
            LoadCategories();

            // ตั้งค่า ComboBox ช่วงเวลา
            cmbTimeRange.Items.Clear(); // เคลียร์ของเก่าเผื่อไว้
            cmbTimeRange.Items.Add("วันนี้");
            cmbTimeRange.Items.Add("สัปดาห์นี้");
            cmbTimeRange.Items.Add("เดือนนี้");
            cmbTimeRange.Items.Add("ปีนี้");
            cmbTimeRange.Items.Add("กำหนดเอง");
            cmbTimeRange.SelectedIndex = 2; // ค่าเริ่มต้น: เดือนนี้

            ToggleDatePickers(false);
            lblTotalSales.Text = "0.00 บาท";

            // ตั้งค่าตาราง Top 5 รอไว้
            SetupTopSellingGrid();
        }

        // ⭐️⭐️⭐️ [เพิ่มฟังก์ชันนี้] โหลดหมวดหมู่จากตาราง categories ⭐️⭐️⭐️
        private void LoadCategories()
        {
            try
            {
                cmbCategoryFilter.Items.Clear();
                // เพิ่มตัวเลือก "สินค้าทั้งหมด" เป็นตัวแรกเสมอ
                cmbCategoryFilter.Items.Add("สินค้าทั้งหมด");

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    // ดึงชื่อหมวดหมู่จากตาราง categories
                    string sql = "SELECT category_name FROM categories ORDER BY category_id ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string catName = reader["category_name"].ToString();
                            cmbCategoryFilter.Items.Add(catName);
                        }
                    }
                }
                // เลือก "สินค้าทั้งหมด" เป็นค่าเริ่มต้น
                cmbCategoryFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดหมวดหมู่ไม่สำเร็จ: " + ex.Message);
                // กรณี Error ให้มีค่า Default ไว้นิดหน่อย
                cmbCategoryFilter.Items.Add("สินค้าทั้งหมด");
                cmbCategoryFilter.SelectedIndex = 0;
            }
        }

        private void cmbTimeRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool showCustomDates = cmbTimeRange.SelectedItem.ToString() == "กำหนดเอง";
            ToggleDatePickers(showCustomDates);
        }

        private void ToggleDatePickers(bool show)
        {
            dtpStartDate.Visible = show;
            dtpEndDate.Visible = show;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            // 1. คำนวณวันที่
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(1);
            string selectedRange = cmbTimeRange.SelectedItem.ToString();
            DateTime now = DateTime.Now;

            switch (selectedRange)
            {
                case "วันนี้":
                    startDate = now.Date;
                    endDate = startDate.AddDays(1);
                    break;
                case "สัปดาห์นี้":
                    startDate = now.Date.AddDays(-(int)now.DayOfWeek);
                    endDate = startDate.AddDays(7);
                    break;
                case "เดือนนี้":
                    startDate = new DateTime(now.Year, now.Month, 1);
                    endDate = startDate.AddMonths(1);
                    break;
                case "ปีนี้":
                    startDate = new DateTime(now.Year, 1, 1);
                    endDate = startDate.AddYears(1);
                    break;
                case "กำหนดเอง":
                    startDate = dtpStartDate.Value.Date;
                    endDate = dtpEndDate.Value.Date.AddDays(1);
                    if (startDate >= endDate)
                    {
                        MessageBox.Show("วันที่เริ่มต้นต้องมาก่อนวันที่สิ้นสุด", "เตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    break;
            }

            // ดึงค่าหมวดหมู่ที่เลือก
            string selectedCategory = cmbCategoryFilter.SelectedItem.ToString();

            // 2. โหลดรายงานหลัก
            LoadMainReport(startDate, endDate, selectedCategory);

            // 3. โหลด Top 5
            LoadTop5Bestsellers(startDate, endDate, selectedCategory);
        }

        private void LoadMainReport(DateTime startDate, DateTime endDate, string selectedCategory)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"
                SELECT 
                    p.name AS 'ชื่อสินค้า', 
                    p.category AS 'หมวดหมู่', 
                    SUM(od.quantity) AS 'จำนวนที่ขายได้', 
                    SUM(od.quantity * od.price_per_item) AS 'ยอดรวม'
                FROM order_details AS od
                JOIN products AS p ON od.product_id = p.product_id
                JOIN orders AS o ON od.order_id = o.order_id
                WHERE (o.order_date >= @startDate AND o.order_date < @endDate) ");

            // ถ้าเลือกหมวดหมู่ที่ไม่ใช่ "สินค้าทั้งหมด" ให้เพิ่มเงื่อนไข WHERE
            if (selectedCategory != "สินค้าทั้งหมด")
            {
                sqlBuilder.Append("AND p.category = @category ");
            }

            sqlBuilder.Append("GROUP BY p.product_id, p.name, p.category ORDER BY SUM(od.quantity) DESC");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqlBuilder.ToString(), conn);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    if (selectedCategory != "สินค้าทั้งหมด")
                    {
                        cmd.Parameters.AddWithValue("@category", selectedCategory);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // คำนวณยอดรวม
                    double totalSales = 0.0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["ยอดรวม"] != DBNull.Value) totalSales += Convert.ToDouble(row["ยอดรวม"]);
                    }
                    lblTotalSales.Text = totalSales.ToString("N2") + " บาท";

                    dgvReport.DataSource = dt;
                    SetupReportGridColumns(dgvReport);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลดรายงานหลักผิดพลาด: " + ex.Message);
            }
        }

        private void SetupTopSellingGrid()
        {
            dgvTopSelling.RowTemplate.Height = 60;
            dgvTopSelling.AllowUserToAddRows = false;
            dgvTopSelling.RowHeadersVisible = false;
            dgvTopSelling.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadTop5Bestsellers(DateTime startDate, DateTime endDate, string selectedCategory)
        {
            dgvTopSelling.Rows.Clear();
            dgvTopSelling.Columns.Clear();

            DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
            imgCol.Name = "Image";
            imgCol.HeaderText = "รูปสินค้า";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            imgCol.Width = 80;
            dgvTopSelling.Columns.Add(imgCol);

            dgvTopSelling.Columns.Add("Name", "สินค้าขายดี");
            dgvTopSelling.Columns.Add("Qty", "ขายแล้ว (ชิ้น)");

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append(@"
                SELECT 
                    p.name, 
                    SUM(od.quantity) AS total_qty, 
                    p.image_path
                FROM order_details AS od
                JOIN products AS p ON od.product_id = p.product_id
                JOIN orders AS o ON od.order_id = o.order_id
                WHERE (o.order_date >= @startDate AND o.order_date < @endDate) ");

            if (selectedCategory != "สินค้าทั้งหมด")
            {
                sqlBuilder.Append("AND p.category = @category ");
            }

            sqlBuilder.Append("GROUP BY p.product_id, p.name, p.image_path ORDER BY total_qty DESC LIMIT 5");

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(sqlBuilder.ToString(), conn);
                    cmd.Parameters.AddWithValue("@startDate", startDate);
                    cmd.Parameters.AddWithValue("@endDate", endDate);

                    if (selectedCategory != "สินค้าทั้งหมด")
                    {
                        cmd.Parameters.AddWithValue("@category", selectedCategory);
                    }

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader["name"].ToString();
                            string qty = reader["total_qty"].ToString();
                            string imagePath = reader["image_path"].ToString();

                            Image img = null;
                            string fullPath = Path.Combine(Application.StartupPath, imagePath);
                            if (File.Exists(fullPath))
                            {
                                try { using (var bmpTemp = new Bitmap(fullPath)) { img = new Bitmap(bmpTemp); } } catch { }
                            }

                            dgvTopSelling.Rows.Add(img, name, qty);
                        }
                    }
                }

                dgvTopSelling.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTopSelling.Columns["Qty"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show("โหลด Top 5 ผิดพลาด: " + ex.Message);
            }
        }

        private void SetupReportGridColumns(DataGridView dgv)
        {
            if (dgv.Columns.Contains("ชื่อสินค้า")) dgv.Columns["ชื่อสินค้า"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            if (dgv.Columns.Contains("หมวดหมู่")) dgv.Columns["หมวดหมู่"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgv.Columns.Contains("จำนวนที่ขายได้")) dgv.Columns["จำนวนที่ขายได้"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            if (dgv.Columns.Contains("ยอดรวม"))
            {
                dgv.Columns["ยอดรวม"].DefaultCellStyle.Format = "N2";
                dgv.Columns["ยอดรวม"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ฟังก์ชันหลอกๆ สำหรับแก้บั๊ก Designer ถ้ามี Error เรื่อง lblTotalSales_Click
        private void lblTotalSales_Click(object sender, EventArgs e) { }
    }
}
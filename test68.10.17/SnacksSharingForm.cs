using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic; // <-- ตรวจสอบว่ามี using นี้
using System.IO; // <-- ตรวจสอบว่ามี using นี้

namespace test68._10._17
{
    public partial class SnacksSharingForm : Form
    {
        private PrivateFontCollection pfc = new PrivateFontCollection();

        public SnacksSharingForm()
        {
            InitializeComponent();
            InitCustomFont();

            // [เพิ่ม] เชื่อม Event FormClosed เพื่อล้าง Memory
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SnacksSharingForm_FormClosed);
        }

        private void InitCustomFont()
        {
            try
            {
                pfc.AddFontFile("Fonts/Sarabun-Regular.ttf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("หาไฟล์ฟอนต์ไม่เจอ ตรวจสอบขั้นตอนใน Visual Studio อีกครั้ง\n\n" + ex.Message, "Error");
            }
        }

        private void SnacksSharingForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // --- [แก้ไข] เมธอด LoadProducts() (เพิ่มการล้าง Memory และ โหลดรูปปลอดภัย) ---
        private void LoadProducts()
        {
            try
            {
                // [แก้ไข] 1. ล้าง Control และคืน Memory รูปภาพเก่าก่อน
                foreach (Control ctrl in flpProducts.Controls)
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
                flpProducts.Controls.Clear();
                // ------------------------------------------

                string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // [แก้ไข] 2. เปลี่ยน Category เป็น 'Snacks Sharing'
                    string sql = "SELECT product_id, name, price, stock, image_path FROM products WHERE category = 'Snacks Sharing' AND is_available = 1";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Panel productPanel = new Panel
                            {
                                Size = new System.Drawing.Size(170, 220),
                                Margin = new Padding(4),
                                BackColor = Color.White,
                                BorderStyle = BorderStyle.FixedSingle
                            };

                            productPanel.Tag = reader.GetInt32("product_id");
                            productPanel.Cursor = Cursors.Hand;
                            productPanel.Click += ProductPanel_Click;

                            PictureBox pb = new PictureBox
                            {
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Dock = DockStyle.Top,
                                Height = 140
                            };

                            // [แก้ไข] 3. โหลดรูปภาพแบบปลอดภัย (ไม่ล็อกไฟล์)
                            try
                            {
                                string fullImagePath = Path.Combine(Application.StartupPath, reader["image_path"].ToString());
                                if (File.Exists(fullImagePath))
                                {
                                    using (var bmpTemp = new Bitmap(fullImagePath))
                                    {
                                        pb.Image = new Bitmap(bmpTemp);
                                    }
                                }
                            }
                            catch { pb.Image = null; }
                            // ------------------------------------------

                            pb.Click += (sender, e) => { ProductPanel_Click(productPanel, e); };

                            Label nameLabel = new Label
                            {
                                Text = reader["name"].ToString(),
                                Font = new Font(pfc.Families[0], 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 25,
                                Cursor = Cursors.Hand
                            };
                            nameLabel.Click += (sender, e) => { ProductPanel_Click(productPanel, e); };

                            Label priceLabel = new Label
                            {
                                Text = string.Format("{0:N2} ฿", reader.GetDecimal("price")),
                                Font = new Font(pfc.Families[0], 10, FontStyle.Regular),
                                ForeColor = Color.DarkGreen,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 20,
                                Cursor = Cursors.Hand
                            };
                            priceLabel.Click += (sender, e) => { ProductPanel_Click(productPanel, e); };

                            Label stockLabel = new Label
                            {
                                Text = $"สต็อก: {reader.GetInt32("stock")} ชิ้น",
                                Font = new Font(pfc.Families[0], 9, FontStyle.Italic),
                                ForeColor = Color.DimGray,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 20,
                                Cursor = Cursors.Hand
                            };
                            stockLabel.Click += (sender, e) => { ProductPanel_Click(productPanel, e); };

                            productPanel.Controls.Add(pb);
                            productPanel.Controls.Add(stockLabel);
                            productPanel.Controls.Add(priceLabel);
                            productPanel.Controls.Add(nameLabel);

                            flpProducts.Controls.Add(productPanel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสินค้า:\n\n" + ex.Message, "Data Load Error");
            }
        }
        // --- [จบส่วนแก้ไข] ---

        // --- [!!! จุดแก้ไขหลัก !!!] ---
        // (เมธอดนี้ถูกแก้ไขให้ตรวจสอบสต็อกเทียบกับตะกร้า)
        private void ProductPanel_Click(object sender, EventArgs e)
        {
            Panel clickedPanel = sender as Panel;
            int productId = (int)clickedPanel.Tag;

            string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM products WHERE product_id = @id AND is_available = 1";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", productId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // 1. ดึงข้อมูลสินค้าจาก DB
                        int stock = reader.GetInt32("stock");
                        string name = reader.GetString("name");

                        // 2. [Check 1] ตรวจสอบว่าสินค้าหมดสต็อกหรือไม่
                        if (stock <= 0)
                        {
                            MessageBox.Show($"ขออภัย '{name}' สินค้าหมดสต็อกค่ะ", "สินค้าหมด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // จบการทำงาน
                        }

                        // 3. [Check 2] ตรวจสอบจำนวนในตะกร้าเทียบกับสต็อก
                        var itemInCart = ShoppingCart.Items.Find(i => i.ProductID == productId);
                        int quantityInCart = (itemInCart != null) ? itemInCart.Quantity : 0;

                        // ถ้าจำนวนที่จะมี (ในตะกร้า + 1) มากกว่าสต็อก
                        if (quantityInCart + 1 > stock)
                        {
                            MessageBox.Show($"ขออภัย, สินค้า '{name}' มีสต็อกไม่เพียงพอ\n\n" +
                                            $"(มีในสต็อก {stock} ชิ้น, คุณมีในตะกร้าแล้ว {quantityInCart} ชิ้น)",
                                            "สต็อกไม่พอ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // จบการทำงาน ไม่เพิ่มของ
                        }

                        // 4. ถ้าผ่านทุกการตรวจสอบ ก็เพิ่มลงตะกร้า
                        CartItem newItem = new CartItem
                        {
                            ProductID = productId,
                            Name = name,
                            Price = reader.GetDecimal("price"),
                            ImagePath = reader.GetString("image_path")
                        };

                        ShoppingCart.AddItem(newItem);
                        MessageBox.Show($"เพิ่ม '{newItem.Name}' ลงในตะกร้าแล้ว!", "เพิ่มสินค้าสำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("ขออภัย ไม่พบข้อมูลสินค้าชิ้นนี้ หรือสินค้าถูกยกเลิกการขายแล้ว", "ไม่พบสินค้า", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // --- [แก้ไข] btnCart_Click (เพิ่ม LoadProducts) ---
        private void btnCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();

            // [แก้ไข] โหลดข้อมูลสินค้าใหม่หลังจากปิดตะกร้า
            LoadProducts();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAboutUs_Click(object sender, EventArgs e)
        {
            this.Hide();
            AboutUsForm aboutUsForm = new AboutUsForm();
            aboutUsForm.ShowDialog();
            this.Show();
        }

        // --- [เพิ่ม] เมธอดสำหรับล้าง Memory ตอนปิดฟอร์ม ---
        private void SnacksSharingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (Control ctrl in flpProducts.Controls)
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
        }
        // --- [จบส่วนเพิ่ม] ---
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace test68._10._17
{
    public partial class ForOneForm : Form
    {
        private PrivateFontCollection pfc = new PrivateFontCollection();

        public ForOneForm()
        {
            InitializeComponent();
            InitCustomFont();
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

        private void ForOneForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        // (เมธอด LoadProducts() ไม่ต้องแก้ไข)
        private void LoadProducts()
        {
            try
            {
                // (ล้าง Control เก่าเพื่อป้องกัน Memory Leak)
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
                    string sql = "SELECT product_id, name, price, stock, image_path FROM products WHERE category = 'For One' AND is_available = 1";
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

                            // (โหลดรูปภาพแบบปลอดภัย)
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

                            pb.Click += (s, e) => { ProductPanel_Click(productPanel, e); };

                            Label nameLabel = new Label
                            {
                                Text = reader["name"].ToString(),
                                Font = new Font(pfc.Families[0], 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 25,
                                Cursor = Cursors.Hand
                            };
                            nameLabel.Click += (s, e) => { ProductPanel_Click(productPanel, e); };

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
                            priceLabel.Click += (s, e) => { ProductPanel_Click(productPanel, e); };

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
                            stockLabel.Click += (s, e) => { ProductPanel_Click(productPanel, e); };

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
                // (ดึงข้อมูลล่าสุดเสมอเผื่อสต็อกเปลี่ยน)
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

                        // (เมธอด AddItem ใน ShoppingCart จะจัดการเองว่า +1 หรือ add new)
                        ShoppingCart.AddItem(newItem);
                        MessageBox.Show($"เพิ่ม '{newItem.Name}' ลงในตะกร้าแล้ว!", "เพิ่มสินค้าสำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // (เผื่อกรณีกดคลิกพร้อมกับที่ Admin ซ่อนสินค้าพอดี)
                        MessageBox.Show("ขออภัย ไม่พบข้อมูลสินค้าชิ้นนี้ หรือสินค้าถูกยกเลิกการขายแล้ว", "ไม่พบสินค้า", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        #region "โค้ดเดิม (Event Handlers)"
        private void btnCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();

            // [เพิ่ม] โหลดข้อมูลสินค้าใหม่หลังจากปิดตะกร้า
            // เผื่อสต็อกมีการเปลี่ยนแปลง (เช่น ถูกยกเลิก หรือ Admin แก้ไข)
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

        // [เพิ่ม] จัดการการปิด Form เพื่อคืน Memory รูปภาพ
        private void ForOneForm_FormClosed(object sender, FormClosedEventArgs e)
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
        #endregion
    }
}
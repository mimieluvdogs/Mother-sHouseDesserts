using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class ShopForm : Form
    {
        private string connectionString = "server=localhost;user=root;password=;database=mothers_house_db;";
        private PrivateFontCollection pfc = new PrivateFontCollection();

        public ShopForm()
        {
            InitializeComponent();
            InitCustomFont();
        }

        private void InitCustomFont()
        {
            try { pfc.AddFontFile("Fonts/Sarabun-Regular.ttf"); } catch { }
        }

        private void ShopForm_Load(object sender, EventArgs e)
        {
            LoadCategoryButtons(); // 1. สร้างปุ่มหมวดหมู่
            LoadProducts("For One"); // 2. โหลดสินค้าหมวดแรกเป็นค่าเริ่มต้น
        }

        // --- ส่วนที่ 1: สร้างปุ่มหมวดหมู่แบบอัตโนมัติ ---
        private void LoadCategoryButtons()
        {
            flpCategories.Controls.Clear();

            // สร้างปุ่ม "ทั้งหมด" (เผื่ออยากดูรวม)
            Button btnAll = CreateCategoryButton("ทั้งหมด");
            flpCategories.Controls.Add(btnAll);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT category_name FROM categories";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string catName = reader.GetString("category_name");
                            Button btn = CreateCategoryButton(catName);
                            flpCategories.Controls.Add(btn);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("โหลดหมวดหมู่ไม่สำเร็จ: " + ex.Message); }
        }

        // เมธอดช่วยสร้างปุ่มให้สวยงาม
        private Button CreateCategoryButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Size = new Size(115, 40);
            btn.Font = new Font(pfc.Families[0], 12, FontStyle.Bold);
            btn.BackColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            // **สำคัญ** เชื่อม Event Click ให้ทุกปุ่มวิ่งไปหาเมธอดเดียวกัน
            btn.Click += CategoryButton_Click;
            return btn;
        }

        // เมื่อกดปุ่มหมวดหมู่
        private void CategoryButton_Click(object sender, EventArgs e)
        {
            Button clickedBtn = sender as Button;
            string selectedCategory = clickedBtn.Text;

            // เปลี่ยนสีปุ่มเพื่อบอกว่าเลือกอันไหนอยู่ (Optional)
            foreach (Control c in flpCategories.Controls) { c.BackColor = Color.White; }
            clickedBtn.BackColor = Color.LightYellow;

            // โหลดสินค้าตามชื่อปุ่มที่กด
            LoadProducts(selectedCategory);
        }

        // --- ส่วนที่ 2: โหลดสินค้า (รับชื่อหมวดหมู่มา) ---
        private void LoadProducts(string categoryName)
        {
            flpProducts.Controls.Clear(); // ล้างสินค้าเก่าออกก่อน

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = "";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;

                    if (categoryName == "ทั้งหมด")
                    {
                        sql = "SELECT product_id, name, price, stock, image_path FROM products WHERE is_available = 1";
                    }
                    else
                    {
                        sql = "SELECT product_id, name, price, stock, image_path FROM products WHERE category = @cat AND is_available = 1";
                        cmd.Parameters.AddWithValue("@cat", categoryName);
                    }

                    cmd.CommandText = sql;

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // --- สร้างการ์ดสินค้า (ใช้โค้ดเดิมที่ปรับขนาดแล้ว) ---
                            Panel productPanel = new Panel
                            {
                                Size = new System.Drawing.Size(170, 220),
                                Margin = new Padding(4),
                                BackColor = Color.White,
                                BorderStyle = BorderStyle.FixedSingle,
                                Tag = reader.GetInt32("product_id"), // เก็บ ID
                                Cursor = Cursors.Hand
                            };
                            productPanel.Click += ProductPanel_Click; // คลิกที่ Panel

                            PictureBox pb = new PictureBox
                            {
                                ImageLocation = reader["image_path"].ToString(), // ใช้ ImageLocation จะจัดการเรื่องไฟล์ไม่เจอได้ง่ายกว่า
                                SizeMode = PictureBoxSizeMode.Zoom,
                                Dock = DockStyle.Top,
                                Height = 140
                            };
                            pb.Click += (s, ev) => ProductPanel_Click(productPanel, ev); // คลิกรูป

                            Label nameLabel = new Label
                            {
                                Text = reader["name"].ToString(),
                                Font = new Font(pfc.Families[0], 11, FontStyle.Bold),
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 25
                            };
                            nameLabel.Click += (s, ev) => ProductPanel_Click(productPanel, ev);

                            Label priceLabel = new Label
                            {
                                Text = string.Format("{0:N2} ฿", reader.GetDecimal("price")),
                                Font = new Font(pfc.Families[0], 10, FontStyle.Regular),
                                ForeColor = Color.DarkGreen,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 20
                            };
                            priceLabel.Click += (s, ev) => ProductPanel_Click(productPanel, ev);

                            Label stockLabel = new Label
                            {
                                Text = $"สต็อก: {reader.GetInt32("stock")} ชิ้น",
                                Font = new Font(pfc.Families[0], 9, FontStyle.Italic),
                                ForeColor = Color.DimGray,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Dock = DockStyle.Bottom,
                                Height = 20
                            };
                            stockLabel.Click += (s, ev) => ProductPanel_Click(productPanel, ev);

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
                MessageBox.Show("โหลดสินค้าไม่สำเร็จ: " + ex.Message);
            }
        }

        // --- Event คลิกสินค้าเพื่อเพิ่มลงตะกร้า (เหมือนเดิม) ---
        private void ProductPanel_Click(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            int productId = (int)pnl.Tag; // ดึง ID ที่ซ่อนไว้

            // ... (คัดลอกโค้ดตรวจสอบสต็อกและเพิ่มลง ShoppingCart จาก ForOneForm มาใส่ตรงนี้ได้เลย) ...
            // ... หรือจะให้ผมเขียนให้ใหม่ก็ได้ครับ แต่หลักการเดิมเป๊ะๆ ...
            AddToCart(productId);
        }

        private void AddToCart(int productId)
        {
            // โค้ดเพิ่มลงตะกร้า (เหมือนใน ForOneForm)
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM products WHERE product_id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", productId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int stock = reader.GetInt32("stock");
                        if (stock <= 0)
                        {
                            MessageBox.Show("สินค้าหมดสต็อกค่ะ", "แจ้งเตือน");
                            return;
                        }
                        CartItem newItem = new CartItem
                        {
                            ProductID = reader.GetInt32("product_id"),
                            Name = reader.GetString("name"),
                            Price = reader.GetDecimal("price"),
                            ImagePath = reader.GetString("image_path")
                        };
                        ShoppingCart.AddItem(newItem);
                        MessageBox.Show($"เพิ่ม '{newItem.Name}' ลงในตะกร้าแล้ว!");
                    }
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e) { this.Close(); }
        

        private void btnProfile_Click(object sender, EventArgs e)
        {
            this.Hide(); // ซ่อนหน้าปัจจุบัน
            ProfileForm profileForm = new ProfileForm(); // สร้างหน้าโปรไฟล์
            profileForm.ShowDialog(); // เปิดหน้าโปรไฟล์ (และรอจนกว่าจะปิด)
            this.Show(); // กลับมาแสดงหน้า CategoryForm
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();
            // อาจจะมีการรีเฟรชหน้าถ้าจำเป็น
        }

        private void btnAboutUs_Click(object sender, EventArgs e)
        {
            // 1. ซ่อนหน้าปัจจุบัน\
            this.Hide();

            // 2. สร้างและเปิดหน้า AboutUsForm แบบ ShowDialog()
            //    ซึ่งจะหยุดรอจนกว่าหน้า AboutUsForm จะถูกปิด
            AboutUsForm aboutUsForm = new AboutUsForm();
            aboutUsForm.ShowDialog();

            // 3. เมื่อหน้า AboutUsForm ถูกปิด (โดยการกดปุ่มย้อนกลับ)
            //    ให้กลับมาแสดง อีกครั้ง
            this.Show();
        }
    }
}
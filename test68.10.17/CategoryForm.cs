using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test68._10._17
{
    public partial class CategoryForm : Form
    {
        public CategoryForm()
        {
            InitializeComponent();
            // ถ้าอยากให้ฟอร์มเต็มจออัตโนมัติ (แต่ในตัวอย่างนี้เราตั้ง Size ไปแล้ว)
            // this.WindowState = FormWindowState.Maximized; 
        }

        // ปุ่มด้านบน
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



        private void btnCart_Click(object sender, EventArgs e)
        {
            CartForm cartForm = new CartForm();
            cartForm.ShowDialog();
            // อาจจะมีการรีเฟรชหน้าถ้าจำเป็น
        }

        // ปุ่มเลือกหมวดหมู่หลักใน Panel
        private void btnForOne_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                ForOneForm forOneForm = new ForOneForm();
                forOneForm.ShowDialog();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "เกิดข้อผิดพลาดตอนพยายามเปิดหน้า ForOneForm:\n\n" +
                    "Error Message: " + ex.Message + "\n\n" +
                    "StackTrace: " + ex.StackTrace,
                    "Runtime Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.Show();
            }
        }

        private void btnSnacksSharing_Click(object sender, EventArgs e)
        {
            this.Hide();
            SnacksSharingForm snacksForm = new SnacksSharingForm();
            snacksForm.ShowDialog();
            this.Show();
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            this.Hide(); // ซ่อนหน้าปัจจุบัน
            ProfileForm profileForm = new ProfileForm(); // สร้างหน้าโปรไฟล์
            profileForm.ShowDialog(); // เปิดหน้าโปรไฟล์ (และรอจนกว่าจะปิด)
            this.Show(); // กลับมาแสดงหน้า CategoryForm
        }

        // (ถ้ามีปุ่มออกจากระบบ อาจจะอยู่ที่มุมบนขวา หรือที่ไหนก็ได้)
        // ตัวอย่างการออกจากระบบ/กลับหน้า Login
        // private void btnLogOut_Click(object sender, EventArgs e)
        // {
        //     Application.Restart(); // ปิดโปรแกรมแล้วเริ่มใหม่ที่หน้า LoginForm
        // }
    }
}

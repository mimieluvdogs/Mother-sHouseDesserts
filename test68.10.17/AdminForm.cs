using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test68._10._17 // ตรวจสอบว่า namespace ตรงกับโปรเจกต์ของคุณ
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            // สามารถเพิ่มโค้ดที่ต้องการให้ทำงานตอนฟอร์มโหลดได้ที่นี่ (ถ้ามี)
        }

        // --- Event Handler สำหรับปุ่ม "Customer" ---
        private void btnCustomer_Click(object sender, EventArgs e) // ตรวจสอบว่าชื่อปุ่มตรงกัน (เช่น btnCustomer)
        {
            this.Hide(); // ซ่อน AdminForm
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog(); // แสดง CustomerForm และรอจนกว่าจะถูกปิด
            this.Show(); // แสดง AdminForm อีกครั้งเมื่อ CustomerForm ถูกปิด
        }

        // --- Event Handler สำหรับปุ่ม "Product" ---
        // **สำคัญ:** แก้ไขชื่อเมธอดนี้ (เช่น button2_Click) ให้ตรงกับ Event Click ของปุ่ม "Product" ในหน้า Designer ของคุณ
        private void btnProduct_Click(object sender, EventArgs e)
        {
            this.Hide(); // ซ่อน AdminForm
            ProductForm productForm = new ProductForm(); // สร้าง ProductForm
            productForm.ShowDialog(); // แสดง ProductForm และรอจนกว่าจะถูกปิด
            this.Show(); // แสดง AdminForm อีกครั้งเมื่อ ProductForm ถูกปิด
        }

        // --- Event Handler สำหรับปุ่ม "Order" (ยังไม่ได้สร้างฟอร์ม) ---
        private void btnOrder_Click(object sender, EventArgs e) // ตรวจสอบว่าชื่อปุ่มตรงกัน
        {
            this.Hide();
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
            this.Show();
        }

        // --- Event Handler สำหรับปุ่ม "Sales Report" (ยังไม่ได้สร้างฟอร์ม) ---
        private void btnSalesReport_Click(object sender, EventArgs e) // ตรวจสอบว่าชื่อปุ่มตรงกัน
        {
            this.Hide();
            SalesReportForm salesReportForm = new SalesReportForm();
            salesReportForm.ShowDialog();
            this.Show();
        }

        // --- (ถ้ามีปุ่ม Logout หรือ Exit เพิ่มเติม) ---
        // private void btnLogout_Click(object sender, EventArgs e)
        // {
        //     Application.Restart(); // กลับไปหน้า Login
        // }

        // private void btnExit_Click(object sender, EventArgs e)
        // {
        //     Application.Exit(); // ปิดโปรแกรมทั้งหมด
        // }
    }
}
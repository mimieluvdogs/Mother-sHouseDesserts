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
    public partial class AboutUsForm : Form
    {
        public AboutUsForm()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // คำสั่งนี้จะปิดฟอร์ม "AboutUsForm"
            // และทำให้โค้ดใน CategoryForm ที่เรียก .ShowDialog() ทำงานต่อไป
            this.Close();
        }

        private void AboutUsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test68._10._17
{
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Name { get; set; }
        public static string FullAddress { get; set; }
        public static string Phone { get; set; } // <-- [เพิ่ม] บรรทัดนี้
    }
}

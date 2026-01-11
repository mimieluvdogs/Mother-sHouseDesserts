using System;
using System.Collections.Generic;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace test68._10._17
{
    public class ReceiptDocument : IDocument
    {
        #region "Data Properties and Constructor"
        private readonly long _orderId;
        private readonly string _customerName;
        private readonly string _shippingAddress;
        private readonly string _customerPhone;
        private readonly decimal _totalAmount;
        private readonly List<CartItem> _items;
        private readonly byte[] _logoData;

        public ReceiptDocument(long orderId, string customerName, string shippingAddress, string customerPhone, decimal totalAmount, List<CartItem> items, byte[] logoData)
        {
            _orderId = orderId;
            _customerName = customerName;
            _shippingAddress = shippingAddress;
            _customerPhone = customerPhone;
            _totalAmount = totalAmount;
            _items = items;
            _logoData = logoData;
        }
        #endregion

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(style => style.FontSize(11));

                    // 1. ส่วนหัว
                    page.Header().Element(ComposeHeader);

                    // 2. ส่วนเนื้อหา (Content)
                    page.Content().Column(column =>
                    {
                        // ส่วนข้อมูลลูกค้า + ร้านค้า (แก้ไขในเมธอดนี้)
                        column.Item().Element(ComposeCustomerInfo);

                        // ส่วนตารางรายการสินค้า
                        column.Item().Element(ComposeContent);

                        // ส่วนสรุปยอดเงิน
                        column.Item().Element(ComposeTotals);
                    });

                    // 3. ส่วนท้าย
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("ขอบคุณที่ใช้บริการค่ะ");
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("RECEIPT").SemiBold().FontSize(28).FontColor(Colors.Blue.Darken2);
                    column.Item().Text("ใบเสร็จรับเงิน").SemiBold().FontSize(16);
                });

                if (_logoData != null)
                {
                    row.ConstantItem(150).Image(_logoData);
                }
            });
        }

        // ⭐️⭐️⭐️ [ส่วนที่แก้ไข] เพิ่มข้อมูลร้านค้าด้านล่างข้อมูลลูกค้า ⭐️⭐️⭐️
        void ComposeCustomerInfo(IContainer container)
        {
            container.PaddingVertical(20).Column(col =>
            {
                // --- แถวที่ 1: ข้อมูลลูกค้า และ เลขที่ใบเสร็จ ---
                col.Item().Row(row =>
                {
                    // คอลัมน์ซ้าย: ข้อมูลลูกค้า
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("ข้อมูลลูกค้า").SemiBold().FontSize(14);
                        column.Item().Text($"ชื่อ-สกุล: {_customerName}");
                        column.Item().Text($"เบอร์โทรศัพท์: {_customerPhone}");
                        column.Item().Text($"ที่อยู่: {_shippingAddress}");
                    });

                    // คอลัมน์ขวา: ข้อมูลใบเสร็จ
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().AlignRight().Text("เลขที่ใบเสร็จรับเงิน").SemiBold();
                        column.Item().AlignRight().Text($"RE{_orderId:D8}");
                        column.Item().AlignRight().Text("เลขที่อ้างอิง").SemiBold();
                        column.Item().AlignRight().Text("-");
                        column.Item().AlignRight().Text("วันที่ชำระเงิน").SemiBold();
                        column.Item().AlignRight().Text($"{DateTime.Now:dd/MM/yyyy}");
                    });
                });

                // --- [แก้ไขจุดที่บัค] เปลี่ยน .Color เป็น .LineColor ---
                col.Item().PaddingTop(10).PaddingBottom(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                // --- แถวที่ 2: ข้อมูลร้านค้า ---
                col.Item().Row(row =>
                {
                    // ฝั่งซ้าย: ชื่อร้าน และ ที่อยู่
                    row.RelativeItem().Column(shopCol =>
                    {
                        shopCol.Item().Text("ข้อมูลร้านค้า").SemiBold().FontSize(14);
                        shopCol.Item().Text("ร้านค้า ขนมบ้านแม่");
                        shopCol.Item().Text("ที่อยู่ 2433 หอ8หลัง อ.เมือง จ.ขอนแก่น 40002");
                    });

                    // ฝั่งขวา: เบอร์โทร และ อีเมล์
                    row.RelativeItem().Column(shopCol =>
                    {
                        shopCol.Item().PaddingTop(25);
                        shopCol.Item().AlignRight().Text("เบอร์โทร 064-0954757");
                        shopCol.Item().AlignRight().Text("อีเมล์ kruewan.p@kkumail.com");
                    });
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(20).Column(column =>
            {
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);
                        columns.RelativeColumn(4);
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        header.Cell().BorderBottom(1).Padding(5).Text("ลำดับ").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).Text("รายการสินค้า").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("จำนวน").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("ราคา/หน่วย").SemiBold();
                        header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("ราคารวม").SemiBold();
                    });

                    int index = 1;
                    foreach (var item in _items)
                    {
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(index++.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(item.Name);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(item.Quantity.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text($"{item.Price:N2}");
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text($"{(item.Quantity * item.Price):N2}");
                    }
                });
            });
        }

        void ComposeTotals(IContainer container)
        {
            container.AlignRight().PaddingTop(20).Column(column =>
            {
                decimal subTotal = _totalAmount;
                decimal vat = subTotal * 0.07m;
                decimal grandTotal = subTotal + vat;

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("รวมเป็นเงิน");
                    row.ConstantItem(100).AlignRight().Text($"{subTotal:N2}");
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("ภาษีมูลค่าเพิ่ม (7%)");
                    row.ConstantItem(100).AlignRight().Text($"{vat:N2}");
                });
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.RelativeItem().AlignRight().Text("รวมเป็นเงินทั้งสิ้น").SemiBold().FontSize(14);
                    row.ConstantItem(100).AlignRight().Text($"{grandTotal:N2} ฿").SemiBold().FontSize(14);
                });
            });
        }
    }
}
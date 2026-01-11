using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test68._10._17
{
    public static class ShoppingCart
    {
        public static List<CartItem> Items { get; private set; } = new List<CartItem>();

        public static void AddItem(CartItem itemToAdd)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductID == itemToAdd.ProductID);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                itemToAdd.Quantity = 1;
                Items.Add(itemToAdd);
            }
        }

        public static void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductID == productId);
        }

        public static decimal GetTotal()
        {
            return Items.Sum(i => i.Price * i.Quantity);
        }

        public static void ClearCart()
        {
            Items.Clear();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class ShoppingCartService /* ИСХОДНЫЙ КЛАСС, НЕ РЕДАКТИРОВАЛСЯ
        Нарушение KISS в классе ShoppingCartService:
    Откорректированный код реализован в классе ShoppingCartService1
    1.1) Для упрощения Методы CalculateTotalPrice и CalculateTotalPriceWithQuantities можно выполнить одним методом CalculateTotalPrice с перегрузкой
    1.2) Для упрощения baseTotal нужно считать используя метод Sum LINQ как для словаря, так и для списка
    1.3) По моему мнению Switch Case читается легче чем IF ELSE в отдельном методе CalculateDiscount

        Нарушение DRY в классе ShoppingCartService:
    Откорректированный код реализован в классе ShoppingCartService1
    1) Для избежания дублирования кода необходимо создать отдельный метод CalculateDiscount
 */
    {
        public decimal CalculateTotalPrice(string customerType, List<decimal> itemPrices)
        {
            decimal baseTotal = 0;
            for (int i = 0; i < itemPrices.Count; i++)
            {
                baseTotal += itemPrices[i];
            }

            decimal discount = 0;

            if (customerType == "Regular")
            {
                discount = baseTotal * 0.05m; // 5%
            }
            else if (customerType == "Premium")
            {
                discount = baseTotal * 0.15m; // 15%
                if (discount > 1000)
                {
                    discount = 1000 + (discount - 1000) * 0.1m;
                }
            }
            else if (customerType == "VIP")
            {
                discount = baseTotal * 0.20m; // 20%
            }

            decimal finalPrice = baseTotal - discount;

            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }

        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities)
        {
            List<decimal> allPrices = new List<decimal>();
            foreach (var item in itemsWithQuantities)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    allPrices.Add(item.Key);
                }
            }
            return CalculateTotalPrice(customerType, allPrices);
        }
    }
}

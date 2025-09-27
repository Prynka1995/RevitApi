using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class ShoppingCartService2 //Рефакторинг (практическая часть). ОТРЕДАКТИРОВАННЫЙ КОД, чтобы запустить его - надо исключить из проекта ShoppingCartService и ShoppingCartService1
    {
        public decimal CalculateTotalPrice(string customerType, List<decimal> itemPrices)
        {
            //decimal baseTotal = 0;
            //decimal discount = 0;
            decimal baseTotal = itemPrices.Sum(x => x);
            decimal discount = CalculateDiscount(customerType, baseTotal);
            decimal finalPrice = baseTotal - discount;
            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }
        public decimal CalculateTotalPrice(string customerType, Dictionary<decimal, int> itemPrices)
        {
            //decimal baseTotal = 0;
            //decimal discount = 0;
            decimal baseTotal = itemPrices.Sum(x => x.Key * x.Value);
            decimal discount = CalculateDiscount(customerType, baseTotal);
            decimal finalPrice = baseTotal - discount;
            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }
        public decimal CalculateDiscount(string customerType, decimal baseTotal)
        {
            decimal discount = 0;
            switch (customerType)
            {
                case "Regular":
                    discount = baseTotal * 0.05m; // 5%
                    break;
            }
            return discount;
        }

    }
}

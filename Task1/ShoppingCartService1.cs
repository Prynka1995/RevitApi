using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    public class ShoppingCartService1 //Анализ (теоретическая часть). ОТРЕДАКТИРОВАННЫЙ КОД, чтобы запустить его - надо исключить из проекта ShoppingCartService и ShoppingCartService2 
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
                case "Premium":
                    //return 0.15m; // 15%
                    if (baseTotal * 0.15m > 1000)
                    {
                        discount = 1000 + (discount - 1000) * 0.1m; // скидка 15%, но если сумма скидки превышает 1000 условных единиц, то на часть превышения применяется дополнительная скидка 10%.
                    }
                    else discount = baseTotal * 0.15m; // 15%
                    break;
                case "VIP":
                    discount = baseTotal * 0.20m; // 20%
                    break;
            }
            return discount;
        }

    }
}

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShoppingCartService1 basket = new ShoppingCartService1();
            List<decimal> lst = new List<decimal>();
            lst.Add(100m);
            lst.Add(200m);
            lst.Add(300m);
            decimal result = basket.CalculateTotalPrice("Regular", lst);
            Console.ReadKey();

            //ShoppingCartService2 basket = new ShoppingCartService2();
            //Dictionary<decimal, int> dict = new Dictionary<decimal, int>();
            //dict.Add(100m, 1);
            //dict.Add(200m, 2);
            //dict.Add(300m, 3);
            //decimal result = basket.CalculateTotalPrice("Regular",dict);
            //Console.ReadKey();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Task2
{
    internal class Program
    //    Требуется разработать модуль для системы уведомлений.Изначально требуется только отправка email-уведомлений, но в будущем потребуется добавить SMS-рассылку, push-уведомления и т.д.

    //Исходный код (с нарушениями):
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ILogger, FileLogger>();
            services.AddSingleton<EmailSender>();
            services.AddSingleton<SmsSender>();
            services.AddSingleton<NotificationService>();
            ServiceProvider provider = services.BuildServiceProvider();

            var logger = provider.GetRequiredService<ILogger>();

            string choice = GetNotificationType();

            INotificationSender sender = choice switch
            {
                "Email" => provider.GetRequiredService<EmailSender>(),
                "SMS" => provider.GetRequiredService<SmsSender>(),
                _ => throw new ArgumentException($"Неизвестный тип: '{choice}'")
            };


            var service = new NotificationService(sender, logger);
            service.SendNotification("Ваш заказ готов", "user@example.com");
        }
        private static string GetNotificationType()
        {
            //var isValid = false;
            while (true)
            {
                Console.WriteLine("Введите желаемый тип рассылки (1/2):");
                Console.WriteLine("1. Email  2. SMS");
                string? choice = Convert.ToString(Console.ReadLine());
                Console.WriteLine();
                switch (choice)
                {
                    case "1":
                        return "Email";
                    case "2":
                        return "SMS";
                    default:
                        Console.WriteLine("Введите корректный тип рассылки (1. Email  2. SMS). Нажмите ^C для выхода");
                        Console.ReadKey();
                        continue;
                }

            }

        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class NotificationService
    {
        private readonly INotificationSender _sender;
        private readonly ILogger _logger;

        public NotificationService(INotificationSender sender, ILogger logger)
        {
        _sender = sender;
        _logger = logger;
        }

        public void SendNotification(string message, string recipient)
        {
            // Логика подготовки уведомления
            string formattedMessage = $"Уведомление: {message}";

            // Отправка email
            _sender.Send(recipient, formattedMessage);
            var _date = DateTime.Now;
            _logger.Log($"Отправлено уведомление для {recipient}");
        }
    }
}

//public class EmailSender
//{
//    public void SendEmail(string to, string message)
//    {
//        // Симуляция отправки email1
//    }
//}

//    class Program
//    {
//        static void Main()
//        {
//            var service = new NotificationService();
//            service.SendNotification("Ваш заказ готов", "user@example.com");
//        }
//    }
//}

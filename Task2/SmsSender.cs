using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    internal class SmsSender : INotificationSender
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"SMS для {to}: {message}");
        }
    }
}

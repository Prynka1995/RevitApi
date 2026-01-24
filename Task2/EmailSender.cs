using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class EmailSender:INotificationSender
    {
        public void Send(string to, string message)
        {
            Console.WriteLine($"Email для {to}: {message}");
        }
    }
}

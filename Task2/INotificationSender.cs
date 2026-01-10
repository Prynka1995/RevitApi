using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    //        Создайте интерфейс INotificationSender
    public interface INotificationSender
    {
    void Send(string recipient, string message);
    }
}

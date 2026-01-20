using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    //        Вынесите логирование в отдельный сервис
    public class FileLogger:ILogger
    {
        public const string FileName = "log.txt";
        //string timestamp = DateTime.Now.ToString();
        public void Log(string message)
        { 
        File.AppendAllText(FileName, message);
        }
    }
}

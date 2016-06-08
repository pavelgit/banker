using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Loggers {
    public class ConsoleLogger : ILogger {
        public void Log(string message, params object[] arg) {
            Console.WriteLine(message, arg);
        }
    }
}

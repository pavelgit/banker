using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Loggers {
    public interface ILogger {
        void Log(string message, params object[] arg);
    }
}

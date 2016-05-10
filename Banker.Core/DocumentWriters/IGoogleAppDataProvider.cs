using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentWriters {
    public interface IGoogleAppDataProvider {
        string GetClientId();
        string GetClientSecret();
    }
}

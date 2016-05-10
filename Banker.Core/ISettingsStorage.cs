using System;

namespace Banker.Core {
    public interface ISettingsStorage {
        void Set(string key, string value);
        string Get(string key);
        bool TryGet(string key, out string value);
    }
}
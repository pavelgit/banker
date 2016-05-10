using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Banker.Core {
    public class ApplicationSettingsStorage : ISettingsStorage {

        private ApplicationSettingsBase applicationSettingsBase;
        public ApplicationSettingsStorage(ApplicationSettingsBase applicationSettingsBase) {
            this.applicationSettingsBase = applicationSettingsBase;
        }

        public string Get(string key) {
            return applicationSettingsBase[key] as string;
        }

        public void Set(string key, string value) {
            applicationSettingsBase[key] = value;
            applicationSettingsBase.Save();
        }

        public bool TryGet(string key, out string value) {
            try {
                value = applicationSettingsBase[key] as string;
                return true;
            } catch (SettingsPropertyNotFoundException e) {
                value = null;
                return false;
            }
        }
    }

}

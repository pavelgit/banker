using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Yaml.Serialization;
using YamlDotNet.Dynamic;
using YamlDotNet.RepresentationModel;

namespace Banker.Core.Tags {
    public class YamlListTagService : ListTagService {
        public void InitFromYaml(string yaml) {
            var yamlSerializer = new YamlSerializer();
            var yamlData = yamlSerializer.Deserialize(yaml);
            try {
                FillTagServices(yamlData);
            }catch(Exception e) {
                throw new Exception("Yaml deserialization exception", e);
            }
        }

        void FillTagServices(object[] yamlData) {
            TagServices = (yamlData[0] as Dictionary<object, object>).Select(kv =>
                new DefaultMatchTagRule(
                    kv.Key as string,
                    (kv.Value as object[]).Cast<string>().ToArray()
                )
            ).ToArray();
        }

    }
}

using Banker.Core.Loggers;
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

        public YamlListTagService(ILogger logger) : base(logger) {

        }

        public void InitFromYaml(string yaml) {
            logger.Log("Start reading yaml tag file");
            var yamlSerializer = new YamlSerializer();
            object[] yamlData;
            try {
                yamlData = yamlSerializer.Deserialize(yaml);
            }catch(Exception e) {
                logger.Log($"Error while deserializing yaml tag file {e.Message}");
                throw new Exception("Yaml deserialization exception", e);
            }
            try {
                FillTagServices(yamlData);
            }catch(Exception e) {
                logger.Log($"Error while creating tag services from deserialized yaml file {e.Message}");
                throw new Exception("Yaml deserialization exception", e);
            }
        }

        void FillTagServices(object[] yamlData) {
            TagServices = (yamlData[0] as Dictionary<object, object>).Select(kv =>
                new DefaultMatchTagRule(
                    kv.Key as string,
                    (kv.Value as object[]).Cast<string>().ToArray(),
                    logger
                )
            ).ToArray();
        }

    }
}

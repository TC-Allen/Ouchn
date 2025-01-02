namespace Ouchn.Util
{
    public class YAML_RW
    {
        public static string YAMLDataPath = "settins.yaml";
        public static YAMLData yaml_data = new YAMLData();

        /// <summary>
        /// 保存YAML_Data数据
        /// </summary>
        public static void SaveYAML_Data()
        {
            YamlDotNet.Serialization.Serializer serializer = new YamlDotNet.Serialization.Serializer();
            StringWriter strWriter = new StringWriter();

            serializer.Serialize(strWriter, yaml_data);
            serializer.Serialize(Console.Out, yaml_data);

            using (TextWriter writer = File.CreateText(YAMLDataPath))
            {
                writer.Write(strWriter.ToString());
            }

        }
        /// <summary>
        /// 读取YAML_Data数据
        /// </summary>
        public static void ReadYAML_Data()
        {
            if (!File.Exists(YAMLDataPath))
            {
                throw new FileNotFoundException();
            }
            StreamReader yamlReader = File.OpenText(YAMLDataPath);
            YamlDotNet.Serialization.Deserializer yamlDeserializer = new YamlDotNet.Serialization.Deserializer();

            yaml_data = yamlDeserializer.Deserialize<YAMLData>(yamlReader);
            yamlReader.Close();
        }
    }
}

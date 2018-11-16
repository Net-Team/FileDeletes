using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FileDeletes
{
    /// <summary>
    /// 表示配置信息
    /// </summary>
    public class Config
    {
        /// <summary>
        /// 表示要删除的文件配置集合
        /// </summary>
        public Option[] Options;

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns></returns>
        public static Config Load()
        {
            var file = Path.ChangeExtension(typeof(Config).Assembly.Location, ".json");
            if (File.Exists(file) == false)
            {
                throw new FileNotFoundException("找不到配置文件", file);
            }

            var json = File.ReadAllText(file, Encoding.Default);
            return JsonConvert.DeserializeObject<Config>(json);
        }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.data
{
    public class AsteroidRoot
    {
        [JsonProperty("data")]
        public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>();
    }

    // 真正的Asteroid数据类
    public class AsteroidData
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Health { get; set; }
        public string Loot { get; set; } = string.Empty;
        public int Distance { get; set; }
        public int Probability { get; set; }
        
        // 添加一个默认的纹理名称属性，以便在其他地方使用
        public string Texture { get; set; } = "default";
    }

    // 工具类用于处理Asteroid数据
    public static class AsteroidDataHelper
    {
        public static List<AsteroidData> GetActualAsteroidData(AsteroidRoot root)
        {
            var result = new List<AsteroidData>();
            
            foreach (var item in root.Data)
            {
                // 检查是否为表头（通过检查ID字段是否为字符串"ID"）
                var idValue = GetObjectValue(item, "编号ID");
                if (idValue?.ToString() == "ID")
                {
                    continue; // 跳过表头
                }
                
                // 尝试从数据中提取值
                var asteroid = new AsteroidData
                {
                    Id = ConvertToInt(GetObjectValue(item, "编号ID")),
                    Name = ConvertToString(GetObjectValue(item, "名称")),
                    Health = ConvertToInt(GetObjectValue(item, "血量")),
                    Loot = ConvertToString(GetObjectValue(item, "掉落物")),
                    Distance = ConvertToInt(GetObjectValue(item, "刷新距离")),
                    Probability = ConvertToInt(GetObjectValue(item, "刷新概率系数")),
                    Texture = GenerateTextureName(ConvertToString(GetObjectValue(item, "名称"))) // 生成纹理名称
                };
                
                // 只添加有效的数据项（ID > 0）
                if (asteroid.Id > 0)
                {
                    result.Add(asteroid);
                }
            }
            
            return result;
        }

        private static string GenerateTextureName(string name)
        {
            // 根据名称生成纹理名称，例如"小行星1" -> "小行星1"
            // 或者可以使用更复杂的逻辑来确定纹理
            return name; 
        }

        private static object GetObjectValue(Dictionary<string, object> dict, string key)
        {
            return dict.ContainsKey(key) ? dict[key] : null;
        }

        private static int ConvertToInt(object value)
        {
            if (value == null) return 0;
            
            if (value is int intValue)
            {
                return intValue;
            }
            else if (value is double doubleValue)
            {
                return (int)doubleValue;
            }
            else if (value is long longValue)
            {
                return (int)longValue;
            }
            else if (value is string stringValue)
            {
                if (int.TryParse(stringValue, out int result))
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private static string ConvertToString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            return value.ToString();
        }
    }
}
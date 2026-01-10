using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CosmicMiningCompany.scripts.serializer;

/// <summary>
/// 定义一个通用的序列化器接口，用于将对象序列化为JSON字符串以及从JSON字符串反序列化对象
/// </summary>
/// <typeparam name="T">要序列化或反序列化的对象类型，必须具有无参数构造函数</typeparam>
public interface ISerializer<T> where T : new()
{
    /// <summary>
    /// 将指定的对象序列化为JSON字符串
    /// </summary>
    /// <param name="data">要序列化的对象实例</param>
    /// <param name="formatting">序列化格式选项，默认为缩进格式</param>
    /// <returns>JSON字符串表示</returns>
    public string Serialize(T data, Formatting formatting = Formatting.Indented) =>
        JsonConvert.SerializeObject(data, formatting);

    /// <summary>
    /// 将指定的对象序列化为JSON字符串，使用自定义的序列化设置
    /// </summary>
    /// <param name="data">要序列化的对象实例</param>
    /// <param name="settings">自定义的JSON序列化设置</param>
    /// <returns>JSON字符串表示</returns>
    public string Serialize(T data, JsonSerializerSettings settings) =>
        JsonConvert.SerializeObject(data, settings);

    /// <summary>
    /// 将JSON字符串反序列化为指定类型的对象
    /// </summary>
    /// <param name="json">要反序列化的JSON字符串</param>
    /// <returns>反序列化后的对象实例，如果反序列化失败则返回新创建的默认实例</returns>
    public T Deserialize(string json) =>
        JsonConvert.DeserializeObject<T>(json)
        ?? new T();

    /// <summary>
    /// 将JSON字符串反序列化为指定类型的对象，使用自定义的序列化设置
    /// </summary>
    /// <param name="json">要反序列化的JSON字符串</param>
    /// <param name="settings">自定义的JSON序列化设置</param>
    /// <returns>反序列化后的对象实例，如果反序列化失败则返回新创建的默认实例</returns>
    public T Deserialize(string json, JsonSerializerSettings settings) =>
        JsonConvert.DeserializeObject<T>(json, settings)
        ?? new T();

    /// <summary>
    /// 尝试将JSON字符串反序列化为指定类型的对象
    /// </summary>
    /// <param name="json">要反序列化的JSON字符串</param>
    /// <param name="result">反序列化后的对象实例，如果反序列化失败则为默认值</param>
    /// <returns>如果反序列化成功则返回true，否则返回false</returns>
    public bool TryDeserialize(string json, out T result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(json)
                ?? new T();
            return true;
        }
        catch
        {
            result = new T();
            return false;
        }
    }

    /// <summary>
    /// 尝试将JSON字符串反序列化为指定类型的对象，使用自定义的序列化设置
    /// </summary>
    /// <param name="json">要反序列化的JSON字符串</param>
    /// <param name="settings">自定义的JSON序列化设置</param>
    /// <param name="result">反序列化后的对象实例，如果反序列化失败则为默认值</param>
    /// <returns>如果反序列化成功则返回true，否则返回false</returns>
    public bool TryDeserialize(string json, JsonSerializerSettings settings, out T result)
    {
        try
        {
            result = JsonConvert.DeserializeObject<T>(json, settings)
                ?? new T();
            return true;
        }
        catch
        {
            result = new T();
            return false;
        }
    }
}

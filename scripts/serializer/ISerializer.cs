using Newtonsoft.Json;

namespace CosmicMiningCompany.scripts.serializer;

/// <summary>
/// 定义一个通用的序列化器接口，用于将对象序列化为JSON字符串以及从JSON字符串反序列化对象
/// </summary>
/// <typeparam name="T">要序列化或反序列化的对象类型，必须具有无参数构造函数</typeparam>
public interface ISerializer<T> where T : new()
{
    /// <summary>
    /// 将指定的对象序列化为格式化的JSON字符串
    /// </summary>
    /// <param name="data">要序列化的对象实例</param>
    /// <returns>格式化的JSON字符串表示</returns>
    public string Serialize(T data) =>
        JsonConvert.SerializeObject(data, Formatting.Indented);

    /// <summary>
    /// 将JSON字符串反序列化为指定类型的对象
    /// </summary>
    /// <param name="json">要反序列化的JSON字符串</param>
    /// <returns>反序列化后的对象实例，如果反序列化失败则返回新创建的默认实例</returns>
    public T Deserialize(string json) =>
        JsonConvert.DeserializeObject<T>(json)
        ?? new T();
}

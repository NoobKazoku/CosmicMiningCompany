using CosmicMiningCompany.scripts.serializer;

namespace CosmicMiningCompany.scripts.asteroid;

/// <summary>
/// 小行星数据序列化器，负责将小行星数据对象序列化为可存储或传输的格式，以及反序列化操作
/// </summary>
/// <remarks>
/// 该类实现了ISerializer接口，提供针对AsteroidData类型数据的序列化功能
/// </remarks>
public class AsteroidDataSerializer: ISerializer<AsteroidData>;

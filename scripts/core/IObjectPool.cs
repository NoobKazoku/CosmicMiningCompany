namespace CosmicMiningCompany.scripts.core;

/// <summary>
/// 对象池接口，用于管理对象的创建、回收和复用
/// </summary>
/// <typeparam name="T">池中存储的对象类型，必须是引用类型</typeparam>
/// <typeparam name="TP">父级对象类型，用于创建对象时传入</typeparam>
public interface IObjectPool<T, in TP> where T : class
{
    /// <summary>
    /// 获取一个可用对象
    /// </summary>
    /// <param name="parent">创建对象时需要的父级对象</param>
    /// <returns>从池中获取的对象实例</returns>
    T Acquire(TP parent);
    
    /// <summary>
    /// 将对象释放回池中
    /// </summary>
    /// <param name="node">需要释放的对象</param>
    void Release(T node);
    
    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="initialCapacity">初始容量大小</param>
    void Initialize(int initialCapacity);
    
    /// <summary>
    /// 清空对象池中的所有对象
    /// </summary>
    void Clear();
    
    /// <summary>
    /// 获取当前池中对象的数量
    /// </summary>
    int Count { get; }
    
    /// <summary>
    /// 获取对象池的容量
    /// </summary>
    int Capacity { get; }
    
    /// <summary>
    /// 尝试获取一个可用对象
    /// </summary>
    /// <param name="item">输出参数，成功时返回获取到的对象，失败时为null</param>
    /// <returns>如果成功获取到对象则返回true，否则返回false</returns>
    bool TryAcquire(out T item);
    
    /// <summary>
    /// 尝试获取一个可用对象
    /// </summary>
    /// <param name="parent">父级对象参数</param>
    /// <param name="item">输出参数，成功时返回获取到的对象，失败时为null</param>
    /// <returns>如果成功获取到对象则返回true，否则返回false</returns>
    bool TryAcquire(TP parent, out T item);

    /// <summary>
    /// 预加载指定数量的对象到池中
    /// </summary>
    /// <param name="count">需要预加载的对象数量</param>
    void Preload(int count);
}

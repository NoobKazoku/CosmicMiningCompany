namespace CosmicMiningCompany.scripts.core;

/// <summary>
/// 可池化节点接口，定义了对象池中对象的生命周期管理方法
/// </summary>
public interface IPoolableNode
{
    /// <summary>
    /// 当对象从池中被获取时调用的回调方法
    /// </summary>
    void OnAcquire();

    /// <summary>
    /// 当对象被释放回池中时调用的回调方法
    /// </summary>
    void OnRelease();

    /// <summary>
    /// 当对象池被销毁时调用的回调方法
    /// </summary>
    void OnPoolDestroy();
    
    /// <summary>
    /// 请求回收对象到池中的方法
    /// </summary>
    void RequestRecycle();
}

using Godot;

namespace CosmicMiningCompany.scripts.core;

/// <summary>
/// 节点对象池接口，用于管理Node类型对象的创建、回收和复用
/// </summary>
/// <typeparam name="T">继承自Node的具体节点类型</typeparam>
public interface INodePool<T> : IObjectPool<T, Node>
    where T : Node;

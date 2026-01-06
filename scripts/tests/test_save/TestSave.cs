using CosmicMiningCompany.scripts.data;
using GFramework.Core.Abstractions.controller;
using GFramework.Core.extensions;
using GFramework.SourceGenerators.Abstractions.logging;
using GFramework.SourceGenerators.Abstractions.rule;
using Godot;

namespace CosmicMiningCompany.scripts.tests.test_save;

[ContextAware]
[Log]
public partial class TestSave : Node, IController
{
    [Export] public int AddLevel { get; set; } = 10;
    private Button Save => GetNode<Button>("%Save");
    private Button Load => GetNode<Button>("%Load");
    private Button SetLevel => GetNode<Button>("%SetLevel");
    private Button Debug => GetNode<Button>("%Debug");
    private ISaveStorageUtility _saveStorageUtility = null!;

    /// <summary>
    /// 节点准备就绪时的回调方法
    /// 在节点添加到场景树后调用
    /// </summary>
    public override void _Ready()
    {
        _saveStorageUtility = ContextAwareExtensions.GetUtility<ISaveStorageUtility>(this)!;
        Save.Pressed += () =>
        {
            _log.Debug("开始保存");
            _saveStorageUtility.Save();
        };
        Load.Pressed += () =>
        {
            _log.Debug("开始加载");
            _saveStorageUtility.Load();
        };
        SetLevel.Pressed += () =>
        {
            _log.Debug("开始设置等级");
            var level = _saveStorageUtility.GetSkillLevel("Test");
            _log.Debug($"当前等级为：{level}");
            _saveStorageUtility.SetSkillLevel("Test", level + AddLevel);
            level = _saveStorageUtility.GetSkillLevel("Test");
            _log.Debug($"增加等级为：{level}");
            _saveStorageUtility.PrintSaveSummary();
        };
        Debug.Pressed += () =>
        {
            _log.Debug("开始打印保存数据");
            _saveStorageUtility.PrintSaveSummary();
        };
    }

    private void Binding()
    {
        Save.Pressed += () =>
        {
            _log.Debug("开始保存");
            _saveStorageUtility.Save();
        };
        Load.Pressed += () =>
        {
            _log.Debug("开始加载");
            _saveStorageUtility.Load();
        };
        SetLevel.Pressed += () =>
        {
            _log.Debug("开始设置等级");
            var level = _saveStorageUtility.GetSkillLevel("Test");
            _log.Debug($"当前等级为：{level}");
            _saveStorageUtility.SetSkillLevel("Test", level + AddLevel);
            level = _saveStorageUtility.GetSkillLevel("Test");
            _log.Debug($"增加等级为：{level}");
            _saveStorageUtility.PrintSaveSummary();
        };
        Debug.Pressed += () =>
        {
            _log.Debug("开始打印保存数据");
            _saveStorageUtility.PrintSaveSummary();
        };
    }
}
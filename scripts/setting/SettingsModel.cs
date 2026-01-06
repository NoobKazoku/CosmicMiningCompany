
using CosmicMiningCompany.scripts.setting.interfaces;
using GFramework.Core.model;

namespace CosmicMiningCompany.scripts.setting;

public class SettingsModel: AbstractModel,ISettingsModel
{
    public GraphicsSettings Graphics { get; init; } = new();
    public AudioSettings Audio { get; init; } = new();
    
    protected override void OnInit()
    {
        
    }
}
using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;

namespace AncientDroneForAll;

public class Options : OptionInterface
{
    private readonly ManualLogSource Logger;

    public Options(AncientDroneForAll modInstance, ManualLogSource loggerSource)
    {
        Logger = loggerSource;
        IsActive = this.config.Bind<bool>("IsActive", true);
    }

    public readonly Configurable<bool> IsActive;
    private UIelement[] UIArrIsActive;
    
    
    public override void Initialize()
    {
        var opTab = new OpTab(this, "Options");
        this.Tabs = new[]
        {
            opTab
        };

        UIArrIsActive = new UIelement[]
        {
            new OpLabel(10f, 550f, "Options", true),
            new OpLabel(10f, 520f, "Is mod active?"),
            new OpCheckBox(IsActive, 10f, 490f),
            
            new OpLabel(1f, 460f, "Fix for Broken Anti Gravity for Rivulet's Expedition is now active.", false){ color = new Color(0f, 0f, 1f) },
            new OpLabel(1f, 460f, "Patch is not active currently.", false){ color = new Color(60f, 65f, 81f)}
        };
        opTab.AddItems(UIArrIsActive);
    }

    public override void Update()
    {
        if (((OpCheckBox)UIArrIsActive[2]).GetValueBool() == true)
        {
            ((OpLabel)UIArrIsActive[3]).Show();
            ((OpLabel)UIArrIsActive[4]).Hide();
        }
        else
        {
            ((OpLabel)UIArrIsActive[3]).Hide();
            ((OpLabel)UIArrIsActive[4]).Show();
        }
    }

}
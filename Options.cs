using BepInEx.Logging;
using Menu.Remix.MixedUI;
using Menu.Remix.MixedUI.ValueTypes;
using UnityEngine;

namespace AncientDroneForAll;

public class PluginOptions : OptionInterface
{
    private readonly ManualLogSource Logger;

    public PluginOptions(AncientDroneForAll modInstance, ManualLogSource loggerSource)
    {
        Logger = loggerSource;
        PebblesIntro = this.config.Bind<bool>("PebblesIntro", true);
        PacifyPebbles = this.config.Bind<bool>("PacifyPebbles", true);
    }

    public readonly Configurable<bool> PebblesIntro;
    public readonly Configurable<bool> PacifyPebbles;
    private UIelement[] UIArrGeneral;
    
    
    public override void Initialize()
    {
        base.Initialize();
        var opTab = new OpTab(this, "Options");
        this.Tabs = new[]
        {
            opTab
        };

        UIArrGeneral = new UIelement[]
        {
            new OpLabel(10f, 550f, "Options", true),
            new OpLabel(10f, 520f, "Change Five Pebbles' intro to accomidate drone presense"),
            new OpCheckBox(PebblesIntro, 10f, 490f),

            new OpLabel(10f, 480f, "Pacify Five Pebbles while in posession of the drone"),
            new OpCheckBox(PacifyPebbles, 10f, 470f),
        };
        opTab.AddItems(UIArrGeneral);
    }

    public override void Update()
    {

    }

}
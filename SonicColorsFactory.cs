using System;
using System.Reflection;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using LiveSplit.SonicColors;

[assembly: ComponentFactory(typeof(SonicColorsFactory))]

namespace LiveSplit.SonicColors
{
    public class SonicColorsFactory : IComponentFactory
    {
        public string ComponentName => "Sonic Colors: Ultimate - Autosplitter";
        public string Description => "Automatic splitting and IGT calculation for Sonic Colors Ultimate";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/SonicSpeedrunning/LiveSplit.SonicColors/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.SonicColors.xml";
        public IComponent Create(LiveSplitState state) { return new Component(state); }
    }
}

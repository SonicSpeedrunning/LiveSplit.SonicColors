using System.Xml;
using System.Windows.Forms;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;

namespace LiveSplit.SonicColors
{
    partial class SonicColorsComponent : LogicComponent
    {
        public override string ComponentName => "Sonic Colors - Autosplitter";
        private readonly Settings settings = new();
        private readonly Watchers watchers;
        private readonly TimerModel timer;

        public SonicColorsComponent(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            timer.OnStart += OnStart;
            watchers = new Watchers(state);

            if (timer.CurrentState.CurrentTimingMethod == TimingMethod.RealTime)
            {
                var question = MessageBox.Show("""
                This game uses in-game time (IGT) as the main timing method.
                LiveSplit is currently set to show Real Time (RTA).
                Would you like to set the timing method to Game Time?
                """, "LiveSplit - Sonic Colors", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (question == DialogResult.Yes)
                    timer.CurrentState.CurrentTimingMethod = TimingMethod.GameTime;
            }
        }

        public override void Dispose()
        {
            timer.OnStart -= OnStart;
            settings.Dispose();
            watchers.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) => settings.GetSettings(document);

        public override Control GetSettingsControl(LayoutMode mode) => settings;

        public override void SetSettings(XmlNode settings) => this.settings.SetSettings(settings);

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            // If LiveSplit is not connected to the game, of course there's no point in going further
            if (!watchers.Init()) return;

            // Main update logic is inside the watcher class in order to avoid exposing unneded stuff to the outside
            if (watchers.Version == GameVersion.Emulator && !watchers.emu_help.Update())
                return;

            watchers.Update();

            if (timer.CurrentState.CurrentPhase == TimerPhase.Running || timer.CurrentState.CurrentPhase == TimerPhase.Paused)
            {
                timer.CurrentState.IsGameTimePaused = IsLoading();
                if (GameTime() != null) timer.CurrentState.SetGameTime(GameTime());
                if (Reset()) timer.Reset();
                else if (Split()) timer.Split();
            }

            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning)
            {
                if (Start()) timer.Start();
            }
        }

        private void OnStart(object sender, EventArgs e)
        {
            timer.CurrentState.IsGameTimePaused = true;
            watchers.ResetVars();
        }
    }
}

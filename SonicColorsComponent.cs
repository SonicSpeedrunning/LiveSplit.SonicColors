using System;
using System.Xml;
using System.Windows.Forms;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.SonicColors
{
    class Component : LogicComponent
    {
        public override string ComponentName => "Sonic Colors: Ultimate - Autosplitter";
        private Settings settings { get; set; }
        private readonly TimerModel timer;
        private readonly System.Timers.Timer update_timer;
        private readonly SplittingLogic SplittingLogic;

        public Component(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            settings = new Settings();
            settings.OnSplitsPopulate += OnSplitsPopulate;

            SplittingLogic = new SplittingLogic();
            SplittingLogic.OnTimerCheck += OnTimerCheck;
            SplittingLogic.OnStartTrigger += OnStartTrigger;
            SplittingLogic.OnGameTimeTrigger += OnGameTimeTrigger;
            SplittingLogic.OnResetTrigger += OnResetTrigger;
            SplittingLogic.OnSplitTrigger += OnSplitTrigger;

            update_timer = new System.Timers.Timer() { Interval = 15, Enabled = true, AutoReset = false };
            update_timer.Elapsed += delegate { SplittingLogic.Update(); update_timer.Start(); };
        }

        private void OnStartTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase == TimerPhase.NotRunning && settings.runStart) timer.Start();
        }

        private void OnGameTimeTrigger(object sender, double value)
        {
            timer.CurrentState.IsGameTimePaused = settings.useIGT;
            if (timer.CurrentState.CurrentPhase == TimerPhase.Running && settings.useIGT) timer.CurrentState.SetGameTime(TimeSpan.FromSeconds(value));
        }

        private bool OnTimerCheck(object sender, EventArgs e) // Returns true if the timer is NOT running
        {
            return timer.CurrentState.CurrentPhase == TimerPhase.NotRunning;
        }

        private void OnSplitTrigger(object sender, string value)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            switch (value)
            {
                case Levels.TropicalResortAct1: if (settings.tropicalResortAct1) timer.Split(); break;
                case Levels.TropicalResortAct2: if (settings.tropicalResortAct2) timer.Split(); break;
                case Levels.TropicalResortAct3: if (settings.tropicalResortAct3) timer.Split(); break;
                case Levels.TropicalResortAct4: if (settings.tropicalResortAct4) timer.Split(); break;
                case Levels.TropicalResortAct5: if (settings.tropicalResortAct5) timer.Split(); break;
                case Levels.TropicalResortAct6: if (settings.tropicalResortAct6) timer.Split(); break;
                case Levels.TropicalResortBoss: if (settings.tropicalResortBoss) timer.Split(); break;
                case Levels.SweetMountainAct1: if (settings.sweetMountainAct1) timer.Split(); break;
                case Levels.SweetMountainAct2: if (settings.sweetMountainAct2) timer.Split(); break;
                case Levels.SweetMountainAct3: if (settings.sweetMountainAct3) timer.Split(); break;
                case Levels.SweetMountainAct4: if (settings.sweetMountainAct4) timer.Split(); break;
                case Levels.SweetMountainAct5: if (settings.sweetMountainAct5) timer.Split(); break;
                case Levels.SweetMountainAct6: if (settings.sweetMountainAct6) timer.Split(); break;
                case Levels.SweetMountainBoss: if (settings.sweetMountainBoss) timer.Split(); break;
                case Levels.StarlightCarnivalAct1: if (settings.starlightCarnivalAct1) timer.Split(); break;
                case Levels.StarlightCarnivalAct2: if (settings.starlightCarnivalAct2) timer.Split(); break;
                case Levels.StarlightCarnivalAct3: if (settings.starlightCarnivalAct3) timer.Split(); break;
                case Levels.StarlightCarnivalAct4: if (settings.starlightCarnivalAct4) timer.Split(); break;
                case Levels.StarlightCarnivalAct5: if (settings.starlightCarnivalAct5) timer.Split(); break;
                case Levels.StarlightCarnivalAct6: if (settings.starlightCarnivalAct6) timer.Split(); break;
                case Levels.StarlightCarnivalBoss: if (settings.starlightCarnivalBoss) timer.Split(); break;
                case Levels.PlanetWispAct1: if (settings.planetWispAct1) timer.Split(); break;
                case Levels.PlanetWispAct2: if (settings.planetWispAct2) timer.Split(); break;
                case Levels.PlanetWispAct3: if (settings.planetWispAct3) timer.Split(); break;
                case Levels.PlanetWispAct4: if (settings.planetWispAct4) timer.Split(); break;
                case Levels.PlanetWispAct5: if (settings.planetWispAct5) timer.Split(); break;
                case Levels.PlanetWispAct6: if (settings.planetWispAct6) timer.Split(); break;
                case Levels.PlanetWispBoss: if (settings.planetWispBoss) timer.Split(); break;
                case Levels.AquariumParkAct1: if (settings.aquariumParkAct1) timer.Split(); break;
                case Levels.AquariumParkAct2: if (settings.aquariumParkAct2) timer.Split(); break;
                case Levels.AquariumParkAct3: if (settings.aquariumParkAct3) timer.Split(); break;
                case Levels.AquariumParkAct4: if (settings.aquariumParkAct4) timer.Split(); break;
                case Levels.AquariumParkAct5: if (settings.aquariumParkAct5) timer.Split(); break;
                case Levels.AquariumParkAct6: if (settings.aquariumParkAct6) timer.Split(); break;
                case Levels.AquariumParkBoss: if (settings.aquariumParkBoss) timer.Split(); break;
                case Levels.AsteroidCoasterAct1: if (settings.asteroidCoasterAct1) timer.Split(); break;
                case Levels.AsteroidCoasterAct2: if (settings.asteroidCoasterAct2) timer.Split(); break;
                case Levels.AsteroidCoasterAct3: if (settings.asteroidCoasterAct3) timer.Split(); break;
                case Levels.AsteroidCoasterAct4: if (settings.asteroidCoasterAct4) timer.Split(); break;
                case Levels.AsteroidCoasterAct5: if (settings.asteroidCoasterAct5) timer.Split(); break;
                case Levels.AsteroidCoasterAct6: if (settings.asteroidCoasterAct6) timer.Split(); break;
                case Levels.AsteroidCoasterBoss: if (settings.asteroidCoasterBoss) timer.Split(); break;
                case Levels.TerminalVelocityAct1: if (settings.terminalVelocityAct1) timer.Split(); break;
                case Levels.TerminalVelocityAct2: if (settings.terminalVelocityAct2) timer.Split(); break;
                case Levels.TerminalVelocityBoss: if (settings.terminalVelocityBoss) timer.Split(); break;
                case Levels.SonicSimulator1_1: if (settings.sonicSim1_1) timer.Split(); break;
                case Levels.SonicSimulator1_2: if (settings.sonicSim1_2) timer.Split(); break;
                case Levels.SonicSimulator1_3: if (settings.sonicSim1_3) timer.Split(); break;
                case Levels.SonicSimulator2_1: if (settings.sonicSim2_1) timer.Split(); break;
                case Levels.SonicSimulator2_2: if (settings.sonicSim2_2) timer.Split(); break;
                case Levels.SonicSimulator2_3: if (settings.sonicSim2_3) timer.Split(); break;
                case Levels.SonicSimulator3_1: if (settings.sonicSim3_1) timer.Split(); break;
                case Levels.SonicSimulator3_2: if (settings.sonicSim3_2) timer.Split(); break;
                case Levels.SonicSimulator3_3: if (settings.sonicSim3_3) timer.Split(); break;
                case Levels.SonicSimulator4_1: if (settings.sonicSim4_1) timer.Split(); break;
                case Levels.SonicSimulator4_2: if (settings.sonicSim4_2) timer.Split(); break;
                case Levels.SonicSimulator4_3: if (settings.sonicSim4_3) timer.Split(); break;
                case Levels.SonicSimulator5_1: if (settings.sonicSim5_1) timer.Split(); break;
                case Levels.SonicSimulator5_2: if (settings.sonicSim5_2) timer.Split(); break;
                case Levels.SonicSimulator5_3: if (settings.sonicSim5_3) timer.Split(); break;
                case Levels.SonicSimulator6_1: if (settings.sonicSim6_1) timer.Split(); break;
                case Levels.SonicSimulator6_2: if (settings.sonicSim6_2) timer.Split(); break;
                case Levels.SonicSimulator6_3: if (settings.sonicSim6_3) timer.Split(); break;
                case Levels.SonicSimulator7_1: if (settings.sonicSim7_1) timer.Split(); break;
                case Levels.SonicSimulator7_2: if (settings.sonicSim7_2) timer.Split(); break;
                case Levels.SonicSimulator7_3: if (settings.sonicSim7_3) timer.Split(); break;
            }
        }

        private void OnResetTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase == TimerPhase.Running && settings.runReset) timer.Reset();
        }

        private void OnSplitsPopulate(object sender, EventArgs e)
        {
            var question = MessageBox.Show("This will set up your splits according to your selected autosplitting options.\n" +
                "WARNING: Any existing PB recorded for the current layout will be deleted.\n\n" +
                "Do you want to continue?", "Livesplit - Sonic Colors: Ultimate", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (question == DialogResult.No) return;
            timer.CurrentState.Run.Clear();
            if (settings.tropicalResortAct1) timer.CurrentState.Run.AddSegment("Tropical Resort 1");
            if (settings.tropicalResortAct2) timer.CurrentState.Run.AddSegment("Tropical Resort 2");
            if (settings.tropicalResortAct3) timer.CurrentState.Run.AddSegment("Tropical Resort 3");
            if (settings.tropicalResortAct4) timer.CurrentState.Run.AddSegment("Tropical Resort 4");
            if (settings.tropicalResortAct5) timer.CurrentState.Run.AddSegment("Tropical Resort 5");
            if (settings.tropicalResortAct6) timer.CurrentState.Run.AddSegment("Tropical Resort 6");
            if (settings.tropicalResortBoss) timer.CurrentState.Run.AddSegment("Tropical Resort Boss");
            if (settings.sweetMountainAct1) timer.CurrentState.Run.AddSegment("Sweet Mountain 1");
            if (settings.sweetMountainAct2) timer.CurrentState.Run.AddSegment("Sweet Mountain 2");
            if (settings.sweetMountainAct3) timer.CurrentState.Run.AddSegment("Sweet Mountain 3");
            if (settings.sweetMountainAct4) timer.CurrentState.Run.AddSegment("Sweet Mountain 4");
            if (settings.sweetMountainAct5) timer.CurrentState.Run.AddSegment("Sweet Mountain 5");
            if (settings.sweetMountainAct6) timer.CurrentState.Run.AddSegment("Sweet Mountain 6");
            if (settings.sweetMountainBoss) timer.CurrentState.Run.AddSegment("Sweet Mountain Boss");
            if (settings.starlightCarnivalAct1) timer.CurrentState.Run.AddSegment("Starlight Carnival 1");
            if (settings.starlightCarnivalAct2) timer.CurrentState.Run.AddSegment("Starlight Carnival 2");
            if (settings.starlightCarnivalAct3) timer.CurrentState.Run.AddSegment("Starlight Carnival 3");
            if (settings.starlightCarnivalAct4) timer.CurrentState.Run.AddSegment("Starlight Carnival 4");
            if (settings.starlightCarnivalAct5) timer.CurrentState.Run.AddSegment("Starlight Carnival 5");
            if (settings.starlightCarnivalAct6) timer.CurrentState.Run.AddSegment("Starlight Carnival 6");
            if (settings.starlightCarnivalBoss) timer.CurrentState.Run.AddSegment("Starlight Carnival Boss");
            if (settings.planetWispAct1) timer.CurrentState.Run.AddSegment("Planet Wisp 1");
            if (settings.planetWispAct2) timer.CurrentState.Run.AddSegment("Planet Wisp 2");
            if (settings.planetWispAct3) timer.CurrentState.Run.AddSegment("Planet Wisp 3");
            if (settings.planetWispAct4) timer.CurrentState.Run.AddSegment("Planet Wisp 4");
            if (settings.planetWispAct5) timer.CurrentState.Run.AddSegment("Planet Wisp 5");
            if (settings.planetWispAct6) timer.CurrentState.Run.AddSegment("Planet Wisp 6");
            if (settings.planetWispBoss) timer.CurrentState.Run.AddSegment("Planet Wisp Boss");
            if (settings.aquariumParkAct1) timer.CurrentState.Run.AddSegment("Aquarium Park 1");
            if (settings.aquariumParkAct2) timer.CurrentState.Run.AddSegment("Aquarium Park 2");
            if (settings.aquariumParkAct3) timer.CurrentState.Run.AddSegment("Aquarium Park 3");
            if (settings.aquariumParkAct4) timer.CurrentState.Run.AddSegment("Aquarium Park 4");
            if (settings.aquariumParkAct5) timer.CurrentState.Run.AddSegment("Aquarium Park 5");
            if (settings.aquariumParkAct6) timer.CurrentState.Run.AddSegment("Aquarium Park 6");
            if (settings.aquariumParkBoss) timer.CurrentState.Run.AddSegment("Aquarium Park Boss");
            if (settings.asteroidCoasterAct1) timer.CurrentState.Run.AddSegment("Asteroid Coaster 1");
            if (settings.asteroidCoasterAct2) timer.CurrentState.Run.AddSegment("Asteroid Coaster 2");
            if (settings.asteroidCoasterAct3) timer.CurrentState.Run.AddSegment("Asteroid Coaster 3");
            if (settings.asteroidCoasterAct4) timer.CurrentState.Run.AddSegment("Asteroid Coaster 4");
            if (settings.asteroidCoasterAct5) timer.CurrentState.Run.AddSegment("Asteroid Coaster 5");
            if (settings.asteroidCoasterAct6) timer.CurrentState.Run.AddSegment("Asteroid Coaster 6");
            if (settings.asteroidCoasterBoss) timer.CurrentState.Run.AddSegment("Asteroid Coaster Boss");
            if (settings.terminalVelocityAct1) timer.CurrentState.Run.AddSegment("Terminal Velocity 1");
            if (settings.terminalVelocityBoss) timer.CurrentState.Run.AddSegment("Terminal Velocity Boss");
            if (settings.terminalVelocityAct2) timer.CurrentState.Run.AddSegment("Terminal Velocity 2");
            if (timer.CurrentState.Run.Count == 0)
            {
                timer.CurrentState.Run.AddSegment("");
            }
        }

        public override void Dispose()
        {
            settings.Dispose();
            update_timer?.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.settings; }

        public override void SetSettings(XmlNode settings) { this.settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using LiveSplit.ComponentUtil;
using LiveSplit.Model;

namespace LiveSplit.SonicColors
{
    partial class Watchers
    {
        // Game version
        private readonly LiveSplitState state;
        public GameVersion Version { get; protected set; } = GameVersion.PC;
        public Wii emu_help { get; protected set; } = null;

        private readonly Dictionary<string, LevelID> ActsDict = new Dictionary<string, LevelID>
        {
            { "stg110", SonicColors.LevelID.TropicalResortAct1 },
            { "stg130", SonicColors.LevelID.TropicalResortAct2 },
            { "stg120", SonicColors.LevelID.TropicalResortAct3 },
            { "stg140", SonicColors.LevelID.TropicalResortAct4 },
            { "stg150", SonicColors.LevelID.TropicalResortAct5 },
            { "stg160", SonicColors.LevelID.TropicalResortAct6 },
            { "stg190", SonicColors.LevelID.TropicalResortBoss },
            { "stg210", SonicColors.LevelID.SweetMountainAct1 },
            { "stg230", SonicColors.LevelID.SweetMountainAct2 },
            { "stg220", SonicColors.LevelID.SweetMountainAct3 },
            { "stg260", SonicColors.LevelID.SweetMountainAct4 },
            { "stg240", SonicColors.LevelID.SweetMountainAct5 },
            { "stg250", SonicColors.LevelID.SweetMountainAct6 },
            { "stg290", SonicColors.LevelID.SweetMountainBoss },
            { "stg310", SonicColors.LevelID.StarlightCarnivalAct1 },
            { "stg330", SonicColors.LevelID.StarlightCarnivalAct2 },
            { "stg340", SonicColors.LevelID.StarlightCarnivalAct3 },
            { "stg350", SonicColors.LevelID.StarlightCarnivalAct4 },
            { "stg320", SonicColors.LevelID.StarlightCarnivalAct5 },
            { "stg360", SonicColors.LevelID.StarlightCarnivalAct6 },
            { "stg390", SonicColors.LevelID.StarlightCarnivalBoss },
            { "stg410", SonicColors.LevelID.PlanetWispAct1 },
            { "stg440", SonicColors.LevelID.PlanetWispAct2 },
            { "stg450", SonicColors.LevelID.PlanetWispAct3 },
            { "stg430", SonicColors.LevelID.PlanetWispAct4 },
            { "stg460", SonicColors.LevelID.PlanetWispAct5 },
            { "stg420", SonicColors.LevelID.PlanetWispAct6 },
            { "stg490", SonicColors.LevelID.PlanetWispBoss },
            { "stg510", SonicColors.LevelID.AquariumParkAct1 },
            { "stg540", SonicColors.LevelID.AquariumParkAct2 },
            { "stg550", SonicColors.LevelID.AquariumParkAct3 },
            { "stg530", SonicColors.LevelID.AquariumParkAct4 },
            { "stg560", SonicColors.LevelID.AquariumParkAct5 },
            { "stg520", SonicColors.LevelID.AquariumParkAct6 },
            { "stg590", SonicColors.LevelID.AquariumParkBoss },
            { "stg610", SonicColors.LevelID.AsteroidCoasterAct1 },
            { "stg630", SonicColors.LevelID.AsteroidCoasterAct2 },
            { "stg640", SonicColors.LevelID.AsteroidCoasterAct3 },
            { "stg650", SonicColors.LevelID.AsteroidCoasterAct4 },
            { "stg660", SonicColors.LevelID.AsteroidCoasterAct5 },
            { "stg620", SonicColors.LevelID.AsteroidCoasterAct6 },
            { "stg690", SonicColors.LevelID.AsteroidCoasterBoss },
            { "stg710", SonicColors.LevelID.TerminalVelocityAct1 },
            { "stg790", SonicColors.LevelID.TerminalVelocityBoss },
            { "stg720", SonicColors.LevelID.TerminalVelocityAct2 },
            { "stgD10", SonicColors.LevelID.SonicSimulatorAct1_1 },
            { "stgB20", SonicColors.LevelID.SonicSimulatorAct1_2 },
            { "stgE50", SonicColors.LevelID.SonicSimulatorAct1_3 },
            { "stgD20", SonicColors.LevelID.SonicSimulatorAct2_1 },
            { "stgB30", SonicColors.LevelID.SonicSimulatorAct2_2 },
            { "stgF30", SonicColors.LevelID.SonicSimulatorAct2_3 },
            { "stgG10", SonicColors.LevelID.SonicSimulatorAct3_1 },
            { "stgG30", SonicColors.LevelID.SonicSimulatorAct3_2 },
            { "stgA10", SonicColors.LevelID.SonicSimulatorAct3_3 },
            { "stgD30", SonicColors.LevelID.SonicSimulatorAct4_1 },
            { "stgG20", SonicColors.LevelID.SonicSimulatorAct4_2 },
            { "stgC50", SonicColors.LevelID.SonicSimulatorAct4_3 },
            { "stgE30", SonicColors.LevelID.SonicSimulatorAct5_1 },
            { "stgB10", SonicColors.LevelID.SonicSimulatorAct5_2 },
            { "stgE40", SonicColors.LevelID.SonicSimulatorAct5_3 },
            { "stgG40", SonicColors.LevelID.SonicSimulatorAct6_1 },
            { "stgC40", SonicColors.LevelID.SonicSimulatorAct6_2 },
            { "stgF40", SonicColors.LevelID.SonicSimulatorAct6_3 },
            { "stgA30", SonicColors.LevelID.SonicSimulatorAct7_1 },
            { "stgE20", SonicColors.LevelID.SonicSimulatorAct7_2 },
            { "stgC10", SonicColors.LevelID.SonicSimulatorAct7_3 }
        };

        // Watchers
        public FakeMemoryWatcher<LevelID> LevelID { get; private set; }
        public FakeMemoryWatcher<TimeSpan> IGT { get; private set; }
        public FakeMemoryWatcher<bool> GoalRingReached { get; private set; }
        public FakeMemoryWatcher<byte> EggShuttle_TotalStages { get; private set; }
        public FakeMemoryWatcher<byte> EggShuttle_ProgressiveID { get; private set; }
        public FakeMemoryWatcher<byte> RunStart { get; private set; }
        public FakeMemoryWatcher<sbyte> TR1Rank { get; private set; }
        public TimeSpan AccumulatedIGT { get;private set; } = default;
        public GameMode CurrentGameMode { get; private set; } = GameMode.AnyPercent;

        public Watchers(LiveSplitState state)
        {
            this.state = state;
            GameProcess = new ProcessHook("SonicColorsUltimate", "Sonic Colors - Ultimate", "Dolphin");
        }

        public void Update()
        {
            LevelID.Update();
            IGT.Update();
            GoalRingReached.Update();
            EggShuttle_TotalStages.Update();
            EggShuttle_ProgressiveID.Update();

            if (Version != GameVersion.Emulator)
            {
                RunStart.Update();
                TR1Rank.Update();
            }

            if (state.CurrentPhase == TimerPhase.NotRunning)
            {
                if (AccumulatedIGT != TimeSpan.Zero)
                    AccumulatedIGT = TimeSpan.Zero;

                CurrentGameMode = EggShuttle_TotalStages.Current > 0 && EggShuttle_TotalStages.Current <= 45 ? GameMode.EggShuttle : GameMode.AnyPercent;
            }

            if (IGT.Old != null && IGT.Current == TimeSpan.Zero && IGT.Old != TimeSpan.Zero)
                AccumulatedIGT += IGT.Old;
        }

        /// <summary>
        /// This function is essentially equivalent of the init descriptor in script-based autosplitters.
        /// Everything you want to be executed when the game gets hooked needs to be put here.
        /// The main purpose of this function is to perform sigscanning and get memory addresses and offsets
        /// needed by the autosplitter.
        /// </summary>
        private void GetAddresses()
        {
            switch (game.ProcessName.ToLower())
            {
                default:
                    emu_help?.Dispose();
                    emu_help = null;
                    Version = GameVersion.PC;
                    IntPtr ptr = game.SigScanner().ScanOrThrow(new SigScanTarget(5, "76 0C 48 8B 0D") { OnFound = (p, _, addr) => addr + 0x4 + p.ReadValue<int>(addr) });
                    var levelid = new DeepPointer(ptr, 0x8, 0x38, 0x60, 0xE0);
                    var igtraw = new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x270);
                    var grr = new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x110);
                    var eggshuttle_totalstages = new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0x0);
                    var eggshuttle_progressiveid = new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0xB8);
                    var runstart = new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x120);
                    var tr1rank = new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x1CC);
                    LevelID = new FakeMemoryWatcher<LevelID>(() => !levelid.DerefString(game, 6, out string id) || !ActsDict.ContainsKey(id) ? SonicColors.LevelID.None : ActsDict[id]);
                    IGT = new FakeMemoryWatcher<TimeSpan>(() => LevelID.Current == SonicColors.LevelID.None ? TimeSpan.Zero : TimeSpan.FromSeconds(Math.Truncate(igtraw.Deref<float>(game) * 100) / 100));
                    GoalRingReached = new FakeMemoryWatcher<bool>(() => LevelID.Current != SonicColors.LevelID.None && grr.Deref<byte>(game).BitCheck(5));
                    EggShuttle_TotalStages = new FakeMemoryWatcher<byte>(() => eggshuttle_totalstages.Deref<byte>(game));
                    EggShuttle_ProgressiveID = new FakeMemoryWatcher<byte>(() => eggshuttle_progressiveid.Deref<byte>(game));
                    RunStart = new FakeMemoryWatcher<byte>(() => runstart.Deref<byte>(game));
                    TR1Rank = new FakeMemoryWatcher<sbyte>(() => tr1rank.Deref<sbyte>(game));
                    break;

                case "dolphin":
                    Version = GameVersion.Emulator;
                    emu_help = new Wii { Gamecodes = new[] { "SNCE8P", "SNCJ8P", "SNCP8P" } };
                    emu_help.Load = (MEM1, MEM2) => new MemoryWatcherList
                    {
                        new StringWatcher(new DeepPointer(MEM1 + 0xB3E0F4, 0x84), 6) { Name = "LevelID" },
                        new MemoryWatcher<float>(new DeepPointer(MEM1 + 0xB3E0F4, 0x190)) { Name = "IGT" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F4, 0xA7)) { Name = "GoalRingReached_raw" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F8, 0xE3)) { Name = "EggShuttle_TotalStages" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F8, 0x19B)) { Name = "EggShuttle_ProgressiveID" },
                    };
                    LevelID = new FakeMemoryWatcher<LevelID>(() => emu_help["LevelID"].Current == null || !ActsDict.ContainsKey((string)emu_help["LevelID"].Current) ? SonicColors.LevelID.None : ActsDict[(string)emu_help["LevelID"].Current]);
                    IGT = new FakeMemoryWatcher<TimeSpan>(() => LevelID.Current == SonicColors.LevelID.None ? TimeSpan.Zero : TimeSpan.FromSeconds(Math.Truncate((float)emu_help["IGT"].Current * 100) / 100));
                    GoalRingReached = new FakeMemoryWatcher<bool>(() => LevelID.Current != SonicColors.LevelID.None && ((byte)emu_help["GoalRingReached_raw"].Current).BitCheck(5));
                    EggShuttle_TotalStages = new FakeMemoryWatcher<byte>(() => (byte)emu_help["EggShuttle_TotalStages"].Current);
                    EggShuttle_ProgressiveID = new FakeMemoryWatcher<byte>(() => (byte)emu_help["EggShuttle_ProgressiveID"].Current);
                    RunStart = new FakeMemoryWatcher<byte>();
                    TR1Rank = new FakeMemoryWatcher<sbyte>();
                    break;
            }
        }

        public void ResetVars()
        {
            AccumulatedIGT = default;
        }
    }
}
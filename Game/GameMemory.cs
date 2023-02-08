using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

        private readonly Dictionary<string, LevelID> ActsDict = new()
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
        private StringWatcher LevelID_raw { get; set; }
        private MemoryWatcher<byte> LevelID_numeric { get; set; }
        private MemoryWatcher<float> IGT_raw { get; set; }
        private MemoryWatcher<byte> GoalRingReached_raw { get; set; }
        public MemoryWatcher<byte> EggShuttle_TotalStages { get; private set; }
        public MemoryWatcher<byte> EggShuttle_ProgressiveID { get; private set; }
        public MemoryWatcher<byte> RunStart { get; private set; }
        public MemoryWatcher<sbyte> TR1Rank { get; private set; }
        public FakeMemoryWatcher<TimeSpan> IGT { get; private set; }
        public FakeMemoryWatcher<LevelID> LevelID { get; private set; }
        public FakeMemoryWatcher<bool> GoalRingReached { get; private set; }
        public TimeSpan AccumulatedIGT { get;private set; } = default;
        public GameMode CurrentGameMode { get; private set; } = GameMode.AnyPercent;

        public Watchers(LiveSplitState state)
        {
            this.state = state;

            LevelID = new FakeMemoryWatcher<LevelID>(() => LevelID_numeric.Current != 0 && ActsDict.ContainsKey(LevelID_raw.Current) ? ActsDict[LevelID_raw.Current] : SonicColors.LevelID.None);
            GoalRingReached = new FakeMemoryWatcher<bool>(() => LevelID.Current != SonicColors.LevelID.None && GoalRingReached_raw.Current.BitCheck(5));
            IGT = new FakeMemoryWatcher<TimeSpan>(() => LevelID.Current == SonicColors.LevelID.None ? TimeSpan.Zero : TimeSpan.FromSeconds(Math.Truncate(IGT_raw.Current * 100) / 100));
            
            GameProcess = new ProcessHook("SonicColorsUltimate", "Sonic Colors - Ultimate", "Dolphin");
        }

        public void Update()
        {
            if (Version == GameVersion.Emulator)
            {
                LevelID_raw.Current = (string)emu_help["LevelID_raw"].Current ?? string.Empty; LevelID_raw.Old = (string)emu_help["LevelID_raw"].Old ?? string.Empty;
                LevelID_numeric.Current = (byte)emu_help["LevelID_numeric"].Current; LevelID_numeric.Old = (byte)emu_help["LevelID_numeric"].Old;
                IGT_raw.Current = (float)emu_help["IGT_raw"].Current; IGT_raw.Old = (float)emu_help["IGT_raw"].Old;
                GoalRingReached_raw.Current = (byte)emu_help["GoalRingReached_raw"].Current; GoalRingReached_raw.Old = (byte)emu_help["GoalRingReached_raw"].Old;
                EggShuttle_TotalStages.Current = (byte)emu_help["EggShuttle_TotalStages"].Current; EggShuttle_TotalStages.Old = (byte)emu_help["EggShuttle_TotalStages"].Old;
                EggShuttle_ProgressiveID.Current = (byte)emu_help["EggShuttle_ProgressiveID"].Current; EggShuttle_ProgressiveID.Old = (byte)emu_help["EggShuttle_ProgressiveID"].Old;
            }
            else
            {
                WatcherList.UpdateAll(game);
            }

            LevelID.Update();
            GoalRingReached.Update();
            IGT.Update();

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
                    LevelID_raw = new StringWatcher(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0xE0), 6) { Current = " ", Old = " " };
                    LevelID_numeric = new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0xE0)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull, Current = 0 };
                    IGT_raw = new MemoryWatcher<float>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x270)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    GoalRingReached_raw = new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x110)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    EggShuttle_TotalStages = new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0x0)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    EggShuttle_ProgressiveID = new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0xB8)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    RunStart = new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x120)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    TR1Rank = new MemoryWatcher<sbyte>(new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x1CC)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
                    WatcherList = new MemoryWatcherList { LevelID_raw, LevelID_numeric, IGT_raw, GoalRingReached_raw, EggShuttle_TotalStages, EggShuttle_ProgressiveID, RunStart, TR1Rank };
                    break;

                case "dolphin":
                    Version = GameVersion.Emulator;
                    emu_help = new Wii { Gamecodes = new[] { "SNCE8P", "SNCJ8P", "SNCP8P" } };
                    emu_help.Load = (MEM1, MEM2) => new MemoryWatcherList
                    {
                        new StringWatcher(new DeepPointer(MEM1 + 0xB3E0F4, 0x84), 6) { Name = "LevelID_raw" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F4, 0x84)) { Name = "LevelID_numeric" },
                        new MemoryWatcher<float>(new DeepPointer(MEM1 + 0xB3E0F4, 0x190)) { Name = "IGT_raw" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F4, 0xA7)) { Name = "GoalRingReached_raw" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F8, 0xE3)) { Name = "EggShuttle_TotalStages" },
                        new MemoryWatcher<byte>(new DeepPointer(MEM1 + 0xB3E0F8, 0x19B)) { Name = "EggShuttle_ProgressiveID" },
                    };
                    LevelID_raw = new StringWatcher(IntPtr.Zero, 1) { Enabled = false };
                    LevelID_numeric = new MemoryWatcher<byte>(IntPtr.Zero) { Enabled = false };
                    IGT_raw = new MemoryWatcher<float>(IntPtr.Zero) { Enabled = false };
                    GoalRingReached_raw = new MemoryWatcher<byte>(IntPtr.Zero) { Enabled = false };
                    EggShuttle_TotalStages = new MemoryWatcher<byte>(IntPtr.Zero) { Enabled = false };
                    EggShuttle_ProgressiveID = new MemoryWatcher<byte>(IntPtr.Zero) { Enabled = false };
                    RunStart = new MemoryWatcher<byte>(IntPtr.Zero) { Enabled = false, Current = default, Old = default };
                    TR1Rank = new MemoryWatcher<sbyte>(IntPtr.Zero) { Enabled = false, Current = default, Old = default };
                    WatcherList = new MemoryWatcherList { RunStart, TR1Rank };
                    break;
            }
        }

        public void ResetVars()
        {
            AccumulatedIGT = default;
        }
    }
}
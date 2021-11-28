using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicColors
{
    class Watchers : MemoryWatcherList
    {
        // Game process
        private readonly Process game;
        public bool IsGameHooked => game != null && !game.HasExited;

        // Imported game data
        public MemoryWatcher<byte> RunStart { get; }
        public MemoryWatcher<sbyte> TR1rank { get; }
        private MemoryWatcher<float> igt { get; }
        private MemoryWatcher<byte> goalRingReached_byte { get; }
        private StringWatcher levelID { get; }
        private MemoryWatcher<byte> levelID_numeric { get; }
        public MemoryWatcher<byte> EggShuttle_TotalStages { get; }
        public MemoryWatcher<byte> EggShuttle_ProgressiveID { get; }

        // Fake MemoryWatchers: used to convert game data into more easily readable formats and to manage some quirks in game memory
        public FakeMemoryWatcher<bool> GoalRingReached => new FakeMemoryWatcher<bool>((this.goalRingReached_byte.Old & (1 << 5)) != 0, (this.goalRingReached_byte.Current & (1 << 5)) != 0);
        public FakeMemoryWatcher<string> LevelID => new FakeMemoryWatcher<string>(this.levelID_numeric.Old == 0 ? "none" : this.levelID.Old, this.levelID_numeric.Current == 0 ? Levels.None : this.levelID.Current);
        public FakeMemoryWatcher<double> IGT => new FakeMemoryWatcher<double>(this.levelID_numeric.Old == 8 || this.levelID_numeric.Old == 0 ? 0 : Math.Truncate(this.igt.Old * 100) / 100,
            this.levelID_numeric.Current == 8 || this.levelID_numeric.Current == 0 ? 0 : Math.Truncate(this.igt.Current * 100) / 100);

        // Useful functions and internal variables
        public bool IGThasReset => this.IGT.Old != 0 && this.IGT.Current == 0;
        public double AccumulatedIGT = 0d;
        public bool IsEggShuttle = false;


        public Watchers()
        {
            foreach (string process in new string[] { "Sonic Colors - Ultimate" })
            {
                game = Process.GetProcessesByName(process).OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
                if (game != null) break;
            }
            if (game == null) throw new Exception("Couldn't connect to the game!");

            var scanner = new SignatureScanner(game, game.MainModule.BaseAddress, game.MainModule.ModuleMemorySize);
            IntPtr ptr;

            // Basic checks
            if (!game.Is64Bit()) throw new Exception();
            ptr = scanner.Scan(new SigScanTarget("53 6F 6E 69 63 20 43 6F 6C 6F 72 73 3A 20 55 6C 74 69 6D 61 74 65"));   // Check if the exe is internally named "Sonic Colors: Ultimate"
            if (ptr == IntPtr.Zero) throw new Exception();

            // Run start (Any% and All Chaos Emeralds)
            // Corresponds to the "name" assigned to the internal savefile
            // New game = "########"
            // Otherwise = "no-name"
            // Can be used for signalling a reset
            ptr = scanner.Scan(new SigScanTarget(5,
                "74 2B",                 // je "Sonic Colors - Ultimate.exe"+16F3948
                "48 8B 0D ????????"));   // mov rcx,["Sonic Colors - Ultimate.exe"+52462A8]
            if (ptr == IntPtr.Zero) throw new Exception();
            this.RunStart = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x60, 0x120)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            this.TR1rank = new MemoryWatcher<sbyte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x60, 0x1CC)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };  // Must be FF in a new game

            // Current level data pointer
            // This region of memory contains basic data about the current level you're in, such as IGT, rings, score, etc. 
            // Also has a lot of flags (inside bitmasks) I didn't bother to investigate
            ptr = scanner.Scan(new SigScanTarget(5,
               "31 C0",                 // xor eax,eax
               "48 89 05 ????????"));   // mov ["Sonic Colors - Ultimate.exe"+52465C0],rax
            if (ptr == IntPtr.Zero) throw new Exception();
            this.igt = new MemoryWatcher<float>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x0, 0x270)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };
            this.goalRingReached_byte = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x0, 0x110)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull }; // Bit 5 gets flipped th emoment the stage is reported by the game as complete and all in-level events stop (eg. IGT stops)                                                                                                                                                                  // Level ID
            this.levelID = new StringWatcher(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x0, 0xE0), 6); // It's a 6-character ID that uniquely reports the level you're in. The IDs are the same as in the Wii version of the game. Check Wii's actstgmission.lua for details
            this.levelID_numeric = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x0, 0xE0)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull };

            // Egg Shuttle data pointer
            // This memory region becomes accessible only when you're inside Egg Shuttle
            ptr = scanner.Scan(new SigScanTarget(5,
                "76 0C",                 // jna "Sonic Colors - Ultimate.exe"+16DF25C
                "48 8B 0D ????????"));   // mov rcx,["Sonic colors - Ultimate.exe"+5245658]
            if (ptr == IntPtr.Zero) throw new Exception();
            // Egg Shuttle total levels (indicates the total number of stages included in Egg Shuttle mode)
            this.EggShuttle_TotalStages = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x8, 0x38, 0x68, 0x110, 0x0)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull }; // This value is always above 0 so it can be used to report whether you're in egg shuttle or not
            this.EggShuttle_ProgressiveID = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr), 0x8, 0x38, 0x68, 0x110, 0xB8)) { FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull }; // Indicates level progression inside Egg Shuttle. Goes from 0 to 44 (44 = Terminal Velocity Act 2). It techically also goes to 45 at the final results screen after Terminal Velocity Act

            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }

        public void Update() => this.UpdateAll(game);
    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed { get; }
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
            this.Changed = !old.Equals(current);
        }
    }

    internal static class Levels
    {
        internal const string TropicalResortAct1 = "stg110";
        internal const string TropicalResortAct2 = "stg130";
        internal const string TropicalResortAct3 = "stg120";
        internal const string TropicalResortAct4 = "stg140";
        internal const string TropicalResortAct5 = "stg150";
        internal const string TropicalResortAct6 = "stg160";
        internal const string TropicalResortBoss = "stg190";
        internal const string SweetMountainAct1 = "stg210";
        internal const string SweetMountainAct2 = "stg230";
        internal const string SweetMountainAct3 = "stg220";
        internal const string SweetMountainAct4 = "stg260";
        internal const string SweetMountainAct5 = "stg240";
        internal const string SweetMountainAct6 = "stg250";
        internal const string SweetMountainBoss = "stg290";
        internal const string StarlightCarnivalAct1 = "stg310";
        internal const string StarlightCarnivalAct2 = "stg330";
        internal const string StarlightCarnivalAct3 = "stg340";
        internal const string StarlightCarnivalAct4 = "stg350";
        internal const string StarlightCarnivalAct5 = "stg320";
        internal const string StarlightCarnivalAct6 = "stg360";
        internal const string StarlightCarnivalBoss = "stg390";
        internal const string PlanetWispAct1 = "stg410";
        internal const string PlanetWispAct2 = "stg440";
        internal const string PlanetWispAct3 = "stg450";
        internal const string PlanetWispAct4 = "stg430";
        internal const string PlanetWispAct5 = "stg460";
        internal const string PlanetWispAct6 = "stg420";
        internal const string PlanetWispBoss = "stg490";
        internal const string AquariumParkAct1 = "stg510";
        internal const string AquariumParkAct2 = "stg540";
        internal const string AquariumParkAct3 = "stg550";
        internal const string AquariumParkAct4 = "stg530";
        internal const string AquariumParkAct5 = "stg560";
        internal const string AquariumParkAct6 = "stg520";
        internal const string AquariumParkBoss = "stg590";
        internal const string AsteroidCoasterAct1 = "stg610";
        internal const string AsteroidCoasterAct2 = "stg630";
        internal const string AsteroidCoasterAct3 = "stg640";
        internal const string AsteroidCoasterAct4 = "stg650";
        internal const string AsteroidCoasterAct5 = "stg660";
        internal const string AsteroidCoasterAct6 = "stg620";
        internal const string AsteroidCoasterBoss = "stg690";
        internal const string TerminalVelocityAct1 = "stg710";
        internal const string TerminalVelocityBoss = "stg790";
        internal const string TerminalVelocityAct2 = "stg720";
        internal const string SonicSimulator1_1 = "stgD10";
        internal const string SonicSimulator1_2 = "stgB20";
        internal const string SonicSimulator1_3 = "stgE50";
        internal const string SonicSimulator2_1 = "stgD20";
        internal const string SonicSimulator2_2 = "stgB30";
        internal const string SonicSimulator2_3 = "stgF30";
        internal const string SonicSimulator3_1 = "stgG10";
        internal const string SonicSimulator3_2 = "stgG30";
        internal const string SonicSimulator3_3 = "stgA10";
        internal const string SonicSimulator4_1 = "stgD30";
        internal const string SonicSimulator4_2 = "stgG20";
        internal const string SonicSimulator4_3 = "stgC50";
        internal const string SonicSimulator5_1 = "stgE30";
        internal const string SonicSimulator5_2 = "stgB10";
        internal const string SonicSimulator5_3 = "stgE40";
        internal const string SonicSimulator6_1 = "stgG40";
        internal const string SonicSimulator6_2 = "stgC40";
        internal const string SonicSimulator6_3 = "stgF40";
        internal const string SonicSimulator7_1 = "stgA30";
        internal const string SonicSimulator7_2 = "stgE20";
        internal const string SonicSimulator7_3 = "stgC10";
        internal const string None = "none";
    }
}

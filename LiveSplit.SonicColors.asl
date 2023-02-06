// Sonic Colors
// Sonic Colors: Ultimate
// Autosplitter and IGT timer
// Coding: Jujstme
// Version 4.0 (May 15th, 2022)
// Sonic Colors speedrunning discord: https://discord.gg/QjWTShBhh8

state("SonicColorsUltimate"){}
state("Sonic Colors - Ultimate"){}
state("Dolphin"){}

init
{
    vars.UsesBigEndian = false;

    // Initializing the main variables we need for sigscanning
    vars.watchers = new MemoryWatcherList();
    IntPtr ptr = IntPtr.Zero;
    Action checkptr = () => { if (ptr == IntPtr.Zero) throw new NullReferenceException(); };
    var scanner = new SignatureScanner(game, modules.First().BaseAddress, modules.First().ModuleMemorySize);

    switch (game.ProcessName.ToLower())
    {
        case "sonic colors - ultimate":
        case "soniccolorsultimate":
            vars.DebugPrint("  => Game version identified: Sonic Colors Ultimate");
            version = "PC (Ultimate)";

            // The game is 64-bit only
            if (!game.Is64Bit()) throw new Exception("Hooked process is not 64bit. Hooked the wrong game process or unsupported game version.");
            
            vars.DebugPrint("  => Initializing sigscan");

            // Level data pointers
            // This memory region contains all the data about the current level you're in, such as IGT, rings, score, etc. Also has a lot of flags (inside bitmasks) I didn't bother to investigate.
            // Also contains data about the current Egg Shuttle run, though the corresponding pointer is accessible only if you're inside Egg Shuttle.

            ptr = scanner.Scan(new SigScanTarget(5, "76 0C 48 8B 0D") { OnFound = (p, s, addr) => addr + 0x4 + p.ReadValue<int>(addr) });
            checkptr();
            vars.DebugPrint("    => Done");
            vars.DebugPrint("  => Setting up MemoryWatchers...");
            vars.watchers.Add(new StringWatcher(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0xE0), 6) { Name = "levelID" });                                                                                     // It's a 6-character ID that uniquely reports the level you're in. The IDs are the same as in the Wii version of the game. Check Wii's actstgmission.lua for details
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0xE0)) { Name = "levelID_numeric", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });                 // Same as above, but deals with the fact that the StringWatcher doesn't have a working ReadFailAction
            vars.watchers.Add(new MemoryWatcher<float>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x270)) { Name = "igt", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });                           // Pretty self-explanatory. It's the internal level timer. Will be garbage numbers outside levels but this is dealth with in the "update" method
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x60, 0x110)) { Name = "goalRingReached_byte", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });           // Bit 5 gets flipped the moment the stage is reported by the game as complete and all in-level events are stopped (including IGT)
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0x0)) { Name = "EggShuttle_TotalStages", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });    // This value is always above 0 if the pointer is accessible, so it can be used to report whether you're in egg shuttle or not
            vars.watchers.Add(new MemoryWatcher<byte>(new DeepPointer(ptr, 0x8, 0x38, 0x68, 0x110, 0xB8)) { Name = "EggShuttle_ProgressiveID", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull }); // Indicates level progression inside Egg Shuttle. Goes from 0 to 44 (44 = Terminal Velocity Act 2). It techically also goes to 45 at the final results screen after Terminal Velocity Act
            vars.watchers.Add(new MemoryWatcher<byte> (new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x120)) { Name = "RunStart", FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });                 // Corresponds to the name assigned to the internal savefile (it's leftover data from the Wii version). New game = "########"; Otherwise = "no-name"
            vars.watchers.Add(new MemoryWatcher<sbyte>(new DeepPointer(ptr, 0x8, 0x8, 0x10, 0x60, 0x1CC)) { Name = "TR1rank",  FailAction = MemoryWatcher.ReadFailAction.SetZeroOrNull });                 // It's the rank for Tropical Resort Act 1. 0xFF (or -1 as a sbyte) means the level has not been completed yet (eg. new game)
            vars.DebugPrint("    => Done");
        break;

        case "dolphin":
            vars.DebugPrint("  => Game version identified: Sonic Colors (Wii - Dolphin)");
            version = "Wii - Dolphin";

            // Gamecube uses Big Endian
            vars.UsesBigEndian = true;

            vars.DebugPrint("  => Locating base RAM address (MEM2)...");
            // Stealing the method used by Dolphin memory engine (https://github.com/aldelaro5/Dolphin-memory-engine) to locate the RAM base address
            ptr = game.MemoryPages(true).FirstOrDefault(p => p.Type == MemPageType.MEM_MAPPED && p.State == MemPageState.MEM_COMMIT && (int)p.RegionSize == 0x4000000).BaseAddress;
            checkptr();
            vars.DebugPrint("  => MEM2 address found at 0x" + ptr.ToString("X"));

            vars.DebugPrint("  => Setting up MemoryWatchers...");
            vars.watchers.Add(new StringWatcher(ptr + 0xAB7064, 6) { Name = "levelID" });
            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB7064) { Name = "levelID_numeric" });
            vars.watchers.Add(new MemoryWatcher<float>(ptr + 0xAB7170) { Name = "igt" });
            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB7087) { Name = "goalRingReached_byte" });

            // Additional memory addresses because, for some reason, Egg Shuttle shifts everything by 0x360 bytes
            vars.watchers.Add(new StringWatcher(ptr + 0xAB7064 + 0x360, 6) { Name = "levelID_360" });
            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB7064 + 0x360) { Name = "levelID_numeric_360" });
            vars.watchers.Add(new MemoryWatcher<float>(ptr + 0xAB7170 + 0x360) { Name = "igt_360" });
            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB7087 + 0x360) { Name = "goalRingReached_byte_360" });

            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB6FE3) { Name = "EggShuttle_TotalStages" });
            vars.watchers.Add(new MemoryWatcher<byte>(ptr + 0xAB709B) { Name = "EggShuttle_ProgressiveID" });

            // Setting "Enabled = false" because those are dummy MemoryWatchers.
            vars.watchers.Add(new MemoryWatcher<byte> (IntPtr.Zero) { Name = "RunStart", Enabled = false });
            vars.watchers.Add(new MemoryWatcher<sbyte>(IntPtr.Zero) { Name = "TR1rank", Enabled = false });

            vars.DebugPrint("    => Done");
        break;
    }

    // Default values
    vars.DebugPrint("  => Setting up default values...");
    current.IGT = TimeSpan.Zero;
    current.LevelID = vars.InvalidLevelID;
    current.GoalRingReached = false;
    vars.DebugPrint("    => Done");

    vars.DebugPrint("  => Init script completed");
}

startup
{
    vars.Acts = new string[]
    {
        "stg110", "stg130", "stg120", "stg140", "stg150", "stg160", "stg190",  // Tropical Resort
        "stg210", "stg230", "stg220", "stg260", "stg240", "stg250", "stg290",  // Sweet Mountain
        "stg310", "stg330", "stg340", "stg350", "stg320", "stg360", "stg390",  // Starlight Carnival
        "stg410", "stg440", "stg450", "stg430", "stg460", "stg420", "stg490",  // Planet Wisp
        "stg510", "stg540", "stg550", "stg530", "stg560", "stg520", "stg590",  // Aquarium Park
        "stg610", "stg630", "stg640", "stg650", "stg660", "stg620", "stg690",  // Asteroid Coaster
        "stg710", "stg790", "stg720",                                          // Terminal Velocity
        "stgD10", "stgB20", "stgE50",                                          // Sonic Simulator 1
        "stgD20", "stgB30", "stgF30",                                          // Sonic Simulator 2
        "stgG10", "stgG30", "stgA10",                                          // Sonic Simulator 3
        "stgD30", "stgG20", "stgC50",                                          // Sonic Simulator 4
        "stgE30", "stgB10", "stgE40",                                          // Sonic Simulator 5
        "stgG40", "stgC40", "stgF40",                                          // Sonic Simulator 6
        "stgA30", "stgE20", "stgC10",                                          // Sonic Simulator 7
    };

    int j = 0;
    string[,] Settings =
    {
        { "autostartSettings", "Autostart options", null },
            { "cleanSave", "Autostart (Any% or 100%)", "autostartSettings" },
            { "sonicSim", "Autostart (Sonic Simulator 1-1)", "autostartSettings" },
            { "eggShuttle", "Autostart (Egg Shuttle)", "autostartSettings" },
        { "resetSettings", "Reset options", null },
            { "resetSave", "Reset upon deleting the save file (for Any% runs)", "resetSettings" },
            { "restarteggShuttle", "Reset upon restarting Egg Shuttle", "resetSettings" },
        { "autosplitting", "Autosplitting options", null },
            { "TR", "Tropical Resort", "autosplitting" },
                { vars.Acts[j++], "Act 1", "TR" },
                { vars.Acts[j++], "Act 2", "TR" },
                { vars.Acts[j++], "Act 3", "TR" },
                { vars.Acts[j++], "Act 4", "TR" },
                { vars.Acts[j++], "Act 5", "TR" },
                { vars.Acts[j++], "Act 6", "TR" },
                { vars.Acts[j++], "Boss", "TR" },
            { "SM", "Sweet Mountain", "autosplitting" },
                { vars.Acts[j++], "Act 1", "SM" },
                { vars.Acts[j++], "Act 2", "SM" },
                { vars.Acts[j++], "Act 3", "SM" },
                { vars.Acts[j++], "Act 4", "SM" },
                { vars.Acts[j++], "Act 5", "SM" },
                { vars.Acts[j++], "Act 6", "SM" },
                { vars.Acts[j++], "Boss", "SM" },
            { "SC", "Starlight Carnival", "autosplitting" },
                { vars.Acts[j++], "Act 1", "SC" },
                { vars.Acts[j++], "Act 2", "SC" },
                { vars.Acts[j++], "Act 3", "SC" },
                { vars.Acts[j++], "Act 4", "SC" },
                { vars.Acts[j++], "Act 5", "SC" },
                { vars.Acts[j++], "Act 6", "SC" },
                { vars.Acts[j++], "Boss", "SC" },
            { "PW", "Planet Wisp", "autosplitting" },
                { vars.Acts[j++], "Act 1", "PW" },
                { vars.Acts[j++], "Act 2", "PW" },
                { vars.Acts[j++], "Act 3", "PW" },
                { vars.Acts[j++], "Act 4", "PW" },
                { vars.Acts[j++], "Act 5", "PW" },
                { vars.Acts[j++], "Act 6", "PW" },
                { vars.Acts[j++], "Boss", "PW" },
            { "AP", "Aquarium Park", "autosplitting" },
                { vars.Acts[j++], "Act 1", "AP" },
                { vars.Acts[j++], "Act 2", "AP" },
                { vars.Acts[j++], "Act 3", "AP" },
                { vars.Acts[j++], "Act 4", "AP" },
                { vars.Acts[j++], "Act 5", "AP" },
                { vars.Acts[j++], "Act 6", "AP" },
                { vars.Acts[j++], "Boss", "AP" },
            { "AC", "Asteroid Coaster", "autosplitting" },
                { vars.Acts[j++], "Act 1", "AC" },
                { vars.Acts[j++], "Act 2", "AC" },
                { vars.Acts[j++], "Act 3", "AC" },
                { vars.Acts[j++], "Act 4", "AC" },
                { vars.Acts[j++], "Act 5", "AC" },
                { vars.Acts[j++], "Act 6", "AC" },
                { vars.Acts[j++], "Boss", "AC" },
            { "TV", "Terminal Velocity", "autosplitting" },
                { vars.Acts[j++], "Act 1", "TV" },
                { vars.Acts[j++], "Boss", "TV" },
                { vars.Acts[j++], "Act 2", "TV" },
            { "SS", "Sonic Simulator", "autosplitting" },
                { vars.Acts[j++], "1-1", "SS" },
                { vars.Acts[j++], "1-2", "SS" },
                { vars.Acts[j++], "1-3", "SS" },
                { vars.Acts[j++], "2-1", "SS" },
                { vars.Acts[j++], "2-2", "SS" },
                { vars.Acts[j++], "2-3", "SS" },
                { vars.Acts[j++], "3-1", "SS" },
                { vars.Acts[j++], "3-2", "SS" },
                { vars.Acts[j++], "3-3", "SS" },
                { vars.Acts[j++], "4-1", "SS" },
                { vars.Acts[j++], "4-2", "SS" },
                { vars.Acts[j++], "4-3", "SS" },
                { vars.Acts[j++], "5-1", "SS" },
                { vars.Acts[j++], "5-2", "SS" },
                { vars.Acts[j++], "5-3", "SS" },
                { vars.Acts[j++], "6-1", "SS" },
                { vars.Acts[j++], "6-2", "SS" },
                { vars.Acts[j++], "6-3", "SS" },
                { vars.Acts[j++], "7-1", "SS" },
                { vars.Acts[j++], "7-2", "SS" },
                { vars.Acts[j++], "7-3", "SS" },
    };
    for (int i = 0; i < Settings.GetLength(0); i++)
        settings.Add(Settings[i, 0], true, Settings[i, 1], Settings[i, 2]);
    
    // Default variables
    vars.AccumulatedIGT = TimeSpan.Zero;
    vars.IsEggShuttle = false;

    // Named enums
    vars.RunStartStatus = new ExpandoObject();
    vars.RunStartStatus.CleanSave = 35;
    vars.RunStartStatus.SavedFile = 110;

    vars.RankNotRanked = -1;
    vars.InvalidLevelID = "none";
    vars.LevelIdentifier = "stg";

    // Custom functions - actually used only to convert the IGT (float)
    vars.ToLittleEndian = (Func<float, float>)(input => {
        byte[] temp = BitConverter.GetBytes(input);
        Array.Reverse(temp);
        return BitConverter.ToSingle(temp, 0);
    });

    // Debug functions
    var debug = true; // Easy flag to quickly enable and disable debug outputs. When they're not needed anymore all it takes is to set this to false.
    vars.DebugPrint = (Action<string>)((string obj) => { if (debug) print("[Sonic Colors] " + obj); });
    vars.resetOffsetTrigger = false;
}

update
{
    // Update the watchers
    vars.watchers.UpdateAll(game);

    // Ugly hack for dealing with the timer offset on the Wii version
    if (vars.resetOffsetTrigger)
    {
        timer.Run.Offset = TimeSpan.Zero;
        vars.resetOffsetTrigger = false;
    }

    // Special fixes for the Wii version of the game
    if (vars.UsesBigEndian)
    {
        if (vars.watchers["EggShuttle_TotalStages"].Current > 0 && vars.watchers["EggShuttle_TotalStages"].Current <= 45)
        {
            vars.watchers["levelID"].Current = vars.watchers["levelID_360"].Current;
            vars.watchers["levelID_numeric"].Current = vars.watchers["levelID_numeric_360"].Current;
            vars.watchers["igt"].Current = vars.watchers["igt_360"].Current;
            vars.watchers["goalRingReached_byte"].Current = vars.watchers["goalRingReached_byte_360"].Current;
        }
        vars.watchers["igt"].Current = vars.ToLittleEndian(vars.watchers["igt"].Current);
    }

    // Convert some game data into more easily readable formats and manage some quirks in game memory
    current.LevelID = vars.watchers["levelID_numeric"].Current == 0 || !vars.watchers["levelID"].Current.Contains(vars.LevelIdentifier) ? vars.InvalidLevelID : vars.watchers["levelID"].Current;
    current.GoalRingReached = current.LevelID == vars.InvalidLevelID ? false : (vars.watchers["goalRingReached_byte"].Current & (1 << 5)) != 0;
    current.IGT = current.LevelID == vars.InvalidLevelID ? TimeSpan.Zero : TimeSpan.FromSeconds(Math.Truncate(vars.watchers["igt"].Current * 100) / 100);

    // if the timer is not running (eg. a run has been reset) these variables need to be reset
    if (timer.CurrentPhase == TimerPhase.NotRunning)
    {
        vars.AccumulatedIGT = TimeSpan.Zero;
        vars.IsEggShuttle = vars.watchers["EggShuttle_TotalStages"].Current > 0 && vars.watchers["EggShuttle_TotalStages"].Current <= 45;
    }

    // Accumulate the time if the IGT resets
    if (old.IGT != TimeSpan.Zero && current.IGT == TimeSpan.Zero)
        vars.AccumulatedIGT += old.IGT;
}

start
{
    if (vars.IsEggShuttle)
    {
        if (settings["eggShuttle"])
        {
            return (!vars.UsesBigEndian && current.LevelID == vars.Acts[0] && old.LevelID == vars.InvalidLevelID) // Used in the PC version
                || (vars.UsesBigEndian && current.LevelID == vars.Acts[0] && current.IGT == TimeSpan.Zero && !old.GoalRingReached); // Used on the Wii version
        }
    } else {
        if (settings["cleanSave"])
        {
            if (vars.UsesBigEndian)
            {
                if (old.LevelID == vars.InvalidLevelID && current.LevelID == vars.Acts[0])
                {
                    timer.Run.Offset = TimeSpan.FromSeconds(1);
                    vars.resetOffsetTrigger = true;
                    return true;
                }
            } else {
                return vars.watchers["TR1rank"].Current == vars.RankNotRanked && vars.watchers["RunStart"].Old == vars.RunStartStatus.CleanSave && vars.watchers["RunStart"].Current == vars.RunStartStatus.SavedFile; // Used on the PC version
            }
        }
        else if (settings["sonicSim"])
        {
            return current.LevelID == vars.Acts[45] && old.LevelID == vars.InvalidLevelID;
        }
    }
}

reset
{
    if (vars.IsEggShuttle)
    {
        if (settings["restarteggShuttle"] && old.IGT != TimeSpan.Zero && current.IGT == TimeSpan.Zero && !old.GoalRingReached)
        {
            vars.AccumulatedIGT = TimeSpan.Zero;
            return true;
        }
    } else {
        return
            settings["resetSave"] &&
            vars.watchers["RunStart"].Old == vars.RunStartStatus.SavedFile &&
            vars.watchers["RunStart"].Current == vars.RunStartStatus.CleanSave;
    }
}

split
{
    // All splitting conditions eventually end with this loop so it's better
    // to just put it inside a function we can call whenever we want.
    Func<bool> checkSplit = () => {
        foreach (var entry in vars.Acts)
        {
            if (old.LevelID == entry)
                return settings[old.LevelID];
        }
        return false;
    };

    if (vars.IsEggShuttle)
    {
        if (vars.watchers["EggShuttle_ProgressiveID"].Old == vars.watchers["EggShuttle_TotalStages"].Current - 1)
        {
            // If we are in the last stage of Egg Shuttle, trigger a split whenever we reach the end of said stage
            if (current.GoalRingReached && !old.GoalRingReached)
                return checkSplit();
        } else {
            // If we are NOT in the last stage of egg shuttle, trigger a split when loading the next level
            if (vars.watchers["EggShuttle_ProgressiveID"].Current == vars.watchers["EggShuttle_ProgressiveID"].Old + 1)
                return checkSplit();
        }
    } else {
        // Special condition for TV act 2: it needs to trigger a split when the screen starts to fade to white
        if (old.LevelID == vars.Acts[44])
            return settings[old.LevelID] && current.GoalRingReached && !old.GoalRingReached;
        else
            if (!current.GoalRingReached && old.GoalRingReached && current.LevelID != old.LevelID)
                return checkSplit();
    }
}

gameTime
{
    return current.IGT + vars.AccumulatedIGT;
}

isLoading
{
    return true;
}

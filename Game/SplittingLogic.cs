using System;

namespace LiveSplit.SonicColors
{
    partial class SonicColorsComponent
    {
        private bool Start()
        {
            if (watchers.Version == GameVersion.Emulator)
            {
                return watchers.CurrentGameMode switch
                {
                    GameMode.EggShuttle => settings.StartEggShuttle && watchers.LevelID.Current == LevelID.TropicalResortAct1 && (watchers.LevelID.Old == LevelID.None || (watchers.IGT.Old > TimeSpan.Zero && watchers.IGT.Current == TimeSpan.Zero)),
                    _ => (settings.StartAny && watchers.LevelID.Current == LevelID.TropicalResortAct1 && watchers.LevelID.Old == LevelID.None)
                         || (settings.StartSonicSimulator && watchers.LevelID.Current == LevelID.SonicSimulatorAct1_1 && watchers.LevelID.Old == LevelID.None),
                };
            }
            else
            {
                return watchers.CurrentGameMode switch
                {
                    GameMode.EggShuttle => settings.StartEggShuttle && watchers.LevelID.Current == LevelID.TropicalResortAct1 && (watchers.LevelID.Old == LevelID.None || (watchers.IGT.Old > TimeSpan.Zero && watchers.IGT.Current == TimeSpan.Zero)),
                    _ => (settings.StartAny && watchers.TR1Rank.Current == -1 && watchers.RunStart.Old == CleanSave && watchers.RunStart.Current == SavedFile)
                         || (settings.StartSonicSimulator && watchers.LevelID.Current == LevelID.SonicSimulatorAct1_1 && watchers.LevelID.Old == LevelID.None),
                };
            }
        }

        private bool Split()
        {
            switch (watchers.CurrentGameMode)
            {
                case GameMode.EggShuttle:
                    if (watchers.EggShuttle_ProgressiveID.Old == watchers.EggShuttle_TotalStages.Current - 1)
                        return settings.TerminalVelocityAct2 && watchers.GoalRingReached.Current && !watchers.GoalRingReached.Old;
                    else return CheckSplit(watchers.LevelID.Old) && watchers.EggShuttle_ProgressiveID.Current == watchers.EggShuttle_ProgressiveID.Old + 1;
                default:
                    if (watchers.LevelID.Old == LevelID.TerminalVelocityAct2)
                        return settings.TerminalVelocityAct2 && watchers.GoalRingReached.Current && !watchers.GoalRingReached.Old;
                    else
                        return CheckSplit(watchers.LevelID.Old) && !watchers.GoalRingReached.Current && watchers.GoalRingReached.Old && watchers.LevelID.Current != watchers.LevelID.Old;
            }
        }

        bool Reset() => watchers.CurrentGameMode switch
        {
            GameMode.EggShuttle => settings.ResetEggShuttle && watchers.IGT.Old != TimeSpan.Zero && watchers.IGT.Current == TimeSpan.Zero && !watchers.GoalRingReached.Old,
            _ => settings.ResetAny && watchers.RunStart.Old == SavedFile && watchers.RunStart.Current == CleanSave,
        };

        bool IsLoading() => true;

        private TimeSpan? GameTime() => watchers.AccumulatedIGT + watchers.IGT.Current;

        private const byte CleanSave = 35;
        private const byte SavedFile = 110;

        private bool CheckSplit(LevelID id) => id switch
        {
            LevelID.TropicalResortAct1 => settings.TropicalResortAct1,
            LevelID.TropicalResortAct2 => settings.TropicalResortAct2,
            LevelID.TropicalResortAct3 => settings.TropicalResortAct3,
            LevelID.TropicalResortAct4 => settings.TropicalResortAct4,
            LevelID.TropicalResortAct5 => settings.TropicalResortAct5,
            LevelID.TropicalResortAct6 => settings.TropicalResortAct6,
            LevelID.TropicalResortBoss => settings.TropicalResortBoss,
            LevelID.SweetMountainAct1 => settings.SweetMountainAct1,
            LevelID.SweetMountainAct2 => settings.SweetMountainAct2,
            LevelID.SweetMountainAct3 => settings.SweetMountainAct3,
            LevelID.SweetMountainAct4 => settings.SweetMountainAct4,
            LevelID.SweetMountainAct5 => settings.SweetMountainAct5,
            LevelID.SweetMountainAct6 => settings.SweetMountainAct6,
            LevelID.SweetMountainBoss => settings.SweetMountainBoss,
            LevelID.StarlightCarnivalAct1 => settings.StarlightCarnivalAct1,
            LevelID.StarlightCarnivalAct2 => settings.StarlightCarnivalAct2,
            LevelID.StarlightCarnivalAct3 => settings.StarlightCarnivalAct3,
            LevelID.StarlightCarnivalAct4 => settings.StarlightCarnivalAct4,
            LevelID.StarlightCarnivalAct5 => settings.StarlightCarnivalAct5,
            LevelID.StarlightCarnivalAct6 => settings.StarlightCarnivalAct6,
            LevelID.StarlightCarnivalBoss => settings.StarlightCarnivalBoss,
            LevelID.PlanetWispAct1 => settings.PlanetWispAct1,
            LevelID.PlanetWispAct2 => settings.PlanetWispAct2,
            LevelID.PlanetWispAct3 => settings.PlanetWispAct3,
            LevelID.PlanetWispAct4 => settings.PlanetWispAct4,
            LevelID.PlanetWispAct5 => settings.PlanetWispAct5,
            LevelID.PlanetWispAct6 => settings.PlanetWispAct6,
            LevelID.PlanetWispBoss => settings.PlanetWispBoss,
            LevelID.AquariumParkAct1 => settings.AquariumParkAct1,
            LevelID.AquariumParkAct2 => settings.AquariumParkAct2,
            LevelID.AquariumParkAct3 => settings.AquariumParkAct3,
            LevelID.AquariumParkAct4 => settings.AquariumParkAct4,
            LevelID.AquariumParkAct5 => settings.AquariumParkAct5,
            LevelID.AquariumParkAct6 => settings.AquariumParkAct6,
            LevelID.AquariumParkBoss => settings.AquariumParkBoss,
            LevelID.AsteroidCoasterAct1 => settings.AsteroidCoasterAct1,
            LevelID.AsteroidCoasterAct2 => settings.AsteroidCoasterAct2,
            LevelID.AsteroidCoasterAct3 => settings.AsteroidCoasterAct3,
            LevelID.AsteroidCoasterAct4 => settings.AsteroidCoasterAct4,
            LevelID.AsteroidCoasterAct5 => settings.AsteroidCoasterAct5,
            LevelID.AsteroidCoasterAct6 => settings.AsteroidCoasterAct6,
            LevelID.AsteroidCoasterBoss => settings.AsteroidCoasterBoss,
            LevelID.TerminalVelocityAct1 => settings.TerminalVelocityAct1,
            LevelID.TerminalVelocityBoss => settings.TerminalVelocityBoss,
            LevelID.TerminalVelocityAct2 => settings.TerminalVelocityAct2,
            LevelID.SonicSimulatorAct1_1 => settings.SonicSim1_1,
            LevelID.SonicSimulatorAct1_2 => settings.SonicSim1_2,
            LevelID.SonicSimulatorAct1_3 => settings.SonicSim1_3,
            LevelID.SonicSimulatorAct2_1 => settings.SonicSim2_1,
            LevelID.SonicSimulatorAct2_2 => settings.SonicSim2_2,
            LevelID.SonicSimulatorAct2_3 => settings.SonicSim2_3,
            LevelID.SonicSimulatorAct3_1 => settings.SonicSim3_1,
            LevelID.SonicSimulatorAct3_2 => settings.SonicSim3_2,
            LevelID.SonicSimulatorAct3_3 => settings.SonicSim3_3,
            LevelID.SonicSimulatorAct4_1 => settings.SonicSim4_1,
            LevelID.SonicSimulatorAct4_2 => settings.SonicSim4_2,
            LevelID.SonicSimulatorAct4_3 => settings.SonicSim4_3,
            LevelID.SonicSimulatorAct5_1 => settings.SonicSim5_1,
            LevelID.SonicSimulatorAct5_2 => settings.SonicSim5_2,
            LevelID.SonicSimulatorAct5_3 => settings.SonicSim5_3,
            LevelID.SonicSimulatorAct6_1 => settings.SonicSim6_1,
            LevelID.SonicSimulatorAct6_2 => settings.SonicSim6_2,
            LevelID.SonicSimulatorAct6_3 => settings.SonicSim6_3,
            LevelID.SonicSimulatorAct7_1 => settings.SonicSim7_1,
            LevelID.SonicSimulatorAct7_2 => settings.SonicSim7_2,
            LevelID.SonicSimulatorAct7_3 => settings.SonicSim7_3,
            _ => false
        };
    }
}
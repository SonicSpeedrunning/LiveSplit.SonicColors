using System;
using System.Threading;

namespace LiveSplit.SonicColors
{
    class SplittingLogic
    {
        private Watchers watchers;

        public delegate bool TimerCheckEventHandler(object sender, EventArgs e);
        public event TimerCheckEventHandler OnTimerCheck;
        public event EventHandler OnStartTrigger;
        public event EventHandler<double> OnGameTimeTrigger;
        public event EventHandler OnResetTrigger;
        public event EventHandler<string> OnSplitTrigger;

        public void Update()
        {
            if (!VerifyOrHookGameProcess()) return;
            watchers.Update();
            if (TimerNotRunning())
            {
                watchers.AccumulatedIGT = 0;
                watchers.IsEggShuttle = watchers.EggShuttle_TotalStages.Current > 0;
            }
            if (watchers.IGThasReset) watchers.AccumulatedIGT += watchers.IGT.Old;
            Start();
            GameTime();
            ResetLogic();
            SplitLogic();
        }

        void Start()
        {
            bool startTrigger = false;
            switch (watchers.IsEggShuttle)
            {
                case true:
                    startTrigger = watchers.LevelID.Current == Levels.TropicalResortAct1 && watchers.LevelID.Old == Levels.None;
                    break;
                case false:
                    startTrigger = (watchers.RunStart.Old == 35 && watchers.RunStart.Old == 110 && watchers.TR1rank.Current == -1) ||
                                   (watchers.LevelID.Current == Levels.SonicSimulator1_1 && watchers.LevelID.Old == Levels.None);
                    break;
            }
            if (startTrigger) this.OnStartTrigger?.Invoke(this, EventArgs.Empty);
        }

        void GameTime()
        {
            this.OnGameTimeTrigger?.Invoke(this, watchers.AccumulatedIGT + watchers.IGT.Current);
        }

        void ResetLogic()
        {
            bool resetTrigger = false;
            switch (watchers.IsEggShuttle)
            {
                case true:
                    resetTrigger = watchers.IGT.Old != 0 && watchers.IGT.Current == 0 && !watchers.GoalRingReached.Old;
                    break;
                case false:
                    resetTrigger = watchers.RunStart.Old == 110 && watchers.RunStart.Current == 35;
                    break;
            }
            if (resetTrigger) this.OnResetTrigger?.Invoke(this, EventArgs.Empty);
        }

        void SplitLogic()
        {
            bool splitTrigger = false;
            switch (watchers.IsEggShuttle)
            {
                case true:
                    bool isLastLevel = watchers.EggShuttle_ProgressiveID.Old == watchers.EggShuttle_TotalStages.Current - 1;
                    switch (isLastLevel)
                    {
                        case false:
                            splitTrigger = watchers.EggShuttle_ProgressiveID.Current == watchers.EggShuttle_ProgressiveID.Old + 1;
                            break;
                        case true:
                            splitTrigger = watchers.GoalRingReached.Current && !watchers.GoalRingReached.Old;
                            break;
                    }
                    break;
                case false:
                    switch (watchers.LevelID.Old)
                    {
                        case Levels.TerminalVelocityAct2:
                            splitTrigger = watchers.GoalRingReached.Current && !watchers.GoalRingReached.Old;
                            break;
                        default:
                            splitTrigger = !watchers.GoalRingReached.Current && watchers.GoalRingReached.Old;
                            break;
                    }
                    break;
            }
            if (splitTrigger) this.OnSplitTrigger?.Invoke(this, watchers.LevelID.Old);
        }

        bool TimerNotRunning()
        {
            return this.OnTimerCheck.Invoke(this, EventArgs.Empty);
        }

        bool VerifyOrHookGameProcess()
        {
            if (watchers != null && watchers.IsGameHooked) return true;
            try { watchers = new Watchers(); } catch { Thread.Sleep(500); return false; }
            return true;
        }
    }
}
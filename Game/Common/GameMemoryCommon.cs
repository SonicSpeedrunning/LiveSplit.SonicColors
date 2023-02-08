using LiveSplit.ComponentUtil;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LiveSplit.SonicColors
{
    partial class Watchers
    {
        /// <summary>
        /// The local ProcessHook instance.
        /// </summary>
        private readonly ProcessHook GameProcess;

        /// <summary>
        /// The currently hooked Process instance.
        /// </summary>
        private Process game => GameProcess.Game;

        /// <summary>
        /// A private MemoryWatcherList that will be used to run actions that are common to all watchers defined in the current class, eg. UpdateAll().
        /// </summary>
        private MemoryWatcherList WatcherList { get; set; }

        /// <summary>
        /// Actions to perform after hooking the target process (eg. getting addresses, sigscanning, etc.).
        /// </summary>
        /// <returns>True if the target process is hooked and the actions complete without errors, otherwise false</returns>
        public bool Init()
        {
            // This "init" function checks if the autosplitter has connected to the game
            // (if it has not, there's no point in going further) and starts a Task to
            // get the needed memory addresses for the other methods.
            if (!GameProcess.IsGameHooked)
                return false;

            // The purpose of this task is to limit the update cycle to 1 every 1.5 seconds
            // (instead of the usual one every 16 msec) in order to avoid wasting resources
            if (GameProcess.InitStatus == ProcessHook.GameInitStatus.NotStarted)
                Task.Run(() =>
                {
                    GameProcess.InitStatus = ProcessHook.GameInitStatus.InProgress;
                    try
                    {
                        GetAddresses();
                        GameProcess.InitStatus = ProcessHook.GameInitStatus.Completed;
                    }
                    catch
                    {
                        Task.Delay(1500).Wait();
                        GameProcess.InitStatus = ProcessHook.GameInitStatus.NotStarted;
                    }
                    // I'm running this manually because the signature scanner, especially
                    // if it runs several times, can take A LOT of memory, to the point of
                    // filling your RAM with several GB of useless data that doesn't get
                    // collected for some reason.
                    GC.Collect();
                });

            // At this point, if init has not been completed yet, return
            // false to avoid running the rest of the splitting logic.
            return GameProcess.InitStatus == ProcessHook.GameInitStatus.Completed;
        }

        /// <summary>
        /// Clears the resources used by the Watchers object.
        /// Remember to always run Dispose() when the component is unloaded in order to cancel the internal task run by ProcessHook
        /// </summary>
        public void Dispose()
        {
            GameProcess?.Dispose();
            emu_help?.Dispose();
        }
    }
}

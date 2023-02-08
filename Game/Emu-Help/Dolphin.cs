using System;
using System.Linq;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicColors
{
    public partial class Wii
    {
        private Tuple<IntPtr, IntPtr, Func<bool>> Dolphin()
        {
            var pages = game.MemoryPages(true);

            IntPtr MEM1 = pages.First(p => p.Type == MemPageType.MEM_MAPPED && p.State == MemPageState.MEM_COMMIT && (int)p.RegionSize == 0x2000000).BaseAddress;
            IntPtr MEM2 = pages.First(p => p.Type == MemPageType.MEM_MAPPED && p.State == MemPageState.MEM_COMMIT && (int)p.RegionSize == 0x4000000).BaseAddress;

            bool checkIfAlive() => game.ReadBytes(MEM1, 1, out _) && game.ReadBytes(MEM2, 1, out _);

            Endianess = Endianess.BigEndian;

            return Tuple.Create(MEM1, MEM2, checkIfAlive);
        }
    }
}
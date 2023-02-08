using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicColors
{
    /// <summary>
    /// Tools for dealing with Endianess
    /// </summary>
    public static class ToLittleEndian
    {
        public static short SwapEndianess(this short value) => BitConverter.ToInt16(BitConverter.GetBytes((short)value).Reverse().ToArray(), 0);
        public static ushort SwapEndianess(this ushort value) => BitConverter.ToUInt16(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static int SwapEndianess(this int value) => BitConverter.ToInt32(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static uint SwapEndianess(this uint value) => BitConverter.ToUInt32(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static long SwapEndianess(this long value) => BitConverter.ToInt64(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static ulong SwapEndianess(this ulong value) => BitConverter.ToUInt64(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static IntPtr SwapEndianess(this IntPtr value) => (IntPtr)BitConverter.ToInt64(BitConverter.GetBytes((long)value).Reverse().ToArray(), 0);
        public static UIntPtr SwapEndianess(this UIntPtr value) => (UIntPtr)BitConverter.ToUInt64(BitConverter.GetBytes((ulong)value).Reverse().ToArray(), 0);
        public static float SwapEndianess(this float value) => BitConverter.ToSingle(BitConverter.GetBytes(value).Reverse().ToArray(), 0);
        public static double SwapEndianess(this double value) => BitConverter.ToDouble(BitConverter.GetBytes(value).Reverse().ToArray(), 0);


        /// <summary>
        /// Creates a new FakeMemoryWatcherList from an existing MemoryWatcherList, with each element having it's Current and Old properties with switched endianess.
        /// </summary>
        /// <param name="Watchers">A MemoryWatcherList with elements we want to convert from Big Endian to Little Endian (or vice versa).</param>
        /// <returns></returns>
        public static FakeMemoryWatcherList SetFakeWatchers(MemoryWatcherList Watchers)
        {
            var list = new FakeMemoryWatcherList();

            foreach (var entry in Watchers)
            {
                switch (entry)
                {
                    case MemoryWatcher<byte>: list.Add(new FakeMemoryWatcher<byte>(() => entry.Current == null ? default : (byte)entry.Current) { Name = entry.Name }); break;
                    case MemoryWatcher<sbyte>: list.Add(new FakeMemoryWatcher<sbyte>(() => entry.Current == null ? default : (sbyte)entry.Current) { Name = entry.Name }); break;
                    case MemoryWatcher<bool>: list.Add(new FakeMemoryWatcher<bool>(() => entry.Current == null ? default : (bool)entry.Current) { Name = entry.Name }); break;
                    case MemoryWatcher<short>: list.Add(new FakeMemoryWatcher<short>(() => entry.Current == null ? default : ((short)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<ushort>: list.Add(new FakeMemoryWatcher<ushort>(() => entry.Current == null ? default : ((ushort)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<int>: list.Add(new FakeMemoryWatcher<int>(() => entry.Current == null ? default : ((int)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<uint>: list.Add(new FakeMemoryWatcher<uint>(() => entry.Current == null ? default : ((uint)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<long>: list.Add(new FakeMemoryWatcher<long>(() => entry.Current == null ? default : ((long)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<ulong>: list.Add(new FakeMemoryWatcher<ulong>(() => entry.Current == null ? default : ((ulong)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<IntPtr>:list.Add(new FakeMemoryWatcher<IntPtr>(() => entry.Current == null ? default : ((IntPtr)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<UIntPtr>: list.Add(new FakeMemoryWatcher<UIntPtr>(() => entry.Current == null ? default : ((UIntPtr)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<float>: list.Add(new FakeMemoryWatcher<float>(() => entry.Current == null ? default : ((float)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<double>: list.Add(new FakeMemoryWatcher<double>(() => entry.Current == null ? default : ((double)entry.Current).SwapEndianess()) { Name = entry.Name }); break;
                    case MemoryWatcher<char>: list.Add(new FakeMemoryWatcher<char>(() => entry.Current == null ? default : (char)entry.Current) { Name = entry.Name }); break;
                    case StringWatcher: list.Add(new FakeMemoryWatcher<string>(() => entry.Current == null ? default : (string)entry.Current) { Name = entry.Name }); break;
                }
            }

            return list;
        }

        public static T GetProperty<T>(this object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = obj.GetType().GetProperty(name, bindingFlags);
            return (T)field?.GetValue(obj, null);
        }

        public static T GetField<T>(this object obj, string name)
        {
            // Set the flags so that private and public fields from instances will be found
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = obj.GetType().GetField(name, bindingFlags);
            return (T)field?.GetValue(obj);
        }
    }

    public enum Endianess
    {
        LittleEndian,
        BigEndian
    }
}


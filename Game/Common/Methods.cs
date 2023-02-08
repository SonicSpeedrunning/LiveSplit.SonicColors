using System;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace LiveSplit.SonicColors
{
    /// <summary>
    /// Custom extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Perform a signature scan, similarly to how it would achieve with SignatureScanner.Scan()
        /// </summary>
        /// <returns>Address of the signature, if found. Otherwise, an Exception will be thrown.</returns>
        public static IntPtr ScanOrThrow(this SignatureScanner scanner, SigScanTarget target, int align = 1)
        {
            IntPtr tempAddr = scanner.Scan(target, align);
            tempAddr.ThrowIfZero();
            return tempAddr;
        }

        /// <summary>
        /// Checks whether a provided IntPtr is equal to IntPtr.Zero. If it is, an Exception will be thrown.
        /// </summary>
        /// <param name="ptr"></param>
        /// <exception cref="SigscanFailedException"></exception>
        public static void ThrowIfZero(this IntPtr ptr)
        {
            if (ptr.IsZero())
                throw new Exception();
        }

        /// <summary>
        /// Checks whether a specific bit inside a byte value is set or not.
        /// </summary>
        /// <param name="value">The byte value in which to perform the check</param>
        /// <param name="bitPos">The bit position (from 0 to 7).</param>
        /// <returns>True if the bit is set, otherwise false.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Will be thrown if a bit was specified in a position outside the allowed interval.</exception>
        public static bool BitCheck(this byte value, byte bitPos)
        {
            if (bitPos < 0 || bitPos > 7)
                throw new ArgumentOutOfRangeException();
            return (value & (1 << bitPos)) != 0;
        }

        /// <summary>
        /// Checks if a provided IntPtr value is equal to IntPtr.Zero
        /// </summary>
        /// <param name="value"></param>
        /// <returns>True if the value is IntPtr.Zero, false otherwise.</returns>
        public static bool IsZero(this IntPtr value)
        {
            return value == IntPtr.Zero;
        }

        /// <summary>
        /// Quickly creates a new SignatureScanner object. If no argument is specified, sigscanning will be performed in the MainModule memory space.
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static SignatureScanner SigScanner(this Process process)
        {
            return new SignatureScanner(process, process.MainModuleWow64Safe().BaseAddress, process.MainModuleWow64Safe().ModuleMemorySize);
        }
    }
}
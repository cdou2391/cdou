using System;
using System.Runtime.InteropServices;

namespace Music_Player
{
    class NativeMethods
    {
        [DllImport("winmm.dll")]
        internal static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
        [DllImport("winmm.dll")]
        internal static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    }
}

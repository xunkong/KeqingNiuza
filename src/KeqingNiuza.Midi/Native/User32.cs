using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using KeqingNiuza.Midi.Native;

namespace KeqingNiuza.Midi.Native
{
    internal static class User32
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, Msg Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, Msg Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SendMessage(IntPtr hWnd, Msg Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SendMessage(IntPtr hWnd, Msg Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fUnknown);

        /// <returns>0 means noerror, others is error code</returns>
        [DllImport("winmm.dll")]
        internal static extern uint timeBeginPeriod(uint uPeriod);

        /// <returns>0 means noerror, others is error code</returns>
        [DllImport("winmm.dll")]
        internal static extern uint timeEndPeriod(uint uPeriod);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    }
}

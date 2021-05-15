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
        internal static extern bool PostMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool PostMessage(IntPtr hWnd, WindowMessage Msg, uint wParam, uint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SendMessage(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SendMessage(IntPtr hWnd, WindowMessage Msg, uint wParam, uint lParam);


        /// <returns>0 means noerror, others is error code</returns>
        [DllImport("winmm.dll")]
        internal static extern uint timeBeginPeriod(uint uPeriod);

        /// <returns>0 means noerror, others is error code</returns>
        [DllImport("winmm.dll")]
        internal static extern uint timeEndPeriod(uint uPeriod);

    }
}

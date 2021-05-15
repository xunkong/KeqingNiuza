using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static KeqingNiuza.Midi.Native.User32;
using static KeqingNiuza.Midi.Native.WindowMessage;
using static KeqingNiuza.Midi.Native.VirtualKey;
using NAudio.Midi;
using KeqingNiuza.Midi.Native;

namespace KeqingNiuza.Midi
{
    public static class Util
    {
        internal static byte GetCharAsciiCode(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("Cannot get ascii value of null");
            }
            return Encoding.ASCII.GetBytes(key).First();
        }

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }


        public static void RestartAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Process.GetCurrentProcess().ProcessName;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
                Environment.Exit(0);
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                throw ex;
            }
        }


        internal static double GetTicksPerBeat(int ticksPerQuarterNote, TimeSignatureEvent timeSignature)
        {
            var ticksPerBeat = ticksPerQuarterNote * 4 / (1 << timeSignature.Denominator);
            return ticksPerBeat;
        }

        /// <summary>
        /// Note到按键的映射
        /// </summary>
        /// <param name="noteNumber">48~83</param>
        /// <returns></returns>
        internal static VirtualKey NoteNumberToVisualKey(int noteNumber)
        {
            return Const.NoteToVisualKeyDictionary[noteNumber];
        }

        internal static void Postkey(IntPtr hWnd, int noteNumber)
        {
            // todo not finish
            PostMessage(hWnd, WM_ACTIVATE, 2, 0);
            PostMessage(hWnd, WM_KEYDOWN, (uint)NoteNumberToVisualKey(noteNumber), 0x1e0001);
            PostMessage(hWnd, WM_CHAR, (uint)NoteNumberToVisualKey(noteNumber), 0x1e0001);
            PostMessage(hWnd, WM_KEYUP, (uint)NoteNumberToVisualKey(noteNumber), 0xc01e0001);
        }

    }
}

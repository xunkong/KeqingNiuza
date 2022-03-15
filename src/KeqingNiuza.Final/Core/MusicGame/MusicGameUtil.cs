using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static KeqingNiuza.Core.Native.FsModifier;
using static KeqingNiuza.Core.Native.VirtualKey;
using KeqingNiuza.Core.Native;
using static KeqingNiuza.Core.Native.User32;
using static KeqingNiuza.Core.Native.Msg;

namespace KeqingNiuza.Core.MusicGame
{
    public static class MusicGameUtil
    {

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }


        public static void RestartAsAdmin()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = Process.GetCurrentProcess().ProcessName,
                Verb = "runas"
            };
            Process.Start(startInfo);
            Environment.Exit(0);
        }


        public static bool RegisterHotKey(IntPtr hWnd)
        {
            if (!User32.RegisterHotKey(hWnd, 1003, (uint)(MOD_CONTROL | MOD_NOREPEAT), (uint)VK_H))
            {
                return false;
            }
            if (!User32.RegisterHotKey(hWnd, 1004, (uint)(MOD_CONTROL | MOD_NOREPEAT), (uint)VK_LEFT))
            {
                return false;
            }
            if (!User32.RegisterHotKey(hWnd, 1005, (uint)(MOD_CONTROL | MOD_NOREPEAT), (uint)VK_RIGHT))
            {
                return false;
            }
            if (!User32.RegisterHotKey(hWnd, 1006, (uint)(MOD_CONTROL | MOD_NOREPEAT), (uint)VK_R))
            {
                return false;
            }
            if (!User32.RegisterHotKey(hWnd, 1007, (uint)(MOD_CONTROL | MOD_NOREPEAT), (uint)VK_T))
            {
                return false;
            }
            return true;
        }


        public static bool UnregisterHotKey(IntPtr hWnd)
        {
            if (!User32.UnregisterHotKey(hWnd, 1003))
            {
                return false;
            }
            if (!User32.UnregisterHotKey(hWnd, 1004))
            {
                return false;
            }
            if (!User32.UnregisterHotKey(hWnd, 1005))
            {
                return false;
            }
            if (!User32.UnregisterHotKey(hWnd, 1006))
            {
                return false;
            }
            if (!User32.UnregisterHotKey(hWnd, 1007))
            {
                return false;
            }
            return true;
        }



        public static void PostKey(IntPtr hWnd, ButtonType button, OperationType operation, bool fRepeat, bool allowBackground = false)
        {
            if (allowBackground)
            {
                PostMessage(hWnd, WM_ACTIVATE, 1, 0);
            }
            switch (operation)
            {
                case OperationType.None:
                    return;
                case OperationType.KeyDown:
                    PostMessage(hWnd, WM_KEYDOWN, (uint)button, (uint)(fRepeat ? 0x401E0001 : 0x001E0001));
                    PostMessage(hWnd, WM_CHAR, (uint)button, (uint)(fRepeat ? 0x401E0001 : 0x001E0001));
                    break;
                case OperationType.KeyUp:
                    PostMessage(hWnd, WM_IME_KEYUP, (uint)button, 0xC01E0001);
                    break;
                case OperationType.KeyDownUp:
                    PostMessage(hWnd, WM_KEYDOWN, (uint)button, 0x001E0001);
                    PostMessage(hWnd, WM_CHAR, (uint)button, 0x001E0001);
                    PostMessage(hWnd, WM_IME_KEYUP, (uint)button, 0xC01E0001);
                    break;
            }

        }

    }
}

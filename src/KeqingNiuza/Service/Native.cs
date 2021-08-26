using System;
using System.Runtime.InteropServices;

namespace KeqingNiuza.Service
{
    static class Native
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();


        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int index);


        public static bool IsWindowBeyondBounds(double width, double heigh)
        {
            IntPtr p = GetDesktopWindow();
            IntPtr hdc = GetDC(IntPtr.Zero);
            // 屏幕横向每英寸像素数
            int dpiX = GetDeviceCaps(hdc, 88);
            // 屏幕纵向每英寸像素数
            int dpiY = GetDeviceCaps(hdc, 90);
            _ = GetWindowRect(p, out RECT lpRect);
            var windowX = lpRect.Width * 96 / dpiX;
            var windowY = lpRect.Height * 96 / dpiY;
            if (width > windowX - 60 || heigh > windowY - 60)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    [StructLayout(LayoutKind.Sequential)]
    struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public int X
        {
            get { return Left; }
            set { Right -= Left - value; Left = value; }
        }

        public int Y
        {
            get { return Top; }
            set { Bottom -= Top - value; Top = value; }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

    }
}

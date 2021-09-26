using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Math;
using GDI = System.Drawing;

namespace KeqingNiuza.Launcher
{
    /// <summary>
    /// KeqingNiuzaProgressLoading.xaml 的交互逻辑
    /// </summary>
    public partial class KeqingNiuzaProgressLoading : UserControl
    {
        public KeqingNiuzaProgressLoading()
        {
            InitializeComponent();
            stopwatch = Stopwatch.StartNew();
        }

        private WriteableBitmap _bitmap;

        private Stopwatch stopwatch;

        private long lastElapsed;

        private long nextElapsed;

        private long nextSpan;

        private float rSpeed = 2 * (float)PI / 1000;

        private float indicatorLenHalf;

        private float progressLenHalf;

        private GDI.Pen pen = Pens.White;

        private GDI.Brush brushFore = GDI.Brushes.White;

        private GDI.Brush brushBack = new GDI.SolidBrush(GDI.Color.FromArgb(0x80, 0, 0, 0));

        private AnimationState currentAnimation;

        private AnimationState nextAnimation;

        private float lastProgresValue;

        private float progressValue;

        public float ProgressValue
        {
            get => progressValue;
            set => progressValue = Clamp(value, 0, 1);
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        private void UserControl_Loaded(object sender, EventArgs e)
        {
            using (var g = Graphics.FromHwnd(IntPtr.Zero))
            {
                var width = Width * g.DpiX / 96;
                var height = Height * g.DpiY / 96;
                _bitmap = new WriteableBitmap((int)width, (int)height, g.DpiX, g.DpiY, PixelFormats.Pbgra32, null);
                _image.Source = _bitmap;
                CompositionTarget.Rendering += CompositionTarget_Rendering;
                indicatorLenHalf = 4 * g.DpiX / 96;
                progressLenHalf = 120 * g.DpiX / 96;
            }

        }


        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (currentAnimation == AnimationState.None)
            {
                return;
            }
            _bitmap.Lock();
            var t = stopwatch.ElapsedMilliseconds;
            switch (currentAnimation)
            {
                case AnimationState.IndicatorAppear:
                    DrawIndicatorAppearAnimation(t);
                    break;
                case AnimationState.IndicatorBlink:
                    DrawIndicatorBlinkAnimation(t);
                    break;
                case AnimationState.ProgressExpand:
                    DrawProgressExpandAnimation(t);
                    break;
                case AnimationState.ProgressShow:
                    DrawProgressShowAnimation(ProgressValue);
                    break;
                case AnimationState.ProgressClose:
                    DrawProgressCloseAnimation(t);
                    break;
                case AnimationState.IndicatorDisappear:
                    DrawIndicatorDisappearAnimation(t);
                    break;
            }

            if (t > nextElapsed)
            {
                if (currentAnimation == AnimationState.IndicatorAppear
                    || currentAnimation == AnimationState.IndicatorDisappear
                    || currentAnimation == AnimationState.ProgressExpand
                    || currentAnimation == AnimationState.ProgressClose)
                {
                    currentAnimation = nextAnimation;
                    nextAnimation = AnimationState.None;
                    t = stopwatch.ElapsedMilliseconds;
                    lastElapsed = t;
                    nextElapsed = t + nextSpan;
                }
            }

            _bitmap.AddDirtyRect(new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight));
            _bitmap.Unlock();
        }


        public async void SetAnimationState(AnimationState state, long spanTime = 400)
        {
            var t = stopwatch.ElapsedMilliseconds;
            if (currentAnimation == AnimationState.None
                || currentAnimation == AnimationState.IndicatorBlink
                || currentAnimation == AnimationState.ProgressShow)
            {
                currentAnimation = state;
                lastElapsed = t;
                nextElapsed = t + spanTime;
            }
            else
            {
                if (nextAnimation != AnimationState.None)
                {
                    await Task.Delay(Convert.ToInt32(nextElapsed - t + spanTime / 2));
                }
                nextAnimation = state;
                nextSpan = spanTime;
            }
        }




        private void DrawIndicatorAppearAnimation(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;

                    var Ox = width / 2;
                    var Oy = height / 2;
                    var ilen = indicatorLenHalf;
                    var t0 = lastElapsed;
                    var t1 = nextElapsed;

                    if (t <= t0)
                    {
                        g.Clear(GDI.Color.Transparent);
                    }
                    else if (t <= t1)
                    {
                        var dy = 2 * ilen * (t1 - t) / (t1 - t0);
                        var a = 255 * Pow((float)(t - t0) / (t1 - t0), 3);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy-dy),
                            new PointF(Ox,      Oy-dy+ilen),
                            new PointF(Ox+ilen, Oy-dy),
                            new PointF(Ox,      Oy-dy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 中菱形
                        g.FillPolygon(new SolidBrush(GDI.Color.FromArgb((int)a, 0xFF, 0xFF, 0xFF)), points1);
                    }
                    else
                    {
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy),
                            new PointF(Ox,      Oy+ilen),
                            new PointF(Ox+ilen, Oy),
                            new PointF(Ox,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                    }
                    g.Flush();
                }
            }
        }


        private void DrawIndicatorDisappearAnimation(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;

                    var Ox = width / 2;
                    var Oy = height / 2;
                    var ilen = indicatorLenHalf;
                    var t0 = lastElapsed;
                    var t1 = nextElapsed;

                    if (t <= t0)
                    {
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy),
                            new PointF(Ox,      Oy+ilen),
                            new PointF(Ox+ilen, Oy),
                            new PointF(Ox,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 中菱形
                        g.FillPolygon(brushFore, points1);
                    }
                    else if (t <= t1)
                    {
                        var dy = 2 * ilen * (t - t0) / (t1 - t0);
                        var a = 255 * Pow((float)(t1 - t) / (t1 - t0), 3);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy-dy),
                            new PointF(Ox,      Oy-dy+ilen),
                            new PointF(Ox+ilen, Oy-dy),
                            new PointF(Ox,      Oy-dy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 中菱形
                        g.FillPolygon(new SolidBrush(GDI.Color.FromArgb((int)a, 0xFF, 0xFF, 0xFF)), points1);
                    }
                    else
                    {
                        g.Clear(GDI.Color.Transparent);
                    }
                    g.Flush();
                }
            }
        }


        private void DrawIndicatorBlinkAnimation(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;

                    var Ox = width / 2;
                    var Oy = height / 2;
                    var ilen = indicatorLenHalf;
                    var rho = PI * t / 1000;
                    var a = 8 * (0.5 + 0.5 * Cos(rho));
                    Debug.WriteLine(a);
                    PointF[] points1 = new PointF[]
                    {
                        new PointF(Ox-ilen, Oy),
                        new PointF(Ox,      Oy+ilen),
                        new PointF(Ox+ilen, Oy),
                        new PointF(Ox,      Oy-ilen),
                    };
                    g.Clear(GDI.Color.Transparent);
                    // 中菱形
                    g.FillPolygon(new SolidBrush(GDI.Color.FromArgb((int)a, 0xFF, 0xFF, 0xFF)), points1);
                }
            }
        }


        [Obsolete]
        private void DrawCircleLoading(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            var Ox = width / 2;
            var Oy = height / 2;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(GDI.Color.Transparent);

                    var ilen = indicatorLenHalf;
                    var angle = rSpeed * t;
                    var dx = 2 * ilen * (float)Cos(angle);
                    var dy = 2 * ilen * (float)Sin(angle);
                    PointF[] points1 = new PointF[]
                    {
                        new PointF(Ox+dx-ilen, Oy+dy),
                        new PointF(Ox+dx,Oy+dy+ilen),
                        new PointF(Ox+dx+ilen,Oy+dy),
                        new PointF(Ox+dx,Oy+dy-ilen),
                    };
                    PointF[] points2 = new PointF[]
                    {
                        new PointF(Ox-dx-ilen, Oy-dy),
                        new PointF(Ox-dx,     Oy-dy+ilen),
                        new PointF(Ox-dx+ilen, Oy-dy),
                        new PointF(Ox-dx,     Oy-dy-ilen),
                    };

                    // 左菱形
                    g.FillPolygon(brushFore, points1);
                    // 右菱形
                    g.FillPolygon(brushFore, points2);

                    g.Flush();
                }
            }
        }



        private void DrawProgressExpandAnimation(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;

                    var Ox = width / 2;
                    var Oy = height / 2;
                    var ilen = indicatorLenHalf;
                    var plen = progressLenHalf;
                    var oilen = 2 * ilen + plen;
                    var t0 = lastElapsed;
                    var t2 = nextElapsed;
                    var t1 = t0 + (t2 - t0) * 3 / 4;

                    if (t <= t0)
                    {
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy),
                            new PointF(Ox,      Oy+ilen),
                            new PointF(Ox+ilen, Oy),
                            new PointF(Ox,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 中菱形
                        g.FillPolygon(brushFore, points1);
                    }
                    else if (t <= t1)
                    {
                        var dx = oilen * (t - t0) / (t1 - t0);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox+dx-ilen, Oy),
                            new PointF(Ox+dx,      Oy+ilen),
                            new PointF(Ox+dx+ilen, Oy),
                            new PointF(Ox+dx,      Oy-ilen),
                        };
                        PointF[] points2 = new PointF[]
                        {
                            new PointF(Ox-dx-ilen, Oy),
                            new PointF(Ox-dx,      Oy+ilen),
                            new PointF(Ox-dx+ilen, Oy),
                            new PointF(Ox-dx,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                        // 右菱形
                        g.FillPolygon(brushFore, points2);

                    }
                    else if (t <= t2)
                    {
                        var dx = oilen;
                        var dy = ilen * (t - t1) / (t2 - t1);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox+dx-ilen, Oy),
                            new PointF(Ox+dx,      Oy+ilen),
                            new PointF(Ox+dx+ilen, Oy),
                            new PointF(Ox+dx,      Oy-ilen),
                        };
                        PointF[] points2 = new PointF[]
                        {
                            new PointF(Ox-dx-ilen, Oy),
                            new PointF(Ox-dx,      Oy+ilen),
                            new PointF(Ox-dx+ilen, Oy),
                            new PointF(Ox-dx,      Oy-ilen),
                        };
                        PointF[] points3 = new PointF[]
                        {
                            new PointF(Ox-plen,      Oy),
                            new PointF(Ox-plen+ilen, Oy-dy),
                            new PointF(Ox+plen-ilen, Oy-dy),
                            new PointF(Ox+plen,      Oy),
                            new PointF(Ox+plen-ilen, Oy+dy),
                            new PointF(Ox-plen+ilen, Oy+dy),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                        // 右菱形
                        g.FillPolygon(brushFore, points2);
                        // 进度条
                        g.FillPolygon(brushBack, points3);
                    }
                    else
                    {
                        var dx = oilen;
                        var dy = ilen;
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox+dx-ilen, Oy),
                            new PointF(Ox+dx,      Oy+ilen),
                            new PointF(Ox+dx+ilen, Oy),
                            new PointF(Ox+dx,      Oy-ilen),
                        };
                        PointF[] points2 = new PointF[]
                        {
                            new PointF(Ox-dx-ilen, Oy),
                            new PointF(Ox-dx,      Oy+ilen),
                            new PointF(Ox-dx+ilen, Oy),
                            new PointF(Ox-dx,      Oy-ilen),
                        };
                        PointF[] points3 = new PointF[]
                        {
                            new PointF(Ox-plen,      Oy),
                            new PointF(Ox-plen+ilen, Oy-dy),
                            new PointF(Ox+plen-ilen, Oy-dy),
                            new PointF(Ox+plen,      Oy),
                            new PointF(Ox+plen-ilen, Oy+dy),
                            new PointF(Ox-plen+ilen, Oy+dy),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                        // 右菱形
                        g.FillPolygon(brushFore, points2);
                        // 进度条
                        g.FillPolygon(brushBack, points3);
                    }
                    g.Flush();
                }
            }
        }



        private void DrawProgressShowAnimation(float realValue)
        {

            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            var Ox = width / 2;
            var Oy = height / 2;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;
                    g.Clear(GDI.Color.Transparent);

                    float value = 0;
                    if (realValue > lastProgresValue)
                    {
                        value = Clamp(lastProgresValue + (realValue - lastProgresValue) / 10, 0, realValue);
                    }
                    else
                    {
                        value = Clamp(lastProgresValue - (lastProgresValue - realValue) / 10, realValue, 1);
                    }
                    lastProgresValue = value;
                    var ilen = indicatorLenHalf;
                    var plen = progressLenHalf;
                    var oilen = 2 * ilen + plen;
                    var vlen = value * 2 * plen;

                    PointF[] points1 = new PointF[]
                    {
                        new PointF(Ox+oilen-ilen, Oy),
                        new PointF(Ox+oilen,Oy+ilen),
                        new PointF(Ox+oilen+ilen,Oy),
                        new PointF(Ox+oilen,Oy-ilen),
                    };
                    PointF[] points2 = new PointF[]
                    {
                        new PointF(Ox-oilen-ilen, Oy),
                        new PointF(Ox-oilen,     Oy+ilen),
                        new PointF(Ox-oilen+ilen, Oy),
                        new PointF(Ox-oilen,     Oy-ilen),
                    };
                    PointF[] points3 = new PointF[]
                    {
                        new PointF(Ox-plen, Oy),
                        new PointF(Ox-plen+ilen, Oy-ilen),
                        new PointF(Ox+plen-ilen, Oy-ilen),
                        new PointF(Ox+plen, Oy),
                        new PointF(Ox+plen-ilen, Oy+ilen),
                        new PointF(Ox-plen+ilen, Oy+ilen),
                    };

                    PointF[] points4;
                    if (vlen <= ilen)
                    {
                        points4 = new PointF[]
                        {
                            new PointF(Ox-plen,Oy),
                            new PointF(Ox-plen+vlen,Oy-vlen),
                            new PointF(Ox-plen+vlen,Oy+vlen),
                        };
                    }
                    else if (vlen <= 2 * plen - ilen)
                    {
                        points4 = new PointF[]
                        {
                            new PointF(Ox-plen,Oy),
                            new PointF(Ox-plen+ilen,Oy-ilen),
                            new PointF(Ox-plen+vlen,Oy-ilen),
                            new PointF(Ox-plen+vlen,Oy+ilen),
                            new PointF(Ox-plen+ilen,Oy+ilen),
                        };
                    }
                    else
                    {
                        points4 = new PointF[]
                        {
                            new PointF(Ox-plen,Oy),
                            new PointF(Ox-plen+ilen,Oy-ilen),
                            new PointF(Ox+plen-ilen,Oy-ilen),
                            new PointF(Ox-plen+vlen,Oy+2*plen-vlen),
                            new PointF(Ox-plen+vlen,Oy-2*plen+vlen),
                            new PointF(Ox+plen-ilen,Oy+ilen),
                            new PointF(Ox-plen+ilen,Oy+ilen),
                        };
                    }

                    // 左菱形
                    g.FillPolygon(brushFore, points1);
                    // 右菱形
                    g.FillPolygon(brushFore, points2);
                    // 进度条底色
                    g.FillPolygon(brushBack, points3);
                    // 进度条
                    g.FillPolygon(brushFore, points4);

                    g.Flush();
                }
            }
        }


        private void DrawProgressCloseAnimation(long t)
        {
            var width = _bitmap.PixelWidth;
            var height = _bitmap.PixelHeight;
            using (var i = new Bitmap(width, height, _bitmap.BackBufferStride, GDI.Imaging.PixelFormat.Format32bppArgb, _bitmap.BackBuffer))
            {
                using (var g = Graphics.FromImage(i))
                {
                    g.SmoothingMode = GDI.Drawing2D.SmoothingMode.AntiAlias;

                    var Ox = width / 2;
                    var Oy = height / 2;
                    var ilen = indicatorLenHalf;
                    var plen = progressLenHalf;
                    var oilen = 2 * ilen + plen;
                    var t0 = lastElapsed;
                    var t2 = nextElapsed;
                    var t1 = t0 + (t2 - t0) / 4;

                    if (t <= t0)
                    {
                        return;
                    }
                    else if (t <= t1)
                    {
                        var dx = oilen;
                        var dy = ilen * (t1 - t) / (t1 - t0);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox+dx-ilen, Oy),
                            new PointF(Ox+dx,      Oy+ilen),
                            new PointF(Ox+dx+ilen, Oy),
                            new PointF(Ox+dx,      Oy-ilen),
                        };
                        PointF[] points2 = new PointF[]
                        {
                            new PointF(Ox-dx-ilen, Oy),
                            new PointF(Ox-dx,      Oy+ilen),
                            new PointF(Ox-dx+ilen, Oy),
                            new PointF(Ox-dx,      Oy-ilen),
                        };
                        PointF[] points3 = new PointF[]
                        {
                            new PointF(Ox-plen,      Oy),
                            new PointF(Ox-plen+ilen, Oy-dy),
                            new PointF(Ox+plen-ilen, Oy-dy),
                            new PointF(Ox+plen,      Oy),
                            new PointF(Ox+plen-ilen, Oy+dy),
                            new PointF(Ox-plen+ilen, Oy+dy),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                        // 右菱形
                        g.FillPolygon(brushFore, points2);
                        // 进度条
                        g.FillPolygon(brushFore, points3);
                    }
                    else if (t <= t2)
                    {
                        var dx = oilen * (t2 - t) / (t2 - t1);
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox+dx-ilen, Oy),
                            new PointF(Ox+dx,      Oy+ilen),
                            new PointF(Ox+dx+ilen, Oy),
                            new PointF(Ox+dx,      Oy-ilen),
                        };
                        PointF[] points2 = new PointF[]
                        {
                            new PointF(Ox-dx-ilen, Oy),
                            new PointF(Ox-dx,      Oy+ilen),
                            new PointF(Ox-dx+ilen, Oy),
                            new PointF(Ox-dx,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 左菱形
                        g.FillPolygon(brushFore, points1);
                        // 右菱形
                        g.FillPolygon(brushFore, points2);
                    }
                    else
                    {
                        PointF[] points1 = new PointF[]
                        {
                            new PointF(Ox-ilen, Oy),
                            new PointF(Ox,      Oy+ilen),
                            new PointF(Ox+ilen, Oy),
                            new PointF(Ox,      Oy-ilen),
                        };
                        g.Clear(GDI.Color.Transparent);
                        // 中菱形
                        g.FillPolygon(brushFore, points1);
                    }
                    g.Flush();
                }
            }
        }


    }

    public enum AnimationState
    {
        None,

        IndicatorAppear,

        IndicatorBlink,

        ProgressExpand,

        ProgressShow,

        ProgressClose,

        IndicatorDisappear,

    }
}

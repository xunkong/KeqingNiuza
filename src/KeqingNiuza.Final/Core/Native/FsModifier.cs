using System;

namespace KeqingNiuza.Core.Native
{
    [Flags]
    enum FsModifier : uint
    {
        MOD_ALT = 0x0001,
        MOD_CONTROL = 0x0002,
        MOD_NOREPEAT = 0x4000,
        MOD_SHIFT = 0x0004,
        MOD_WIN = 0x0008
    }
}

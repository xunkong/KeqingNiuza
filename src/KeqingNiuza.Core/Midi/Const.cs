using System.Collections.Generic;
using KeqingNiuza.Core.Native;

namespace KeqingNiuza.Core.Midi
{
    static class Const
    {
        internal static Dictionary<int, VirtualKey> NoteToVisualKeyDictionary = new Dictionary<int, VirtualKey>
        {
            { 48,VirtualKey.VK_Z},
            { 50,VirtualKey.VK_X},
            { 52,VirtualKey.VK_C},
            { 53,VirtualKey.VK_V},
            { 55,VirtualKey.VK_B},
            { 57,VirtualKey.VK_N},
            { 59,VirtualKey.VK_M},

            { 60,VirtualKey.VK_A},
            { 62,VirtualKey.VK_S},
            { 64,VirtualKey.VK_D},
            { 65,VirtualKey.VK_F},
            { 67,VirtualKey.VK_G},
            { 69,VirtualKey.VK_H},
            { 71,VirtualKey.VK_J},

            { 72,VirtualKey.VK_Q},
            { 74,VirtualKey.VK_W},
            { 76,VirtualKey.VK_E},
            { 77,VirtualKey.VK_R},
            { 79,VirtualKey.VK_T},
            { 81,VirtualKey.VK_Y},
            { 83,VirtualKey.VK_U},

        };

        internal static Dictionary<int, string> NoteToCharDictionary = new Dictionary<int, string>
        {
            { 48,"z"},
            { 50,"x"},
            { 52,"c"},
            { 53,"v"},
            { 55,"b"},
            { 57,"n"},
            { 59,"m"},

            { 60,"a"},
            { 62,"s"},
            { 64,"d"},
            { 65,"f"},
            { 67,"g"},
            { 69,"h"},
            { 71,"j"},

            { 72,"q"},
            { 74,"w"},
            { 76,"e"},
            { 77,"r"},
            { 79,"t"},
            { 81,"y"},
            { 83,"u"},

        };
    }
}

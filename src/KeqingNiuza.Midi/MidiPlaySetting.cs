using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Midi
{
    public class MidiPlaySetting
    {
        public bool PlayBackground { get; set; }

        public bool AutoSwitchToGenshinWindow { get; set; }

        public double Speed { get; set; }

        public int NoteLevel { get; set; }
    }
}

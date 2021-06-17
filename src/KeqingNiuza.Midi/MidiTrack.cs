using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Midi
{
    public class MidiTrack
    {
        public TrackChunk Track { get; set; }

        public string Name { get; set; }

        public bool IsCheck { get; set; }

        public bool CanBeChecked => NoteNumber > 0;

        public int NoteNumber { get; set; }

        public int CanPlayNoteNumber { get; set; }

        public double CanPlayNoteRadio => (double)CanPlayNoteNumber / NoteNumber;

        public int MaxNoteLevel { get; set; }

        public int MinNoteLevel { get; set; }


        public MidiTrack(TrackChunk track)
        {
            Track = track;
            Name = track.Events.OfType<SequenceTrackNameEvent>().FirstOrDefault()?.Text;
            NoteNumber = track.Events.Count(x => x.EventType == MidiEventType.NoteOn);
            CanPlayNoteNumber = track.Events.Where(x => x.EventType == MidiEventType.NoteOn).Count(x => Const.NoteToVisualKeyDictionary.ContainsKey((x as NoteOnEvent).NoteNumber));
            if (CanBeChecked)
            {
                MaxNoteLevel = track.Events.Where(x => x.EventType == MidiEventType.NoteOn).Max(x => (x as NoteOnEvent).NoteNumber);
                MinNoteLevel = track.Events.Where(x => x.EventType == MidiEventType.NoteOn).Min(x => (x as NoteOnEvent).NoteNumber);
            }

        }
    }
}

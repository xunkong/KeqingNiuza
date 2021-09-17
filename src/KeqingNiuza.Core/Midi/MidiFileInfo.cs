using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melanchall.DryWetMidi.Core;

namespace KeqingNiuza.Core.Midi
{
    public class MidiFileInfo
    {

        public string Name { get; set; }

        public FileInfo FileInfo { get; private set; }

        public MidiFile MidiFile { get; private set; }

        public List<MidiTrack> MidiTracks { get; set; }

        public List<MidiTrack> CanPlayTracks { get; set; }

        public int NoteNumber { get; set; }

        public int CanPlayNoteNumber { get; set; }

        public double CanPlayNoteRadio => (double)CanPlayNoteNumber / NoteNumber;

        public int MaxNoteLevel { get; set; }

        public int MinNoteLevel { get; set; }

        public MidiFileInfo(string path)
        {
            MidiFile = MidiFile.Read(Path.GetFullPath(path));
            Name = Path.GetFileNameWithoutExtension(path);
            MidiTracks = MidiFile.GetTrackChunks().Select(x => new MidiTrack(x)).ToList();
            CanPlayTracks = MidiTracks.Where(x => x.CanBeChecked).ToList();
            CanPlayTracks.ForEach(x => x.IsCheck = true);
            NoteNumber = CanPlayTracks.Sum(x => x.NoteNumber);
            CanPlayNoteNumber = CanPlayTracks.Sum(x => x.CanPlayNoteNumber);
            MaxNoteLevel = CanPlayTracks.Max(x => x.MaxNoteLevel);
            MinNoteLevel = CanPlayTracks.Min(x => x.MinNoteLevel);
        }

        public void RefreshTracksByNoteLevel(int noteLevel)
        {
            CanPlayTracks.ForEach(x => x.RefreshByNoteLevel(noteLevel));
            NoteNumber = CanPlayTracks.Sum(x => x.NoteNumber);
            CanPlayNoteNumber = CanPlayTracks.Sum(x => x.CanPlayNoteNumber);
            MaxNoteLevel = CanPlayTracks.Max(x => x.MaxNoteLevel);
            MinNoteLevel = CanPlayTracks.Min(x => x.MinNoteLevel);
        }

    }
}

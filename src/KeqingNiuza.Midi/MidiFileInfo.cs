using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Midi
{
    public class MidiFileInfo:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
            }
        }


        public FileInfo FileInfo { get; private set; }

        public MidiFile MidiFile { get; private set; }

        public List<MidiTrack> MidiTracks { get; set; }

        public List<MidiTrack> CanPlayedTracks { get; set; }

        public MidiFileInfo(string path)
        {
            MidiFile = MidiFile.Read(Path.GetFullPath(path));
            Name = Path.GetFileNameWithoutExtension(path);
            MidiTracks = MidiFile.GetTrackChunks().Select(x => new MidiTrack(x)).ToList();
            CanPlayedTracks = MidiTracks.Where(x => x.CanBeChecked).ToList();
            CanPlayedTracks.ForEach(x => x.IsCheck = true);
        }

    }
}

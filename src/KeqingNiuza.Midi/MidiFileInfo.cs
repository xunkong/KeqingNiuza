using Melanchall.DryWetMidi.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeqingNiuza.Midi
{
    public class MidiFileInfo
    {
        public string Name { get; private set; }

        public FileInfo FileInfo { get; private set; }

        public MidiFile MidiFile { get; private set; }


        public MidiFileInfo(string path)
        {
            FileInfo = new FileInfo(path);
            MidiFile = MidiFile.Read(FileInfo.FullName);
            Name = FileInfo.Name.Replace(FileInfo.Extension, "");
        }

    }
}

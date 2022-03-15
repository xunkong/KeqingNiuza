using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.MusicGame
{
    public class NoteInfo
    {

        [JsonConverter(typeof(JsonStringEnumConverter)), JsonPropertyName("button")]
        public ButtonType ButtonType { get; set; }

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("note")]
        public int Note { get; set; }
    }

}

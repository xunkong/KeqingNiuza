using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace KeqingNiuza.Core.Wish
{
    public class ImportedWishData : WishData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonIgnore]
        public bool IsError { get; set; }


        private string _Comment;
        [JsonIgnore]
        public string Comment
        {
            get { return _Comment; }
            set
            {
                _Comment = value;
                OnPropertyChanged();
            }
        }

    }
}

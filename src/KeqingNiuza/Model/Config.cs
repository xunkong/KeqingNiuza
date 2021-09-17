using System.Collections.ObjectModel;

namespace KeqingNiuza.Model
{
    public class Config
    {

        public int LatestUid { get; set; }

        public ObservableCollection<UserData> UserDataList { get; set; }


    }
}

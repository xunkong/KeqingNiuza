using Newtonsoft.Json;

namespace DGP.Genshin.MiHoYoAPI.UserInfo
{
    public class Achieve
    {
        [JsonProperty("like_num")] public string LikeNum { get; set; }
        [JsonProperty("post_num")] public string PostNum { get; set; }
        [JsonProperty("replypost_num")] public string ReplypostNum { get; set; }
        [JsonProperty("follow_cnt")] public string FollowCnt { get; set; }
        [JsonProperty("followed_cnt")] public string FollowedCnt { get; set; }
        [JsonProperty("topic_cnt")] public string TopicCnt { get; set; }
        [JsonProperty("new_follower_num")] public string NewFollowerNum { get; set; }
        [JsonProperty("good_post_num")] public string GoodPostNum { get; set; }
        [JsonProperty("follow_collection_cnt")] public string FollowCollectionCnt { get; set; }
    }
}

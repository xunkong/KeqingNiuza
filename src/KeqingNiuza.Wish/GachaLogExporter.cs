using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeqingNiuza.Wish
{
    public class GachaLogExporter
    {
        private readonly string baseRequestUrl = @"https://hk4e-api.mihoyo.com/event/gacha_info/api/getGachaLog";

        private readonly string authString;

        private readonly HttpClient HttpClient;

        public GachaLogExporter(string url)
        {
            if (url.EndsWith("#/log"))
            {
                authString = url.Substring(url.IndexOf('?')).Replace("#/log", "");
                HttpClient = new HttpClient();
            }
            else
            {
                throw new ArgumentException("Url does not meet the requirement!");
            }
        }

        public async Task<List<WishData>> GetAllLog()
        {
            List<WishData> wishDatas = new List<WishData>();
            await Task.Run(() =>
            {
                QueryParam param = new QueryParam() { WishType = WishType.Novice, Page = 1, Size = 6, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.Permanent, Page = 1, Size = 6, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.CharacterEvent, Page = 1, Size = 6, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.WeaponEvent, Page = 1, Size = 6, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
            });
            return wishDatas;
        }

        public async Task<List<WishData>> GetAllLog(int size)
        {
            List<WishData> wishDatas = new List<WishData>();
            await Task.Run(() =>
            {
                QueryParam param = new QueryParam() { WishType = WishType.Novice, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.Permanent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.CharacterEvent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
                param = new QueryParam() { WishType = WishType.WeaponEvent, Page = 1, Size = size, EndId = 0 };
                wishDatas.AddRange(GetWishLogList(param));
            });
            return wishDatas;
        }

        public List<WishData> GetWishLogList(WishType type)
        {
            QueryParam param = new QueryParam() { WishType = type, Page = 1, Size = 6, EndId = 0 };
            return GetWishLogList(param);
        }

        public List<WishData> GetWishLogList(QueryParam param)
        {
            List<WishData> list = new List<WishData>();
            string url, str;
            ResponseData result;
            do
            {
                url = $@"{baseRequestUrl}{authString}&{param}";
                str = HttpClient.GetStringAsync(url).Result;
                result = JsonSerializer.Deserialize<ResponseData>(str);
                if (result.retcode != 0)
                {
                    throw new ArgumentException(result.message);
                }
                if (result.data.list.Count != 0)
                {
                    list.AddRange(result.data.list);
                    param.Page++;
                    param.EndId = result.data.list.Last().Id;
                }

            } while (result.data.list.Count == param.Size);
            return list;
        }
    }
}

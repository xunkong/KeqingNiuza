using ControlzEx.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.XunkongApi
{
    internal class XunkongApiClient
    {


        public HttpClient HttpClient;

        private JsonSerializerOptions _options;


        public XunkongApiClient()
        {
            HttpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate });
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        }



        private async Task<T> CommonGetAsync<T>(string url) where T : class
        {
            var dto = await HttpClient.GetFromJsonAsync<ResponseBaseWrapper<T>>(url, _options);
            if (dto is null)
            {
                throw new XunkongException(-1, "Response body is null.");
            }
            if (dto.Code != 0)
            {
                throw new XunkongException(dto.Code, dto.Message);
            }
            // warning 不确定是否应该判断响应data为null的情况
            if (dto.Data is null)
            {
                throw new XunkongException(-1, "Response data is null.");
            }
            return dto.Data;
        }



        public async Task<IEnumerable<CharacterInfo>> GetCharacterInfosAsync()
        {
            var url = $"https://api.xunkong.cc/v0.1/genshin/metadata/character";
            var result = await CommonGetAsync<MetadataDto<CharacterInfo>>(url);
            return result.List;
        }


        public async Task<IEnumerable<WeaponInfo>> GetWeaponInfosAsync()
        {
            var url = $"https://api.xunkong.cc/v0.1/genshin/metadata/weapon";
            var result = await CommonGetAsync<MetadataDto<WeaponInfo>>(url);
            return result.List;
        }


        public async Task<IEnumerable<WishEventInfo>> GetWishEventInfosAsync()
        {
            var url = $"https://api.xunkong.cc/v0.1/genshin/metadata/wishevent";
            var result = await CommonGetAsync<MetadataDto<WishEventInfo>>(url);
            return result.List;
        }

    }
}

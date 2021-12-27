using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace KeqingNiuza.Core.Wish
{
    public class JsonImporter
    {
        private readonly List<WishData> wishDatas;

        private readonly JsonSerializerOptions options;

        public JsonImporter()
        {
            wishDatas = new List<WishData>();
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }


        public List<WishData> Deserialize(string json)
        {
            var baseNode = JsonNode.Parse(json);
            ParseJson(baseNode);
            return wishDatas.Distinct().OrderBy(x => x.Id).ToList();
        }


        private void ParseJson(JsonNode node)
        {
            if (node is JsonValue value)
            {
                return;
            }

            if (node is JsonObject obj)
            {
                if (obj.ContainsKey("uid")
                    && obj.ContainsKey("gacha_type")
                    && obj.ContainsKey("time")
                    && obj.ContainsKey("name")
                    && obj.ContainsKey("item_type")
                    && obj.ContainsKey("id"))
                {
                    var data = obj.Deserialize<WishData>(options);
                    if (data != null)
                    {
                        wishDatas.Add(data);
                    }
                    return;
                }
                else
                {
                    foreach (var property in obj)
                    {
                        ParseJson(property.Value);
                    }
                }
            }

            if (node is JsonArray array)
            {
                foreach (var item in array)
                {
                    ParseJson(item);
                }
            }

        }

    }
}

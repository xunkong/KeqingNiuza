using System;
using System.Collections.Generic;

namespace DGP.Genshin.MiHoYoAPI.Gacha
{
    /// <summary>
    /// 包装了包含Uid与抽卡记录的字典
    /// 所有与抽卡记录相关的服务都基于对此类的操作
    /// </summary>
    public class GachaDataCollection : Dictionary<string, GachaData>
    {
        public event Action<string> UidAdded;

        public new void Add(string uid, GachaData data)
        {
            base.Add(uid, data);
            UidAdded?.Invoke(uid);
        }

        /// <summary>
        /// 获取最新的时间戳id
        /// </summary>
        /// <returns>default 0</returns>
        public long GetNewestTimeId(ConfigType type, string uid)
        {
            //有uid有卡池记录就读取最新物品的id,否则返回0
            if (uid != null && ContainsKey(uid))
            {
                if (type.Key != null)
                {
                    if (this[uid] is GachaData one)
                    {
                        if (one.ContainsKey(type.Key))
                        {
                            List<GachaLogItem> item = one[type.Key];
                            if (item != null)
                            {
                                return item[0].TimeId;
                            }
                        }
                    }
                }
            }
            return 0;
        }
    }
}

using System;
using System.ComponentModel;
using System.Reflection;

namespace KeqingNiuza.Core.Wish
{
    public enum WishType
    {
        /// <summary>
        /// 新手祈愿
        /// </summary>
        [Description("新手祈愿")]
        Novice = 100,

        /// <summary>
        /// 常驻祈愿
        /// </summary>
        [Description("常驻祈愿")]
        Permanent = 200,

        /// <summary>
        /// 角色活动祈愿
        /// </summary>
        [Description("角色活动祈愿")]
        CharacterEvent = 301,

        /// <summary>
        /// 武器活动祈愿
        /// </summary>
        [Description("武器活动祈愿")]
        WeaponEvent = 302,

        /// <summary>
        /// 角色活动祈愿-2
        /// </summary>
        [Description("角色活动祈愿-2")]
        CharacterEvent_2 = 400,
    }


    public static class WishTypeExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}

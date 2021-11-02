using System;
using System.Collections.Generic;
using System.Linq;

namespace DGP.Genshin.Common.Request.QueryString
{
    public static class ListExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="shouldRemovePredicate"></param>
        /// <returns></returns>
        public static bool RemoveFirstWhere<T>(this IList<T> list, Func<T, bool> shouldRemovePredicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (shouldRemovePredicate.Invoke(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private static readonly Random Random = new Random();
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Next(0, list.Count)];
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        //public static List<T> ClonePartially<T>(this List<T> listToClone) where T : IPartiallyCloneable<T> =>
        //    listToClone.Select(item => item.ClonePartially()).ToList();
    }
}

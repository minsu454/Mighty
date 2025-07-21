using System;
using System.Collections.Generic;

namespace Common.ListEx
{
    public static class ListExtensions
    {
        /// <summary>
        /// 해당 오브젝트가 있는지 키로 확인 함수
        /// </summary>
        public static bool ContainsTuple<TKey, TValue>(this List<(TKey, TValue)> list, TKey target)
        {
            foreach (var item in list)
            {
                if (item.Item1 == null)
                    throw new Exception("List<(TKey, TValue)> list.Item1 is None.");

                if (item.Item1.Equals(target))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 해당 오브젝트가 있는지 키로 확인 후 반환 함수
        /// </summary>
        public static bool TryGetTupleByKey<TKey, TValue>(this List<(TKey, TValue)> list, TKey target, out (TKey, TValue) result)
        {
            foreach (var item in list)
            {
                if (item.Item1 == null)
                    throw new Exception("List<(TKey, TValue)> list.Item1 is None.");

                if (item.Item1.Equals(target))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }

        /// <summary>
        /// 해당 오브젝트가 있는지 값으로 확인 후 반환 함수
        /// </summary>
        public static bool TryGetTupleByValue<TKey, TValue>(this List<(TKey, TValue)> list, TValue target, out (TKey, TValue) result)
        {
            foreach (var item in list)
            {
                if (item.Item2 == null)
                    throw new Exception("List<(TKey, TValue)> list.Item1 is None.");

                if (item.Item2.Equals(target))
                {
                    result = item;
                    return true;
                }
            }

            result = default;
            return false;
        }

        /// <summary>
        /// list 셔플해주는 함수
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            Random random = new Random();

            for (int i = list.Count - 1; i > 0; i--)
            {
                int n = random.Next(i + 1);
                T value = list[n];
                list[n] = list[i];
                list[i] = value;
            }
        }

        /// <summary>
        /// RemoveAt시 해당 오브젝트 반환 함수
        /// </summary>
        public static T TryRemoveAt<T>(this IList<T> list, int index)
        {
            T value = list[index];
            list.RemoveAt(index);

            return value;
        }
    }
}

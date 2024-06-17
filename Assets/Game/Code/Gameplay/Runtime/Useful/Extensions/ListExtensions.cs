using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace YellowSquad.CashierSimulator.Gameplay.Useful
{
    public static class ListExtensions
    {
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            for (int i = 1; i < list.Count; i++)
                list.Swap(i - 1, Random.Range(i, list.Count));

            return list;
        }

        public static IList<T> Shuffle<T>(this IList<T> list, Func<int, int, int> randomizer)
        {
            for (int i = 1; i < list.Count; i++)
                list.Swap(i - 1, randomizer(i, list.Count));

            return list;
        }

        public static void Swap<T>(this IList<T> list, int first, int second)
        {
            (list[first], list[second]) = (list[second], list[first]);
        }
    }
}

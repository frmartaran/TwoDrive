using System;
using System.Collections.Generic;
using System.Text;

namespace TwoDrive.BusinessLogic.Helpers
{
    public static class CollectionHelper
    {
        public static void AddRange<T>(this ICollection<T> mainList, List<T> elements)
        {
            foreach (var element in elements)
            {
                mainList.Add(element);
            }
        }

        public static void RemoveRange<T>(this ICollection<T> mainList, List<T> elements)
        {
            foreach (var element in elements)
            {
                mainList.Remove(element);
            }
        }
    }
}

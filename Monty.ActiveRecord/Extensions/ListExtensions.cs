using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Monty.ActiveRecord
{
    /// <summary>
    /// List Extensions
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Explodes the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="padding">The padding.</param>
        /// <returns></returns>
        public static string Explode(this IList list, string padding)
        {
            string subPadding = padding + "\t";

            StringBuilder result = new StringBuilder();

            result.AppendFormat("[", padding);

            for (int i = 0; i < list.Count; i++)
            {
                object item = list[i];

                if (item == null)
                    result.AppendFormat("\n{0}#{1}: [NULL]", subPadding, i);
                else
                    result.AppendFormat("\n{0}#{1}: {2}", subPadding, i, item);
            }

            result.AppendFormat("\n{0}]", padding);

            return result.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentManagement.Application.DTOs.Common.Extensions
{
    public static class StringEnumerableExtensions
    {
        /// <summary>
        /// Joins the elements of an IEnumerable<string> into a single string, separated by a comma.
        /// </summary>
        /// <param name="source">The collection of strings to join.</param>
        /// <returns>A comma-separated string representation of the collection.</returns>
        public static string ToCommaSeparatedString(this IEnumerable<string> source)
        {
            if (source == null || !source.Any())
                return string.Empty;

            return string.Join(",", source);
        }
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;

// ReSharper disable once CheckNamespace - for discoverability
namespace System.Linq
{
    internal static class MappingExtensions
    {
        public static IReadOnlyCollection<TOut> MapAsReadOnly<TIn, TOut>(this IEnumerable<TIn> toMap, Func<TIn, TOut> map)
        {
            _ = toMap ?? throw new ArgumentNullException(nameof(toMap));
            _ = map ?? throw new ArgumentNullException(nameof(map));
            
            return toMap switch
            {
                IReadOnlyCollection<TIn> readOnly => MapWithKnownCount(readOnly, readOnly.Count, map),
                ICollection<TIn> collection => MapWithKnownCount(collection, collection.Count, map),
                _ => toMap.Select(map).ToList().AsReadOnly()
            };
        }

        private static IReadOnlyCollection<TOut> MapWithKnownCount<TIn, TOut>(
            IEnumerable<TIn> collection,
            int count,
            Func<TIn, TOut> map)
        {
            if (count == 0)
            {
                return Array.Empty<TOut>();
            }
            
            var result = new TOut[count];
            var i = 0;
            foreach (var entry in collection)
            {
                result[i] = map(entry);
                ++i;
            }

            return new ReadOnlyCollection<TOut>(result);
        }
    }
}
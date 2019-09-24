using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace ClearSpam.Common
{
    public static class Extensions
    {
        public static IEnumerable<TDestination> MapList<TSource, TDestination>(this IMapper mapper, IEnumerable<TSource> list)
        {
            return list.Select(x => mapper.Map<TDestination>(x));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MusicCatalog.Entities;

namespace MusicCatalog.SearchStrategies
{
    public class GenreSearchStrategy : ISearchStrategy
    {
        public List<object> Search(Catalog catalog, object criteria, SearchType searchType)
        {
            if (searchType == SearchType.Track)
            {
                return catalog.GetTracks()
                    .Where(tr => tr.Genre.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .Cast<object>()
                    .ToList();
            }

            throw new ArgumentOutOfRangeException("Genre search is only for tracks");
        }
    }
}
